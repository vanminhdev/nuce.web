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

        public async Task<PaginationModel<GraduateStudent>> GetAll(GraduateStudentFilter filter, int skip = 0, int take = 20)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var query = _context.AsEduSurveyGraduateStudent
                .Join(_context.AsEduSurveyGraduateSurveyRound, o => o.DotKhaoSatId, o => o.Id, (sv, dotks) => new { sv, dotks });
            var recordsTotal = query.Count();

            if (!string.IsNullOrWhiteSpace(filter.Masv))
            {
                query = query.Where(o => o.sv.Masv.Contains(filter.Masv));
            }

            var recordsFiltered = query.Count();

            var querySkip = query
                .OrderBy(u => u.sv.Id)
                .Skip(skip).Take(take);

            var data = await querySkip
                .Select(o => new GraduateStudent {
                    id = o.sv.Id,
                    dottotnghiep = o.sv.Dottotnghiep,
                    sovaoso = o.sv.Sovaoso,
                    masv = o.sv.Masv,
                    noisiti = o.sv.Noisiti,
                    tbcht = o.sv.Tbcht,
                    xeploai = o.sv.Xeploai,
                    soqdtn = o.sv.Soqdtn,
                    sohieuba = o.sv.Sohieuba,
                    tinh = o.sv.Tinh,
                    truong = o.sv.Truong,
                    gioitinh = o.sv.Gioitinh,
                    ngaysinh = o.sv.Ngaysinh,
                    tkhau = o.sv.Tkhau,
                    lop12 = o.sv.Lop12,
                    namtn = o.sv.Namtn,
                    sobaodanh = o.sv.Sobaodanh,
                    tcong = o.sv.Tcong,
                    ghichuThi = o.sv.GhichuThi,
                    lopqd = o.sv.Lopqd,
                    k = o.sv.K,
                    dtoc = o.sv.Dtoc,
                    quoctich = o.sv.Quoctich,
                    bangclc = o.sv.Bangclc,
                    manganh = o.sv.Manganh,
                    tenchnga = o.sv.Tenchnga,
                    tennganh = o.sv.Tennganh,
                    hedaotao = o.sv.Hedaotao,
                    khoahoc = o.sv.Khoahoc,
                    tensinhvien = o.sv.Tensinhvien,
                    email = o.sv.Email,
                    email1 = o.sv.Email1,
                    email2 = o.sv.Email2,
                    mobile = o.sv.Mobile,
                    mobile1 = o.sv.Mobile1,
                    mobile2 = o.sv.Mobile2,
                    thongtinthem = o.sv.Thongtinthem,
                    thongtinthem1 = o.sv.Thongtinthem1,
                    dotKhaoSatId = o.sv.DotKhaoSatId,
                    tenDotKhaoSat = o.dotks.Name,
                    checksum = o.sv.Checksum,
                    exMasv = o.sv.ExMasv,
                    type = o.sv.Type,
                    status = o.sv.Status
                }).ToListAsync();

            return new PaginationModel<GraduateStudent>
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
                throw new RecordNotFoundException("Không tìm thấy sinh viên");
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
                throw new RecordNotFoundException("Mã sinh viên không tồn tại trong danh sách");
            }

            if (sinhvien.Psw == pwd)
            {
                return true;
            }
            return false;
        }
    }
}
