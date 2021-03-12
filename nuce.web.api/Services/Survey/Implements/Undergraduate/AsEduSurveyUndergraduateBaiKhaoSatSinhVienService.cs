using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.EduData;
using nuce.web.api.Models.Status;
using nuce.web.api.Models.Survey;
using nuce.web.api.Models.Survey.JsonData;
using nuce.web.api.Services.Shared;
using nuce.web.api.Services.Survey.Base;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Survey;
using nuce.web.api.ViewModel.Survey.Undergraduate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements
{
    class AsEduSurveyUndergraduateBaiKhaoSatSinhVienService : BaiKhaoSatSinhVienServiceBase, IAsEduSurveyUndergraduateBaiKhaoSatSinhVienService
    {
        private readonly ILogger<AsEduSurveyUndergraduateBaiKhaoSatSinhVienService> _logger;
        private readonly SurveyContext _context;
        private readonly EduDataContext _eduContext;
        private readonly StatusContext _statusContext;
        private readonly IConfiguration _configuration;
        private readonly FakerService _fakerService;

        public AsEduSurveyUndergraduateBaiKhaoSatSinhVienService(ILogger<AsEduSurveyUndergraduateBaiKhaoSatSinhVienService> logger, SurveyContext context, EduDataContext eduContext, StatusContext statusContext, IConfiguration configuration, FakerService fakerService)
        {
            _logger = logger;
            _context = context;
            _eduContext = eduContext;
            _statusContext = statusContext;
            _configuration = configuration;
            _fakerService = fakerService;
            _fakerService.NoTrackingIfFakeStudent(_context);
        }

        private async Task<bool> IsOpenSurveyRound(string studentCode)
        {
            var studentSurveyRound = await _context.AsEduSurveyUndergraduateStudent.FirstOrDefaultAsync(o => o.ExMasv == studentCode);
            if (studentSurveyRound == null)
            {
                throw new RecordNotFoundException("Sinh viên không tồn tại trong đợt khảo sát");
            }

            var surveyRound = await _context.AsEduSurveyUndergraduateSurveyRound.FirstOrDefaultAsync(o => o.Id == studentSurveyRound.DotKhaoSatId);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát của sinh viên");
            }

            if (surveyRound.Status == (int)SurveyRoundStatus.Opened)
            {
                return true;
            }
            return false;
        } 

        public async Task<string> GetTheSurveyContent(string studentCode, Guid theSurveyId)
        {
            if(!await IsOpenSurveyRound(studentCode))
            {
                throw new RecordNotFoundException("Đợt khảo sát không còn mở");
            }

            //không phải đang xác thực hoặc đã hoàn thành
            var baiKSSinhViens = await _context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien
                .Where(o => o.StudentCode == studentCode && (o.Status != (int)SurveyStudentStatus.RequestAuthorize && o.Status != (int)SurveyStudentStatus.Done)).ToListAsync();
            var baiKSsv = baiKSSinhViens.FirstOrDefault(o => o.BaiKhaoSatId == theSurveyId);
            if (baiKSsv == null)
            {
                throw new RecordNotFoundException("Sinh viên không có bài khảo sát này hoặc bài khảo sát đã hoàn thành");
            }

            if (baiKSsv.Status != (int)SurveyStudentStatus.Doing)
            {
                baiKSsv.Status = (int)SurveyStudentStatus.Doing;
                baiKSsv.NgayGioBatDau = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            var theSurvey = await _context.AsEduSurveyUndergraduateBaiKhaoSat.FirstOrDefaultAsync(o => o.Id == theSurveyId);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát");
            }

            return theSurvey.NoiDungDeThi;
        }

        public async Task<List<UndergraduateTheSurveyStudent>> GetTheSurvey(string studentCode)
        {
            if (!await IsOpenSurveyRound(studentCode))
            {
                throw new RecordNotFoundException("Đợt khảo sát không còn mở");
            }

            return await _context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien.Where(o => o.StudentCode == studentCode)
                .Join(_context.AsEduSurveyUndergraduateBaiKhaoSat, o => o.BaiKhaoSatId, o => o.Id, (baikssv, baiks) => new { baikssv, baiks })
                .Select(o => new UndergraduateTheSurveyStudent
                {
                    Id = o.baikssv.Id,
                    BaiKhaoSatId = o.baikssv.BaiKhaoSatId,
                    
                    Name = o.baiks.Name,
                    Type = o.baikssv.Type,
                    Status = o.baikssv.Status,
                })
                .ToListAsync();
        }

        public async Task<int> GetGenerateTheSurveyStudentStatus()
        {
            var status = await _statusContext.AsStatusTableTask.FirstOrDefaultAsync(o => o.TableName == "AS_Edu_Survey_Undergraduate_BaiKhaoSat_SinhVien");
            if (status == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bản ghi cập nhật trạng thái cho bảng bài khảo sát sinh viên");
            }
            return status.Status;
        }

        public async Task GenerateTheSurveyStudent(Guid theSurveyId)
        {
            var status = await _statusContext.AsStatusTableTask.FirstOrDefaultAsync(o => o.TableName == "AS_Edu_Survey_Undergraduate_BaiKhaoSat_SinhVien");
            if(status == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bản ghi cập nhật trạng thái cho bảng bài khảo sát sinh viên");
            }

            //bảng đang làm việc
            if (status.Status == (int)TableTaskStatus.Doing)
            {
                throw new TableBusyException("Đang tạo bài khảo sát cho từng sinh viên, thao tác bị huỷ");
            }

            status.Status = (int)TableTaskStatus.Doing;
            await _statusContext.SaveChangesAsync();

            var theSurvey = await _context.AsEduSurveyUndergraduateBaiKhaoSat.FirstOrDefaultAsync(o => o.Id == theSurveyId && (o.Status == (int)TheSurveyStatus.New || o.Status == (int)TheSurveyStatus.Published));
            if (theSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát");
            }
            theSurvey.Status = (int)TheSurveyStatus.Published;

            //đóng tất cả bài khảo sát trước đó
            var oldTheSurvey = await _context.AsEduSurveyUndergraduateBaiKhaoSat.Where(o => o.Id != theSurveyId && o.Status != (int)TheSurveyStatus.Deleted).ToListAsync();
            foreach(var item in oldTheSurvey)
            {
                item.Status = (int)TheSurveyStatus.Deactive;
            }

            try
            {
                IQueryable<AsEduSurveyUndergraduateStudent> query = _context.AsEduSurveyUndergraduateStudent.OrderBy(o => o.Id);
                var numStudent = query.Count();

                List<AsEduSurveyUndergraduateStudent> students;
                students = await query.ToListAsync();
                foreach (var student in students)
                {
                    if(string.IsNullOrWhiteSpace(student.ExMasv))
                    {
                        _logger.LogWarning($"du lieu dau vao sai ma sv de trong");
                        continue;
                    }
                    var baikssv = await _context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien.FirstOrDefaultAsync(o => o.StudentCode == student.ExMasv);
                    //nếu sinh viên đấy chưa có
                    if (baikssv == null)
                    {
                        _context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien.Add(new AsEduSurveyUndergraduateBaiKhaoSatSinhVien
                        {
                            Id = Guid.NewGuid(),
                            BaiKhaoSatId = theSurvey.Id,
                            Nganh = student.Tennganh,
                            ChuyenNganh = student.Tenchnga,
                            StudentCode = student.ExMasv,
                            DeThi = "",
                            BaiLam = "",
                            NgayGioBatDau = DateTime.Now,
                            NgayGioNopBai = DateTime.Now,
                            Status = (int)SurveyStudentStatus.DoNot,
                            Type = 1,
                        });
                    }
                    else //nếu có rồi và chưa làm bài thì cập nhật theo mẫu mới
                    {
                        //có rồi và chưa làm thì cập nhật bài ks mới
                        if(baikssv.Status != (int)SurveyStudentStatus.RequestAuthorize && baikssv.Status != (int)SurveyStudentStatus.Done)
                        {
                            baikssv.BaiKhaoSatId = theSurvey.Id;
                            baikssv.NgayGioBatDau = DateTime.Now;
                            baikssv.NgayGioNopBai = DateTime.Now;
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                status.Status = (int)TableTaskStatus.Done;
                status.IsSuccess = false;
                status.Message = UtilsException.GetMainMessage(e);
                await _statusContext.SaveChangesAsync();
                throw e;
            }
            await _context.SaveChangesAsync();

            status.Status = (int)TableTaskStatus.Done;
            status.IsSuccess = true;
            await _statusContext.SaveChangesAsync();
        }

        public async Task<string> GetSelectedAnswerAutoSave(Guid theSurveyId, string studentCode)
        {
            if (!await IsOpenSurveyRound(studentCode))
            {
                throw new RecordNotFoundException("Đợt khảo sát không còn mở");
            }

            var surveyStudent = await _context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien
                .FirstOrDefaultAsync(o => o.BaiKhaoSatId == theSurveyId && o.StudentCode == studentCode && o.Status != (int)SurveyStudentStatus.Done && o.Status != (int)SurveyStudentStatus.RequestAuthorize);
            if (surveyStudent == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát sinh viên");
            }
            return surveyStudent.BaiLam;
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
        public async Task AutoSave(Guid theSurveyId, string studentCode, string questionCode, string answerCode, string answerCodeInMulSelect, string answerContent, int? numStar, string city, bool isAnswerCodesAdd = true)
        {
            if (!await IsOpenSurveyRound(studentCode))
            {
                throw new RecordNotFoundException("Đợt khảo sát không còn mở");
            }

            //bài khảo sát sinh viên trạng thái không phải là đã hoàn thành
            var surveyStudent = await _context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien
                .FirstOrDefaultAsync(o => o.BaiKhaoSatId == theSurveyId && o.StudentCode == studentCode && o.Status != (int)SurveyStudentStatus.Done && o.Status != (int)SurveyStudentStatus.RequestAuthorize);
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
        /// Lưu bài làm sau khi hoàn thành
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task SaveSelectedAnswer(Guid theSurveyId, string studentCode, string ipAddress)
        {
            if (!await IsOpenSurveyRound(studentCode))
            {
                throw new RecordNotFoundException("Đợt khảo sát không còn mở");
            }

            var surveyStudent = await _context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien
                .FirstOrDefaultAsync(o => o.BaiKhaoSatId == theSurveyId && o.StudentCode == studentCode);
            
            if (surveyStudent == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài làm");
            }

            if (surveyStudent.Status == (int)SurveyStudentStatus.Done || surveyStudent.Status == (int)SurveyStudentStatus.RequestAuthorize)
            {
                throw new RecordNotFoundException("Bài khảo sát đã hoàn thành");
            }

            //kiểm tra đủ số câu hỏi bắt buộc
            var theSurvey = await _context.AsEduSurveyUndergraduateBaiKhaoSat.FirstOrDefaultAsync(o => o.Id == surveyStudent.BaiKhaoSatId);
            if (theSurvey == null)
            {                
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát");
            }

            var questions = JsonSerializer.Deserialize<List<QuestionJson>>(theSurvey.NoiDungDeThi);
            //var answerSave = JsonSerializer.Deserialize<List<SelectedAnswer>>(surveyStudent.BaiLam);

            //bỏ qua một số câu hỏi không visible

            //những câu hỏi có đáp án làm show hide câu hỏi khác
            //var questionShowHide = questions.Where(o => (o.Type == QuestionType.SC || o.Type == QuestionType.MC) && o.Answers != null && o.Answers.FirstOrDefault(a => a.HideQuestion != null || a.ShowQuestion != null) != null).ToList();


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
            //    else if (q.Type == QuestionType.StarRating && answerSave.FirstOrDefault(o => o.QuestionCode == q.Code && star.Contains(o.AnswerContent)) == null)
            //    {
            //        throw new InvalidDataException("Chưa trả lời đủ số câu hỏi");
            //    }
            //}

            surveyStudent.NgayGioNopBai = DateTime.Now;
            surveyStudent.Status = (int)SurveyStudentStatus.RequestAuthorize;
            surveyStudent.LogIp = ipAddress;
            await _context.SaveChangesAsync();
        }

        public async Task<string> Verification(string studentCode, VerificationStudent verification)
        {
            var student = await _context.AsEduSurveyUndergraduateStudent.FirstOrDefaultAsync(o => o.ExMasv == studentCode);
            if (student == null)
            {
                throw new RecordNotFoundException("Không tìm thấy sinh viên");
            }

            student.Email = verification.Email;
            student.Mobile = verification.Phone;
            student.Cmnd = verification.CMND;

            student.KeyAuthorize = Guid.NewGuid().ToString();
            await _context.SaveChangesAsync();

            return student.KeyAuthorize;
        }

        public async Task<bool> VerifyByToken(string studentCode, string token)
        {
            var student = await _context.AsEduSurveyUndergraduateStudent.FirstOrDefaultAsync(o => o.ExMasv == studentCode);
            if (student == null)
            {
                throw new RecordNotFoundException("Không tìm thấy sinh viên");
            }

            if (student.KeyAuthorize == token)
            {
                var baikssv = await _context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien.Where(o => o.StudentCode == studentCode && o.Status == (int)SurveyStudentStatus.RequestAuthorize).ToListAsync();
                if(baikssv.Count == 0)
                {
                    throw new RecordNotFoundException("Không tìm thấy bài khảo sát của sinh viên đang chờ xác thực hoặc bài khảo sát đã được xác thực");
                }

                foreach(var bai in baikssv)
                {
                    bai.Status = (int)SurveyStudentStatus.Done;
                }
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task SendEmailVerify(string email, string url)
        {
            HttpClient client = new HttpClient();
            var strContent = JsonSerializer.Serialize(new {
                emails = new[] { 
                    new { 
                        email,
                        data = new {
                            url
                        }
                    }   
                },
                template = 25,
                subject = "Xác thực hoàn thành bài khảo sát",
                email_identifier = "emails",
                datetime = DateTime.Now.ToString("dd-MM-yyyy hh:mm"),
                send_later_email = 0,
                timezone = 7
            });
            var content = new StringContent(strContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync(_configuration.GetValue<string>("ApiSendEmail"), content);
            if (!response.IsSuccessStatusCode)
            {
                var test = await response.Content.ReadAsStringAsync();
                _logger.LogError(test);
                throw new SendEmailException(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
