//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace nuce.web.survey.student.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class AS_Edu_Survey_Undergraduate_DapAn
    {
        public System.Guid ID { get; set; }
        public string Code { get; set; }
        public System.Guid CauHoiID { get; set; }
        public string CauHoiCode { get; set; }
        public Nullable<System.Guid> ChildQuestionId { get; set; }
        public string Content { get; set; }
        public Nullable<bool> IsCheck { get; set; }
        public Nullable<System.Guid> Matched { get; set; }
        public string Matching { get; set; }
        public Nullable<double> Mark { get; set; }
        public Nullable<double> MarkFail { get; set; }
        public Nullable<int> Order { get; set; }
        public int Status { get; set; }
        public string Explain { get; set; }
        public string ShowQuestion { get; set; }
        public string HideQuestion { get; set; }
    }
}
