using System;
using System.Collections.Generic;

namespace nuce.web.model
{
    public class CauHoi
    {
        // noi dung cau tra loi
        // M id cau tra loi 
        public int CauHoiID { get; set; }
        public int DoKhoID { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public float Mark { get; set; }
        public string Explain { get; set; }
        public string Type { get; set; }
        public int SoCauTraLoi { get; set; }
        public string A1 { get; set; }
        public string D1 { get; set; }
        public string M1 { get; set; }
        public string A2 { get; set; }
        public string D2 { get; set; }
        public string M2 { get; set; }
        public string A3 { get; set; }
        public string D3 { get; set; }
        public string M3 { get; set; }
        public string A4 { get; set; }
        public string D4 { get; set; }
        public string M4 { get; set; }
        public string A5 { get; set; }
        public string D5 { get; set; }
        public string M5 { get; set; }
        public string A6 { get; set; }
        public string D6 { get; set; }
        public string M6 { get; set; }
        public string A7 { get; set; }
        public string D7 { get; set; }
        public string M7 { get; set; }
        public string A8 { get; set; }
        public string D8 { get; set; }
        public string M8 { get; set; }
        public string A9 { get; set; }
        public string D9 { get; set; }
        public string M9 { get; set; }
        public string A10 { get; set; }
        public string D10{ get; set; }
        public string M10{ get; set; }
        public string A11 { get; set; }
        public string D11 { get; set; }
        public string M11 { get; set; }
        public string A12 { get; set; }
        public string D12 { get; set; }
        public string M12 { get; set; }
        public string A13 { get; set; }
        public string D13 { get; set; }
        public string M13 { get; set; }
        public string A14 { get; set; }
        public string D14 { get; set; }
        public string M14 { get; set; }
        public string A15 { get; set; }
        public string D15 { get; set; }
        public string M15 { get; set; }
        public List<CauHoi> ChildCauHois { get; set; }
    }
    public class DapAn : IEquatable<DapAn>, IComparable<DapAn>
    {
        public int CauHoiID { get; set; }
        public string Match { get; set; }
        public float Mark { get; set; }
        public string Type { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            DapAn objAsPart = obj as DapAn;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        public int CompareTo(DapAn dapAn)
        {
            // A null value means that this object is greater.
            if (dapAn == null)
                return 1;

            else
                return this.CauHoiID.CompareTo(dapAn.CauHoiID);
        }
        public int SortByNameAscending(string name1, string name2)
        {
            return name1.CompareTo(name2);
        }
        public override int GetHashCode()
        {
            return CauHoiID;
        }

        public bool Equals(DapAn other)
        {
            if (other == null) return false;
            return (this.CauHoiID.Equals(other.CauHoiID) && this.Match.Equals(other.Match));
        }
    }
    public class reportTotal
    {
        public int SemesterId { get; set; }
        public int CampaignId { get; set; }
        public int CampaignTemplateId { get; set; }
        public int ClassRoomId { get; set; }
        public int LecturerId { get; set; }
        public int QuestionType { get; set; }
        public int QuestionId { get; set; }
        public int QuestionIndex { get; set; }
        public int AnswerId { get; set; }
        public int Total { get; set; }
        public string Value { get; set; }
    }
}
