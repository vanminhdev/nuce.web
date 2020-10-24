using nuce.web.data;
using System;
using System.Data;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class CapNhatHoSoDayDu : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Kiểm tra xem nếu chưa đến đợt cập nhật thì thông báo cho người dùng và thoát
                string sql = "select Value from GS_Setting where Code='CAP_NHAT_HO_SO_DAY_DU' and Enabled=1";
                DataTable dt1 = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    //Kiểm tra xem đã đến thời gian cập nhật hay chưa
                    if (checkCapNhat(dt1.Rows[0][0].ToString(), m_SinhVien.MaSV))
                    {
                        sql = "select * from AS_Academy_Student where Status<>4 and ID=" + m_SinhVien.SinhVienID;
                        DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            // Xử lý dữ liệu sinh viên
                            string Email = dt.Rows[0].IsNull("Email") ? "" : dt.Rows[0]["Email"].ToString();
                            string Mobile = dt.Rows[0].IsNull("Mobile") ? "" : dt.Rows[0]["Mobile"].ToString();
                            string NgaySinh = dt.Rows[0].IsNull("NgaySinh") ? "1/1/1900" : DateTime.Parse(dt.Rows[0]["NgaySinh"].ToString()).ToString("dd/MM/yyyy");
                            string BirthPlace = dt.Rows[0].IsNull("BirthPlace") ? "" : dt.Rows[0]["BirthPlace"].ToString();
                            string DanToc = dt.Rows[0].IsNull("DanToc") ? "" : dt.Rows[0]["DanToc"].ToString();
                            string TonGiao = dt.Rows[0].IsNull("TonGiao") ? "" : dt.Rows[0]["TonGiao"].ToString();
                            string HKTT_SoNha = dt.Rows[0].IsNull("HKTT_SoNha") ? "" : dt.Rows[0]["HKTT_SoNha"].ToString();
                            string HKTT_Pho = dt.Rows[0].IsNull("HKTT_Pho") ? "" : dt.Rows[0]["HKTT_Pho"].ToString();
                            string HKTT_Phuong = dt.Rows[0].IsNull("HKTT_Phuong") ? "" : dt.Rows[0]["HKTT_Phuong"].ToString();
                            string HKTT_Quan = dt.Rows[0].IsNull("HKTT_Quan") ? "" : dt.Rows[0]["HKTT_Quan"].ToString();
                            string HKTT_Tinh = dt.Rows[0].IsNull("HKTT_Tinh") ? "" : dt.Rows[0]["HKTT_Tinh"].ToString();
                            string CMT = dt.Rows[0].IsNull("CMT") ? "" : dt.Rows[0]["CMT"].ToString();
                            string CMT_NoiCap = dt.Rows[0].IsNull("CMT_NoiCap") ? "" : dt.Rows[0]["CMT_NoiCap"].ToString();
                            string CMT_NgayCap = dt.Rows[0].IsNull("CMT_NgayCap") ? "1/1/1900" : DateTime.Parse(dt.Rows[0]["CMT_NgayCap"].ToString()).ToString("dd/MM/yyyy");
                            string NamTotNghiepPTTH = dt.Rows[0].IsNull("NamTotNghiepPTTH") ? "" : dt.Rows[0]["NamTotNghiepPTTH"].ToString();
                            string NgayVaoDoan = dt.Rows[0].IsNull("NgayVaoDoan") ? "1/1/1900" : DateTime.Parse(dt.Rows[0]["NgayVaoDoan"].ToString()).ToString("dd/MM/yyyy");
                            string NgayVaoDang = dt.Rows[0].IsNull("NgayVaoDang") ? "1/1/1900" : DateTime.Parse(dt.Rows[0]["NgayVaoDang"].ToString()).ToString("dd/MM/yyyy");
                            string DiemThiPTTH = dt.Rows[0].IsNull("DiemThiPTTH") ? "" : dt.Rows[0]["DiemThiPTTH"].ToString();
                            string KhuVucHKTT = dt.Rows[0].IsNull("KhuVucHKTT") ? "" : dt.Rows[0]["KhuVucHKTT"].ToString();
                            string DoiTuongUuTien = dt.Rows[0].IsNull("DoiTuongUuTien") ? "" : dt.Rows[0]["DoiTuongUuTien"].ToString();
                            bool DaTungLamCanBoLop = dt.Rows[0].IsNull("DaTungLamCanBoLop") ? false : bool.Parse(dt.Rows[0]["DaTungLamCanBoLop"].ToString());
                            bool DaTungLamCanBoDoan = dt.Rows[0].IsNull("DaTungLamCanBoLop") ? false : bool.Parse(dt.Rows[0]["DaTungLamCanBoDoan"].ToString());
                            bool DaThamGiaDoiTuyenThiHSG = dt.Rows[0].IsNull("DaThamGiaDoiTuyenThiHSG") ? false : bool.Parse(dt.Rows[0]["DaThamGiaDoiTuyenThiHSG"].ToString());
                            string BaoTin_DiaChi = dt.Rows[0].IsNull("BaoTin_DiaChi") ? "" : dt.Rows[0]["BaoTin_DiaChi"].ToString();
                            string BaoTin_HoVaTen = dt.Rows[0].IsNull("BaoTin_HoVaTen") ? "" : dt.Rows[0]["BaoTin_HoVaTen"].ToString();
                            string BaoTin_DiaChiNguoiNhan = dt.Rows[0].IsNull("BaoTin_DiaChiNguoiNhan") ? "" : dt.Rows[0]["BaoTin_DiaChiNguoiNhan"].ToString();
                            string BaoTin_SoDienThoai = dt.Rows[0].IsNull("BaoTin_SoDienThoai") ? "" : dt.Rows[0]["BaoTin_SoDienThoai"].ToString();
                            string BaoTin_Email = dt.Rows[0].IsNull("BaoTin_Email") ? "" : dt.Rows[0]["BaoTin_Email"].ToString();
                            bool LaNoiTru = dt.Rows[0].IsNull("LaNoiTru") ? false : bool.Parse(dt.Rows[0]["LaNoiTru"].ToString());
                            string DiaChiCuThe = dt.Rows[0].IsNull("DiaChiCuThe") ? "" : dt.Rows[0]["DiaChiCuThe"].ToString();

                            string File1 = dt.Rows[0].IsNull("File1") ? "" : dt.Rows[0]["File1"].ToString();
                            string File2 = dt.Rows[0].IsNull("File2") ? "" : dt.Rows[0]["File2"].ToString();
                            txtEmail.Text = Email;
                            txtMobile.Text = Mobile;
                            txtBaoTin_DiaChi.Text = BaoTin_DiaChi;
                            txtBaoTin_DiaChiNguoiNhan.Text = BaoTin_DiaChiNguoiNhan;
                            txtBaoTin_HoVaTen.Text = BaoTin_HoVaTen;
                            txtBaoTin_SoDienThoai.Text = BaoTin_SoDienThoai;
                            txtBaoTin_Email.Text = BaoTin_Email;
                            chkLaNoiTru.Checked = LaNoiTru;
                            txtDiaChiCuThe.Text = DiaChiCuThe;
                        }
                        else
                            btnCapNhat.Visible = false;
                    }
                    else
                    {
                        spScript.InnerHtml = string.Format("<script>CapNhatHoSo.hienthithongbao();</script>");
                        btnCapNhat.Visible = false;
                    }
                }
                else
                {
                    spScript.InnerHtml = string.Format("<script>CapNhatHoSo.hienthithongbao();</script>");
                    btnCapNhat.Visible = false;
                }
            }
        }
        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            divThongBao.InnerText = "";
            string Email = txtEmail.Text.Trim();
            if (!IsValidEmail(Email))
            {
                divThongBao.InnerText = "Email sai quy cách";
                return;
            }
            string Mobile = txtMobile.Text.Trim();
            if (!IsMobile(Mobile))
            {
                divThongBao.InnerText = "Mobile sai quy cách";
                return;
            }
            string sql = string.Format(@"UPDATE [dbo].[AS_Academy_Student]
   SET [Email] = N'{0}'
      ,[Mobile] = N'{1}',[BaoTin_DiaChi] =  N'{2}'
      ,[BaoTin_HoVaTen] = N'{3}',[BaoTin_DiaChiNguoiNhan] =  N'{4}',[BaoTin_SoDienThoai] =  N'{5}'
      ,[BaoTin_Email] =  N'{6}',[LaNoiTru] = {7},[DiaChiCuThe] =  N'{8}'  WHERE ID={9}",
      Email, Mobile,txtBaoTin_DiaChi.Text.Trim()
      , txtBaoTin_HoVaTen.Text.Trim(), txtBaoTin_DiaChiNguoiNhan.Text.Trim(), txtBaoTin_SoDienThoai.Text.Trim()
      , txtBaoTin_Email.Text.Trim(), chkLaNoiTru.Checked ? 1 : 0, txtDiaChiCuThe.Text.Trim(), m_SinhVien.SinhVienID);
            int iReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(Nuce_Common.ConnectionString, CommandType.Text, sql);
            if (iReturn > 0)
            {
               
                    divThongBao.InnerHtml = "Cập nhật thông tin thành công";
                    return;
            }
            else
            {
                divThongBao.InnerText = "Cập nhật thất bại - lỗi hệ thống";
                return;
            }

        }
        private bool checkCapNhat(string value,string masv)
        {
            string[] values = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string v in values)
            {
                string v1 = v.Replace(" ", "");
                if (v1.Equals(masv.Substring(masv.Length - v1.Length)))
                    return true;
            }
            return false;
        }
        #region email
        public static string Send_Email(string Subject, string Message, string EmailTo)
        {
            try
            {
                // Gui Mail
                string strSubject = Subject;
                string strMessage = Message;
                string strEmailTo = EmailTo;

                string strEmailSend = "ks.ktdb@nuce.edu.vn";
                string strSmpt = "smtp.gmail.com";
                int iPort = 587;
                //int iIsSSL = 1;
                bool isSSL = true;
                string strUserNameSend = "ks.ktdb@nuce.edu.vn";
                string strPasswordSend = "khaosatktdb@123";
                //khaosatktdb@123
                return Send_Email(strEmailSend, strEmailTo, strSubject, strMessage, strSmpt, iPort, isSSL, strUserNameSend, strPasswordSend);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public static string Send_Email(string SendFrom, string SendTo, string Subject, string Body, string smtpAdr, int port, bool isSSL, string UserName, string Password)
        {
            try
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");


                bool result = regex.IsMatch(SendTo);
                if (result == false)
                {
                    return "Địa chỉ email không hợp lệ.";
                }
                else
                {
                    System.Net.Mail.SmtpClient smtp = new SmtpClient();
                    System.Net.Mail.MailMessage msg = new MailMessage(SendFrom, SendTo, Subject, Body);
                    msg.IsBodyHtml = true;
                    smtp.Host = smtpAdr;//"smtp.gmail.com";//Sử dụng SMTP của gmail 
                    smtp.Port = port;
                    smtp.Credentials = new System.Net.NetworkCredential(UserName, Password);
                    smtp.EnableSsl = isSSL;
                    smtp.Send(msg);
                    return "Email đã được gửi đến: " + SendTo + ".";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public static string Send_Email(string SendFrom, string SendTo, string Subject, string Body, bool isHtmlBody, string smtpAdr, int port, bool isSSL, string UserName, string Password)
        {
            try
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");


                bool result = regex.IsMatch(SendTo);
                if (result == false)
                {
                    return "Địa chỉ email không hợp lệ.";
                }
                else
                {
                    System.Net.Mail.SmtpClient smtp = new SmtpClient();
                    System.Net.Mail.MailMessage msg = new MailMessage(SendFrom, SendTo, Subject, Body);
                    msg.IsBodyHtml = isHtmlBody;
                    smtp.Host = smtpAdr;//"smtp.gmail.com";//Sử dụng SMTP của gmail 
                    smtp.Port = port;
                    smtp.Credentials = new System.Net.NetworkCredential(UserName, Password);
                    smtp.EnableSsl = isSSL;
                    smtp.Send(msg);
                    return "Email đã được gửi đến: " + SendTo + ".";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public static string Send_Email_With_Attachment(string SendTo, string SendFrom, string Subject, string Body, string AttachmentPath, string smtpAdr, int port, bool isSSL, string UserName, string Password)
        {
            try
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                string from = SendFrom;
                string to = SendTo;
                string subject = Subject;
                string body = Body;
                bool result = regex.IsMatch(to);
                if (result == false)
                {
                    return "Địa chỉ email không hợp lệ.";
                }
                else
                {
                    try
                    {
                        MailMessage em = new MailMessage(from, to, subject, body);
                        Attachment attach = new Attachment(AttachmentPath);
                        em.Attachments.Add(attach);
                        em.Bcc.Add(from);
                        System.Net.Mail.SmtpClient smtp = new SmtpClient();
                        smtp.Host = smtpAdr;//"smtp.gmail.com";//Ví dụ xử dụng SMTP của gmail 
                        smtp.Port = port;
                        smtp.EnableSsl = isSSL;
                        smtp.Credentials = new System.Net.NetworkCredential(UserName, Password);
                        smtp.Send(em);
                        return "";
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string Send_Email_With_BCC_Attachment(string SendTo, string SendBCC, string SendFrom, string Subject, string Body, string AttachmentPath, string smtpAdr, int port, bool isSSL, string UserName, string Password)
        {
            try
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                string from = SendFrom;
                string to = SendTo; //Danh sách email được ngăn cách nhau bởi dấu ";" 
                string subject = Subject;
                string body = Body;
                string bcc = SendBCC;
                bool result = true;
                String[] ALL_EMAILS = to.Split(';');
                foreach (string emailaddress in ALL_EMAILS)
                {
                    result = regex.IsMatch(emailaddress);
                    if (result == false)
                    {
                        return "Địa chỉ email không hợp lệ.";
                    }
                }
                if (result == true)
                {
                    try
                    {
                        MailMessage em = new MailMessage(from, to, subject, body);
                        Attachment attach = new Attachment(AttachmentPath);
                        em.Attachments.Add(attach);
                        em.Bcc.Add(bcc);


                        System.Net.Mail.SmtpClient smtp = new SmtpClient();
                        smtp.Host = smtpAdr;//"smtp.gmail.com";//Ví dụ xử dụng SMTP của gmail 
                        smtp.Port = port;
                        smtp.EnableSsl = isSSL;
                        smtp.Credentials = new System.Net.NetworkCredential(UserName, Password);
                        smtp.Send(em);

                        return "";
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
        #region validate
        public bool IsValidEmail(string emailaddress)
        {
            if (emailaddress.Length == 0)
                return false;
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public bool IsMobile(string mobile)
        {
            if (mobile.Length == 0)
                return false;
            try
            {

                Regex digitsOnly = new Regex(@"[^\d]");
                string _mobile = digitsOnly.Replace(mobile, "");
                if (_mobile[0] != '0')
                    return false;
                if (_mobile.Length < 10 || _mobile.Length > 11)
                    return false;
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        #endregion
    }
}