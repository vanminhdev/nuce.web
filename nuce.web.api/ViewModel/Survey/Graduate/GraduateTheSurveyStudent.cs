using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey.Graduate
{
    public class GraduateTheSurveyStudent
    {
        public Guid Id { get; set; }
        public Guid BaiKhaoSatId { get; set; }
        public string Nganh { get; set; }
        public string ChuyenNganh { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
    }
}
