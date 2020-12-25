﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<PaginationModel<TheSurvey>> GetTheSurvey(TheSurveyFilter filter, int skip = 0, int take = 20)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            IQueryable<AsEduSurveyBaiKhaoSat> query = _context.AsEduSurveyBaiKhaoSat.Where(o => o.Status != (int)SurveyRoundStatus.Deleted);
            var join = query.Join(_context.AsEduSurveyDotKhaoSat, o => o.DotKhaoSatId, o => o.Id, (baikhaosat, dotkhaosat) => new { baikhaosat, dotkhaosat });

            var recordsTotal = join.Count();

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
                    FromDate = o.baikhaosat.FromDate,
                    EndDate = o.baikhaosat.EndDate,
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
            var surveyRound = await _context.AsEduSurveyDotKhaoSat.FirstOrDefaultAsync(o => o.Id == theSurvey.DotKhaoSatId && o.Status == (int)SurveyRoundStatus.New);
            if(surveyRound == null)
            {
                throw new RecordNotFoundException("Id đợt khảo sát không tồn tại");
            }

            var examQuestion = await _context.AsEduSurveyDeThi.FirstOrDefaultAsync(o => o.Id == theSurvey.DeThiId);
            if(examQuestion == null)
            {
                throw new RecordNotFoundException("Id đề thi không tồn tại");
            }

            var theActivedSurvey = await _context.AsEduSurveyBaiKhaoSat
                .FirstOrDefaultAsync(o => o.DotKhaoSatId == theSurvey.DotKhaoSatId && (o.Status == (int)TheSurveyStatus.New || o.Status == (int)TheSurveyStatus.Published) && o.Type == theSurvey.Type );
            if (theActivedSurvey != null)
            {
                throw new InvalidDataException($"Đợt khảo sát \"{surveyRound.Name}\" đang có bài khảo sát cùng loại còn hoạt động");
            }

            _context.AsEduSurveyBaiKhaoSat.Add(new AsEduSurveyBaiKhaoSat
            {
                Id = Guid.NewGuid(),
                DotKhaoSatId = theSurvey.DotKhaoSatId.Value,
                DeThiId = theSurvey.DeThiId.Value,
                Name = theSurvey.Name.Trim(),
                NoiDungDeThi = examQuestion.NoiDungDeThi,
                DapAn = examQuestion.DapAn,
                FromDate = theSurvey.FromDate.Value,
                EndDate = theSurvey.EndDate.Value,
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
                throw new InvalidDataException("Bài khảo sát không phải mới tạo, không thể sửa");
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
                    throw new InvalidDataException($"Đợt khảo sát \"{surveyRound.Name}\" đang có bài khảo sát cùng loại còn hoạt động");
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
                theSurveyUpdate.NoiDungDeThi = examQuestion.NoiDungDeThi;
                theSurveyUpdate.DapAn = examQuestion.DapAn;
            }
            theSurveyUpdate.Name = theSurvey.Name.Trim();
            theSurveyUpdate.FromDate = theSurvey.FromDate.Value;
            theSurveyUpdate.EndDate = theSurvey.EndDate.Value;
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
            await _context.Database.ExecuteSqlRawAsync($"update AS_Edu_Survey_BaiKhaoSat_SinhVien set Status = {(int)SurveyStudentStatus.Close} where BaiKhaoSatID = '{theSurvey.Id}'");
            await _context.SaveChangesAsync();
        }
    }
}
