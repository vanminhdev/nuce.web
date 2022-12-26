using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsEduUndergraduateReportTotalSv
    {
        public int Id { get; set; }
        public Guid? SurveyRoundId { get; set; }
        public Guid? TheSurveyId { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public string QuestionCode { get; set; }
        public string AnswerCode { get; set; }
        public string QuestionContent { get; set; }
        public string AnswerContent { get; set; }
        public string ClassRoom { get; set; }
        public string Nganh { get; set; }
        public string ChuyenNganh { get; set; }
        public DateTime? NgayLamKhaoSat { get; set; }
        public string GioiTinh { get; set; }
        public int LoaiKS { get; set; }
    }
}
