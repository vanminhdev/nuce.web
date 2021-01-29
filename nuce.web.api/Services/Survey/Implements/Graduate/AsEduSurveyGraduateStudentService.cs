using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Helper;
using nuce.web.api.Models.Status;
using nuce.web.api.Models.Survey;
using nuce.web.api.Models.Survey.JsonData;
using nuce.web.api.Services.Status.Interfaces;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Survey.Graduate;
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
            IQueryable<AsEduSurveyGraduateStudent> dssv = _context.AsEduSurveyGraduateStudent;

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

            var svdotks = querySkip.Join(_context.AsEduSurveyGraduateSurveyRound, o => o.DotKhaoSatId, o => o.Id, (sv, dotks) => new { sv, dotks });

            var leftJoin = svdotks.GroupJoin(_context.AsEduSurveyGraduateBaiKhaoSatSinhVien, o => o.sv.ExMasv, o => o.StudentCode, (svdotks, baikssv) => new { svdotks, baikssv })
                .SelectMany(o => o.baikssv.DefaultIfEmpty(), (r, baikssv) => new { r.svdotks.sv, r.svdotks.dotks, baikssv });

            var data = await leftJoin
                .Select(o => new GraduateStudent {
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
                    makhoa = o.sv.Makhoa,
                    malop = o.sv.Malop,
                    nguoiphatbang = o.sv.Nguoiphatbang,

                    surveyStudentStatus = o.baikssv != null ? o.baikssv.Status : (int)SurveyStudentStatus.HaveNot,
                    loaiHinh = o.baikssv.LoaiHinh
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
            foreach (var s in students)
            {
                var stu = await _context.AsEduSurveyGraduateStudent.FirstOrDefaultAsync(o => o.ExMasv == s.ExMasv);
                if (stu == null)
                {
                    _context.AsEduSurveyGraduateStudent.Add(s);
                }
                else
                {
                    PropertyCopier<AsEduSurveyGraduateStudent, AsEduSurveyGraduateStudent>.Copy(s, stu, PropertyCopierOption.AllowLowerCase, "Id", "Type", "Status");
                }
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

        public async Task TransferDataFromUndergraduate(Guid surveyRoundId, TransferDataUndergraduateModel filter)
        {
            var surveyRound = await _context.AsEduSurveyGraduateSurveyRound.FirstOrDefaultAsync(o => o.Id == surveyRoundId);
            if(surveyRound == null)
            {
                throw new RecordNotFoundException("Không tồn tại đợt khảo sát");
            }

            //lấy ds từ trước tốt nghiệp sang
            var query = _context.AsEduSurveyUndergraduateStudent
                .Where(o => o.Ngayraqd != null)
                .Where(o => filter.FromDate <= o.Ngayraqd && filter.ToDate >= o.Ngayraqd)
                .Join(_context.AsEduSurveyUndergraduateBaiKhaoSatSinhVien.Where(o => o.Status == (int)SurveyStudentStatus.Done), o => o.ExMasv, o => o.StudentCode, (sv, baikssv) => sv);

            if (filter.HeTotNghieps != null)
            {
                filter.HeTotNghieps.ForEach(h => h = h.ToLower());
                query = query.Where(o => o.Hedaotao != null && filter.HeTotNghieps.Contains(o.Hedaotao.ToLower()));
            }

            var underStudents = await query.ToListAsync();

            foreach(var underStu in underStudents)
            {
                var graStu = await _context.AsEduSurveyGraduateStudent.FirstOrDefaultAsync(o => o.ExMasv == underStu.ExMasv);
                if(graStu == null)
                {
                    graStu = new AsEduSurveyGraduateStudent();
                    PropertyCopier<AsEduSurveyUndergraduateStudent, AsEduSurveyGraduateStudent>.Copy(underStu, graStu, PropertyCopierOption.AllowLowerCase, "DotKhaoSatId", "Type", "Status");
                    graStu.Psw = StringHelper.ConvertToLatin(graStu.Tensinhvien).Replace(" ", "").ToLower();
                    graStu.DotKhaoSatId = surveyRoundId;
                    graStu.Type = 1;
                    graStu.Status = 1;
                    _context.AsEduSurveyGraduateStudent.Add(graStu);
                }
                else
                {
                    PropertyCopier<AsEduSurveyUndergraduateStudent, AsEduSurveyGraduateStudent>.Copy(underStu, graStu, PropertyCopierOption.AllowLowerCase, "DotKhaoSatId", "Type", "Status");
                }                    
            }
            _context.SaveChanges();
        }

        public async Task<byte[]> DownloadListStudent(Guid surveyRoundId)
        {
            ExcelPackage excel = new ExcelPackage();
            var worksheet = excel.Workbook.Worksheets.Add("Danh sách cựu sinh viên");
            worksheet.DefaultRowHeight = 14.25;
            worksheet.DefaultColWidth = 15;

            worksheet.Cells.Style.WrapText = true;
            worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            worksheet.Cells.Style.Font.Name = "Times New Roman";
            worksheet.Cells.Style.Font.Size = 12;

            worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            worksheet.Cells[1, 1, 1, 14].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[1, 1, 1, 14].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[1, 1, 1, 14].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[1, 1, 1, 14].Style.Border.Right.Style = ExcelBorderStyle.Thin;

            worksheet.Row(1).Height = 53.25;
            worksheet.Row(1).Style.Font.Bold = true;

            worksheet.Column(1).Width = 9.75;
            worksheet.Column(2).Width = 21.83;
            worksheet.Column(3).Width = 29.38;
            worksheet.Column(4).Width = 17.75;
            worksheet.Column(5).Width = 16;
            worksheet.Column(6).Width = 11.75;
            worksheet.Column(7).Width = 9.75;
            worksheet.Column(8).Width = 16.25;
            worksheet.Column(9).Width = 33.57;
            worksheet.Column(10).Width = 41.86;
            worksheet.Column(11).Width = 22.5;
            worksheet.Column(12).Width = 15.38;
            worksheet.Column(13).Width = 68.86;
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

            worksheet.Column(2).Style.Numberformat.Format = "@";
            worksheet.Column(5).Style.Numberformat.Format = "dd-MM-yy";
            worksheet.Column(7).Style.Numberformat.Format = "0.0";
            worksheet.Column(14).Style.Numberformat.Format = "dd-MM-yy";

            var students = await _context.AsEduSurveyGraduateStudent
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
                worksheet.Cells[$"E{row}"].Value = student.Ngaysinh?.ToString("dd-MM-yyyy");
                worksheet.Cells[$"F{row}"].Value = student.Gioitinh;
                worksheet.Cells[$"G{row}"].Value = student.Tbcht;
                worksheet.Cells[$"H{row}"].Value = student.Xeploai;
                worksheet.Cells[$"I{row}"].Value = student.Tennganh;
                worksheet.Cells[$"J{row}"].Value = student.Tenchnga;
                worksheet.Cells[$"K{row}"].Value = student.Hedaotao;
                worksheet.Cells[$"L{row}"].Value = student.Makhoa;
                worksheet.Cells[$"M{row}"].Value = student.Soqdtn;
                worksheet.Cells[$"N{row}"].Value = student.Ngayraqd?.ToString("dd-MM-yyyy");
                row++;
            }

            MemoryStream ms = new MemoryStream();
            await excel.SaveAsAsync(ms);
            return ms.ToArray();
        }

        public async Task Delete(string studentCode)
        {
            var student = await _context.AsEduSurveyGraduateStudent.FirstOrDefaultAsync(o => o.ExMasv == studentCode);
            if (student == null)
            {
                throw new RecordNotFoundException("Không tìm thấy sinh viên");
            }
            _context.AsEduSurveyGraduateStudent.Remove(student);
            var baikssv = await _context.AsEduSurveyGraduateBaiKhaoSatSinhVien.FirstOrDefaultAsync(o => o.StudentCode == student.ExMasv);
            if (baikssv != null)
            {
                _context.AsEduSurveyGraduateBaiKhaoSatSinhVien.Remove(baikssv);
            }
            _context.SaveChanges();
        }
    }
}
