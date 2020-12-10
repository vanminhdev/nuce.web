using Microsoft.EntityFrameworkCore;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey;
using nuce.web.api.ViewModel.Survey.Graduate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements
{
    class AsEduSurveyGraduateBaiKhaoSatService : IAsEduSurveyGraduateBaiKhaoSatService
    {
        private readonly SurveyContext _context;

        public AsEduSurveyGraduateBaiKhaoSatService(SurveyContext context)
        {
            _context = context;
        }

        public async Task<PaginationModel<GraduateTheSurvey>> GetTheSurvey(GraduateTheSurveyFilter filter, int skip = 0, int take = 20)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            IQueryable<AsEduSurveyGraduateBaiKhaoSat> query = _context.AsEduSurveyGraduateBaiKhaoSat.Where(o => o.Status != (int)SurveyRoundStatus.Deleted);
            var join = query.Join(_context.AsEduSurveyGraduateSurveyRound, o => o.DotKhaoSatId, o => o.Id, (baikhaosat, dotkhaosat) => new { baikhaosat, dotkhaosat });

            var recordsTotal = join.Count();

            var recordsFiltered = join.Count();

            var querySkip = join
                .OrderBy(u => u.baikhaosat.Id)
                .Skip(skip).Take(take)
                .Select(o => new GraduateTheSurvey
                {
                    Id = o.baikhaosat.Id,
                    DotKhaoSatId = o.baikhaosat.DotKhaoSatId,
                    DeThiId = o.baikhaosat.DeThiId,
                    Name = o.baikhaosat.Name,
                    FromDate = o.baikhaosat.FromDate,
                    EndDate = o.baikhaosat.EndDate,
                    Description = o.baikhaosat.Description,
                    Note = o.baikhaosat.Note,
                    Status = o.baikhaosat.Status,
                    Type = o.baikhaosat.Type,
                    SurveyRoundName = o.dotkhaosat.Name
                });

            var data = await querySkip.ToListAsync();

            return new PaginationModel<GraduateTheSurvey>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }

        public async Task Create(GraduateTheSurveyCreate theSurvey)
        {
            var surveyRound = await _context.AsEduSurveyGraduateSurveyRound.FirstOrDefaultAsync(o => o.Id == theSurvey.DotKhaoSatId && o.Status == (int)TheSurveyStatus.New);
            if(surveyRound == null)
            {
                throw new RecordNotFoundException("Id đợt khảo sát không tồn tại");
            }

            var examQuestion = await _context.AsEduSurveyDeThi.FirstOrDefaultAsync(o => o.Id == theSurvey.DeThiId);
            if(examQuestion == null)
            {
                throw new RecordNotFoundException("Id đề thi không tồn tại");
            }

            var theActivedSurvey = await _context.AsEduSurveyGraduateBaiKhaoSat
                .FirstOrDefaultAsync(o => o.DotKhaoSatId == theSurvey.DotKhaoSatId && (o.Status == (int)TheSurveyStatus.New || o.Status == (int)TheSurveyStatus.Published));
            if (theActivedSurvey != null)
            {
                throw new InvalidDataException($"Đợt khảo sát \"{surveyRound.Name}\" đang có bài khảo sát còn hoạt động");
            }

            _context.AsEduSurveyGraduateBaiKhaoSat.Add(new AsEduSurveyGraduateBaiKhaoSat
            {
                Id = Guid.NewGuid(),
                DotKhaoSatId = theSurvey.DotKhaoSatId.Value,
                DeThiId = theSurvey.DeThiId.Value,
                Name = theSurvey.Name,
                NoiDungDeThi = examQuestion.NoiDungDeThi,
                DapAn = examQuestion.DapAn,
                FromDate = theSurvey.FromDate.Value,
                EndDate = theSurvey.EndDate.Value,
                Description = theSurvey.Description != null ? theSurvey.Description.Trim() : null,
                Note = theSurvey.Note != null ? theSurvey.Note.Trim() : null,
                Status = (int)TheSurveyStatus.New,
                Type = theSurvey.Type.Value,
            });
            await _context.SaveChangesAsync();
        }

        public async Task Update(string id, GraduateTheSurveyUpdate theSurvey)
        {
            var theSurveyUpdate = await _context.AsEduSurveyGraduateBaiKhaoSat.FirstOrDefaultAsync(o => o.Id.ToString() == id && o.Status != (int)TheSurveyStatus.Deleted);
            if (theSurveyUpdate == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát cần cập nhật");
            }

            if (theSurveyUpdate.Status != (int)TheSurveyStatus.New)
            {
                throw new InvalidDataException("Bài khảo sát không phải mới tạo, không thể sửa");
            }

            //đổi đợt khảo sát
            if (theSurvey.DotKhaoSatId != theSurveyUpdate.DotKhaoSatId)
            {
                var surveyRound = await _context.AsEduSurveyGraduateSurveyRound.FirstOrDefaultAsync(o => o.Id == theSurvey.DotKhaoSatId && o.Status == (int)TheSurveyStatus.New);
                if (surveyRound == null)
                {
                    throw new RecordNotFoundException("Id đợt khảo sát không tồn tại");
                }

                //có bài đang active rồi
                var theActivedSurvey = await _context.AsEduSurveyGraduateBaiKhaoSat.FirstOrDefaultAsync(o => o.DotKhaoSatId == theSurvey.DotKhaoSatId && o.Status == (int)TheSurveyStatus.New);
                if (theActivedSurvey != null)
                {
                    throw new InvalidDataException($"Đợt khảo sát \"{surveyRound.Name}\" đang có bài khảo sát còn hoạt động");
                }

                theSurveyUpdate.DotKhaoSatId = theSurvey.DotKhaoSatId.Value;
            }

            if(theSurveyUpdate.DeThiId != theSurvey.DeThiId)
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

            theSurveyUpdate.Name = theSurvey.Name.Trim();
            theSurveyUpdate.FromDate = theSurvey.FromDate.Value;
            theSurveyUpdate.EndDate = theSurvey.EndDate.Value;
            theSurveyUpdate.Description = theSurvey.Description != null ? theSurvey.Description.Trim() : null;
            theSurveyUpdate.Note = theSurvey.Note != null ? theSurvey.Note.Trim() : null;
            theSurveyUpdate.Type = theSurvey.Type.Value;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var theSurvey = await _context.AsEduSurveyGraduateBaiKhaoSat.FirstOrDefaultAsync(o => o.Id.ToString() == id && o.Status != (int)TheSurveyStatus.Deleted);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException();
            }

            if (theSurvey.Status == (int)TheSurveyStatus.New)
            {
                throw new InvalidDataException("Bài khảo sát còn hoạt động không thể xoá");
            }

            theSurvey.Status = (int)TheSurveyStatus.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<AsEduSurveyGraduateBaiKhaoSat> GetTheSurveyById(string id)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var theSurvey = await _context.AsEduSurveyGraduateBaiKhaoSat.FirstOrDefaultAsync(o => o.Id.ToString() == id && o.Status != (int)TheSurveyStatus.Deleted);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát");
            }
            return theSurvey;
        }
    }
}
