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
    
    public partial class AS_Edu_Survey_Undergraduate_CauHoi
    {
        public System.Guid ID { get; set; }
        public int BoCauHoiID { get; set; }
        public int DoKhoID { get; set; }
        public string Code { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Media_URL { get; set; }
        public string Media_Setting { get; set; }
        public Nullable<double> Mark { get; set; }
        public Nullable<double> MarkFail { get; set; }
        public Nullable<bool> IsOption { get; set; }
        public Nullable<int> PartId { get; set; }
        public string Explain { get; set; }
        public Nullable<int> NoAnswerOnRow { get; set; }
        public Nullable<System.DateTime> ExpiredDate { get; set; }
        public Nullable<int> TimeVisibleAnswer { get; set; }
        public Nullable<int> InsertedUser { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public System.DateTime InsertedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public int Order { get; set; }
        public int Level { get; set; }
        public string Type { get; set; }
        public int Status { get; set; }
        public string CheckImport { get; set; }
        public string ParentCode { get; set; }
    }
}
