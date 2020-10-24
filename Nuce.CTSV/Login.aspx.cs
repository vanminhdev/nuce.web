using Newtonsoft.Json;
using Nuce.CTSV.Services;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class Login : Page
    {
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
        public void GetToken(string code)
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
            GetuserProfile(obj.access_token);
        }
        public void GetuserProfile(string accesstoken)
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
            //txtMaDangNhap.Text = userinfo.email;
            //string strMaSV = txtMaDangNhap.Text.Trim();
            //string strSql = string.Format("SELECT * FROM [dbo].[AS_Academy_Student] where Code='{0}'", strMaSV);
            string strSql = string.Format("SELECT * FROM [dbo].[AS_Academy_Student] where EmailNhaTruong=@Param1 and DaXacThucEmailNhaTruong=1");

            strSql += string.Format(@"INSERT INTO [dbo].[AS_Logs]
           ([UserId]
           ,[UserCode]
           ,[Status]
           ,[Code]
           ,[Message]
           ,[CreatedTime])
     VALUES
           (-1
           ,'{0}'
           ,1
           ,'LOGIN'
           ,'{2}'
           ,'{1}') ;", userinfo.email, DateTime.Now, 3);

            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@Param1", userinfo.email);
            //sqlParams[0].ParameterName = "@Param1";
            //sqlParams[0].SqlDbType = SqlDbType.VarChar;
            //sqlParams[0].Value = strMaSV;
            DataTable dtData = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(nuce.web.data.Nuce_Common.ConnectionString, CommandType.Text, strSql, sqlParams).Tables[0];
            if (dtData != null && dtData.Rows.Count > 0)
            {
                nuce.web.model.SinhVien SinhVien = new nuce.web.model.SinhVien();
                string strFullName = dtData.Rows[0]["FulName"].ToString();
                string[] strFullNames = strFullName.Split(new char[] { ' ' });
                SinhVien.Ho = strFullName;
                SinhVien.Ten = strFullNames[strFullNames.Length - 1];
                //SinhVien.TrangThai = int.Parse(dtData.Rows[0]["status"].ToString());
                SinhVien.SinhVienID = int.Parse(dtData.Rows[0]["ID"].ToString());
                SinhVien.Email = dtData.Rows[0].IsNull("EmailNhaTruong") ? "" : dtData.Rows[0]["EmailNhaTruong"].ToString();
                SinhVien.Mobile = dtData.Rows[0]["Mobile"].ToString();
                SinhVien.MaSV = dtData.Rows[0]["Code"].ToString();
                string File1 = dtData.Rows[0].IsNull("File1") ? "" : dtData.Rows[0]["File1"].ToString();
                if (!File1.Trim().Equals(""))
                {
                    SinhVien.IMG = File1;
                }
                else
                    SinhVien.IMG = "/Data/images/noimage_human.png";

                Session[Utils.session_sinhvien] = SinhVien;
                Response.Redirect("/Default.aspx");
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
        protected void Page_Load(object sender, EventArgs e)
        {
            //https://www.oauth.com/oauth2-servers/signing-in-with-google/verifying-the-user-info/
            //https://www.c-sharpcorner.com/Blogs/login-with-google-account-api-in-asp-net-and-get-google-plus-profile-details-in-c-sharp
            //747341024576-mud1ao0e5jij2dkm56sfu0i0fqv9ggc0.apps.googleusercontent.com
            //0TQze0o2lGXb4gUw77S2vw7l
            if (!IsPostBack)
            {
                if (Request.QueryString["code"] != null)
                {
                    GetToken(Request.QueryString["code"].ToString());
                }
            }
        }

        protected async void btnDangNhap_Click(object sender, EventArgs e)
        {
            string strMaSV = txtMaDangNhap.Text.Trim();
            string strMatKhau = txtMatKhau.Text.Trim();
            if (strMaSV == "" || strMatKhau == "")
            {
                spAlert.InnerHtml = string.Format(@"<div class='alert alert-warning alert-dismissible' style='position: absolute; top: 0; right: 0;'>
                                                <a href = '#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>
            {0}</div>", "Bạn không được để trắng tên đăng nhập hoặc mật khẩu");
                return;
            }

            //Kiểm tra đăng nhập

            Service sv = new Service();
            services_direct.Service sv_1 = new services_direct.Service();
            int iTypeDichVu = -1;
            string apiUrl = ConfigurationManager.AppSettings["API_URL"];

            HttpClient httpClient = new HttpClient();
            string body = JsonConvert.SerializeObject(new { username = strMaSV, password = strMatKhau, isStudent = true });
            var content = new StringContent(body, Encoding.UTF8, "application/json");


            try
            {
                var res = await httpClient.PostAsync($"{apiUrl}/api/User/login", content);
                if (res.IsSuccessStatusCode)
                {
                    iTypeDichVu = 1;
                }
                else
                {
                    spAlert.InnerHtml = string.Format(@"<div class='alert alert-warning alert-dismissible' style='position: absolute; top: 0; right: 0;'>
                                                <a href = '#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>
                    {0}</div>", "Thông tin đăng nhập sai");
                    return;
                }
            }
            catch (Exception ex)
            {
                iTypeDichVu = 999;
            }

            #region old
            //try
            //{
            //    if (sv.authen(strMaSV, strMatKhau) <= 0)
            //    {
            //        spAlert.InnerHtml = string.Format(@"<div class='alert alert-warning alert-dismissible' style='position: absolute; top: 0; right: 0;'>
            //                                    <a href = '#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>
            //{0}</div>", "Thông tin đăng nhập sai");
            //        return;
            //    }
            //    iTypeDichVu = 1;
            //}
            //catch (Exception ex)
            //{
            //    try
            //    {
            //        if (sv_1.authen(strMaSV, strMatKhau) <= 0)
            //        {
            //            spAlert.InnerHtml = string.Format(@"<div class='alert alert-warning alert-dismissible' style='position: absolute; top: 0; right: 0;'>
            //                                    <a href = '#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>
            //{0}</div>", "Thông tin đăng nhập sai");
            //            return;
            //        }
            //        iTypeDichVu = 2;
            //    }
            //    catch (Exception ex1)
            //    {
            //        iTypeDichVu = 999;
            //    }
            //}
            //string strSql = string.Format("SELECT * FROM [dbo].[AS_Academy_Student] where Code='{0}'", strMaSV);
            #endregion
            body = JsonConvert.SerializeObject(new
            {
                userId = -1,
                userCode = strMaSV,
                status = 1,
                code = "LOGIN",
                message = iTypeDichVu.ToString()
            });
            content = new StringContent(body, Encoding.UTF8, "application/json");
            await httpClient.PostAsync($"{apiUrl}/api/Log/insert-log", content);
            
            StudentModel student = null;
            var baseAddress = new Uri(apiUrl);
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                foreach (var key in Request.Cookies.AllKeys)
                {
                    cookieContainer.Add(baseAddress, new Cookie(key, Request.Cookies[key].Value));
                }
                var message = new HttpRequestMessage(HttpMethod.Get, $"/api/Student/student/{strMaSV}");
                
                HttpResponseMessage studentResponse = await client.SendAsync(message);
                if (studentResponse.IsSuccessStatusCode)
                {
                    var strResponse = await studentResponse.Content.ReadAsStringAsync();
                    student = JsonConvert.DeserializeObject<StudentModel>(strResponse);
                } else if (studentResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var endPoint = $"/api/User/refreshToken";
                    message = new HttpRequestMessage(HttpMethod.Post, endPoint);
                    await client.SendAsync(message);
                }
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
                    SinhVien.IMG = File1;
                }
                else
                    SinhVien.IMG = "/Data/images/noimage_human.png";
                Session[Utils.session_sinhvien] = SinhVien;
                Response.Redirect("/DichVuSinhVien.aspx", false);
            }
            else
            {
                spAlert.InnerHtml = string.Format(@"<div class='alert alert-warning alert-dismissible' style='position: absolute; top: 0; right: 0;'>
                                                <a href = '#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>
                {0}</div>", "Không tồn tại dữ liệu sinh viên");
            }
        }
        public class StudentModel
        {
            public long Id { get; set; }
            public string Code { get; set; }
            public string FulName { get; set; }
            public long? ClassId { get; set; }
            public string ClassCode { get; set; }
            public string DateOfBirth { get; set; }
            public string BirthPlace { get; set; }
            public string Email { get; set; }
            public string Mobile { get; set; }
            public Guid? KeyAuthorize { get; set; }
            public int? Status { get; set; }
            public DateTime? CreatedDate { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public DateTime? NgaySinh { get; set; }
            public string DanToc { get; set; }
            public string TonGiao { get; set; }
            public string HkttSoNha { get; set; }
            public string HkttPho { get; set; }
            public string HkttPhuong { get; set; }
            public string HkttQuan { get; set; }
            public string HkttTinh { get; set; }
            public string Cmt { get; set; }
            public DateTime? CmtNgayCap { get; set; }
            public string CmtNoiCap { get; set; }
            public string NamTotNghiepPtth { get; set; }
            public DateTime? NgayVaoDoan { get; set; }
            public DateTime? NgayVaoDang { get; set; }
            public string DiemThiPtth { get; set; }
            public string KhuVucHktt { get; set; }
            public string DoiTuongUuTien { get; set; }
            public bool? DaTungLamCanBoLop { get; set; }
            public bool? DaTungLamCanBoDoan { get; set; }
            public bool? DaThamGiaDoiTuyenThiHsg { get; set; }
            public string BaoTinDiaChi { get; set; }
            public string BaoTinHoVaTen { get; set; }
            public string BaoTinDiaChiNguoiNhan { get; set; }
            public string BaoTinSoDienThoai { get; set; }
            public string BaoTinEmail { get; set; }
            public bool? LaNoiTru { get; set; }
            public string DiaChiCuThe { get; set; }
            public string File1 { get; set; }
            public string File2 { get; set; }
            public string File3 { get; set; }
            public string Mobile1 { get; set; }
            public string Email1 { get; set; }
            public string GioiTinh { get; set; }
            public string EmailNhaTruong { get; set; }
            public bool? DaXacThucEmailNhaTruong { get; set; }
        }
    }
}