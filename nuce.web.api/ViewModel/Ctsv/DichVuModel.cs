using GemBox.Document;
using nuce.web.api.Models.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static nuce.web.api.Common.Ctsv;

namespace nuce.web.api.ViewModel.Ctsv
{
    public class DichVuModel
    {
        public int Type { get; set; }
        public string LyDo { get; set; }
        public string KyLuat { get; set; }
        public string ThuocDien { get; set; }
        public string ThuocDoiTuong { get; set; }
        public string DonVi { get; set; }
        public string DenGap { get; set; }
        public string VeViec { get; set; }
        public string MaXacNhan { get; set; }
        public string PhanHoi { get; set; }
        public int VeBusTuyenType { get; set; }
        public string VeBusTuyenCode { get; set; }
        public string VeBusTuyenName { get; set; }
        public string VeBusNoiNhanThe { get; set; }
    }

    public class AllTypeDichVuModel
    {
        public string TenDichVu { get; set; }
        public string LinkDichVu { get; set; }
        public int TongSo { get; set; }
        public int MoiGui { get; set; }
        public int DaXuLy { get; set; }
        public int DangXuLy { get; set; }
        public int Stt { get; set; }
    }

    public class QuanLyDichVuDetailModel
    {
        public int Type { get; set; }
        public string SearchText { get; set; }
        public int DayRange { get; set; }
        public int? Page { get; set; }
        public int? Size { get; set; }
    }

    public class QuanLyDichVuDetailResponse
    {
        public object DichVu { get; set; }
        public AsAcademyStudent Student { get; set; }
    }

    public class UpdateRequestStatusModel
    {
        public int Type { get; set; }
        public int RequestID { get; set; }
        public string PhanHoi { get; set; }
        public DateTime? NgayHenBatDau { get; set; }
        public DateTime? NgayHenKetThuc { get; set; }
        public bool AutoUpdateNgayHen { get; set; }
        public int Status { get; set; }
    }

    public class GetAllForAdminResponseRepo<T>
    {
        public IQueryable<T> FinalData { get; set; }
        public IQueryable<T> BeforeFilteredData { get; set; }
    }

    public class ExportModel
    {
        public List<DichVuExport> DichVuList { get; set; }
        public DichVu DichVuType { get; set; }
    }

    public class DichVuExport
    {
        public int ID { get; set; }
    }

    public class StudentDichVuModel
    {
        public AsAcademyStudent Student { get; set; }
        public AsAcademyClass AcademyClass { get; set; }
        public AsAcademyFaculty Faculty { get; set; }
        public AsAcademyDepartment Department { get; set; }
        public AsAcademyAcademics Academics { get; set; }
        public AsAcademyYear Year { get; set; }
    }

    public class YeuCauDichVuStudentModel<T> : StudentDichVuModel
    {
        public T YeuCauDichVu { get; set; }
    }

    public class ExportFileOutputModel
    {
        public DocumentModel document { get; set; }
        public string filePath { get; set; }
    }

    public class UpdateStatusMultiFourModel
    {
        public DichVu LoaiDichVu { get; set; }
        public List<DichVuExport> YeuCauList { get; set; }
    }

    public class GetUpdateStatusNgayHenModel
    {
        public DateTime? NgayHenBatDau { get; set; }
        public DateTime? NgayHenKetThuc { get; set; }
    }
}
