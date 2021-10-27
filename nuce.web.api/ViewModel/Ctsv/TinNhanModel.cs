using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Ctsv
{
    public class TinNhanModel
    {
        public string StudentName { get; set; }
        public int StudentID { get; set; }
        public string StudentCode { get; set; }
        public string TenDichVu { get; set; }
        public string StudentEmail { get; set; }
        public string TinNhanCode { get; set; }
        public string TinNhanTitle { get; set; }
        public int YeuCauStatus { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayHen { get; set; }
        /// <summary>
        /// Tên template trong thư mục "Templates/Ctsv" thay vì template mặc định
        /// </summary>
        public string TemplateName { get; set; }
        /// <summary>
        /// Tên template Đã có hẹn trong thư mục "Templates/Ctsv" thay vì template Đã có hẹn mặc định
        /// </summary>
        public string TemplateNameDaCoHen { get; set; }
        /// <summary>
        /// Nếu true thì sẽ dùng template Đã có hẹn chuyển phát nhanh, không xét theo TemplateNameDaCoHen
        /// </summary>
        public bool ChuyenPhatNhanh { get; set; }
    }
}
