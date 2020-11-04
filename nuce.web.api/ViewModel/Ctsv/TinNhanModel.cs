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
    }
}
