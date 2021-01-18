using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.EduData;
using nuce.web.api.Models.Status;
using nuce.web.api.Models.Survey;
using nuce.web.api.Models.Survey.JsonData;
using nuce.web.api.Services.Background;
using nuce.web.api.Services.Shared;
using nuce.web.api.Services.Survey.Base;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Survey;
using nuce.web.api.ViewModel.Survey.Normal.TheSurvey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements
{
    public class AsEduSurveyBaiKhaoSatSinhVienService : BaiKhaoSatSinhVienServiceBase, IAsEduSurveyBaiKhaoSatSinhVienService
    {
        private readonly ILogger<AsEduSurveyBaiKhaoSatSinhVienService> _logger;
        private readonly SurveyContext _context;
        private readonly EduDataContext _eduContext;
        private readonly StatusContext _statusContext;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly CancellationToken _cancellationToken;
        private readonly FakerService _fakerService;

        public AsEduSurveyBaiKhaoSatSinhVienService(ILogger<AsEduSurveyBaiKhaoSatSinhVienService> logger, SurveyContext context, StatusContext statusContext, EduDataContext eduDataContext,
            IBackgroundTaskQueue taskQueue, IHostApplicationLifetime applicationLifetime, FakerService fakerService)
        {
            _logger = logger;
            _context = context;
            _statusContext = statusContext;
            _eduContext = eduDataContext;

            _taskQueue = taskQueue;
            _cancellationToken = applicationLifetime.ApplicationStopping;
            _fakerService = fakerService;
            _fakerService.NoTrackingIfFakeStudent(_context);
            _fakerService.NoTrackingIfFakeStudent(_statusContext);
        }

        private async Task<bool> IsOpenSurveyRound()
        {
            //lấy đợt ks cuối cùng
            var surveyRound = await _context.AsEduSurveyDotKhaoSat.OrderByDescending(o => o.FromDate).FirstOrDefaultAsync();
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát của sinh viên");
            }
            if (DateTime.Now >= surveyRound.FromDate && DateTime.Now < surveyRound.EndDate && (surveyRound.Status == (int)SurveyRoundStatus.Opened || surveyRound.Status == (int)SurveyRoundStatus.New))
            {
                return true;
            }
            return false;
        }

        public async Task<TheSurveyContent> GetTheSurveyContent(string studentCode, string classroomCode, string nhhk, Guid theSurveyId)
        {
            if (!await IsOpenSurveyRound())
            {
                throw new RecordNotFoundException("Đợt khảo sát không còn mở");
            }

            var baiKSSinhViens = await _context.AsEduSurveyBaiKhaoSatSinhVien
                .Where(o => o.StudentCode == studentCode && o.ClassRoomCode == classroomCode && o.Nhhk == nhhk && (o.Status != (int)SurveyStudentStatus.Close || o.Status != (int)SurveyStudentStatus.Done)).ToListAsync();
            var baiKSsv = baiKSSinhViens.FirstOrDefault(o => o.BaiKhaoSatId == theSurveyId);
            if (baiKSsv == null)
            {
                throw new RecordNotFoundException("Sinh viên không có bài khảo sát này hoặc bài khảo sát đã kết thúc");
            }

            if(baiKSsv.Status != (int)SurveyStudentStatus.Doing)
            {
                baiKSsv.Status = (int)SurveyStudentStatus.Doing;
                baiKSsv.NgayGioBatDau = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            var theSurvey = await _context.AsEduSurveyBaiKhaoSat.FirstOrDefaultAsync(o => o.Id == theSurveyId);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát");
            }

            var surveyRound = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id == theSurvey.DotKhaoSatId);
            var classroom = await _eduContext.AsAcademyClassRoom.FirstOrDefaultAsync(o => o.Code == baiKSsv.ClassRoomCode);

            var result = new TheSurveyContent
            {
                ClassroomName = classroom?.ClassCode,
                LeturerName = baiKSsv.LecturerName,
                SurveyRoundName = surveyRound?.Name,
                NoiDungDeKhaoSat = theSurvey.NoiDungDeThi
            };

            return result;
        }

        public async Task<List<TheSurveyStudent>> GetTheSurvey(string studentCode)
        {
            if (!await IsOpenSurveyRound())
            {
                throw new RecordNotFoundException("Đợt khảo sát không còn mở");
            }

            return await _context.AsEduSurveyBaiKhaoSatSinhVien.Where(o => o.StudentCode == studentCode)
                .Select(o => new TheSurveyStudent
                {
                    Id = o.Id,
                    BaiKhaoSatId = o.BaiKhaoSatId,
                    StudentCode = o.SubjectCode,
                    LecturerCode = o.LecturerCode,
                    LecturerName = o.LecturerName,
                    ClassRoomCode = o.ClassRoomCode,
                    NHHK = o.Nhhk,
                    SubjectCode = o.SubjectCode,
                    SubjectName = o.SubjectName,
                    SubjectType = o.SubjectType,
                    DepartmentCode = o.DepartmentCode,
                    Type = o.Type,
                    Status = o.Status,
                })
                .ToListAsync();
        }

        public async Task<int> GetGenerateTheSurveyStudentStatus()
        {
            var status = await _statusContext.AsStatusTableTask.FirstOrDefaultAsync(o => o.TableName == "AS_Edu_Survey_BaiKhaoSat_SinhVien");
            if (status == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bản ghi cập nhật trạng thái cho bảng bài khảo sát sinh viên");
            }
            return status.Status;
        }

        public async Task<string> GetSelectedAnswerAutoSave(string studentCode, string classroomCode, string nhhk)
        {
            if (!await IsOpenSurveyRound())
            {
                throw new RecordNotFoundException("Đợt khảo sát không còn mở");
            }

            var studentSurvey = await _context.AsEduSurveyBaiKhaoSatSinhVien
                .FirstOrDefaultAsync(o => o.StudentCode == studentCode && o.ClassRoomCode == classroomCode && o.Nhhk == nhhk && (o.Status != (int)SurveyStudentStatus.Close || o.Status != (int)SurveyStudentStatus.Done));
            if (studentSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát sinh viên");
            }
            return studentSurvey.BaiLam;
        }

        /// <summary>
        /// Tự động lưu khi click
        /// </summary>
        /// <param name="id"></param>
        /// <param name="questionCode"></param>
        /// <param name="answerCode"></param>
        /// <param name="answerCodeInMulSelect"></param>
        /// <param name="isAnswerCodesAdd">là thêm đáp án lựa chọn nhiều hay bỏ đi</param>
        /// <param name="answerContent"></param>
        /// <returns></returns>
        public async Task AutoSave(string studentCode, string classroomCode, string nhhk, string questionCode, string answerCode, 
            string answerCodeInMulSelect, string answerContent, int? numStar, string city,
            bool isAnswerCodesAdd = true)
        {
            if (!await IsOpenSurveyRound())
            {
                throw new RecordNotFoundException("Đợt khảo sát không còn mở");
            }

            var surveyStudent = await _context.AsEduSurveyBaiKhaoSatSinhVien
                .FirstOrDefaultAsync(o => o.StudentCode == studentCode && o.ClassRoomCode == classroomCode && o.Nhhk == nhhk && o.Status != (int)SurveyStudentStatus.Done && o.Status != (int)SurveyStudentStatus.Close);
            if (surveyStudent == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài làm");
            }

            List<SelectedAnswer> selectedAnswer;
            try
            {
                selectedAnswer = JsonSerializer.Deserialize<List<SelectedAnswer>>(surveyStudent.BaiLam);
            }
            catch
            {
                selectedAnswer = new List<SelectedAnswer>();
            }

            surveyStudent.BaiLam = base.AutoSaveBaiLam(selectedAnswer, questionCode, answerCode, answerCodeInMulSelect, answerContent, numStar, city, isAnswerCodesAdd);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Lưu bài làm
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task SaveSelectedAnswer(string studentCode, string classroomCode, string nhhk, string ipAddress)
        {
            if (!await IsOpenSurveyRound())
            {
                throw new RecordNotFoundException("Đợt khảo sát không còn mở");
            }

            var surveyStudent = await _context.AsEduSurveyBaiKhaoSatSinhVien
                .FirstOrDefaultAsync(o => o.StudentCode == studentCode && o.ClassRoomCode == classroomCode && o.Nhhk == nhhk && o.Status != (int)SurveyStudentStatus.Done && o.Status != (int)SurveyStudentStatus.Close);
            if (surveyStudent == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài làm");
            }

            //kiểm tra đủ số câu hỏi bắt buộc
            var theSurvey = await _context.AsEduSurveyBaiKhaoSat.FirstOrDefaultAsync(o => o.Id == surveyStudent.BaiKhaoSatId);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát");
            }

            //var questions = JsonSerializer.Deserialize<List<QuestionJson>>(examQuestions.NoiDungDeThi);
            //var answerSave = JsonSerializer.Deserialize<List<SelectedAnswer>>(surveyStudent.BaiLam);

            //var test = questions.Where(o => o.Type == QuestionType.SC || o.Type == QuestionType.MC).ToList();
            //foreach (var q in questions)
            //{
            //    //Câu hỏi ngắn không bắt buộc
            //    if(q.Type == QuestionType.SC && answerSave.FirstOrDefault(o => o.QuestionCode == q.Code && o.AnswerCode != null) == null)
            //    {
            //        throw new InvalidDataException("Chưa trả lời đủ số câu hỏi");
            //    }
            //    else if (q.Type == QuestionType.MC && answerSave.FirstOrDefault(o => o.QuestionCode == q.Code && o.AnswerCodes != null && o.AnswerCodes.Count > 0) == null)
            //    {
            //        throw new InvalidDataException("Chưa trả lời đủ số câu hỏi");
            //    }
            //}

            surveyStudent.NgayGioNopBai = DateTime.Now;
            surveyStudent.Status = (int)SurveyStudentStatus.Done;
            surveyStudent.LogIp = ipAddress;
            await _context.SaveChangesAsync();
        }

        public async Task<Tuple<int, int>> CountGenerateTheSurveyStudent(Guid surveyRoundId)
        {
            var surveyRound = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id == surveyRoundId);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }
            IQueryable<AsEduSurveyBaiKhaoSatSinhVien> baiKsSv = _context.AsEduSurveyBaiKhaoSatSinhVien; 

            var queryTheSurvey = _context.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == surveyRound.Id);

            List<Guid> baiKsIds = new List<Guid>();
            var baiLyThuyet = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalSubjects);
            if(baiLyThuyet != null)
            {
                baiKsIds.Add(baiLyThuyet.Id);
            }
            var baiThucHanh = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.PracticalSubjects);
            if (baiThucHanh != null)
            {
                baiKsIds.Add(baiThucHanh.Id);
            }
            var baiLyThuyetThucHanh = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalPracticalSubjects);
            if (baiLyThuyetThucHanh != null)
            {
                baiKsIds.Add(baiLyThuyetThucHanh.Id);
            }
            var baiDoAn = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.AssignmentSubjects);
            if (baiDoAn != null)
            {
                baiKsIds.Add(baiDoAn.Id);
            }

            baiKsSv = baiKsSv.Where(o => baiKsIds.Contains(o.BaiKhaoSatId));

            var countStuClassroom = await _eduContext.AsAcademyStudentClassRoom.CountAsync();
            var countBaikssv = await baiKsSv.CountAsync();
            return new Tuple<int, int>(countBaikssv, countStuClassroom);
        }
    }
}
