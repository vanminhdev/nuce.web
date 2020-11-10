using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Survey;
using nuce.web.api.Repositories.Ctsv.Implements;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace nuce.web.api.Services.Survey.Implements
{
    public class AsEduSurveyDeThiService : IAsEduSurveyDeThiService
    {
        private readonly SurveyContext _surveyContext;

        public AsEduSurveyDeThiService(SurveyContext surveyContext)
        {
            _surveyContext = surveyContext;
        }

        public async Task AddQuestion(string examQuestionId, string maCauHoi, int order)
        {
            var question = await _surveyContext.AsEduSurveyCauHoi.FirstOrDefaultAsync(q => q.Ma == maCauHoi);
            if(question == null)
            {
                throw new RecordNotFoundException();
            }
            _surveyContext.AsEduSurveyCauTrucDe.Add(new AsEduSurveyCauTrucDe {
                Id = Guid.NewGuid(),
                CauHoiId = question.Id,
                DeThi = Guid.Parse(examQuestionId),
                Count = order
            });
            await _surveyContext.SaveChangesAsync();
        }

        public async Task CreateExamQuestions(string code, string name)
        {
            var examQuestions = new AsEduSurveyDeThi()
            {
                Id = Guid.NewGuid(),
                Code = code,
                Name = name,
                NoiDungDeThi = "",
                DapAn = ""
            };
            _surveyContext.AsEduSurveyDeThi.Add(examQuestions);
            await _surveyContext.SaveChangesAsync();
        }

        public async Task GenerateExam(string examQuestionId)
        {
            var questions = _surveyContext.AsEduSurveyCauTrucDe
                .Join(_surveyContext.AsEduSurveyCauHoi, examStructure => examStructure.CauHoiId, question => question.Id,
                (examStructure, question) => new { examStructure, question })
                .Where(result => result.examStructure.DeThi.ToString() == examQuestionId)
                .OrderBy(result => result.examStructure.Count)
                .Select(result => result.question);

            var test = _surveyContext.AsEduSurveyDapAn
                .Join(_surveyContext.AsEduSurveyCauHoi, answer => answer.CauHoiGid, question => question.Id, (question, answer) => new { question, answer });

            var questionAnswerJoin = await _surveyContext.AsEduSurveyDapAn
                .Join(questions, answer => answer.CauHoiGid, question => question.Id,
                    (answer, question) => new
                    {
                        questionSelect = new
                        {
                            question.Id,
                            question.Ma,
                            question.Level,
                            question.Content,
                            question.Image,
                            question.Mark,
                            question.Type,
                            question.Explain
                        },
                        answerSelect = new
                        {
                            answer.Content,
                            answer.DapAnId,
                            answer.Order
                        }
                    })
                .OrderBy(r => r.answerSelect.Order)
                .ToListAsync();

            var questionAnswers = questionAnswerJoin
                .GroupBy(result => result.questionSelect, result => result.answerSelect)
                .ToList();
            var CauHoiJsonData = new List<Models.Survey.JsonData.CauHoi>();
            foreach (var question in questionAnswers)
            {
                var data = new Models.Survey.JsonData.CauHoi
                {
                    CauHoiID = question.Key.Ma,
                    DoKhoID = question.Key.Level,
                    Content = question.Key.Content,
                    Image = question.Key.Image,
                    Mark = (float)(question.Key?.Mark ?? 0),
                    Explain = question.Key.Explain,
                    Type = question.Key.Type,
                    SoCauTraLoi = question.Count(),
                };
                var indexAnswer = 1;
                foreach (var answer in question)
                {
                    switch (indexAnswer)
                    {
                        #region
                        case 1:
                            data.A1 = answer.Content;
                            data.M1 = answer.DapAnId.ToString();
                            break;
                        case 2:
                            data.A2 = answer.Content;
                            data.M2 = answer.DapAnId.ToString();
                            break;
                        case 3:
                            data.A3 = answer.Content;
                            data.M3 = answer.DapAnId.ToString();
                            break;
                        case 4:
                            data.A4 = answer.Content;
                            data.M4 = answer.DapAnId.ToString();
                            break;
                        case 5:
                            data.A5 = answer.Content;
                            data.M5 = answer.DapAnId.ToString();
                            break;
                        case 6:
                            data.A6 = answer.Content;
                            data.M6 = answer.DapAnId.ToString();
                            break;
                        case 7:
                            data.A7 = answer.Content;
                            data.M7 = answer.DapAnId.ToString();
                            break;
                        case 8:
                            data.A8 = answer.Content;
                            data.M8 = answer.DapAnId.ToString();
                            break;
                        case 9:
                            data.A9 = answer.Content;
                            data.M9 = answer.DapAnId.ToString();
                            break;
                        case 10:
                            data.A10 = answer.Content;
                            data.M10 = answer.DapAnId.ToString();
                            break;
                        case 11:
                            data.A11 = answer.Content;
                            data.M11 = answer.DapAnId.ToString();
                            break;
                        case 12:
                            data.A12 = answer.Content;
                            data.M12 = answer.DapAnId.ToString();
                            break;
                        case 13:
                            data.A13 = answer.Content;
                            data.M13 = answer.DapAnId.ToString();
                            break;
                        case 14:
                            data.A14 = answer.Content;
                            data.M14 = answer.DapAnId.ToString();
                            break;
                        case 15:
                            data.A15 = answer.Content;
                            data.M15 = answer.DapAnId.ToString();
                            break;
                        default: break;
                            #endregion
                    }
                    indexAnswer++;
                }
                CauHoiJsonData.Add(data);
            }

            var jsonString = JsonConvert.SerializeObject(CauHoiJsonData);

            var examQuestion = await _surveyContext.AsEduSurveyDeThi.FindAsync(Guid.Parse(examQuestionId));
            if(examQuestion == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đề thi");
            }
            examQuestion.NoiDungDeThi = jsonString;
            await _surveyContext.SaveChangesAsync();
        }

        public async Task<List<ExamQuestions>> GetAll()
        {
            _surveyContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return await _surveyContext.AsEduSurveyDeThi
                .Select(eq => new ExamQuestions
                {
                    Id = eq.Id.ToString(),
                    Code = eq.Code,
                    Name = eq.Name
                }).ToListAsync();
        }

        public async Task<string> GetExamDetailJsonString(string examQuestionId)
        {
            _surveyContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var exam = await _surveyContext.AsEduSurveyDeThi.FirstOrDefaultAsync(exam => exam.Id.ToString() == examQuestionId);
            if(exam == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đề khảo sát");
            }
            return exam.NoiDungDeThi;
        }

        public async Task<List<ExamStructure>> GetExamStructure(string id)
        {
            _surveyContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return await _surveyContext.AsEduSurveyCauTrucDe
                .Join(_surveyContext.AsEduSurveyCauHoi, eq => eq.CauHoiId, q => q.Id, (eq, q) => new { examQuestionId = eq.DeThi, count = eq.Count, question = q })
                .Where(r => r.examQuestionId.ToString() == id)
                .OrderBy(r => r.count)
                .Select(r => new ExamStructure
                {
                    Ma = r.question.Ma,
                    Content = HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(r.question.Content)),
                    Type = r.question.Type,
                    Order = r.count, //thứ tự được lưu lại là bao nhiêu trong đề
                    QuestionId = r.question.Id.ToString()
                })
                .ToListAsync();
        }


    }
}
