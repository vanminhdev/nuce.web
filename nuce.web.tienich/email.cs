using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace nuce.web.tienich
{
    public static class email
    {
        #region email
        public static string Send_Email(string Subject, string Message, string EmailTo, string UserName, string Password)
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
                //string strUserNameSend = "ks.ktdb@nuce.edu.vn";
                //string strPasswordSend = "khaosatktdb@123";
                //khaosatktdb@123
                return Send_Email(strEmailSend, strEmailTo, strSubject, strMessage, strSmpt, iPort, isSSL, UserName, Password);
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
                    //smtp.UseDefaultCredentials = false;
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
        public static bool IsValidEmail(string emailaddress)
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
        public static bool IsMobile(string mobile)
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
        #region Sinh ma Ngau nhien
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder sb = new StringBuilder();
            char c;
            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
                sb.Append(c);
            }
            if (lowerCase)
            {
                return sb.ToString().ToLower();
            }
            return sb.ToString();
        }
        #endregion
    }
}
