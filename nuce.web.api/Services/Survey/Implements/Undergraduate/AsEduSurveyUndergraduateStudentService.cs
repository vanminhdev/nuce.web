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
using nuce.web.api.ViewModel.Survey.Undergraduate;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements
{
    class AsEduSurveyUndergraduateStudentService : IAsEduSurveyUndergraduateStudentService
    {
        private readonly SurveyContext _context;

        public AsEduSurveyUndergraduateStudentService(SurveyContext context)
        {
            _context = context;
        }

        public async Task<PaginationModel<UndergraduateStudent>> GetAll(UndergraduateStudentFilter filter, int skip = 0, int take = 20)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            IQueryable<AsEduSurveyUndergraduateStudent> dssv = _context.AsEduSurveyUndergraduateStudent;
                
            var recordsTotal = dssv.Count();

            if (!string.IsNullOrWhiteSpace(filter.Masv))
            {
                dssv = dssv.Where(o => o.Masv.Contains(filter.Masv));
            }

            if (filter.DotKhaoSatId != null)
            {
                dssv = dssv.Where(o => o.DotKhaoSatId == filter.DotKhaoSatId);
            }

            var recordsFiltered = dssv.Count();

            var querySkip = dssv
                .Skip(skip).Take(take);

            var svdotks = querySkip.Join(_context.AsEduSurveyUndergraduateSurveyRound, o => o.DotKhaoSatId, o => o.Id, (sv, dotks) => new { sv, dotks });

            var leftJoin = svdotks.GroupJoin(_context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien, o => o.sv.ExMasv, o => o.StudentCode, (svdotks, baikssv) => new { svdotks, baikssv })
                .SelectMany(o => o.baikssv.DefaultIfEmpty(), (r, baikssv) => new { r.svdotks.sv, r.svdotks.dotks, baikssv });

            var data = await leftJoin
                .Select(o => new UndergraduateStudent
                {
                    id = o.sv.Id,
                    dottotnghiep = o.sv.Dottotnghiep,
                    sovaoso = o.sv.Sovaoso,
                    masv = o.sv.Masv,
                    noisiti = o.sv.Noisiti,
                    tbcht = o.sv.Tbcht,
                    xeploai = o.sv.Xeploai,
                    soqdtn = o.sv.Soqdtn,
                    ngayraqd = o.sv.Ngayraqd,
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
                    status = o.sv.Status,
                    ghichuphatbang = o.sv.Ghichuphatbang,
                    makhoa = o.sv.Makhoa,
                    malop = o.sv.Malop,
                    nguoiphatbang = o.sv.Nguoiphatbang,
                    cnOrder = o.sv.CnOrder,

                    surveyStudentStatus = o.baikssv != null ? o.baikssv.Status : (int)SurveyStudentStatus.HaveNot
                })
                .OrderBy(o => o.dotKhaoSatId)
                .ThenBy(o => o.exMasv)
                .ToListAsync();

            return new PaginationModel<UndergraduateStudent>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }

        public async Task<AsEduSurveyUndergraduateStudent> GetById(string id)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var record = await _context.AsEduSurveyUndergraduateStudent.FirstOrDefaultAsync(o => o.Id.ToString() == id);
            if (record == null)
            {
                throw new RecordNotFoundException("Không tìm thấy sinh viên");
            }
            return record;
        }

        public async Task<AsEduSurveyUndergraduateStudent> GetByStudentCode(string studentCode)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var record = await _context.AsEduSurveyUndergraduateStudent.FirstOrDefaultAsync(o => o.ExMasv == studentCode);
            return record;
        }

        public async Task Create(AsEduSurveyUndergraduateStudent student)
        {
            if(await _context.AsEduSurveyUndergraduateStudent.FirstOrDefaultAsync(o => o.Masv == student.Masv) != null)
            {
                return;
            }
            _context.AsEduSurveyUndergraduateStudent.Add(student);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAll(List<AsEduSurveyUndergraduateStudent> students)
        {
            foreach(var s in students)
            {
                var stu = await _context.AsEduSurveyUndergraduateStudent.FirstOrDefaultAsync(o => o.ExMasv == s.ExMasv);
                if (stu == null)
                {
                    _context.AsEduSurveyUndergraduateStudent.Add(s);
                }
                else
                {
                    stu.Tensinhvien = s.Tensinhvien;
                    stu.Lopqd = s.Lopqd;
                    stu.Ngaysinh = s.Ngaysinh;
                    stu.Gioitinh = s.Gioitinh;
                    stu.Tbcht = s.Tbcht;
                    stu.Xeploai = s.Xeploai;
                    stu.Tennganh = s.Tennganh;
                    stu.Tenchnga = s.Tenchnga;
                    stu.Makhoa = s.Makhoa;
                    stu.Hedaotao = s.Hedaotao;
                    stu.Soqdtn = s.Soqdtn;
                    stu.Ngayraqd = s.Ngayraqd;
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task TruncateTable()
        {
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE AS_Edu_Survey_Undergraduate_Student");
        }

        public async Task Delete(string studentCode)
        {
            var student =  await _context.AsEduSurveyUndergraduateStudent.FirstOrDefaultAsync(o => o.ExMasv == studentCode);
            if(student == null)
            {
                throw new RecordNotFoundException("Không tìm thấy sinh viên");
            }
            _context.AsEduSurveyUndergraduateStudent.Remove(student);
            var baikssv = await _context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien.FirstOrDefaultAsync(o => o.StudentCode == student.ExMasv);
            if(baikssv != null)
            {
                _context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien.Remove(baikssv);
            }
            _context.SaveChanges();
        }

        public async Task<byte[]> DownloadListStudent(Guid surveyRoundId)
        {
            ExcelPackage excel = new ExcelPackage();
            var worksheet = excel.Workbook.Worksheets.Add("Danh sách sinh viên");
            worksheet.DefaultRowHeight = 14.25;
            worksheet.DefaultColWidth = 15;

            worksheet.Cells.Style.WrapText = true;

            //worksheet.Cells.Style.Font.Name = "Arial";
            worksheet.Cells.Style.Font.Size = 11;

            worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            worksheet.Row(1).Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Row(1).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Row(1).Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Row(1).Style.Border.Right.Style = ExcelBorderStyle.Thin;

            worksheet.Column(1).Width = 9.75;
            worksheet.Column(2).Width = 21.83;
            worksheet.Column(3).Width = 29.38;
            worksheet.Column(4).Width = 17.75;
            worksheet.Column(5).Width = 16;
            worksheet.Column(6).Width = 11.75;
            worksheet.Column(7).Width = 9.75;
            worksheet.Column(8).Width = 16.25;
            worksheet.Column(9).Width = 26;
            worksheet.Column(10).Width = 26;
            worksheet.Column(11).Width = 22.5;
            worksheet.Column(12).Width = 15.38;
            worksheet.Column(13).Width = 47.38;
            worksheet.Column(14).Width = 19.75;

            worksheet.Cells["A1"].Value = "STT";
            worksheet.Cells["B1"].Value = "Mã SV";
            worksheet.Cells["C1"].Value = "Họ và Tên";
            worksheet.Cells["D1"].Value = "Lớp";
            worksheet.Cells["E1"].Value = "Ngày sinh";
            worksheet.Cells["F1"].Value = "Giới";
            worksheet.Cells["G1"].Value = "TBCHT";
            worksheet.Cells["H1"].Value = "Xếp loại tốt nghiệp";
            worksheet.Cells["I1"].Value = "Tên ngành";
            worksheet.Cells["J1"].Value = "Tên chuyên ngành";
            worksheet.Cells["K1"].Value = "Hệ tốt nghiệp";
            worksheet.Cells["L1"].Value = "Mã Khoa";
            worksheet.Cells["M1"].Value = "Số quyết định và ngày ra quyết định tốt nghiệp";
            worksheet.Cells["N1"].Value = "Ngày ra quyết định";


            var students = await _context.AsEduSurveyUndergraduateStudent
                .Where(o => o.DotKhaoSatId == surveyRoundId)
                .ToListAsync();
            
            int row = 2;
            int stt = 1;
            foreach (var student in students)
            {
                worksheet.Cells[$"A{row}"].Value = stt++;
                worksheet.Cells[$"B{row}"].Value = student.ExMasv;
                worksheet.Cells[$"C{row}"].Value = student.Tensinhvien;
                worksheet.Cells[$"D{row}"].Value = student.Lopqd;
                worksheet.Cells[$"E{row}"].Value = student.Ngaysinh;
                worksheet.Cells[$"F{row}"].Value = student.Gioitinh;
                worksheet.Cells[$"G{row}"].Value = student.Tbcht;
                worksheet.Cells[$"H{row}"].Value = student.Xeploai;
                worksheet.Cells[$"I{row}"].Value = student.Tennganh;
                worksheet.Cells[$"J{row}"].Value = student.Tenchnga;
                worksheet.Cells[$"K{row}"].Value = student.Hedaotao;
                worksheet.Cells[$"L{row}"].Value = student.Makhoa;
                worksheet.Cells[$"M{row}"].Value = student.Soqdtn;
                worksheet.Cells[$"N{row}"].Value = student.Ngayraqd;
                row++;
            }

            MemoryStream ms = new MemoryStream();
            await excel.SaveAsAsync(ms);
            return ms.ToArray();
        }
    }
}
