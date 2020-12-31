using Microsoft.EntityFrameworkCore;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey;
using nuce.web.api.ViewModel.Survey.Undergraduate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements
{
    class AsEduSurveyUndergraduateBaiKhaoSatService : IAsEduSurveyUndergraduateBaiKhaoSatService
    {
        private readonly SurveyContext _context;

        public AsEduSurveyUndergraduateBaiKhaoSatService(SurveyContext context)
        {
            _context = context;
        }

        public async Task<PaginationModel<UndergraduateTheSurvey>> GetTheSurvey(UndergraduateTheSurveyFilter filter, int skip = 0, int take = 20)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            IQueryable<AsEduSurveyUndergraduateBaiKhaoSat> query = _context.AsEduSurveyUndergraduateBaiKhaoSat.Where(o => o.Status != (int)TheSurveyStatus.Deleted);

            var recordsTotal = query.Count();

            var recordsFiltered = query.Count();

            var querySkip = query
                .OrderByDescending(u => u.Id)
                .Skip(skip).Take(take)
                .Select(o => new UndergraduateTheSurvey
                {
                    Id = o.Id,
                    DeThiId = o.DeThiId,
                    Name = o.Name,
                    Description = o.Description,
                    Note = o.Note,
                    Status = o.Status.Value
                });

            var data = await querySkip.ToListAsync();

            return new PaginationModel<UndergraduateTheSurvey>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }

        public async Task Create(UndergraduateTheSurveyCreate theSurvey)
        {
            var examQuestion = await _context.AsEduSurveyDeThi.FirstOrDefaultAsync(o => o.Id == theSurvey.DeThiId);
            if(examQuestion == null)
            {
                throw new RecordNotFoundException("Id phiếu khảo sát không tồn tại");
            }

            _context.AsEduSurveyUndergraduateBaiKhaoSat.Add(new AsEduSurveyUndergraduateBaiKhaoSat
            {
                Id = Guid.NewGuid(),
                DeThiId = theSurvey.DeThiId.Value,
                NoiDungDeThi = examQuestion.NoiDungDeThi,
                Name = examQuestion.Name,
                Description = theSurvey.Description?.Trim(),
                Note = theSurvey.Note?.Trim(),
                Status = (int)TheSurveyStatus.New
            });
            await _context.SaveChangesAsync();
        }

        public async Task Update(Guid id, UndergraduateTheSurveyUpdate theSurvey)
        {
            var theSurveyUpdate = await _context.AsEduSurveyUndergraduateBaiKhaoSat.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)TheSurveyStatus.Deleted);
            if (theSurveyUpdate == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát cần cập nhật");
            }

            if (theSurveyUpdate.Status != (int)TheSurveyStatus.New)
            {
                throw new InvalidDataException("Bài khảo sát không phải mới tạo, không thể sửa");
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
                theSurveyUpdate.Name = examQuestion.Name;
            }

            theSurveyUpdate.Description = theSurvey.Description?.Trim();
            theSurveyUpdate.Note = theSurvey.Note?.Trim();
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var theSurvey = await _context.AsEduSurveyUndergraduateBaiKhaoSat.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)TheSurveyStatus.Deleted);
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

        public async Task<AsEduSurveyUndergraduateBaiKhaoSat> GetTheSurveyById(Guid id)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var theSurvey = await _context.AsEduSurveyUndergraduateBaiKhaoSat.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)TheSurveyStatus.Deleted);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát");
            }
            return theSurvey;
        }

        public async Task Deactive(Guid id)
        {
            var theSurvey = await _context.AsEduSurveyUndergraduateBaiKhaoSat.FirstOrDefaultAsync(o => o.Id == id && o.Status != (int)TheSurveyStatus.Deleted);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException();
            }
            theSurvey.Status = (int)TheSurveyStatus.Deactive;
            await _context.Database.ExecuteSqlRawAsync($"update AS_Edu_Survey_Graduate_BaiKhaoSat_SinhVien set Status = {(int)SurveyStudentStatus.Close} where BaiKhaoSatID = '{theSurvey.Id}'");
            await _context.SaveChangesAsync();
        }
    }
}
