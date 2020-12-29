using Newtonsoft.Json;
using nuce.web.data;
using Nuce.CTSV.ApiModels;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class CapLaiTheSinhVien_YeuCauMoi : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var studentAva = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Get, $"{ApiEndPoint.GetStudentAva}{m_SinhVien.MaSV}?width=1&height=1", "");
                if (!studentAva.IsSuccessStatusCode)
                {
                    divBtnContainer.Visible = false;
                    divThongBao.InnerHtml = $"Sinh viên chưa có thẻ. Trực tiếp lên Phòng Công tác Chính trị và Quản lý Sinh viên tại phòng 302, nhà A1 để làm việc";
                    return;
                }
                var studentResponse = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Get, $"{ApiEndPoint.GetStudentInfo}/{m_SinhVien.MaSV}", "");
                string thongBao = "";
                if (studentResponse.IsSuccessStatusCode)
                {
                    var strResponse = await studentResponse.Content.ReadAsStringAsync();
                    var student = JsonConvert.DeserializeObject<StudentModel>(strResponse);

                    if (string.IsNullOrEmpty(student.HkttTinh?.Trim()))
                    {
                        thongBao += " tỉnh/thành phố";
                    }

                    if (string.IsNullOrEmpty(student.HkttQuan?.Trim()))
                    {
                        thongBao += $"{(thongBao != "" ? "," : "")} quận/huyện";
                    }

                    if (string.IsNullOrEmpty(student.HkttPhuong?.Trim()))
                    {
                        thongBao += $"{(thongBao != "" ? "," : "")} phường/xã";
                    }

                    if (!string.IsNullOrEmpty(thongBao))
                    {
                        divBtnContainer.Visible = false;
                        divThongBao.InnerHtml = $"Yêu cầu cập nhật<a href=\"/capnhathoso.aspx\">{thongBao}</a></br >";
                        return;
                    }
                }
            }
        }
        protected async void btnCapNhat_Click(object sender, EventArgs e)
        {
            //Kiem tra captcha
            //if(!(txtCaptcha.Text.ToLower() == Session["CaptchaVerify"].ToString()))
            //{
            //    divThongBao.InnerHtml = "Bạn nhập sai mã Captcha";
            //    return;
            //}
            if (!Development.Enabled)
            {
                string EncodedResponse = Request.Form["g-Recaptcha-Response"];
                bool IsCaptchaValid = (ReCaptchaClass.Validate(EncodedResponse) == "true" ? true : false);

                if (!IsCaptchaValid)
                {
                    divThongBao.InnerHtml = "Bạn chưa xác thực Captcha";
                    return;
                }
            }

            string strRandom = nuce.web.tienich.email.RandomString(6, false);
            var body = new AddDichVuModel()
            {
                type = (int)DichVu.CapLaiThe,
                maXacNhan = strRandom
            };

            var jsonBody = JsonConvert.SerializeObject(body);
            var response = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Post, "api/DichVu/add", jsonBody);

            if (response.IsSuccessStatusCode)
            {
                divThongBao.InnerHtml = "Thêm mới dịch vụ thành công";
                divThongBaoCapNhat.InnerHtml = "Thêm mới dịch vụ thành công";
                spScript.InnerHtml = string.Format("<script>$('#myModalThongBao').modal();</script>");
                return;
            }
            try
            {
                var error = await CustomizeHttp.DeserializeAsync<ResponseBody>(response.Content);
                divThongBao.InnerText = $"{error.Message}";
            }
            catch (Exception)
            {
                divThongBao.InnerText = "Thêm mới dịch vụ thất bại - lỗi hệ thống";
                divThongBaoCapNhat.InnerHtml = "Cập nhật thất bại - lỗi hệ thống";
                return;
            }
        }
    }
}