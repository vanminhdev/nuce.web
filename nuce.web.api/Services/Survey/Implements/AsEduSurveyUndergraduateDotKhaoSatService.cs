using Microsoft.EntityFrameworkCore;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Undergraduate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements
{
    class AsEduSurveyUndergraduateDotKhaoSatService : IAsEduSurveyUndergraduateDotKhaoSatService
    {
        private readonly SurveyContext _context;

        public AsEduSurveyUndergraduateDotKhaoSatService(SurveyContext context)
        {
            _context = context;
        }

        public async Task<PaginationModel<AsEduSurveyUndergraduateSurveyRound>> GetSurveyRound(UndergraduateSurveyRoundFilter filter, int skip = 0, int take = 20)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            IQueryable<AsEduSurveyUndergraduateSurveyRound> query = _context.AsEduSurveyUndergraduateSurveyRound.Where(o => o.Status != (int)SurveyRoundStatus.Deleted);
            var recordsTotal = query.Count();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(o => o.Name.Contains(filter.Name));
            }

            var recordsFiltered = query.Count();

            var querySkip = query
                .OrderByDescending(u => u.FromDate)
                .ThenByDescending(u => u.Id)
                .Skip(skip).Take(take);

            var data = await querySkip.ToListAsync();

            return new PaginationModel<AsEduSurveyUndergraduateSurveyRound>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }

        public async Task<AsEduSurveyUndergraduateSurveyRound> GetSurveyRoundById(string id)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var surveyRound = await _context.AsEduSurveyUndergraduateSurveyRound.FirstOrDefaultAsync(o => o.Id.ToString() == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if(surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }
            return surveyRound;
        }

        public async Task Create(UndergraduateSurveyRoundCreate surveyRound)
        {
            if(await _context.AsEduSurveyUndergraduateSurveyRound.FirstOrDefaultAsync(o => o.Status == (int)SurveyRoundStatus.New) != null)
            {
                throw new InvalidDataException("Một thời điểm chỉ có một đợt khảo sát mới");
            }

            if (await _context.AsEduSurveyUndergraduateSurveyRound.FirstOrDefaultAsync(o => o.Status == (int)SurveyRoundStatus.Opened) != null)
            {
                throw new InvalidDataException("Đang có đợt khảo sát còn hoạt động không thể thêm mới");
            }

            _context.AsEduSurveyUndergraduateSurveyRound.Add(new AsEduSurveyUndergraduateSurveyRound
            {
                Id = Guid.NewGuid(),
                Name = surveyRound.Name.Trim(),
                FromDate = surveyRound.FromDate,
                EndDate = surveyRound.EndDate,
                Description = surveyRound.Description?.Trim(),
                Note = surveyRound.Note?.Trim(),
                Status = (int)SurveyRoundStatus.New
            });
            await _context.SaveChangesAsync();
        }

        public async Task Update(Guid id, UndergraduateSurveyRoundUpdate surveyRound)
        {
            var surveyRoundUpdate = await _context.AsEduSurveyUndergraduateSurveyRound.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if(surveyRoundUpdate == null)
            {
                throw new RecordNotFoundException();
            }

            if (surveyRoundUpdate.Status == (int)SurveyRoundStatus.Opened)
            {
                throw new InvalidDataException("Đợt khảo sát đang mở cửa, không thể sửa");
            }

            surveyRoundUpdate.Name = surveyRound.Name.Trim();
            surveyRoundUpdate.FromDate = surveyRound.FromDate;
            surveyRoundUpdate.EndDate = surveyRound.EndDate;
            surveyRoundUpdate.Description = surveyRound.Description?.Trim();
            surveyRoundUpdate.Note = surveyRound.Note?.Trim();
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var surveyRoundUpdate = await _context.AsEduSurveyUndergraduateSurveyRound.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRoundUpdate == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }

            if(surveyRoundUpdate.Status == (int)SurveyRoundStatus.Opened)
            {
                throw new InvalidDataException("Đợt khảo sát đang mở cửa");
            }

            if (surveyRoundUpdate.Status == (int)SurveyRoundStatus.Closed)
            {
                throw new InvalidDataException("Đợt khảo sát chưa kết thúc không thể xoá");
            }

            surveyRoundUpdate.Status = (int)SurveyRoundStatus.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task Open(Guid id)
        {
            var surveyRound = await _context.AsEduSurveyUndergraduateSurveyRound.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException();
            }

            var lstOld = await _context.AsEduSurveyUndergraduateSurveyRound.Where(o => o.Status == (int)SurveyRoundStatus.Closed).Where(o => o.Id != surveyRound.Id).ToListAsync();
            foreach (var item in lstOld)
            {
                item.Status = (int)SurveyRoundStatus.End;
            }

            if (surveyRound.Status != (int)SurveyRoundStatus.Closed && surveyRound.Status != (int)SurveyRoundStatus.New)
            {
                throw new InvalidDataException("Đợt khảo sát không ở trạng thái đóng cửa hoặc mới tạo");
            }

            surveyRound.Status = (int)SurveyRoundStatus.Opened;
            await _context.SaveChangesAsync();
        }

        public async Task Close(Guid id)
        {
            var surveyRound = await _context.AsEduSurveyUndergraduateSurveyRound.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException();
            }
            surveyRound.Status = (int)SurveyRoundStatus.Closed;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Đợt khảo sát mở cửa, đóng cửa, vừa thêm
        /// </summary>
        /// <returns></returns>
        public async Task<List<AsEduSurveyUndergraduateSurveyRound>> GetSurveyRoundActive()
        {
            var surveyRounds = await _context.AsEduSurveyUndergraduateSurveyRound.Where(o => o.Status == (int)SurveyRoundStatus.New || o.Status == (int)SurveyRoundStatus.Closed || o.Status == (int)SurveyRoundStatus.Opened).ToListAsync();
            return surveyRounds;
        }

        public async Task<List<AsEduSurveyUndergraduateSurveyRound>> GetAllSurveyRound()
        {
            var surveyRounds = await _context.AsEduSurveyUndergraduateSurveyRound.Where(o => o.Status != (int)SurveyRoundStatus.Deleted).ToListAsync();
            return surveyRounds;
        }
    }
}
