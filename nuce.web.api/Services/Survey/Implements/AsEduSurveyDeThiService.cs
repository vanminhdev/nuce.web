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
    class AsEduSurveyDeThiService : IAsEduSurveyDeThiService
    {
        private readonly SurveyContext _surveyContext;

        public AsEduSurveyDeThiService(SurveyContext surveyContext)
        {
            _surveyContext = surveyContext;
        }

        public async Task AddQuestion(string examQuestionId, string maCauHoi, int order)
        {
            var question = await _surveyContext.AsEduSurveyCauHoi.FirstOrDefaultAsync(q => q.Code == maCauHoi);
            if(question == null)
            {
                throw new RecordNotFoundException();
            }
            _surveyContext.AsEduSurveyCauTrucDe.Add(new AsEduSurveyCauTrucDe {
                Id = Guid.NewGuid(),
                CauHoiId = question.Id,
                DeThiId = Guid.Parse(examQuestionId),
                Order = order
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
                .Where(result => result.examStructure.DeThiId.ToString() == examQuestionId)
                .OrderBy(result => result.examStructure.Order)
                .Select(result => new { payload = result.question, result.examStructure.Order });

            var questionAnswerJoin = await questions
                .GroupJoin(_surveyContext.AsEduSurveyDapAn, question => question.payload.Id, answer => answer.CauHoiId, (questionOrder, answer) => new { questionOrder, answer })
                .SelectMany(o => o.answer.DefaultIfEmpty(), (r, answer) => new { r.questionOrder, answer })
                .ToListAsync();

            var questionAnswersGroup = questionAnswerJoin.GroupBy(r => r.questionOrder.payload, r => r.answer).ToList();

            var QuestionJsonData = new List<Models.Survey.JsonData.QuestionJson>();
            foreach (var itemGroup in questionAnswersGroup)
            {
                var questionJson = new Models.Survey.JsonData.QuestionJson
                {
                    Code = itemGroup.Key.Code,
                    DifficultID = itemGroup.Key.DoKhoId,
                    Content = itemGroup.Key.Content,
                    Image = itemGroup.Key.Image,
                    Mark = (float)(itemGroup.Key?.Mark ?? 0),
                    Explain = itemGroup.Key.Explain,
                    Type = itemGroup.Key.Type,
                };
                questionJson.Answers = new List<Models.Survey.JsonData.AnswerJson>();
                var answerOrder = itemGroup.OrderBy(o => o?.Order ?? 0);
                foreach (var answer in answerOrder)
                {
                    if(answer == null)
                    {
                        break;
                    }

                    questionJson.Answers.Add(new Models.Survey.JsonData.AnswerJson
                    {
                        Code = answer.Code,
                        Content = answer.Content
                    });
                }

                if(itemGroup.Key.ParentCode != null) //thêm con vào cha
                {
                    var parent = QuestionJsonData.FirstOrDefault(o => o.Code == itemGroup.Key.ParentCode);
                    if(parent != null)
                    {
                        if(parent.ChildQuestion == null)
                        {
                            parent.ChildQuestion = new List<Models.Survey.JsonData.QuestionJson>() { questionJson };
                        }
                        else
                        {
                            parent.ChildQuestion.Add(questionJson);
                        }
                    }
                }
                else //thêm mới
                {
                    QuestionJsonData.Add(questionJson);
                }
            }

            var jsonString = JsonConvert.SerializeObject(QuestionJsonData);

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
                .Join(_surveyContext.AsEduSurveyCauHoi, eq => eq.CauHoiId, q => q.Id, (eq, q) => new { examQuestionId = eq.DeThiId, order = eq.Order, question = q })
                .Where(r => r.examQuestionId.ToString() == id)
                .OrderBy(r => r.order)
                .Select(r => new ExamStructure
                {
                    Code = r.question.Code,
                    Content = HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(r.question.Content)),
                    Type = r.question.Type,
                    Order = r.order, //thứ tự được lưu lại là bao nhiêu trong đề
                    QuestionId = r.question.Id.ToString()
                })
                .ToListAsync();
        }


    }
}
