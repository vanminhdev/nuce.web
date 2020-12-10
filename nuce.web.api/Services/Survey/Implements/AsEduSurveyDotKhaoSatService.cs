using Microsoft.EntityFrameworkCore;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements
{
    class AsEduSurveyDotKhaoSatService : IAsEduSurveyDotKhaoSatService
    {
        private readonly SurveyContext _context;

        public AsEduSurveyDotKhaoSatService(SurveyContext context)
        {
            _context = context;
        }

        public async Task<PaginationModel<AsEduSurveyDotKhaoSat>> GetSurveyRound(SurveyRoundFilter filter, int skip = 0, int take = 20)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            IQueryable<AsEduSurveyDotKhaoSat> query = _context.AsEduSurveyDotKhaoSat.Where(o => o.Status != (int)SurveyRoundStatus.Deleted);
            var recordsTotal = query.Count();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(o => o.Name.Contains(filter.Name));
            }

            var recordsFiltered = query.Count();

            var querySkip = query
                .OrderBy(u => u.Id)
                .Skip(skip).Take(take);

            var data = await querySkip.ToListAsync();

            return new PaginationModel<AsEduSurveyDotKhaoSat>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }

        public async Task Create(SurveyRoundCreate surveyRound)
        {
            _context.AsEduSurveyDotKhaoSat.Add(new AsEduSurveyDotKhaoSat
            {
                Id = Guid.NewGuid(),
                Name = surveyRound.Name.Trim(),
                FromDate = surveyRound.FromDate.Value,
                EndDate = surveyRound.EndDate.Value,
                Description = surveyRound.Description.Trim(),
                Note = surveyRound.Note.Trim(),
                Status = (int)SurveyRoundStatus.Active,
                //Type = surveyRound.Type
                Type = (int)SurveyRoundType.RatingTeachingQuality
            });
            await _context.SaveChangesAsync();
        }

        public async Task Update(string id, SurveyRoundUpdate surveyRound)
        {
            var surveyRoundUpdate = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id.ToString() == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if(surveyRoundUpdate == null)
            {
                throw new RecordNotFoundException();
            }
            surveyRoundUpdate.Name = surveyRound.Name.Trim();
            surveyRoundUpdate.FromDate = surveyRound.FromDate.Value;
            surveyRoundUpdate.EndDate = surveyRound.EndDate.Value;
            surveyRoundUpdate.Description = surveyRound.Description.Trim();
            surveyRoundUpdate.Note = surveyRound.Note.Trim();
            //surveyRoundUpdate.Type = surveyRound.Type;
            surveyRoundUpdate.Type = (int)SurveyRoundType.RatingTeachingQuality;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var surveyRoundUpdate = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id.ToString() == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRoundUpdate == null)
            {
                throw new RecordNotFoundException();
            }
            surveyRoundUpdate.Status = (int)SurveyRoundStatus.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}
