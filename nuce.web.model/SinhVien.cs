using System.Collections.Generic;

namespace nuce.web.model
{
    public class SinhVien
    {
        public int SinhVienID { get; set; }
        public string MaSV { get; set; }
        public string Ten { get; set; }
        public string Ho { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string CMT { get; set; }
        public string IMG { get; set; }
        public int TrangThai { get; set; }
    }
    public class CanBo
    {
        public int ID { get; set; }
        public string MaCB { get; set; }
        public string Ten { get; set; }
        public string Khoa { get; set; }
        public string BoMon { get; set; }
        public int Type { get; set; }
    }
    public class User
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public List<Role> Roles { get; set; }
    }
    public class Role
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }


}
