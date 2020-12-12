using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.EduData;
using nuce.web.api.Models.Status;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Background;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace nuce.web.api.Controllers.Survey
{
    /// <summary>
    /// Bài khảo sát sinh viên
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TheSurveyStudentController : ControllerBase
    {
        private readonly ILogger<TheSurveyStudentController> _logger;
        private readonly IAsEduSurveyBaiKhaoSatSinhVienService _asEduSurveyBaiKhaoSatSinhVienService;
        private readonly IUserService _userService;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly CancellationToken _cancellationToken;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly MonitorLoop _monitorLoop;

        public TheSurveyStudentController(ILogger<TheSurveyStudentController> logger, IAsEduSurveyBaiKhaoSatSinhVienService asEduSurveyBaiKhaoSatSinhVienService, IUserService userService,
            IBackgroundTaskQueue taskQueue, IHostApplicationLifetime applicationLifetime, IServiceScopeFactory scopeFactory, MonitorLoop monitorLoop)
        {
            _logger = logger;
            _asEduSurveyBaiKhaoSatSinhVienService = asEduSurveyBaiKhaoSatSinhVienService;
            _userService = userService;
            _taskQueue = taskQueue;
            _cancellationToken = applicationLifetime.ApplicationStopping;

            _scopeFactory = scopeFactory;
            _monitorLoop = monitorLoop;
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetTheSurvey()
        {
            var studentCode = _userService.GetCurrentStudentCode();
            var result = await _asEduSurveyBaiKhaoSatSinhVienService.GetTheSurvey(studentCode);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetTheSurveyContent([Required(AllowEmptyStrings = false)] Guid? id, [Required(AllowEmptyStrings = false)] string classroomCode)
        {
            try
            {
                //mã sinh viên kiểm tra sinh viên có bài khảo sát đó thật không
                var studentCode = _userService.GetCurrentStudentCode();
                var result = await _asEduSurveyBaiKhaoSatSinhVienService.GetTheSurveyContent(studentCode, classroomCode, id.Value);
                return Ok(result);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không lấy được nội dung bài khảo sát", detailMessage = e.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetSelectedAnswerAutoSave(
            [Required(AllowEmptyStrings = false)]
            [NotContainWhiteSpace]
            string classRoomCode)
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var result = await _asEduSurveyBaiKhaoSatSinhVienService.GetSelectedAnswerAutoSave(studentCode, classRoomCode);
                return Ok(result);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không lấy được câu trả lời đã chọn", detailMessage = e.Message });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> AutoSave([FromBody] SelectedAnswerAutoSave content)
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                await _asEduSurveyBaiKhaoSatSinhVienService.AutoSave(studentCode, content.ClassRoomCode, content.QuestionCode, content.AnswerCode, content.AnswerCodeInMulSelect,
                    content.AnswerContent, content.IsAnswerCodesAdd != null ? content.IsAnswerCodesAdd.Value : true);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tự lưu được bài làm", detailMessage = e.Message });
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không tự lưu được bài làm", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tự lưu được bài làm", detailMessage = mainMessage });
            }
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SaveSelectedAnswer([FromBody]
            [Required(AllowEmptyStrings = false)]
            [NotContainWhiteSpace]
            string classRoomCode)
        {
            try
            {
                var studentCode = _userService.GetCurrentStudentCode();
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                await _asEduSurveyBaiKhaoSatSinhVienService.SaveSelectedAnswer(studentCode, classRoomCode, ip);
            }
            catch (InvalidDataException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lưu được bài khảo sát", detailMessage = e.Message });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lưu được bài khảo sát", detailMessage = e.Message });
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không lưu được bài làm", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lưu được bài khảo sát", detailMessage = mainMessage });
            }
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "P_KhaoThi")]
        public async Task<IActionResult> GetGenerateTheSurveyStudentStatus()
        {
            try
            {
                var status = await _asEduSurveyBaiKhaoSatSinhVienService.GetGenerateTheSurveyStudentStatus();
                return Ok(status);
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = "Không lấy được trạng thái", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không lấy được trạng thái", detailMessage = mainMessage });
            }
        }

        private void GenerateTheSurveyStudent(Guid surveyRoundId)
        {
            var scope = _scopeFactory.CreateScope();
            var eduContext = scope.ServiceProvider.GetRequiredService<EduDataContext>();
            var surveyContext = scope.ServiceProvider.GetRequiredService<SurveyContext>();
            var statusContext = scope.ServiceProvider.GetRequiredService<StatusContext>();

            var surveyRound = surveyContext.AsEduSurveyDotKhaoSat.FirstOrDefault(o => o.Id == surveyRoundId && o.Status == (int)SurveyRoundStatus.Active);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }

            var status = statusContext.AsStatusTableTask.FirstOrDefault(o => o.TableName == "AS_Edu_Survey_BaiKhaoSat_SinhVien");
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
                var idLyThuyet = new SqlParameter("@BaiKSLoai1", theoreticalSubjects.Id);

                var practicalSubjects = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.PracticalSubjects);
                if (practicalSubjects == null)
                {
                    throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn thực hành, thực tập, thí nghiệm trong đợt khảo sát này");
                }
                var idThucHanhThucTap = new SqlParameter("@BaiKSLoai2", practicalSubjects.Id);

                var theoreticalPracticalSubjects = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalPracticalSubjects);
                if (theoreticalPracticalSubjects == null)
                {
                    throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn lý thuyết + thực hành trong đợt khảo sát này");
                }
                var idLyThuyetThucHanh = new SqlParameter("@BaiKSLoai3", theoreticalPracticalSubjects.Id);

                var assignmentSubjects = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.AssignmentSubjects);
                if (assignmentSubjects == null)
                {
                    throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn đồ án trong đợt khảo sát này");
                }
                var idDoAn = new SqlParameter("@BaiKSLoai4", assignmentSubjects.Id);

                var defaultSubjects = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.DefaultSubjects);
                if (defaultSubjects == null)
                {
                    throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn không được phân loại trong đợt khảo sát này");
                }
                var idMacDinh = new SqlParameter("@BaiKSLoaiMacDinh", defaultSubjects.Id);

                _logger.LogInformation("Generate BaiKhaoSat_SinhVien is starting.");

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
                        if(lectureClassroom != null)
                        {
                            lecturer = eduContext.AsAcademyLecturer.FirstOrDefault(o => o.Code == lectureClassroom.LecturerCode);
                        }
                        else
                        {
                            lecturer = null;
                        }

                        classroom = eduContext.AsAcademyClassRoom.FirstOrDefault(o => o.Code == sc.ClassRoomCode);
                        if(classroom != null)
                        {
                            subject = eduContext.AsAcademySubject.FirstOrDefault(o => o.Code == classroom.SubjectCode);
                            if (subject != null)
                            {
                                subjectExtend = eduContext.AsAcademySubjectExtend.FirstOrDefault(o => o.Code == subject.Code);
                                baiKhaoSatId = defaultSubjects.Id; //mặc định
                                if (subjectExtend != null && subjectExtend.Type != null)
                                {
                                    if(subjectExtend.Type == (int)TheSurveyType.TheoreticalSubjects)
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
                    skip += take;
                }


                //surveyContext.Database.SetCommandTimeout(0);
                //surveyContext.Database.ExecuteSqlRaw("exec generate_the_survey_student @BaiKSLoai1, @BaiKSLoai2, @BaiKSLoai3, @BaiKSLoai4, @BaiKSLoaiMacDinh",
                //        idLyThuyet, idThucHanhThucTap, idLyThuyetThucHanh, idDoAn, idMacDinh);

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


        [HttpPost]
        [Authorize(Roles = "P_KhaoThi")]
        public async Task<IActionResult> GenerateTheSurveyStudent([Required(AllowEmptyStrings = false)] Guid? surveyRoundId)
        {
            try
            {
                //await _asEduSurveyBaiKhaoSatSinhVienService.GenerateTheSurveyStudent(surveyRoundId.Value);
                _monitorLoop.StartAction(() => {
                    GenerateTheSurveyStudent(surveyRoundId.Value);
                });
            }
            catch (TableBusyException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                var mainMessage = UtilsException.GetMainMessage(e);
                _logger.LogError(e, mainMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không tạo được bài khảo sát cho từng sinh viên", detailMessage = mainMessage });
            }
            return Ok();
        }
    }
}
