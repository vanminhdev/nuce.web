﻿using Newtonsoft.Json;
using Nuce.CTSV.ApiModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Nuce.CTSV
{
    public static class Utils
    {
        //public static int moduleCheckQlpm = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["MODQLPM"]);
        public static string DataTableToJSONWithJavaScriptSerializer(DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }
        public static string getNgayFromDate(DateTime dtInput)
        {
            string[] thu = new string[] { "Chủ nhật", "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy" };
            DateTime d = new DateTime(2010, 11, 22);
            int i = (int)dtInput.DayOfWeek;
            return thu[i];
        }
        public static string code_diem_danh = "DIEM_DANH";
        public static string session_sinhvien = "session_sinhvien";
        public static string session_kithi_lophoc_sinhvien = "session_kithi_lop_hoc_sinhvien";
        public static int tab_login_sinhvien = 120;
        public static int tab_changepassword_sinhvien = 121;
        public static int tab_trangchu_sinhvien = 119;
        public static string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
        public static String StripUnicodeCharactersFromString(string inputValue)
        {
            return Regex.Replace(inputValue, @"[^\u0000-\u007F]", String.Empty);
        }

        #region Role
        public static string role_Admin = "Administrators";
        public static string role_QuanTriThongTinChung = "Quản trị thông tin chung";
        public static string role_QuanTriPhongMay = "Quản trị phòng máy";
        public static string role_GiangVien = "Giảng viên";
        #endregion
        #region Random
        public static int getRandom(int begin, int end)
        {
            Random rnd = new Random();
            return rnd.Next(begin, end); //
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
                string strPasswordSend = "khaosatktdb";
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
        #region loginwithgoogle
        //public static string lwg_clientid = "747341024576-mud1ao0e5jij2dkm56sfu0i0fqv9ggc0.apps.googleusercontent.com";
        ////your client secret  
        //public static string lwg_clientsecret = "0TQze0o2lGXb4gUw77S2vw7l";
        ////your redirection url  
        //public static string lwg_redirection_url = "http://localhost:9000/Login";
        //public static string lwg_url = "https://accounts.google.com/o/oauth2/token";
        public static string lwg_clientid = "834469135571-lin0veiq4vfl6i90tb7l8t28ccagojln.apps.googleusercontent.com";
        //your client secret  
        public static string lwg_clientsecret = "PGqQbo6WnK-a-jWD0OjSJwyB";
        //your redirection url  
        public static string lwg_redirection_url = "https://sv.nuce.edu.vn/Login";
        public static string lwg_url = "https://accounts.google.com/o/oauth2/token";
        #endregion

    }

    public class ReCaptchaClass
    {
        public static string Validate(string EncodedResponse)
        {
            var client = new System.Net.WebClient();

            string PrivateKey = "6Lf3Lc8ZAAAAAAYtaO7VQ2tIfilYmmH8Sw8atDqU";

            var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey, EncodedResponse));

            var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ReCaptchaClass>(GoogleReply);

            return captchaResponse.Success.ToLower();
        }

        [JsonProperty("success")]
        public string Success
        {
            get { return m_Success; }
            set { m_Success = value; }
        }

        private string m_Success;
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get { return m_ErrorCodes; }
            set { m_ErrorCodes = value; }
        }


        private List<string> m_ErrorCodes;
    }

    public static class CustomizeHttp
    {
        public static string API_URI = ConfigurationManager.AppSettings["API_URL"];

        public static Uri BASE_ADDRESS = new Uri(API_URI);

        public static async Task<HttpResponseMessage> SendRequest(HttpRequest Request, HttpResponse Response, HttpMethod method, string path, string content)
        {
            using (HttpClientHandler handler = new HttpClientHandler { UseCookies = false })
            using (HttpClient client = new HttpClient(handler) { BaseAddress = BASE_ADDRESS } )
            {
                string cookies = "";
                foreach (var key in Request.Cookies.AllKeys)
                {
                    cookies += $"{key}={Request.Cookies[key].Value};";
                }
                HttpRequestMessage req = CreateRequest(method, path, cookies, content);
                var firstResponse = await client.SendAsync(req);
                req.Dispose();
                if (firstResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var endPoint = $"/api/User/refreshToken";
                    var refreshTokenRequest = new HttpRequestMessage(HttpMethod.Post, endPoint);
                    refreshTokenRequest.Headers.Add("Cookie", cookies);
                    var refreshResponse = await client.SendAsync(refreshTokenRequest);
                    refreshTokenRequest.Dispose();
                    if (refreshResponse.IsSuccessStatusCode)
                    {
                        var newCookies = refreshResponse.Headers.GetValues("Set-Cookie");
                        foreach (var responseCookie in newCookies)
                        {
                            var splited = responseCookie.Split(';');
                            var value = splited[0].Split('=');
                            var expires = splited[1].Split('=');
                            var tmpCookie = new HttpCookie(value[0], value[1]);

                            if (splited.Length > 1)
                            {
                                DateTime expireDate;
                                bool parsed = DateTime.TryParse(expires[1], out expireDate);
                                if (parsed)
                                {
                                    tmpCookie.Expires = expireDate;
                                }
                            }

                            Request.Cookies.Set(tmpCookie);
                            Response.Cookies.Set(tmpCookie);
                        }
                        cookies = "";
                        foreach (var key in Request.Cookies.AllKeys)
                        {
                            cookies += $"{key}={Request.Cookies[key].Value};";
                        }
                        var newReq = CreateRequest(method, path, cookies, content);
                        return await client.SendAsync(newReq);
                    } 
                    else
                    {
                        foreach (var cookie in Request.Cookies.AllKeys)
                        {
                            var clearCookie = new HttpCookie(cookie);
                            clearCookie.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(clearCookie);
                        }
                        Response.Redirect("/Login.aspx");
                    }
                    return refreshResponse;
                }
                return firstResponse;
            }
        }
        public static async Task<HttpResponseMessage> SendRequest(HttpRequest Request, HttpResponse Response, string path, UploadFileModel model)
        {
            using (HttpClientHandler handler = new HttpClientHandler { UseCookies = false })
            using (HttpClient client = new HttpClient(handler) { BaseAddress = BASE_ADDRESS })
            {

                string cookies = "";
                foreach (var key in Request.Cookies.AllKeys)
                {
                    cookies += $"{key}={Request.Cookies[key].Value};";
                }
                var req = CreateRequest(path, cookies, model);
                var firstResponse = await client.SendAsync(req);
                req.Dispose();

                if (firstResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var endPoint = $"/api/User/refreshToken";
                    var refreshTokenRequest = new HttpRequestMessage(HttpMethod.Post, endPoint);
                    refreshTokenRequest.Headers.Add("Cookie", cookies);
                    var refreshResponse = await client.SendAsync(refreshTokenRequest);
                    refreshTokenRequest.Dispose();
                    if (refreshResponse.IsSuccessStatusCode)
                    {
                        var newCookies = refreshResponse.Headers.GetValues("Set-Cookie");
                        foreach (var responseCookie in newCookies)
                        {
                            var splited = responseCookie.Split(';');
                            var value = splited[0].Split('=');
                            var expires = splited[1].Split('=');
                            var tmpCookie = new HttpCookie(value[0], value[1]);

                            if (splited.Length > 1)
                            {
                                DateTime expireDate;
                                bool parsed = DateTime.TryParse(expires[1], out expireDate);
                                if (parsed)
                                {
                                    tmpCookie.Expires = expireDate;
                                }
                            }

                            Request.Cookies.Set(tmpCookie);
                            Response.Cookies.Set(tmpCookie);
                        }
                        cookies = "";
                        foreach (var key in Request.Cookies.AllKeys)
                        {
                            cookies += $"{key}={Request.Cookies[key].Value};";
                        }
                        var newReq = CreateRequest(path, cookies, model);
                        return await client.SendAsync(newReq);
                    }
                    else
                    {
                        foreach (var cookie in Request.Cookies.AllKeys)
                        {
                            var clearCookie = new HttpCookie(cookie);
                            clearCookie.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(clearCookie);
                        }
                        Response.Redirect("/Login.aspx");
                    }
                    return refreshResponse;
                }
                return firstResponse;
            }
        }
        public static async Task<T> DeserializeAsync<T>(HttpContent responseContent){
            string content = await responseContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
        private static HttpRequestMessage CreateRequest(HttpMethod method, string path, string cookies, string content)
        {
            HttpRequestMessage req = new HttpRequestMessage(method, path);
            req.Headers.Add("Cookie", cookies);
            if (method != HttpMethod.Get)
            {
                req.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }
            return req;
        }
        private static HttpRequestMessage CreateRequest(string path, string cookies, UploadFileModel model)
        {
            var req = new MultipartFormDataContent();
            req.Headers.Add("Cookie", cookies);
            req.Headers.Add("ContentType", model.ContentType ?? "");
            req.Add(new StreamContent(new MemoryStream(model.Content)), model.Key, model.FileName);
            return new HttpRequestMessage(HttpMethod.Post, path) { Content = req }; ;
        }
    }

    public static class Development
    {
        private static string API_URI = ConfigurationManager.AppSettings["DEV"];
        public static bool Enabled = API_URI == "1";
    }
}

