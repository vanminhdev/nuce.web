using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nuce.web.survey.student.Models.SurveyResult
{
    public class KetQuaMonHocCuaBoMon
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public int SoSVHoc { get; set; }
        public int SoSVLamKhaoSat { get; set; }
    }

    public class KetQuaGiangVienCuaMonHoc
    {
        public string LecturerCode { get; set; }
        public string LecturerName { get; set; }
        public string MaLop { get; set; }
        public int SoSVHoc { get; set; }
        public int SoSVLamKhaoSat { get; set; }
    }
}