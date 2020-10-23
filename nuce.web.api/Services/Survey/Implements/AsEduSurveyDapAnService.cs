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

        public async Task Create(AnswerCreate answer)
        {
            var answerCreate = new AsEduSurveyDapAn();
            answerCreate.Id = Guid.NewGuid();
            answerCreate.DapAnId = answer.DapAnId.Value;
            try
            {
                answerCreate.CauHoiGid = Guid.Parse(answer.CauHoiGId);
            }
            catch
            {
                throw new InvalidDataException("'CauHoiGId' not valid");
            }
            answerCreate.CauHoiId = answer.CauHoiId.Value;
            answerCreate.SubQuestionId = -1;
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
            answer.Status = (int)AnswerStatus.Deactive;
            await _surveyContext.SaveChangesAsync();
        }

        public async Task<List<Answer>> GetByQuestionIdActiveStatus(string questionsId)
        {
            var list = await _surveyContext.AsEduSurveyDapAn.AsNoTracking()
                .OrderBy(a => a.Order)
                .Where(a => a.Status != (int)AnswerStatus.Deactive && a.CauHoiGid != null && a.CauHoiGid.ToString() == questionsId)
                .ToListAsync();
            return
                list.Select(a => new Answer
                {
                    Id = a.Id.ToString(),
                    DapAnId = a.DapAnId,
                    Content = HttpUtility.HtmlDecode(a.Content),
                    Order = a.Order,
                    CauHoiGId = a.CauHoiGid.ToString(),
                    CauHoiId = a.CauHoiId
                }).ToList();
        }

        public async Task<Answer> GetById(string id)
        {
            var answer = await _surveyContext.AsEduSurveyDapAn
                .AsNoTracking()
                .Where(a => a.Status != (int)AnswerStatus.Deactive && a.Id.ToString() == id)
                .FirstOrDefaultAsync();
            if (answer == null)
                return null;
            return new Answer
            {
                Id = answer.Id.ToString(),
                DapAnId = answer.DapAnId,
                Content = HttpUtility.HtmlDecode(answer.Content),
                Order = answer.Order,
                CauHoiGId = answer.CauHoiGid.ToString(),
                CauHoiId = answer.CauHoiId
            };
        }

        public async Task Update(string id, AnswerUpdate answer)
        {
            var answerUpdate = _surveyContext.AsEduSurveyDapAn
                .FirstOrDefault(a => a.Id.ToString() == id);
            if (answerUpdate == null)
            {
                throw new RecordNotFoundException();
            }
            answerUpdate.DapAnId = answer.DapAnId.Value;
            answerUpdate.Content = answer.Content;
            answerUpdate.Order = answer.Order;
            await _surveyContext.SaveChangesAsync();
        }
    }
}
