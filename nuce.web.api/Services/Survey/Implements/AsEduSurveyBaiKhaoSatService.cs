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
    class AsEduSurveyBaiKhaoSatService : IAsEduSurveyBaiKhaoSatService
    {
        private readonly SurveyContext _context;

        public AsEduSurveyBaiKhaoSatService(SurveyContext context)
        {
            _context = context;
        }

        public async Task<PaginationModel<AsEduSurveyBaiKhaoSat>> GetTheSurvey(TheSurveyFilter filter, int skip = 0, int take = 20)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            IQueryable<AsEduSurveyBaiKhaoSat> query = _context.AsEduSurveyBaiKhaoSat.Where(o => o.Status != (int)SurveyRoundStatus.Deleted);
            var recordsTotal = query.Count();

            var recordsFiltered = query.Count();

            var querySkip = query
                .OrderBy(u => u.Id)
                .Skip(skip).Take(take);

            var data = await querySkip.ToListAsync();

            return new PaginationModel<AsEduSurveyBaiKhaoSat>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }

        public async Task Create(TheSurveyCreate theSurvey)
        {
            var surveyRound = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id == theSurvey.DotKhaoSatId && o.Status == (int)TheSurveyStatus.Active);
            if(surveyRound == null)
            {
                throw new RecordNotFoundException("Id đợt khảo sát không tồn tại");
            }

            var examQuestion = await _context.AsEduSurveyDeThi.FirstOrDefaultAsync(o => o.Id == theSurvey.DeThiId);
            if(examQuestion == null)
            {
                throw new RecordNotFoundException("Id đề thi không tồn tại");
            }

            _context.AsEduSurveyBaiKhaoSat.Add(new AsEduSurveyBaiKhaoSat
            {
                Id = Guid.NewGuid(),
                DotKhaoSatId = theSurvey.DotKhaoSatId.Value,
                DeThiId = theSurvey.DeThiId.Value,
                NoiDungDeThi = examQuestion.NoiDungDeThi,
                DapAn = examQuestion.DapAn,
                FromDate = theSurvey.FromDate.Value,
                EndDate = theSurvey.EndDate.Value,
                Description = theSurvey.Description.Trim(),
                Note = theSurvey.Note.Trim(),
                Status = (int)TheSurveyStatus.Active,
                //Type = theSurvey.Type.Value
                Type = (int)TheSurveyType.TheoreticalSubjects
            });
            await _context.SaveChangesAsync();
        }

        public async Task Update(string id, TheSurveyUpdate theSurvey)
        {
            var theSurveyUpdate = await _context.AsEduSurveyBaiKhaoSat.FirstOrDefaultAsync(o => o.Id.ToString() == id && o.Status != (int)TheSurveyStatus.Deleted);
            if (theSurveyUpdate == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát cần cập nhật");
            }

            if(theSurvey.DotKhaoSatId != theSurveyUpdate.DotKhaoSatId)
            {
                var surveyRound = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id == theSurvey.DotKhaoSatId && o.Status == (int)TheSurveyStatus.Active);
                if (surveyRound == null)
                {
                    throw new RecordNotFoundException("Id đợt khảo sát không tồn tại");
                }
                theSurveyUpdate.DotKhaoSatId = theSurvey.DotKhaoSatId.Value;
            }

            if(theSurvey.DeThiId != theSurvey.DeThiId)
            {
                var examQuestion = await _context.AsEduSurveyDeThi.FirstOrDefaultAsync(o => o.Id == theSurvey.DeThiId);
                if (examQuestion == null)
                {
                    throw new RecordNotFoundException("Id đề thi không tồn tại");
                }
                theSurveyUpdate.DeThiId = theSurvey.DeThiId.Value;
                theSurveyUpdate.NoiDungDeThi = examQuestion.NoiDungDeThi;
                theSurveyUpdate.DapAn = examQuestion.DapAn;
            }

            theSurveyUpdate.FromDate = theSurvey.FromDate.Value;
            theSurveyUpdate.EndDate = theSurvey.EndDate.Value;
            theSurveyUpdate.Description = theSurvey.Description.Trim();
            theSurveyUpdate.Note = theSurvey.Note.Trim();
            //theSurveyUpdate.Type = theSurvey.Type.Value;
            theSurveyUpdate.Type = (int)TheSurveyType.TheoreticalSubjects;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var theSurvey = await _context.AsEduSurveyBaiKhaoSat.FirstOrDefaultAsync(o => o.Id.ToString() == id && o.Status != (int)TheSurveyStatus.Deleted);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException();
            }
            theSurvey.Status = (int)TheSurveyStatus.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}
