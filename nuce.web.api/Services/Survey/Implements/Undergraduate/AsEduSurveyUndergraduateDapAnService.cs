﻿using Microsoft.EntityFrameworkCore;
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
    class AsEduSurveyUndergraduateDapAnService : IAsEduSurveyUndergraduateDapAnService
    {
        private readonly SurveyContext _surveyContext;

        public AsEduSurveyUndergraduateDapAnService(SurveyContext surveyContext)
        {
            _surveyContext = surveyContext;
        }

        public async Task Create(AnswerCreateModel answer)
        {
            var answerCreate = new AsEduSurveyUndergraduateDapAn();
            answerCreate.Id = Guid.NewGuid();
            answerCreate.Code = $"{_surveyContext.AsEduSurveyUndergraduateDapAn.Count() + 1:00000}";

            if (await _surveyContext.AsEduSurveyUndergraduateCauHoi.FirstOrDefaultAsync(o => o.Id.ToString() == answer.CauHoiId) == null)
            {
                throw new RecordNotFoundException("Không tìm thấy câu hỏi là cha của đáp án");
            }

            if(answer.ChildQuestionId != null)
            {
                var answerChildQuestion = await _surveyContext.AsEduSurveyUndergraduateCauHoi.FirstOrDefaultAsync(o => o.Id.ToString() == answer.ChildQuestionId);
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
                throw new InvalidInputDataException("Câu hỏi Id không hợp lệ");
            }
            answerCreate.CauHoiCode = answer.CauHoiCode;
            answerCreate.Content = answer.Content;
            answerCreate.IsCheck = false;
            answerCreate.Order = answer.Order;
            answerCreate.Status = (int)AnswerStatus.Active;

            _surveyContext.AsEduSurveyUndergraduateDapAn.Add(answerCreate);
            await _surveyContext.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var answer = await _surveyContext.AsEduSurveyUndergraduateDapAn.FirstOrDefaultAsync(a => a.Id.ToString() == id);
            if (answer == null)
            {
                throw new RecordNotFoundException();
            }
            answer.Status = (int)AnswerStatus.Deleted;
            await _surveyContext.SaveChangesAsync();
        }

        public async Task<List<AnswerModel>> GetByQuestionIdActiveStatus(string questionsId)
        {
            var list = await _surveyContext.AsEduSurveyUndergraduateDapAn.AsNoTracking()
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
                    ChildQuestionId = a.ChildQuestionId?.ToString(),
                    CauHoiCode = a.CauHoiCode
                }).ToList();
        }

        public async Task<AnswerModel> GetById(string id)
        {
            var answer = await _surveyContext.AsEduSurveyUndergraduateDapAn
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
                ChildQuestionId = answer.ChildQuestionId?.ToString(),
                CauHoiCode = answer.CauHoiCode
            };
        }

        public async Task Update(string id, AnswerUpdateModel answer)
        {
            var answerUpdate = _surveyContext.AsEduSurveyUndergraduateDapAn
                .FirstOrDefault(a => a.Id.ToString() == id);
            if (answerUpdate == null)
            {
                throw new RecordNotFoundException();
            }
            answerUpdate.Content = answer.Content;
            answerUpdate.Order = answer.Order;
            answerUpdate.ChildQuestionId = answer.childQuestionId;
            await _surveyContext.SaveChangesAsync();
        }
    }
}
