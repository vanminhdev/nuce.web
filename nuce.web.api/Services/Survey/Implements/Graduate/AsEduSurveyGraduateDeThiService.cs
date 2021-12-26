using Microsoft.EntityFrameworkCore;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Survey;
using nuce.web.api.Models.Survey.JsonData;
using nuce.web.api.Repositories.Ctsv.Implements;

using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Web;

namespace nuce.web.api.Services.Survey.Implements
{
    public class AsEduSurveyGraduateDeThiService
    {
        private readonly SurveyContext _surveyContext;

        public AsEduSurveyGraduateDeThiService(SurveyContext surveyContext)
        {
            _surveyContext = surveyContext;
        }

        public async Task<PaginationModel<ExamQuestions>> GetAll(ExamQuestionsFilter filter, int skip = 0, int take = 20)
        {
            _surveyContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var query = _surveyContext.AsEduSurveyGraduateDeThi.Where(q => q.Status != (int)QuestionStatus.Deleted);
            var recordsTotal = query.Count();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(u => u.Name.Contains(filter.Name));
            }

            var recordsFiltered = query.Count();

            var querySkip = query
                .Skip(skip).Take(take);

            var data = await querySkip
                .OrderBy(o => o.Code)
                .Select(o => new ExamQuestions
                {
                    Id = o.Id,
                    Code = o.Code,
                    Name = o.Name
                })
                .ToListAsync();

            return new PaginationModel<ExamQuestions>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }

        public async Task AddQuestion(Guid examQuestionId, string maCauHoi, int order)
        {
            var question = await _surveyContext.AsEduSurveyGraduateCauHoi.FirstOrDefaultAsync(q => q.Code == maCauHoi);
            if (question == null)
            {
                throw new RecordNotFoundException("Không tìm thấy câu hỏi");
            }

            if (question.ParentCode != null)
            {
                throw new InvalidInputDataException("Câu hỏi là câu hỏi con của một câu hỏi khác, vui lòng thêm bằng cách thêm câu hỏi cha");
            }

            if ( _surveyContext.AsEduSurveyGraduateCauTrucDe.FirstOrDefault(o => o.DeThiId == examQuestionId && o.CauHoiId == question.Id) != null)
            {
                throw new InvalidInputDataException("Câu hỏi đã tồn tại");
            }

            _surveyContext.AsEduSurveyGraduateCauTrucDe.Add(new AsEduSurveyGraduateCauTrucDe {
                Id = Guid.NewGuid(),
                CauHoiId = question.Id,
                DeThiId = examQuestionId,
                Order = order
            });
            await _surveyContext.SaveChangesAsync();
        }

        public async Task Create(ExamQuestionsCreate exam)
        {
            var examQuestions = new AsEduSurveyGraduateDeThi()
            {
                Id = Guid.NewGuid(),
                Code = $"{_surveyContext.AsEduSurveyGraduateDeThi.Count() + 1:00000}",
                Name = exam.Name,
                NoiDungDeThi = "",
                DapAn = "",
                Status = (int)ExamQuestionStatus.Active
            };
            _surveyContext.AsEduSurveyGraduateDeThi.Add(examQuestions);
            await _surveyContext.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var dethi = await _surveyContext.AsEduSurveyGraduateDeThi.FirstOrDefaultAsync(q => q.Id == id);
            if (dethi == null)
            {
                throw new RecordNotFoundException();
            }
            dethi.Status = (int)ExamQuestionStatus.Deleted;
            await _surveyContext.SaveChangesAsync();
        }

        public async Task DeleteQuestionFromStructure(string id)
        {
            var question = await _surveyContext.AsEduSurveyGraduateCauTrucDe.FirstOrDefaultAsync(o => o.Id.ToString() == id);
            if (question == null)
            {
                throw new RecordNotFoundException("Không tìm thấy câu hỏi");
            }
            _surveyContext.AsEduSurveyGraduateCauTrucDe.Remove(question);
            await _surveyContext.SaveChangesAsync();
        }

