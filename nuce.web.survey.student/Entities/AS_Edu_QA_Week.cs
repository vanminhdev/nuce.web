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
    
    public partial class AS_Edu_QA_Week
    {
        public long ID { get; set; }
        public long ClassRoomID { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public System.DateTime CreatedTime { get; set; }
        public System.DateTime UpdatedTime { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
    }
}