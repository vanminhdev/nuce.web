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

namespace nuce.web.api.Services.Survey.Implements
{
    public class AsEduSurveyBaiKhaoSatService
    {
        private readonly SurveyContext _context;

        public AsEduSurveyBaiKhaoSatService(SurveyContext context)
        {
            _context = context;
        }

        public async Task<PaginationModel<TheSurvey>> GetTheSurvey(TheSurveyFilter filter, int skip = 0, int take = 20)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            IQueryable<AsEduSurveyBaiKhaoSat> query = _context.AsEduSurveyBaiKhaoSat.Where(o => o.Status != (int)SurveyRoundStatus.Deleted);
            var join = query.Join(_context.AsEduSurveyDotKhaoSat, o => o.DotKhaoSatId, o => o.Id, (baikhaosat, dotkhaosat) => new { baikhaosat, dotkhaosat });

            var recordsTotal = join.Count();
            
            if(filter.DotKhaoSatId != null)
            {
                join = join.Where(o => o.dotkhaosat.Id == filter.DotKhaoSatId);
            }

            var recordsFiltered = join.Count();

            var querySkip = join
                .OrderByDescending(o => o.dotkhaosat.FromDate)
                .Skip(skip).Take(take)
                .Select(o => new TheSurvey
                {
                    Id = o.baikhaosat.Id,
                    DotKhaoSatId = o.baikhaosat.DotKhaoSatId,
                    DeThiId = o.baikhaosat.DeThiId,
                    Name = o.baikhaosat.Name,
                    Description = o.baikhaosat.Description,
                    Note = o.baikhaosat.Note,
                    Status = o.baikhaosat.Status,
                    Type = o.baikhaosat.Type,
                    SurveyRoundName = o.dotkhaosat.Name
                });

            var data = await querySkip.ToListAsync();

            return new PaginationModel<TheSurvey>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }

        public async Task Create(TheSurveyCreate theSurvey)
        {
            var surveyRound = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id == theSurvey.DotKhaoSatId);
            if(surveyRound == null)
            {
                throw new RecordNotFoundException("Id đợt khảo sát không tồn tại");
            }

            if (DateTime.Now >= surveyRound.FromDate)
            {
                throw new InvalidInputDataException("Đợt khảo sát đã bắt đầu không thể thêm bài khảo sát");
            }

            var examQuestion = await _context.AsEduSurveyDeThi.FirstOrDefaultAsync(o => o.Id == theSurvey.DeThiId);
            if(examQuestion == null)
            {
                throw new RecordNotFoundException("Id phiếu khảo sát không tồn tại");
            }

            var theActivedSurvey = await _context.AsEduSurveyBaiKhaoSat
                .FirstOrDefaultAsync(o => o.DotKhaoSatId == theSurvey.DotKhaoSatId && (o.Status == (int)TheSurveyStatus.New || o.Status == (int)TheSurveyStatus.Published) && o.Type == theSurvey.Type );
            if (theActivedSurvey != null)
            {
                throw new InvalidInputDataException($"Đợt khảo sát \"{surveyRound.Name}\" đang có bài khảo sát cùng loại còn hoạt động");
            }

            _context.AsEduSurveyBaiKhaoSat.Add(new AsEduSurveyBaiKhaoSat
            {
                Id = Guid.NewGuid(),
                DotKhaoSatId = theSurvey.DotKhaoSatId.Value,
                DeThiId = examQuestion.Id,
                Name = examQuestion.Name,
                NoiDungDeThi = examQuestion.NoiDungDeThi,
                DapAn = examQuestion.DapAn,
                Description = theSurvey.Description?.Trim(),
                Note = theSurvey.Note?.Trim(),
                Status = (int)TheSurveyStatus.New,
                Type = theSurvey.Type.Value,
            });
            await _context.SaveChangesAsync();
        }

        public async Task Update(Guid id, TheSurveyUpdate theSurvey)
        {
            var theSurveyUpdate = await _context.AsEduSurveyBaiKhaoSat.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)TheSurveyStatus.Deleted);
            if (theSurveyUpdate == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát cần cập nhật");
            }

            if (theSurveyUpdate.Status != (int)TheSurveyStatus.New)
            {
                throw new InvalidInputDataException("Bài khảo sát không phải mới tạo, không thể sửa");
            }

            //đổi đợt khảo sát
            if (theSurvey.DotKhaoSatId != theSurveyUpdate.DotKhaoSatId)
            {
                var surveyRound = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id == theSurvey.DotKhaoSatId && o.Status == (int)SurveyRoundStatus.New);
                if (surveyRound == null)
                {
                    throw new RecordNotFoundException("Id đợt khảo sát không tồn tại");
                }

                var theActivedSurvey = await _context.AsEduSurveyBaiKhaoSat
                .FirstOrDefaultAsync(o => o.DotKhaoSatId == theSurvey.DotKhaoSatId && (o.Status == (int)TheSurveyStatus.New || o.Status == (int)TheSurveyStatus.Published) && o.Type == theSurvey.Type);
                if (theActivedSurvey != null)
                {
                    throw new InvalidInputDataException($"Đợt khảo sát \"{surveyRound.Name}\" đang có bài khảo sát cùng loại còn hoạt động");
                }

                theSurveyUpdate.DotKhaoSatId = theSurvey.DotKhaoSatId.Value;
            }

            //đổi bài thi
            if(theSurvey.DeThiId != theSurvey.DeThiId)
            {
                var examQuestion = await _context.AsEduSurveyDeThi.FirstOrDefaultAsync(o => o.Id == theSurvey.DeThiId);
                if (examQuestion == null)
                {
                    throw new RecordNotFoundException("Id đề thi không tồn tại");
                }
                theSurveyUpdate.DeThiId = theSurvey.DeThiId.Value;
                theSurveyUpdate.Name = examQuestion.Name;
                theSurveyUpdate.NoiDungDeThi = examQuestion.NoiDungDeThi;
                theSurveyUpdate.DapAn = examQuestion.DapAn;
            }
            theSurveyUpdate.Description = theSurvey.Description?.Trim();
            theSurveyUpdate.Note = theSurvey.Note?.Trim();
            theSurveyUpdate.Type = theSurvey.Type.Value;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var theSurvey = await _context.AsEduSurveyBaiKhaoSat.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)TheSurveyStatus.Deleted);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException();
            }

            if(theSurvey.Status != (int)TheSurveyStatus.New)
            {
                throw new InvalidInputDataException("Bài khảo sát không phải là mới tạo, không thể xoá");
            }

            theSurvey.Status = (int)TheSurveyStatus.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<AsEduSurveyBaiKhaoSat> GetTheSurveyById(Guid id)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var theSurvey = await _context.AsEduSurveyBaiKhaoSat.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)TheSurveyStatus.Deleted);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát");
            }
            return theSurvey;
        }

        public async Task Deactive(Guid id)
        {
            var theSurvey = await _context.AsEduSurveyBaiKhaoSat.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)TheSurveyStatus.Deleted);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException();
            }
            theSurvey.Status = (int)TheSurveyStatus.Deactive;
            await _context.SaveChangesAsync();
        }
    }
}
