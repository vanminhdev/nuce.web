using Newtonsoft.Json;
using nuce.web.data;
using System;
using System.Data;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class CapNhatHoSo : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                var res = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Get, $"{ApiModels.ApiEndPoint.GetAllowUpdateStudent}/{m_SinhVien.MaSV}", "");
                if (!res.IsSuccessStatusCode)
                {
                    return;
                }
                var response = await CustomizeHttp.DeserializeAsync<ApiModels.StudentAllowUpdateModel>(res.Content);
                //Kiểm tra xem nếu chưa đến đợt cập nhật thì thông báo cho người dùng và thoát
                if (!response.Enabled)
                {
                    spScript.InnerHtml = string.Format("<script>CapNhatHoSo.hienthithongbao();</script>");
                    btnCapNhat.Visible = false;
                    return;
                }

                var student = response.Student;
                if (student != null)
                {
                    // Xử lý dữ liệu sinh viên
                    string Email = (student.Email ?? "").Trim();
                    string Mobile = (student.Mobile ?? "").Trim();
                    string BaoTin_DiaChi = (student.BaoTinDiaChi ?? "").Trim();
                    string BaoTin_HoVaTen = (student.BaoTinHoVaTen ?? "").Trim();
                    string BaoTin_DiaChiNguoiNhan = (student.BaoTinDiaChiNguoiNhan ?? "").Trim();
                    string BaoTin_SoDienThoai = (student.BaoTinSoDienThoai ?? "").Trim();
                    string BaoTin_Email = (student.BaoTinEmail ?? "").Trim();
                    bool LaNoiTru = student.LaNoiTru ?? false;
                    string DiaChiCuThe = (student.DiaChiCuThe ?? "").Trim();

                    txtEmail.Text = Email;
                    txtMobile.Text = Mobile;
                    txtBaoTin_DiaChi.Text = BaoTin_DiaChi;
                    txtBaoTin_DiaChiNguoiNhan.Text = BaoTin_DiaChiNguoiNhan;
                    txtBaoTin_HoVaTen.Text = BaoTin_HoVaTen;
                    txtBaoTin_SoDienThoai.Text = BaoTin_SoDienThoai;
                    txtBaoTin_Email.Text = BaoTin_Email;
                    chkLaNoiTru.Checked = LaNoiTru;
                    txtDiaChiCuThe.Text = DiaChiCuThe;

                    spScript.InnerHtml = string.Format($"<script>CapNhatHoSo.initSelectForm('{(student.HkttTinh ?? "").Trim()}', '{(student.HkttQuan ?? "").Trim()}', '{(student.HkttPhuong ?? "").Trim()}');</script>");
                }
            }
        }
        protected async void btnCapNhat_Click(object sender, EventArgs e)
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

            ApiModels.CapNhatHoSoModel model = new ApiModels.CapNhatHoSoModel
            {
                Email = Email,
                Mobile = Mobile,
                DiaChiBaotin = txtBaoTin_DiaChi.Text.Trim(),
                HoTenBaoTin = txtBaoTin_HoVaTen.Text.Trim(),
                DiaChiNguoiNhanBaotin = txtBaoTin_DiaChiNguoiNhan.Text.Trim(),
                MobileBaoTin = txtBaoTin_SoDienThoai.Text.Trim(),
                EmailBaoTin = txtBaoTin_Email.Text.Trim(),
                DiaChiCuThe = txtDiaChiCuThe.Text.Trim(),
                CoNoiOCuThe = chkLaNoiTru.Checked,
                PhuongXa = Request.Form[slPhuong.UniqueID],
                QuanHuyen = Request.Form[slQuan.UniqueID],
                TinhThanhPho = Request.Form[slThanhPho.UniqueID]
            };

            var body = JsonConvert.SerializeObject(model);
            var response = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Post, ApiModels.ApiEndPoint.PostStudentBasicUpdate, body);

            if (response.IsSuccessStatusCode)
            {
                divThongBao.InnerHtml = "Cập nhật thông tin thành công";
                spScript.InnerHtml = string.Format($"<script>CapNhatHoSo.initSelectForm('{model.TinhThanhPho}', '{model.QuanHuyen}', '{model.PhuongXa}');</script>");
                return;
            }
            else
            {
                divThongBao.InnerText = "Cập nhật thất bại - lỗi hệ thống";
                return;
            }

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