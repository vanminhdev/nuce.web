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

            var surveyRound = surveyContext.AsEduSurveyDotKhaoSat.FirstOrDefault(o => o.Id == surveyRoundId && o.Status == (int)SurveyRoundStatus.New);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }

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
                var queryTheSurvey = surveyContext.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == surveyRound.Id && o.Status == (int)TheSurveyStatus.New);
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

                var defaultSubjects = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.DefaultSubjects);
                if (defaultSubjects == null)
                {
                    throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn không được phân loại trong đợt khảo sát này");
                }

                _logger.LogInformation("Generate BaiKhaoSat_SinhVien is start.");

                var query = eduContext.AsAcademyStudentClassRoom.OrderBy(o => o.Id);
                var numStudent = query.Count();
                var skip = 0;
                var take = 500;

                List<AsAcademyStudentClassRoom> studentClassrooms;
                AsAcademyLecturerClassRoom lectureClassroom = null;
                AsAcademyLecturer lecturer = null;
                AsAcademySubject subject = null;
                AsAcademySubjectExtend subjectExtend = null;
                AsAcademyClassRoom classroom = null;
                Guid baiKhaoSatId = defaultSubjects.Id;
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
                            subject = eduContext.AsAcademySubject.FirstOrDefault(o => o.Code == classroom.SubjectCode);
                            if (subject != null)
                            {
                                subjectExtend = eduContext.AsAcademySubjectExtend.FirstOrDefault(o => o.Code == subject.Code);
                                baiKhaoSatId = defaultSubjects.Id; //mặc định
                                if (subjectExtend != null && subjectExtend.Type != null)
                                {
                                    if (subjectExtend.Type == (int)TheSurveyType.TheoreticalSubjects)
                                    {
                                        baiKhaoSatId = theoreticalSubjects.Id;
                                    }
                                    else if (subjectExtend.Type == (int)TheSurveyType.PracticalSubjects)
                                    {
                                        baiKhaoSatId = practicalSubjects.Id;
                                    }
                                    else if (subjectExtend.Type == (int)TheSurveyType.TheoreticalPracticalSubjects)
                                    {
                                        baiKhaoSatId = theoreticalPracticalSubjects.Id;
                                    }
                                    else if (subjectExtend.Type == (int)TheSurveyType.AssignmentSubjects)
                                    {
                                        baiKhaoSatId = assignmentSubjects.Id;
                                    }
                                }
                            }
                        }
                        else
                        {
                            subject = null;
                        }

                        //nếu chưa có thì thêm
                        if (surveyContext.AsEduSurveyBaiKhaoSatSinhVien.FirstOrDefault(o => o.StudentCode == sc.StudentCode && o.ClassRoomCode == sc.ClassRoomCode) == null)
                        {
                            surveyContext.AsEduSurveyBaiKhaoSatSinhVien.Add(new AsEduSurveyBaiKhaoSatSinhVien
                            {
                                Id = Guid.NewGuid(),
                                BaiKhaoSatId = baiKhaoSatId,
                                DepartmentCode = lecturer != null ? lecturer.DepartmentCode : "",
                                ClassRoomCode = sc.ClassRoomCode,
                                LecturerCode = lecturer != null ? lecturer.Code : "",
                                LecturerName = lecturer != null ? lecturer.FullName : "",
                                SubjectCode = subject != null ? subject.Code : "",
                                SubjectName = subject != null ? subject.Name : "",
                                SubjectType = subjectExtend != null ? subjectExtend.Type != null ? subjectExtend.Type.Value : -1 : -1,
                                StudentId = sc.StudentId != null ? sc.StudentId.Value : -1,
                                StudentCode = sc.StudentCode,
                                DeThi = "",
                                BaiLam = "",
                                NgayGioBatDau = DateTime.Now,
                                NgayGioNopBai = DateTime.Now,
                                Status = (int)SurveyStudentStatus.DoNot,
                                Type = 1,
                            });
                        }
                    }
                    surveyContext.SaveChanges();
                    _logger.LogInformation($"Generate BaiKhaoSat_SinhVien loading {skip}/{numStudent}.");
                    skip += take;
                }

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

            _backgroundTaskWorker.StartAction(() =>
            {
                GenerateTheSurveyStudentBG(surveyRoundId);
            });
        }

    }
}