        public async Task GenerateExam(GenerateExam generateExam)
        {

            var examQuestion = await _surveyContext.AsEduSurveyGraduateDeThi.FindAsync(generateExam.ExamQuestionId.Value);
            if (examQuestion == null)
            {
                throw new RecordNotFoundException("Không tìm thấy phiếu khảo sát");
            }

            var cautruc = _surveyContext.AsEduSurveyGraduateCauTrucDe.Where(o => o.DeThiId == examQuestion.Id);

            foreach (var s in generateExam.SortResult)
            {
                var stru = cautruc.FirstOrDefault(o => o.CauHoiId == s.CauHoiId);
                if(stru != null)
                {
                    stru.Order = s.Order;
                }
            }
            _surveyContext.SaveChanges();

            var questions = _surveyContext.AsEduSurveyGraduateCauTrucDe
                .Join(_surveyContext.AsEduSurveyGraduateCauHoi.Where(ch => ch.Status == (int)QuestionStatus.Active), examStructure => examStructure.CauHoiId, question => question.Id,
                (examStructure, question) => new { examStructure, question })
                .Where(result => result.examStructure.DeThiId == generateExam.ExamQuestionId.Value)
                .OrderBy(result => result.examStructure.Order)
                .Select(result => new { payload = result.question, result.examStructure.Order });

            var questionAnswerJoin = await questions
                .GroupJoin(_surveyContext.AsEduSurveyGraduateDapAn.Where(da => da.Status == (int)AnswerStatus.Active), question => question.payload.Id, answer => answer.CauHoiId, (questionOrder, answer) => new { questionOrder, answer })
                .SelectMany(o => o.answer.DefaultIfEmpty(), (r, answer) => new { r.questionOrder, answer })
                .ToListAsync();

            //var test1 = questionAnswerJoin.Where(o => o.questionOrder.payload.Code == "00071").ToList();

            var questionAnswersGroup = questionAnswerJoin.GroupBy(r => r.questionOrder.payload, r => r.answer).ToList();

            var QuestionJsonData = new List<Models.Survey.JsonData.QuestionJson>();
            foreach (var itemGroup in questionAnswersGroup)
            {
                var test = itemGroup.Count();
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
                var answerOrder = itemGroup.OrderBy(o => o?.Order ?? 0);
                AsEduSurveyGraduateCauHoi answerChildQuestion = null;
                QuestionJson answerChildQuestionJson = null;
                
                questionJson.Answers = new List<AnswerJson>();
                foreach (var answer in answerOrder)
                {
                    if(answer == null)
                    {
                        break;
                    }
                    if(answer.ChildQuestionId != null) //câu hỏi con của đáp án
                    {
                        answerChildQuestion = await _surveyContext.AsEduSurveyGraduateCauHoi.FirstOrDefaultAsync(o => o.Id == answer.ChildQuestionId);
                        if (answerChildQuestion == null)
                        {
                            throw new RecordNotFoundException("Không tìm thấy câu hỏi con của đáp án có mã: " + answer.Code);
                        }

                        answerChildQuestionJson = new QuestionJson
                        {
                            Code =  $"{answer.Code}_{answerChildQuestion.Code}",
                            DifficultID = answerChildQuestion.DoKhoId,
                            Content = answerChildQuestion.Content,
                            Image = answerChildQuestion.Image,
                            Mark = (float)(answerChildQuestion?.Mark ?? 0),
                            Explain = answerChildQuestion.Explain,
                            Type = answerChildQuestion.Type,
                        };
                    }
                    else
                    {
                        answerChildQuestionJson = null;
                    }

                    //tồn tại đáp án rồi bỏ qua
                    if(questionJson.Answers.FirstOrDefault(o => o.Code == answer.Code) != null)
                    {
                        continue;
                    }

                    var answerJson = new Models.Survey.JsonData.AnswerJson
                    {
                        Code = answer.Code,
                        Content = answer.Content,
                        AnswerChildQuestion = answerChildQuestionJson
                    };

                    if(answer.ShowQuestion != null)
                    {
                        answerJson.ShowQuestion = JsonSerializer.Deserialize<List<string>>(answer.ShowQuestion);
                    }

                    if (answer.HideQuestion != null)
                    {
                        answerJson.HideQuestion = JsonSerializer.Deserialize<List<string>>(answer.HideQuestion);
                    }

                    questionJson.Answers.Add(answerJson);
                }

                if(itemGroup.Key.Type == QuestionType.GQ) //nếu có nhiều câu hỏi con
                {
                    var childs = _surveyContext.AsEduSurveyGraduateCauHoi.Where(o => o.ParentCode == itemGroup.Key.Code).OrderBy(o => o.Order).Select(o => new QuestionJson
                    {
                        Code = o.Code,
                        DifficultID = o.DoKhoId,
                        Content = o.Content,
                        Image = o.Image,
                        Mark = (float)(o.Mark ?? 0),
                        Explain = o.Explain,
                        Type = o.Type,
                    }).ToList();
                    questionJson.ChildQuestion = childs;
                }

                QuestionJsonData.Add(questionJson);
            }
            var options = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
            };
            var jsonString = JsonSerializer.Serialize(QuestionJsonData, options);
            examQuestion.NoiDungDeThi = jsonString;
            await _surveyContext.SaveChangesAsync();
        }

        public async Task<List<ExamQuestions>> GetAll()
        {
            _surveyContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return await _surveyContext.AsEduSurveyGraduateDeThi
                .Where(o => o.Status == (int)ExamQuestionStatus.Active)
                .Select(eq => new ExamQuestions
                {
                    Id = eq.Id,
                    Code = eq.Code,
                    Name = eq.Name
                }).ToListAsync();
        }


        public async Task<string> GetExamDetailJsonString(string examQuestionId)
        {
            _surveyContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var exam = await _surveyContext.AsEduSurveyGraduateDeThi.FirstOrDefaultAsync(exam => exam.Id.ToString() == examQuestionId);
            if(exam == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đề khảo sát");
            }
            return exam.NoiDungDeThi;
        }

        public async Task<List<ExamStructure>> GetExamStructure(string id)
        {
            _surveyContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return await _surveyContext.AsEduSurveyGraduateCauTrucDe
                .Join(_surveyContext.AsEduSurveyGraduateCauHoi, eq => eq.CauHoiId, q => q.Id, (examquestion, question) => new { examquestion, question })
                .Where(r => r.examquestion.DeThiId.ToString() == id)
                .OrderBy(r => r.examquestion.Order)
                .Select(r => new ExamStructure
                {
                    Id = r.examquestion.Id,
                    Code = r.question.Code,
                    Content = HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(r.question.Content)),
                    Type = r.question.Type,
                    Order = r.examquestion.Order, //thứ tự được lưu lại là bao nhiêu trong đề
                    QuestionId = r.question.Id
                })
                .ToListAsync();
        }
    }
}
