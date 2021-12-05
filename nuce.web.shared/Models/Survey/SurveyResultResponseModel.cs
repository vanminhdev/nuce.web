using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.shared.Models.Survey
{
    /// <summary>
    /// Kết quả khoa ban
    /// </summary>
    public class FacultyResultModel
    {
        public string FacultyCode { get; set; }
        public string FacultyName { get; set; }
        public List<SurveyResultResponseModel> Result { get; set; }
    }
    /// <summary>
    /// Kết quả bộ môn
    /// </summary>
    public class DepartmentResultModel
    {
        public string FacultyCode { get; set; }
        public string FacultyName { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public List<SurveyResultResponseModel> Result { get; set; }
    }
    /// <summary>
    /// Kết quả chi tiết | Giảng viên
    /// </summary>
    public class SurveyResultResponseModel
    {
        public string FacultyCode { get; set; }
        public string FacultyName { get; set; }
        public string DepartmentCode { get; set; }
        public string DeparmentName { get; set; }
        public string LecturerCode { get; set; }
        public string LecturerName { get; set; }
        public int SoSvThamGiaKhaoSat { get; set; }
        public int SoSvDuocKhaoSat { get; set; }
        public int SoPhieuThuVe { get; set; }
        public int SoPhieuPhatRa { get; set; }
        public int SoGiangVienDaKhaoSat { get; set; }
        public int SoGiangVienCanLayYKien { get; set; }
        public bool IsTotal { get; set; }
        public List<ChiTietLopMonHoc> ChiTietTungLopMonHoc { get; set; }
    }

    public class ChiTietLopMonHoc
    {
        public string TenLop { get; set; }
        public string TenMon { get; set; }
        public int SoBaiKhaoSat { get; set; }
        public List<ChiTietCauHoi> CauHoi { get; set; }
    }

    public class ChiTietCauHoi
    {
        public string OnlyTitle { get; set; }
        public string Content { get; set; }

        public Dictionary<string, string> DanhSachDiem = new Dictionary<string, string> {};
    }
}
