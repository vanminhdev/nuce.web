using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Status;
using nuce.web.api.Models.Survey;
using nuce.web.api.Models.Survey.JsonData;
using nuce.web.api.Services.Status.Interfaces;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Graduate;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements
{
    class AsEduSurveyGraduateStudentService : IAsEduSurveyGraduateStudentService
    {
        private readonly SurveyContext _context;

        public AsEduSurveyGraduateStudentService(SurveyContext context)
        {
            _context = context;
        }

        public async Task<PaginationModel<AsEduSurveyGraduateStudent>> GetAll(GraduateStudentFilter filter, int skip = 0, int take = 20)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            IQueryable<AsEduSurveyGraduateStudent> query = _context.AsEduSurveyGraduateStudent;
            var recordsTotal = query.Count();

            if (!string.IsNullOrWhiteSpace(filter.Masv))
            {
                query = query.Where(o => o.Masv.Contains(filter.Masv));
            }

            var recordsFiltered = query.Count();

            var querySkip = query
                .OrderBy(u => u.Id)
                .Skip(skip).Take(take);

            var data = await querySkip.ToListAsync();

            return new PaginationModel<AsEduSurveyGraduateStudent>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }

        public async Task<AsEduSurveyGraduateStudent> GetById(string id)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var record = await _context.AsEduSurveyGraduateStudent.FirstOrDefaultAsync(o => o.Id.ToString() == id);
            if (record == null)
            {
                throw new RecordNotFoundException("Không tìm thấy đợt khảo sát");
            }
            return record;
        }

        public async Task Create(AsEduSurveyGraduateStudent student)
        {
            if(await _context.AsEduSurveyGraduateStudent.FirstOrDefaultAsync(o => o.Masv == student.Masv) != null)
            {
                return;
            }
            _context.AsEduSurveyGraduateStudent.Add(student);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAll(List<AsEduSurveyGraduateStudent> students)
        {
            foreach(var s in students)
            {
                if (await _context.AsEduSurveyGraduateStudent.FirstOrDefaultAsync(o => o.Masv == s.Masv) != null)
                {
                    continue;
                }
                _context.AsEduSurveyGraduateStudent.Add(s);
            }
            await _context.SaveChangesAsync();
        }

        public async Task TruncateTable()
        {
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE AS_Edu_Survey_Graduate_Student");
        }

        public async Task<bool> Login(string masv, string pwd)
        {
            var sinhvien = await _context.AsEduSurveyGraduateStudent.FirstOrDefaultAsync(o => o.ExMasv == masv);
            if (sinhvien == null)
            {
                return false;
            }

            if (sinhvien.Psw == pwd)
            {
                return true;
            }
            return false;
        }
    }
}
