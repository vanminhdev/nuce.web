using Newtonsoft.Json;
using Nuce.CTSV.ApiModels;
using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class DieuHuong : Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {

            var key = Request.QueryString["key"];
            //Kiểm tra đăng nhập
            await DangNhapTuMotCua(key);
        }

        

        private void showThongBao(string msg)
        {
            divThongBaoCapNhat.InnerText = msg;
            string script = $"<script>$('#myModalThongBao').modal()</script>";
            spAlert.InnerHtml = script;
        }

        private async Task DangNhapTuMotCua(string key)
        {
            //Kiểm tra đăng nhập
            StudentModel student = null;
            using (HttpClient httpClient = new HttpClient())
            {
                string body = JsonConvert.SerializeObject(new { key });
                var content = new StringContent(body, Encoding.UTF8, "application/json");

                var res = await httpClient.PostAsync($"{CustomizeHttp.API_URI}/api/User/LoginStudentMotCua", content);
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
                else if (res.StatusCode == HttpStatusCode.NotFound)
                {
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
                {
                    SinhVien.IMG = "/Data/images/noimage_human.png";
                }

                Session[Utils.session_sinhvien] = SinhVien;
                Response.Redirect("/dichvusinhvien.aspx", false);
            }
            else
            {
                spAlert.InnerHtml = string.Format(@"<div class='alert alert-warning alert-dismissible' style='position: absolute; top: 0; right: 0;'>
                                                <a href = '#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>
                                        {0}</div>", "Không đúng tên đăng nhập");
            }
        }
    }
}