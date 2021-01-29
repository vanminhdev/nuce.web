using Microsoft.EntityFrameworkCore;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Graduate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements
{
    class AsEduSurveyGraduateDotKhaoSatService : IAsEduSurveyGraduateDotKhaoSatService
    {
        private readonly SurveyContext _context;

        public AsEduSurveyGraduateDotKhaoSatService(SurveyContext context)
        {
            _context = context;
        }

        public async Task<PaginationModel<AsEduSurveyGraduateSurveyRound>> GetSurveyRound(GraduateSurveyRoundFilter filter, int skip = 0, int take = 20)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            IQueryable<AsEduSurveyGraduateSurveyRound> query = _context.AsEduSurveyGraduateSurveyRound.Where(o => o.Status != (int)SurveyRoundStatus.Deleted);
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

            return new PaginationModel<AsEduSurveyGraduateSurveyRound>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }

        public async Task<AsEduSurveyGraduateSurveyRound> GetSurveyRoundById(Guid id)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var surveyRound = await _context.AsEduSurveyGraduateSurveyRound.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if(surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }
            return surveyRound;
        }

        public async Task Create(GraduateSurveyRoundCreate surveyRound)
        {
            if (await _context.AsEduSurveyGraduateSurveyRound.FirstOrDefaultAsync(o => DateTime.Now < o.EndDate && o.Status != (int)SurveyRoundStatus.Deleted) != null)
            {
                throw new InvalidInputDataException("Một thời điểm chỉ có một đợt khảo sát được lên lịch, qua ngày kết thúc mới tạo được đợt mới");
            }

            //đóng tất cả các đợt khảo sát trước đang đóng hoặc quá ngày kết thúc
            var lstDotKs = await _context.AsEduSurveyGraduateSurveyRound.Where(o => (o.Status == (int)SurveyRoundStatus.Closed || DateTime.Now >= o.EndDate) && o.Status != (int)SurveyRoundStatus.Deleted).ToListAsync();
            foreach (var item in lstDotKs)
            {
                item.Status = (int)SurveyRoundStatus.End;
            }
            await _context.SaveChangesAsync();

            _context.AsEduSurveyGraduateSurveyRound.Add(new AsEduSurveyGraduateSurveyRound
            {
                Id = Guid.NewGuid(),
                Name = surveyRound.Name.Trim(),
                FromDate = surveyRound.FromDate.Value,
                EndDate = surveyRound.EndDate.Value,
                Description = surveyRound.Description?.Trim(),
                Note = surveyRound.Note?.Trim(),
                Status = (int)SurveyRoundStatus.New
            });
            await _context.SaveChangesAsync();
        }

        public async Task Update(Guid id, GraduateSurveyRoundUpdate surveyRound)
        {
            var surveyRoundUpdate = await _context.AsEduSurveyGraduateSurveyRound.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if(surveyRoundUpdate == null)
            {
                throw new RecordNotFoundException();
            }

            if (DateTime.Now >= surveyRoundUpdate.FromDate && DateTime.Now < surveyRoundUpdate.EndDate)
            {
                throw new InvalidInputDataException("Đợt khảo sát đang trong thời gian hoạt động không thể sửa");
            }

            surveyRoundUpdate.Name = surveyRound.Name.Trim();
            surveyRoundUpdate.FromDate = surveyRound.FromDate.Value;
            surveyRoundUpdate.EndDate = surveyRound.EndDate.Value;
            surveyRoundUpdate.Description = surveyRound.Description?.Trim();
            surveyRoundUpdate.Note = surveyRound.Note?.Trim();
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var surveyRound = await _context.AsEduSurveyGraduateSurveyRound.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }

            if (DateTime.Now >= surveyRound.FromDate && DateTime.Now < surveyRound.EndDate)
            {
                throw new InvalidInputDataException("Đợt khảo sát đang trong thời gian hoạt động");
            }

            if (surveyRound.Status != (int)SurveyRoundStatus.End)
            {
                throw new InvalidInputDataException("Đợt khảo sát chưa kết thúc không thể xoá");
            }

            if (surveyRound.Status == (int)SurveyRoundStatus.New && DateTime.Now < surveyRound.FromDate)
            {
                surveyRound.Status = (int)SurveyRoundStatus.Deleted;
            }
            else
            {
                surveyRound.Status = (int)SurveyRoundStatus.Deleted;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Close(Guid id)
        {
            var surveyRound = await _context.AsEduSurveyGraduateSurveyRound.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)SurveyRoundStatus.Deleted);
            if (surveyRound == null)
            {
                throw new RecordNotFoundException();
            }
            surveyRound.Status = (int)SurveyRoundStatus.Closed;
            //kết thúc tất cả bài khảo sát là con của đợt này
            var baiKhaoSats = await _context.AsEduSurveyGraduateBaiKhaoSat.Where(o => o.DotKhaoSatId == surveyRound.Id).ToListAsync();
            foreach(var item in baiKhaoSats)
            {
                item.Status = (int)TheSurveyStatus.Deactive;
                //kết thúc tất cả bài khảo sát sinh viên là con của đợt khảo sát này
                await _context.Database.ExecuteSqlRawAsync($"update AS_Edu_Survey_Graduate_BaiKhaoSat_SinhVien set Status = {(int)SurveyStudentStatus.Close} where BaiKhaoSatID = '{item.Id}'");
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<AsEduSurveyGraduateSurveyRound>> GetSurveyRoundActive()
        {
            var surveyRounds = await _context.AsEduSurveyGraduateSurveyRound.Where(o => o.Status == (int)SurveyRoundStatus.New || o.Status == (int)SurveyRoundStatus.Closed || o.Status == (int)SurveyRoundStatus.Opened).ToListAsync();
            return surveyRounds;
        }

        public async Task<List<AsEduSurveyGraduateSurveyRound>> GetSurveyRoundClosedOrEnd()
        {
            var surveyRounds = await _context.AsEduSurveyGraduateSurveyRound.Where(o => o.Status == (int)SurveyRoundStatus.Closed || o.EndDate <= DateTime.Now || o.Status == (int)SurveyRoundStatus.End)
                .OrderByDescending(o => o.FromDate).ToListAsync();
            return surveyRounds;
        }
    }
}
