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
            if (await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => DateTime.Now < o.EndDate && o.Status != (int)SurveyRoundStatus.Deleted) != null)
            {
                throw new InvalidInputDataException("Một thời điểm chỉ có một đợt khảo sát được lên lịch, qua ngày kết thúc mới tạo được đợt mới");
            }

            //đóng tất cả các đợt khảo sát trước đang đóng hoặc quá ngày kết thúc
            var lstDotKs = await _context.AsEduSurveyDotKhaoSat.Where(o => (o.Status == (int)SurveyRoundStatus.Closed || DateTime.Now >= o.EndDate) && o.Status != (int)SurveyRoundStatus.Deleted).ToListAsync();
            foreach(var item in lstDotKs)
            {
                item.Status = (int)SurveyRoundStatus.End;
            }
            await _context.SaveChangesAsync();

            _context.AsEduSurveyDotKhaoSat.Add(new AsEduSurveyDotKhaoSat
            {
                Id = Guid.NewGuid(),
                Name = surveyRound.Name.Trim(),
                FromDate = surveyRound.FromDate.Value,
                EndDate = surveyRound.EndDate.Value,
                Description = surveyRound.Description?.Trim(),
                Note = surveyRound.Note?.Trim(),
                Status = (int)SurveyRoundStatus.New,
                Type = (int)SurveyRoundType.RatingTeachingQuality
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

            if(DateTime.Now >= surveyRoundUpdate.FromDate && DateTime.Now < surveyRoundUpdate.EndDate)
            {
                throw new InvalidInputDataException("Đợt khảo sát đang trong thời gian hoạt động không thể sửa");
            }

            surveyRoundUpdate.Name = surveyRound.Name.Trim();
            surveyRoundUpdate.FromDate = surveyRound.FromDate.Value;
            surveyRoundUpdate.EndDate = surveyRound.EndDate.Value;
            surveyRoundUpdate.Description = surveyRound.Description?.Trim();
            surveyRoundUpdate.Note = surveyRound.Note?.Trim();
            surveyRoundUpdate.Type = (int)SurveyRoundType.RatingTeachingQuality;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var surveyRound = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }

            if (DateTime.Now >= surveyRound.FromDate && DateTime.Now < surveyRound.EndDate)
            {
                throw new InvalidInputDataException("Đợt khảo sát đang mở");
            }

            if (await _context.AsEduSurveyBaiKhaoSat.Where(o => o.DotKhaoSatId == surveyRound.Id).FirstOrDefaultAsync() != null && surveyRound.Status != (int)SurveyRoundStatus.End)
            {
                throw new InvalidInputDataException("Đợt khảo sát đã có bài khảo sát không thể xoá");
            }

            if (surveyRound.Status == (int)SurveyRoundStatus.New && DateTime.Now < surveyRound.FromDate)
            {
                _context.AsEduSurveyDotKhaoSat.Remove(surveyRound);
            }
            else
            {
                surveyRound.Status = (int)SurveyRoundStatus.Deleted;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Open(Guid id)
        {
            var surveyRound = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException();
            }

            var lstOld = await _context.AsEduSurveyDotKhaoSat.Where(o => o.Status == (int)SurveyRoundStatus.Closed).Where(o => o.Id != surveyRound.Id).ToListAsync();
            foreach (var item in lstOld)
            {
                item.Status = (int)SurveyRoundStatus.End;
            }

            if (surveyRound.Status != (int)SurveyRoundStatus.Closed && surveyRound.Status != (int)SurveyRoundStatus.New)
            {
                throw new InvalidInputDataException("Đợt khảo sát không ở trạng thái đóng cửa hoặc mới tạo");
            }

            surveyRound.Status = (int)SurveyRoundStatus.Opened;
            await _context.SaveChangesAsync();
        }

        public async Task Close(Guid id)
        {
            var surveyRound = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException();
            }

            if (!(DateTime.Now >= surveyRound.FromDate && DateTime.Now < surveyRound.EndDate))
            {
                throw new InvalidInputDataException("Đợt khảo sát đang không mở không thể đóng");
            }
            surveyRound.Status = (int)SurveyRoundStatus.Closed;
            await _context.SaveChangesAsync();
        }

        public async Task AddEndDate(Guid id, DateTime endDate)
        {
            var surveyRound = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException();
            }

            if(endDate <= surveyRound.FromDate)
            {
                throw new InvalidInputDataException("Ngày kết thúc phải lớn hơn ngày bắt đầu");
            }

            if (endDate <= DateTime.Now)
            {
                throw new InvalidInputDataException("Ngày kết thúc phải lớn hơn ngày hiện tại");
            }

            surveyRound.EndDate = endDate;
            surveyRound.Status = (int)SurveyRoundStatus.Opened;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Đợt khảo sát vừa tạo
        /// </summary>
        /// <returns></returns>
        public async Task<List<AsEduSurveyDotKhaoSat>> GetSurveyRoundActive()
        {
            var surveyRounds = await _context.AsEduSurveyDotKhaoSat.Where(o => o.Status == (int)SurveyRoundStatus.New).ToListAsync();
            return surveyRounds;
        }

        public async Task<List<AsEduSurveyDotKhaoSat>> GetSurveyRoundClosedOrEnd()
        {
            var surveyRounds = await _context.AsEduSurveyDotKhaoSat.Where(o => o.Status == (int)SurveyRoundStatus.Closed || o.EndDate <= DateTime.Now || o.Status == (int)SurveyRoundStatus.End)
                .OrderByDescending(o => o.FromDate).ToListAsync();
            return surveyRounds;
        }
    }
}
