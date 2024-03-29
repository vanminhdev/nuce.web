﻿using Newtonsoft.Json;
using Nuce.CTSV.ApiModels;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class Login : Page
    {
        private bool IsPageRefresh = false;
        #region Google
        //your client id  
        string clientid = Utils.lwg_clientid;
        //your client secret  
        string clientsecret = Utils.lwg_clientsecret;
        //your redirection url  
        string redirection_url = Utils.lwg_redirection_url;
        string url = Utils.lwg_url;
        public class Tokenclass
        {
            public string access_token
            {
                get;
                set;
            }
            public string token_type
            {
                get;
                set;
            }
            public int expires_in
            {
                get;
                set;
            }
            public string refresh_token
            {
                get;
                set;
            }
        }
        public class Userclass
        {
            public string id
            {
                get;
                set;
            }
            public string name
            {
                get;
                set;
            }
            public string given_name
            {
                get;
                set;
            }
            public string family_name
            {
                get;
                set;
            }
            public string link
            {
                get;
                set;
            }
            public string picture
            {
                get;
                set;
            }
            public string gender
            {
                get;
                set;
            }
            public string locale
            {
                get;
                set;
            }
            public string email
            {
                get;
                set;
            }
        }
        public async Task GetToken(string code)
        {
            string poststring = "grant_type=authorization_code&code=" + code + "&client_id=" + clientid + "&client_secret=" + clientsecret + "&redirect_uri=" + redirection_url + "&scope=email profile";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            UTF8Encoding utfenc = new UTF8Encoding();
            byte[] bytes = utfenc.GetBytes(poststring);
            Stream outputstream = null;
            try
            {
                request.ContentLength = bytes.Length;
                outputstream = request.GetRequestStream();
                outputstream.Write(bytes, 0, bytes.Length);
            }
            catch { }
            var response = (HttpWebResponse)request.GetResponse();
            var streamReader = new StreamReader(response.GetResponseStream());
            string responseFromServer = streamReader.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            Tokenclass obj = js.Deserialize<Tokenclass>(responseFromServer);
            await GetuserProfile(obj.access_token);
        }
        public async Task GetuserProfile(string accesstoken)
        {
            string url = "https://www.googleapis.com/oauth2/v2/userinfo?alt=json&access_token=" + accesstoken + "";
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            response.Close();
            JavaScriptSerializer js = new JavaScriptSerializer();
            Userclass userinfo = js.Deserialize<Userclass>(responseFromServer);
            
            StudentModel student = null;
            using (HttpClient httpClient = new HttpClient())
            {
                string body = JsonConvert.SerializeObject(new { username = userinfo.email, password = userinfo.email, loginUserType = 1 });
                var content = new StringContent(body, Encoding.UTF8, "application/json");

                var res = await httpClient.PostAsync($"{CustomizeHttp.API_URI}/{ApiModels.ApiEndPoint.PostLoginEduEmail}", content);
                if (res.IsSuccessStatusCode)
                {
                    var cookies = res.Headers.GetValues("Set-Cookie");
                    foreach (var responseCookie in cookies)
                    {
                        var splited = responseCookie.Split(';')[0].Split('=');
                        Request.Cookies.Add(new HttpCookie(splited[0], splited[1]));
                        Response.Cookies.Add(new HttpCookie(splited[0], splited[1]));
                    }
                    student = await CustomizeHttp.DeserializeAsync<StudentModel>(res.Content);
                } 
                else if (res.StatusCode == HttpStatusCode.NotFound) {
                    spAlert.InnerHtml = string.Format(@"<div class='alert alert-warning alert-dismissible' style='position: absolute; top: 0; right: 0;'>
                                            <a href = '#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>
                {0}</div>", "Sinh viên không tồn tại");
                    return;
                }
                else
                {
                    spAlert.InnerHtml = string.Format(@"<div class='alert alert-warning alert-dismissible' style='position: absolute; top: 0; right: 0;'>
                                            <a href = '#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>
                {0}</div>", "Lỗi hệ thống");
                    return;
                }
            }
            if (student != null)
            {
                nuce.web.model.SinhVien SinhVien = new nuce.web.model.SinhVien();
                string strFullName = student.FulName;
                string[] strFullNames = strFullName.Split(new char[] { ' ' });
                SinhVien.Ho = strFullName;
                SinhVien.Ten = strFullNames[strFullNames.Length - 1];
                //SinhVien.TrangThai = int.Parse(dtData.Rows[0]["status"].ToString());
                SinhVien.SinhVienID = (int)student.Id;
                SinhVien.Email = student.EmailNhaTruong ?? "";
                SinhVien.Mobile = student.Mobile ?? "";
                SinhVien.MaSV = student.Code;
                string File1 = student.File1 ?? "";
                if (!File1.Trim().Equals(""))
                {
                    SinhVien.IMG = File1;
                }
                else
                    SinhVien.IMG = "/Data/images/noimage_human.png";

                Session[Utils.session_sinhvien] = SinhVien;
                Response.Redirect("/dichvusinhvien.aspx");
            }
            else
            {
                spAlert.InnerHtml = string.Format(@"<div class='alert alert-warning alert-dismissible' style='position: absolute; top: 0; right: 0;'>
                                                <a href = '#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>
                                        {0}</div>", "Không đúng tên đăng nhập");
            }

            //Kiem tra ho ten xem co trung khong sau do lay trong csdl

            //imgprofile.ImageUrl = userinfo.picture;
            //lblid.Text = userinfo.id;
            //lblgender.Text = userinfo.gender;
            //lbllocale.Text = userinfo.locale;
            //lblname.Text = userinfo.name;
            //hylprofile.NavigateUrl = userinfo.link;
        }
        #endregion
        protected async void Page_Load(object sender, EventArgs e)
        {
            //https://www.oauth.com/oauth2-servers/signing-in-with-google/verifying-the-user-info/
            //https://www.c-sharpcorner.com/Blogs/login-with-google-account-api-in-asp-net-and-get-google-plus-profile-details-in-c-sharp
            //747341024576-mud1ao0e5jij2dkm56sfu0i0fqv9ggc0.apps.googleusercontent.com
            //0TQze0o2lGXb4gUw77S2vw7l
            if (!IsPostBack)
            {
                ViewState["postGuids"] = System.Guid.NewGuid().ToString();
                Session["postGuid"] = ViewState["postGuids"].ToString();
                if (Request.QueryString["code"] != null)
                {
                    await GetToken(Request.QueryString["code"].ToString());
                }
            } else
            {
                if (ViewState["postGuids"].ToString() != Session["postGuid"].ToString())
                {
                    IsPageRefresh = true;
                }
                Session["postGuid"] = System.Guid.NewGuid().ToString();
                ViewState["postGuids"] = Session["postGuid"].ToString();
            }
        }

        private void showThongBao(string msg)
        {
            divThongBaoCapNhat.InnerText = msg;
            string script = $"<script>$('#myModalThongBao').modal()</script>";
            spAlert.InnerHtml = script;
        }

        protected async void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (!IsPageRefresh)
            {
                string strMaSV = txtMaDangNhap.Text.Trim();
                string strMatKhau = txtMatKhau.Text.Trim();

                bool maSvEmpty = string.IsNullOrEmpty(strMaSV);
                bool matKhauEmpty = string.IsNullOrEmpty(strMatKhau);
                if (maSvEmpty)
                {
                    txtMaDangNhap.Focus();
                }
                else if (matKhauEmpty)
                {
                    txtMatKhau.Focus();
                }
                if (maSvEmpty || matKhauEmpty)
                {
                    showThongBao("Bạn không được để trắng tên đăng nhập hoặc mật khẩu");
                    return;
                }

                //Kiểm tra đăng nhập

                using (HttpClient httpClient = new HttpClient())
                {
                    string body = JsonConvert.SerializeObject(new { username = strMaSV, password = strMatKhau, loginUserType = 1 });
                    var content = new StringContent(body, Encoding.UTF8, "application/json");

                    var res = await httpClient.PostAsync($"{CustomizeHttp.API_URI}/api/User/login", content);
                    if (res.IsSuccessStatusCode)
                    {
                        var cookies = res.Headers.GetValues("Set-Cookie");
                        foreach (var responseCookie in cookies)
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
                    }
                    else if (res.StatusCode == HttpStatusCode.BadGateway)
                    {
                        showThongBao(@"Chức năng đăng nhập qua mã số sinh viên tạm thời đang nâng cấp. 
                                Vui lòng đăng nhập qua email bằng cách nhấp chuột vào nút 'Đăng nhập qua email @nuce.edu.vn'");
                        return;
                    }
                    else
                    {
                        try
                        {
                            var error = await CustomizeHttp.DeserializeAsync<ResponseBody>(res.Content);
                            showThongBao(error.Message);
                        }
                        catch (Exception ex)
                        {
                            showThongBao("Thông tin đăng nhập sai");
                        }
                        return;
                    }
                }

                StudentModel student = null;

                var studentResponse = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Get, $"{ApiEndPoint.GetStudentInfo}/{strMaSV}", "");
                if (studentResponse.IsSuccessStatusCode)
                {
                    student = await CustomizeHttp.DeserializeAsync<StudentModel>(studentResponse.Content);
                }
                if (student != null)
                {
                    nuce.web.model.SinhVien SinhVien = new nuce.web.model.SinhVien();
                    string strFullName = student.FulName;
                    string[] strFullNames = strFullName.Split(new char[] { ' ' });

                    SinhVien.Ho = strFullName;
                    SinhVien.Ten = strFullNames[strFullNames.Length - 1];
                    SinhVien.MaSV = student.Code;
                    SinhVien.SinhVienID = Convert.ToInt32(student.Id);
                    SinhVien.Email = student.EmailNhaTruong ?? "";
                    SinhVien.Mobile = student.Mobile ?? "";
                    string File1 = student.File1 ?? "";

                    if (!File1.Trim().Equals(""))
                    {
                        SinhVien.IMG = $"{CustomizeHttp.API_URI}/{File1}";
                    }
                    else
                    {
                        SinhVien.IMG = "/Data/images/noimage_human.png";
                    }

                    Session[Utils.session_sinhvien] = SinhVien;
                    Response.Redirect("/DichVuSinhVien.aspx", false);
                }
                else
                {
                    showThongBao("Không tồn tại dữ liệu sinh viên");
                }
            }
        }
        
    }
}