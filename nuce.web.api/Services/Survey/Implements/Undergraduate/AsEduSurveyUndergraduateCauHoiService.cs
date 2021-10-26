using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Survey;

using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace nuce.web.api.Services.Survey.Implements
{
    public class AsEduSurveyUndergraduateCauHoiService
    {
        private readonly SurveyContext _surveyContext;

        public AsEduSurveyUndergraduateCauHoiService(SurveyContext surveyContext)
        {
            _surveyContext = surveyContext;
        }

        public async Task<PaginationModel<QuestionModel>> GetAllActiveStatus(QuestionFilter filter, int skip = 0, int take = 20)
        {
            _surveyContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var query = _surveyContext.AsEduSurveyUndergraduateCauHoi.Where(q => q.Status != (int)QuestionStatus.Deleted);
            var recordsTotal = query.Count();

            if (!string.IsNullOrWhiteSpace(filter.Code))
            {
                query = query.Where(u => u.Code == filter.Code);
            }
            if (!string.IsNullOrWhiteSpace(filter.Content))
            {
                query = query.Where(u => u.Content.Contains(filter.Content));
            }
            if (!string.IsNullOrWhiteSpace(filter.Type))
            {
                query = query.Where(u => u.Type == filter.Type);
            }

            var recordsFiltered = query.Count();

            var querySkip = query
                .OrderByDescending(u => u.Code)
                .Skip(skip).Take(take);

            var data = await querySkip
                .Select(q => new QuestionModel
                {
                    Id = q.Id,
                    Code = q.Code,
                    Content = HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(q.Content)),
                    Type = q.Type,
                    Order = q.Order,
                    ParentCode = q.ParentCode
                })
                .ToListAsync();

            return new PaginationModel<QuestionModel>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }

        public async Task<List<QuestionModel>> GetAllByStatus(QuestionStatus status)
        {
            var list = await _surveyContext.AsEduSurveyUndergraduateCauHoi.AsNoTracking()
                .OrderBy(q => q.Order)
                .Where(q => q.Status == (int)status).ToListAsync();
            return
                list.Select(q => new QuestionModel
                {
                    Code = q.Code,
                    Content = HttpUtility.HtmlDecode(q.Content),
                    Type = q.Type,
                    Order = q.Order
                }).ToList();
        }

        public async Task<QuestionModel> GetById(Guid Id)
        {
            _surveyContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var question = await _surveyContext.AsEduSurveyUndergraduateCauHoi
                .Where(q => q.Status != (int)QuestionStatus.Deleted && q.Id == Id)
                .FirstOrDefaultAsync();

            var childs = await _surveyContext.AsEduSurveyUndergraduateCauHoi.Where(o => o.ParentCode == question.Code && o.Status != (int)QuestionStatus.Deleted).ToListAsync();

            if (question == null)
            {
                throw new RecordNotFoundException();
            }
            return new QuestionModel {
                Id = Id,
                Code = question.Code,
                Content = HttpUtility.HtmlDecode(question.Content),
                Type = question.Type,
                Order = question.Order,
                ParentCode = question.ParentCode,
                QuestionChilds = childs.Select(o => new QuestionModel
                {
                    Id = o.Id,
                    Code = o.Code,
                    Content = HttpUtility.HtmlDecode(o.Content),
                    Type = o.Type,
                    Order = o.Order,
                    ParentCode = o.ParentCode
                }).ToList()
            };
        }

        //private void task()
        //{
        //    var questions = _surveyContext.AsEduSurveyUndergraduateCauHoi.OrderBy(o => o.Id).ToList();

        //    for(int i = 0; i < questions.Count; i++)
        //    {
        //        questions[i].Code = $"{i + 1:00000}";
        //    }

        //    _surveyContext.SaveChanges();

        //    var answers = _surveyContext.AsEduSurveyDapAn.OrderBy(o => o.Id).ToList();

        //    for (int i = 0; i < answers.Count; i++)
        //    {
        //        answers[i].Code = $"{i + 1:00000}";

        //        var q = _surveyContext.AsEduSurveyUndergraduateCauHoi.Find(answers[i].CauHoiId);

        //        if(q != null)
        //        {
        //            answers[i].CauHoiCode = q.Code;
        //        }
        //    }
        //}

        public async Task Create(QuestionCreateModel question)
        {
            var questionCreate = new AsEduSurveyUndergraduateCauHoi
            {
                Id = Guid.NewGuid(),
                BoCauHoiId = -1,
                DoKhoId = 1,
                Code = $"{_surveyContext.AsEduSurveyUndergraduateCauHoi.Count() + 1:00000}",
                Content = question.Content,
                InsertedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Order = question.Order.Value,
                Level = 1,
                Type = question.Type,
                Status = (int)QuestionStatus.Active
            };
            _surveyContext.AsEduSurveyUndergraduateCauHoi.Add(questionCreate);
            if (question.Type == QuestionType.GQ && question.QuestionChildCodes != null)
            {
                foreach (var code in question.QuestionChildCodes)
                {
                    var questionChild = await _surveyContext.AsEduSurveyCauHoi.FirstOrDefaultAsync(o => o.Code == code && o.Status != (int)QuestionStatus.Deleted);
                    if (questionChild != null)
                    {
                        questionChild.ParentCode = questionCreate.Code;
                    }
                }
            }
            await _surveyContext.SaveChangesAsync();
        }

        public async Task Update(Guid id, QuestionUpdateModel question)
        {
            var questionUpdate = _surveyContext.AsEduSurveyUndergraduateCauHoi
                .FirstOrDefault(q => q.Id == id);
            if(questionUpdate == null)
            {
                throw new RecordNotFoundException();
            }
            //questionUpdate.Code = question.Code; //không cập nhật mã
            questionUpdate.Content = question.Content;
            questionUpdate.UpdatedDate = DateTime.Now;
            questionUpdate.Order = question.Order.Value;
            questionUpdate.Type = question.Type;

            if(question.QuestionChildCodes != null)
            {
                //xoá cái cũ
                var oldChilds = await _surveyContext.AsEduSurveyUndergraduateCauHoi.Where(o => o.ParentCode == questionUpdate.Code && o.Status != (int)QuestionStatus.Deleted).ToListAsync();
                foreach (var item in oldChilds)
                {
                    item.ParentCode = null;
                }

                //thêm lại
                foreach (var code in question.QuestionChildCodes)
                {
                    var questionChild = await _surveyContext.AsEduSurveyUndergraduateCauHoi.FirstOrDefaultAsync(o => o.Code == code && o.Status != (int)QuestionStatus.Deleted);
                    if(questionChild != null)
                    {
                        questionChild.ParentCode = questionUpdate.Code;
                    }
                }
            }
            await _surveyContext.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var question = await _surveyContext.AsEduSurveyUndergraduateCauHoi.FirstOrDefaultAsync(q => q.Id == id);
            if(question == null)
            {
                throw new RecordNotFoundException();
            }
            question.Status = (int)QuestionStatus.Deleted;
            await _surveyContext.SaveChangesAsync();
        }
    }
}
