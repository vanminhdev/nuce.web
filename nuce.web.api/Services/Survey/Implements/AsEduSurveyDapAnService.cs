using Microsoft.EntityFrameworkCore;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace nuce.web.api.Services.Survey.Implements
{
    class AsEduSurveyDapAnService : IAsEduSurveyDapAnService
    {
        private readonly SurveyContext _surveyContext;

        public AsEduSurveyDapAnService(SurveyContext surveyContext)
        {
            _surveyContext = surveyContext;
        }

        public async Task Create(AnswerCreateModel answer)
        {
            var answerCreate = new AsEduSurveyDapAn();
            answerCreate.Id = Guid.NewGuid();
            answerCreate.Code = answer.Code;

            if(await _surveyContext.AsEduSurveyCauHoi.FirstOrDefaultAsync(o => o.Id.ToString() == answer.CauHoiId) == null)
            {
                throw new RecordNotFoundException("Không tìm thấy câu hỏi là cha của đáp án");
            }

            if(answer.ChildQuestionId != null)
            {
                var answerChildQuestion = await _surveyContext.AsEduSurveyCauHoi.FirstOrDefaultAsync(o => o.Id.ToString() == answer.ChildQuestionId);
                if (answerChildQuestion == null)
                {
                    throw new RecordNotFoundException("Không tìm thấy câu hỏi là câu hỏi con của đáp án");
                }
                answerCreate.ChildQuestionId = answerChildQuestion.Id;
            }

            try
            {
                answerCreate.CauHoiId = Guid.Parse(answer.CauHoiId);
            }
            catch
            {
                throw new InvalidDataException("Câu hỏi Id không hợp lệ");
            }
            answerCreate.CauHoiCode = answer.CauHoiCode;
            answerCreate.Content = answer.Content;
            answerCreate.IsCheck = false;
            answerCreate.Order = answer.Order;
            answerCreate.Status = (int)AnswerStatus.Active;

            _surveyContext.AsEduSurveyDapAn.Add(answerCreate);
            await _surveyContext.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var answer = await _surveyContext.AsEduSurveyDapAn.FirstOrDefaultAsync(a => a.Id.ToString() == id);
            if (answer == null)
            {
                throw new RecordNotFoundException();
            }
            answer.Status = (int)AnswerStatus.Deleted;
            await _surveyContext.SaveChangesAsync();
        }

        public async Task<List<AnswerModel>> GetByQuestionIdActiveStatus(string questionsId)
        {
            var list = await _surveyContext.AsEduSurveyDapAn.AsNoTracking()
                .OrderBy(a => a.Order)
                .Where(a => a.Status != (int)AnswerStatus.Deleted && a.CauHoiId.ToString() == questionsId)
                .ToListAsync();
            return
                list.Select(a => new AnswerModel
                {
                    Id = a.Id.ToString(),
                    Code = a.Code,
                    Content = HttpUtility.HtmlDecode(a.Content),
                    Order = a.Order,
                    CauHoiId = a.CauHoiId.ToString(),
                    CauHoiCode = a.CauHoiCode
                }).ToList();
        }

        public async Task<AnswerModel> GetById(string id)
        {
            var answer = await _surveyContext.AsEduSurveyDapAn
                .AsNoTracking()
                .Where(a => a.Status != (int)AnswerStatus.Deleted && a.Id.ToString() == id)
                .FirstOrDefaultAsync();
            if (answer == null)
                return null;
            return new AnswerModel
            {
                Id = answer.Id.ToString(),
                Code = answer.Code,
                Content = HttpUtility.HtmlDecode(answer.Content),
                Order = answer.Order,
                CauHoiId = answer.CauHoiId.ToString(),
                CauHoiCode = answer.CauHoiCode
            };
        }

        public async Task Update(string id, AnswerUpdateModel answer)
        {
            var answerUpdate = _surveyContext.AsEduSurveyDapAn
                .FirstOrDefault(a => a.Id.ToString() == id);
            if (answerUpdate == null)
            {
                throw new RecordNotFoundException();
            }
            answerUpdate.Code = answer.Code;
            answerUpdate.Content = answer.Content;
            answerUpdate.Order = answer.Order;
            await _surveyContext.SaveChangesAsync();
        }
    }
}
