using Microsoft.AspNetCore.Mvc;
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
    class AsEduSurveyCauHoiService : IAsEduSurveyCauHoiService
    {
        private readonly SurveyContext _surveyContext;

        public AsEduSurveyCauHoiService(SurveyContext surveyContext)
        {
            _surveyContext = surveyContext;
        }

        public async Task<QuestionPaginationModel> GetAllActiveStatus(QuestionFilter filter, int skip = 0, int take = 20)
        {
            _surveyContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var query = _surveyContext.AsEduSurveyCauHoi.Where(q => q.Status != (int)QuestionStatus.Deactive);
            var recordsTotal = query.Count();

            if (!string.IsNullOrWhiteSpace(filter.Ma))
            {
                query = query.Where(u => u.Ma.Contains(filter.Ma));
            }
            if (!string.IsNullOrWhiteSpace(filter.Content))
            {
                filter.Content = HttpUtility.HtmlEncode(filter.Content);
                query = query.Where(u => u.Content.Contains(filter.Content));
            }
            if (!string.IsNullOrWhiteSpace(filter.Type))
            {
                query = query.Where(u => u.Type.Contains(filter.Type));
            }

            var recordsFiltered = query.Count();

            var querySkip = query
                .OrderBy(u => u.Order)
                .Skip(skip).Take(take);

            var data = await querySkip
                .Select(q => new QuestionModel
                {
                    Id = q.Id.ToString(),
                    Ma = q.Ma,
                    Content = HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(q.Content)),
                    Type = q.Type,
                    Order = q.Order
                })
                .ToListAsync();

            return new QuestionPaginationModel
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }

        public async Task<List<QuestionModel>> GetAllByStatus(QuestionStatus status)
        {
            var list = await _surveyContext.AsEduSurveyCauHoi.AsNoTracking()
                .OrderBy(q => q.Order)
                .Where(q => q.Status == (int)status).ToListAsync();
            return
                list.Select(q => new QuestionModel
                {
                    Ma = q.Ma,
                    Content = HttpUtility.HtmlDecode(q.Content),
                    Type = q.Type,
                    Order = q.Order
                }).ToList();
        }

        public async Task<QuestionModel> GetById(string Id)
        {
            var question = await _surveyContext.AsEduSurveyCauHoi
                .AsNoTracking()
                .Where(q => q.Status != (int)QuestionStatus.Deactive && q.Id.ToString() == Id)
                .FirstOrDefaultAsync();
            if (question == null)
                return null;
            return new QuestionModel {
                Id = Id,
                Ma = question.Ma,
                Content = HttpUtility.HtmlDecode(question.Content),
                Type = question.Type,
                Order = question.Order
            };
        }

        public async Task Create(QuestionCreateModel question)
        {
            var questionCreate = new AsEduSurveyCauHoi();
            try
            {
                questionCreate.CauHoiId = int.Parse(question.Ma);
            }
            catch
            {
                throw new InvalidDataException("Mã phải ở dạng số");
            }
            questionCreate.Id = Guid.NewGuid();
            questionCreate.BoCauHoiId = -1;
            questionCreate.DoKhoId = 1;
            questionCreate.Ma = question.Ma;
            questionCreate.Content = question.Content;
            questionCreate.InsertedDate = DateTime.Now;
            questionCreate.UpdatedDate = DateTime.Now;
            questionCreate.Order = question.Order.Value;
            questionCreate.Level = 1;
            questionCreate.Type = question.Type;
            questionCreate.Status = (int)QuestionStatus.Active;

            _surveyContext.AsEduSurveyCauHoi.Add(questionCreate);
            await _surveyContext.SaveChangesAsync();
        }

        public async Task Update(string id, QuestionUpdateModel question)
        {
            var questionUpdate = _surveyContext.AsEduSurveyCauHoi
                .FirstOrDefault(q => q.Id.ToString() == id);
            if(questionUpdate == null)
            {
                throw new RecordNotFoundException();
            }

            try
            {
                questionUpdate.CauHoiId = int.Parse(question.Ma);
            }
            catch
            {
                throw new InvalidDataException("Mã phải là ở dạng số");
            }
            questionUpdate.Ma = question.Ma;
            questionUpdate.Content = question.Content;
            questionUpdate.UpdatedDate = DateTime.Now;
            questionUpdate.Order = question.Order.Value;
            questionUpdate.Type = question.Type;
             await _surveyContext.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var question = await _surveyContext.AsEduSurveyCauHoi.FirstOrDefaultAsync(q => q.Id.ToString() == id);
            if(question == null)
            {
                throw new RecordNotFoundException();
            }
            question.Status = (int)QuestionStatus.Deactive;
            await _surveyContext.SaveChangesAsync();
        }
    }
}
