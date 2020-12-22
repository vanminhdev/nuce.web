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
                .OrderByDescending(u => u.FromDate)
                .Skip(skip).Take(take);

            var data = await querySkip.ToListAsync();

            return new PaginationModel<AsEduSurveyDotKhaoSat>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }

        public async Task<AsEduSurveyDotKhaoSat> GetSurveyRoundById(Guid id)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var surveyRound = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }
            return surveyRound;
        }

        public async Task Create(SurveyRoundCreate surveyRound)
        {
            var surveyRoundActive = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Status == (int)SurveyRoundStatus.New);
            if(surveyRoundActive != null)
            {
                throw new InvalidDataException("Một thời điểm chỉ có một đợt khảo sát được hoạt động");
            }

            _context.AsEduSurveyDotKhaoSat.Add(new AsEduSurveyDotKhaoSat
            {
                Id = Guid.NewGuid(),
                Name = surveyRound.Name.Trim(),
                FromDate = surveyRound.FromDate.Value,
                EndDate = surveyRound.EndDate.Value,
                Description = surveyRound.Description?.Trim(),
                Note = surveyRound.Note?.Trim(),
                Status = (int)SurveyRoundStatus.New,
                Type = surveyRound.Type
            });
            await _context.SaveChangesAsync();
        }

        public async Task Update(Guid id, SurveyRoundUpdate surveyRound)
        {
            var surveyRoundUpdate = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if(surveyRoundUpdate == null)
            {
                throw new RecordNotFoundException();
            }

            if (surveyRoundUpdate.Status != (int)SurveyRoundStatus.New)
            {
                throw new InvalidDataException("Đợt khảo sát không còn hoạt động, không thể sửa");
            }

            surveyRoundUpdate.Name = surveyRound.Name.Trim();
            surveyRoundUpdate.FromDate = surveyRound.FromDate.Value;
            surveyRoundUpdate.EndDate = surveyRound.EndDate.Value;
            surveyRoundUpdate.Description = surveyRound.Description?.Trim();
            surveyRoundUpdate.Note = surveyRound.Note?.Trim();
            surveyRoundUpdate.Type = surveyRound.Type;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var surveyRoundUpdate = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRoundUpdate == null)
            {
                throw new RecordNotFoundException();
            }

            if (surveyRoundUpdate.Status == (int)SurveyRoundStatus.New)
            {
                throw new InvalidDataException("Đợt khảo sát còn hoạt động không thể xoá");
            }

            surveyRoundUpdate.Status = (int)SurveyRoundStatus.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task Close(Guid id)
        {
            var surveyRound = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException();
            }
            surveyRound.Status = (int)SurveyRoundStatus.Closed;
            //kết thúc tất cả bài khảo sát là con của đợt này
            var baiKhaoSats = await _context.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == surveyRound.Id).ToListAsync();
            foreach (var item in baiKhaoSats)
            {
                item.Status = (int)TheSurveyStatus.Deactive;
                //kết thúc tất cả bài khảo sát sinh viên là con của đợt khảo sát này
                await _context.Database.ExecuteSqlRawAsync($"update AS_Edu_Survey_BaiKhaoSat_SinhVien set Status = {(int)SurveyStudentStatus.Close} where BaiKhaoSatID = '{item.Id}'");
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<AsEduSurveyDotKhaoSat>> GetSurveyRoundActive()
        {
            var surveyRounds = await _context.AsEduSurveyDotKhaoSat.Where(o => o.Status == (int)SurveyRoundStatus.New).ToListAsync();
            return surveyRounds;
        }

        public async Task<List<AsEduSurveyDotKhaoSat>> GetSurveyRoundEnd()
        {
            var surveyRounds = await _context.AsEduSurveyDotKhaoSat.Where(o => o.Status == (int)SurveyRoundStatus.Closed).ToListAsync();
            return surveyRounds;
        }
    }
}
