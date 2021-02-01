using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.EduData;
using nuce.web.api.Models.Status;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Background;
using nuce.web.api.Services.Survey.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace nuce.web.api.Services.Survey.BackgroundTasks
{
    public class BaiKhaoSatSinhVienBackgroundTask
    {
        private readonly ILogger<BaiKhaoSatSinhVienBackgroundTask> _logger;
        private readonly IAsEduSurveyBaiKhaoSatSinhVienService _asEduSurveyBaiKhaoSatSinhVienService;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly BackgroundTaskWorkder _backgroundTaskWorker;

        public BaiKhaoSatSinhVienBackgroundTask(ILogger<BaiKhaoSatSinhVienBackgroundTask> logger,
            IAsEduSurveyBaiKhaoSatSinhVienService asEduSurveyBaiKhaoSatSinhVienService,
            IServiceScopeFactory scopeFactory, BackgroundTaskWorkder backgroundTaskWorker)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _backgroundTaskWorker = backgroundTaskWorker;
            _asEduSurveyBaiKhaoSatSinhVienService = asEduSurveyBaiKhaoSatSinhVienService;
        }

        private void GenerateTheSurveyStudentBG(Guid surveyRoundId)
        {
            var scope = _scopeFactory.CreateScope();
            var eduContext = scope.ServiceProvider.GetRequiredService<EduDataContext>();
            var surveyContext = scope.ServiceProvider.GetRequiredService<SurveyContext>();
            var statusContext = scope.ServiceProvider.GetRequiredService<StatusContext>();

            var status = statusContext.AsStatusTableTask.FirstOrDefault(o => o.TableName == TableNameTask.AsEduSurveyBaiKhaoSatSinhVien);
            if (status == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bản ghi cập nhật trạng thái cho bảng bài khảo sát sinh viên");
            }

            //bảng đang làm việc
            if (status.Status == (int)TableTaskStatus.Doing)
            {
                throw new TableBusyException("Đang tạo bài khảo sát cho từng sinh viên, thao tác bị huỷ");
            }

            status.Status = (int)TableTaskStatus.Doing;
            statusContext.SaveChanges();

            try
            {
                var surveyRound = surveyContext.AsEduSurveyDotKhaoSat.FirstOrDefault(o => o.Id == surveyRoundId && o.Status != (int)SurveyRoundStatus.Deleted);
                if (surveyRound == null)
                {
                    throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
                }

                if (surveyRound.Status == (int)SurveyRoundStatus.End || DateTime.Now >= surveyRound.EndDate || surveyRound.Status == (int)SurveyRoundStatus.Closed)
                {
                    throw new InvalidInputDataException("Đợt khảo sát không còn hoạt động");
                }

                var queryTheSurvey = surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == surveyRound.Id && o.Status == (int)TheSurveyStatus.Published);
                var theoreticalSubjects = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalSubjects);
                if (theoreticalSubjects == null)
                {
                    throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn lý thuyết trong đợt khảo sát này");
                }

                var practicalSubjects = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.PracticalSubjects);
                if (practicalSubjects == null)
                {
                    throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn thực hành, thực tập, thí nghiệm trong đợt khảo sát này");
                }

                var theoreticalPracticalSubjects = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalPracticalSubjects);
                if (theoreticalPracticalSubjects == null)
                {
                    throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn lý thuyết + thực hành trong đợt khảo sát này");
                }

                var assignmentSubjects = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.AssignmentSubjects);
                if (assignmentSubjects == null)
                {
                    throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn đồ án trong đợt khảo sát này");
                }

                _logger.LogInformation("Generate BaiKhaoSat_SinhVien is start.");

                var query = eduContext.AsAcademyStudentClassRoom.OrderBy(o => o.Id);
                var numStudent = query.Count();
                var skip = 0;
                var take = 1000;

                List<AsAcademyStudentClassRoom> studentClassrooms;
                AsAcademyLecturerClassRoom lectureClassroom = null;
                AsAcademyLecturer lecturer = null;
                AsAcademySubject subject = null;
                AsAcademySubjectExtend subjectExtend = null;
                AsAcademyClassRoom classroom = null;

                var defaultTheSurveyTypeId = theoreticalSubjects.Id;
                Guid baiKhaoSatId = defaultTheSurveyTypeId; //mặc định

                bool chuaDungLoaiMon = false;

                while (skip <= numStudent)
                {
                    studentClassrooms = query.Skip(skip).Take(take).ToList();
                    foreach (var sc in studentClassrooms)
                    {
                        lectureClassroom = eduContext.AsAcademyLecturerClassRoom.FirstOrDefault(o => o.ClassRoomCode == sc.ClassRoomCode);
                        if (lectureClassroom != null)
                        {
                            lecturer = eduContext.AsAcademyLecturer.FirstOrDefault(o => o.Code == lectureClassroom.LecturerCode);
                        }
                        else
                        {
                            lecturer = null;
                        }

                        classroom = eduContext.AsAcademyClassRoom.FirstOrDefault(o => o.Code == sc.ClassRoomCode);
                        if (classroom != null)
                        {
                            //lớp có môn học là gì
                            subject = eduContext.AsAcademySubject.FirstOrDefault(o => o.Code == classroom.SubjectCode);
                            if (subject != null)
                            {
                                //loại môn của môn đó
                                subjectExtend = eduContext.AsAcademySubjectExtend.FirstOrDefault(o => o.Code == subject.Code);
                                baiKhaoSatId = defaultTheSurveyTypeId; //mặc định
                                chuaDungLoaiMon = false;
                                if (subjectExtend != null && subjectExtend.Type != null)
                                {
                                    if (subjectExtend.Type == (int)TheSurveyType.TheoreticalSubjects)
                                    {
                                        baiKhaoSatId = theoreticalSubjects.Id;
                                    }
                                    else if (subjectExtend.Type == (int)TheSurveyType.TheoreticalPracticalSubjects)
                                    {
                                        baiKhaoSatId = theoreticalPracticalSubjects.Id;
                                    }
                                    else if (subjectExtend.Type == (int)TheSurveyType.PracticalSubjects)
                                    {
                                        baiKhaoSatId = practicalSubjects.Id;
                                    }
                                    else if (subjectExtend.Type == (int)TheSurveyType.AssignmentSubjects)
                                    {
                                        baiKhaoSatId = assignmentSubjects.Id;
                                    }
                                    else
                                    {
                                        //trường hợp môn không thuộc 4 loại 
                                        chuaDungLoaiMon = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            _logger.LogWarning($"Lop co ma {sc.ClassRoomCode} khong ton tai");
                            subject = null;
                        }

                        var recordBaikssv = surveyContext.AsEduSurveyBaiKhaoSatSinhVien
                            .FirstOrDefault(o => o.BaiKhaoSatId == baiKhaoSatId && o.StudentCode == sc.StudentCode && o.ClassRoomCode == sc.ClassRoomCode 
                            && o.Nhhk == sc.Nhhk && o.Status != (int)SurveyStudentStatus.Done); //sinh viên lớp môn của kỳ đó có chưa
                        //nếu chưa có thì thêm
                        if (recordBaikssv == null)
                        {
                            surveyContext.AsEduSurveyBaiKhaoSatSinhVien.Add(new AsEduSurveyBaiKhaoSatSinhVien
                            {
                                Id = Guid.NewGuid(),
                                BaiKhaoSatId = baiKhaoSatId,
                                DepartmentCode = lecturer != null ? lecturer.DepartmentCode : "",
                                ClassRoomCode = sc.ClassRoomCode,
                                Nhhk = sc.Nhhk,
                                LecturerCode = lecturer != null ? lecturer.Code : "",
                                LecturerName = lecturer != null ? lecturer.FullName : "",
                                SubjectCode = subject != null ? subject.Code : "",
                                SubjectName = subject != null ? subject.Name : "",
                                SubjectType = subjectExtend != null ? subjectExtend.Type != null ? subjectExtend.Type.Value : -1 : -1,
                                StudentCode = sc.StudentCode,
                                DeThi = "",
                                BaiLam = "",
                                NgayGioBatDau = DateTime.Now,
                                NgayGioNopBai = DateTime.Now,
                                Status = chuaDungLoaiMon ? (int)SurveyStudentStatus.HaveNot : (int)SurveyStudentStatus.DoNot,
                                Type = 1,
                            });
                        }
                        else //có rồi thì update một số trường bị thiếu
                        {
                            if ((recordBaikssv.Status == (int)SurveyStudentStatus.DoNot || recordBaikssv.Status == (int)SurveyStudentStatus.HaveNot) && recordBaikssv.BaiKhaoSatId != baiKhaoSatId)
                            {
                                recordBaikssv.BaiKhaoSatId = baiKhaoSatId;
                            }
                            recordBaikssv.DepartmentCode = lecturer != null ? lecturer.DepartmentCode : "";
                            recordBaikssv.LecturerCode = lecturer != null ? lecturer.Code : "";
                            recordBaikssv.LecturerName = lecturer != null ? lecturer.FullName : "";
                            recordBaikssv.SubjectCode = subject != null ? subject.Code : "";
                            recordBaikssv.SubjectName = subject != null ? subject.Name : "";
                            recordBaikssv.SubjectType = subjectExtend != null ? subjectExtend.Type != null ? subjectExtend.Type.Value : -1 : -1;
                        }
                    }
                    surveyContext.SaveChanges();
                    _logger.LogInformation($"Generate BaiKhaoSat_SinhVien loading {skip}/{numStudent}.");
                    status.Message = $"{skip}/{numStudent}";
                    statusContext.SaveChanges();
                    skip += take;
                }
                status.Message = $"{numStudent}/{numStudent}";
                statusContext.SaveChanges();

                _logger.LogInformation("Generate BaiKhaoSat_SinhVien is done.");

                //hoàn thành
                status.Status = (int)TableTaskStatus.Done;
                status.IsSuccess = true;
                status.Message = null;
                statusContext.SaveChanges();
            }
            catch (Exception e)
            {
                status.Status = (int)TableTaskStatus.Done;
                status.IsSuccess = false;
                status.Message = e.Message;
                statusContext.SaveChanges();
                throw e;
            }
        }

        public async System.Threading.Tasks.Task GenerateTheSurveyStudent(Guid surveyRoundId)
        {
            var status = await _asEduSurveyBaiKhaoSatSinhVienService.GetGenerateTheSurveyStudentStatus();
            if (status == (int)TableTaskStatus.Doing)
            {
                throw new TableBusyException("Bảng đang làm việc, thao tác bị huỷ");
            }

            var scope = _scopeFactory.CreateScope();
            var surveyContext = scope.ServiceProvider.GetRequiredService<SurveyContext>();

            var surveyRound = surveyContext.AsEduSurveyDotKhaoSat.FirstOrDefault(o => o.Id == surveyRoundId && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }

            if (surveyRound.Status == (int)SurveyRoundStatus.End || DateTime.Now >= surveyRound.EndDate || surveyRound.Status == (int)SurveyRoundStatus.Closed)
            {
                throw new InvalidInputDataException("Đợt khảo sát không còn hoạt động");
            }

            var transaction = surveyContext.Database.BeginTransaction();
            try
            {
                var queryTheSurvey = surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == surveyRoundId && o.Status != (int)TheSurveyStatus.Deleted);
                var theoreticalSubjects = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalSubjects);
                if (theoreticalSubjects == null)
                {
                    throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn lý thuyết trong đợt khảo sát này");
                }

                var practicalSubjects = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.PracticalSubjects);
                if (practicalSubjects == null)
                {
                    throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn thực hành, thực tập, thí nghiệm trong đợt khảo sát này");
                }

                var theoreticalPracticalSubjects = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalPracticalSubjects);
                if (theoreticalPracticalSubjects == null)
                {
                    throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn lý thuyết + thực hành trong đợt khảo sát này");
                }

                var assignmentSubjects = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.AssignmentSubjects);
                if (assignmentSubjects == null)
                {
                    throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn đồ án trong đợt khảo sát này");
                }

                theoreticalSubjects.Status = (int)TheSurveyStatus.Published;
                practicalSubjects.Status = (int)TheSurveyStatus.Published;
                theoreticalPracticalSubjects.Status = (int)TheSurveyStatus.Published;
                assignmentSubjects.Status = (int)TheSurveyStatus.Published;
                await surveyContext.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw e;
            }

            _backgroundTaskWorker.StartAction(() =>
            {
                GenerateTheSurveyStudentBG(surveyRoundId);
            });
        }

    }
}
