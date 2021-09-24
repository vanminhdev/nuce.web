using Newtonsoft.Json;
using nuce.web.data;
using Nuce.CTSV.ApiModels;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net.Http;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class MuonHocBaGoc_YeuCauMoi : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }
        protected async void btnCapNhat_Click(object sender, EventArgs e)
        {
            //Kiem tra captcha
            //if (!(txtCaptcha.Text.ToLower() == Session["CaptchaVerify"].ToString()))
            //{
            //    divThongBao.InnerHtml = "Bạn nhập sai mã Captcha";
            //    return;
            //}
            // tmp
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
                type = (int)DichVu.MuonHocBaGoc,
                lyDo = "",
                maXacNhan = strRandom,
                thoiGianMuon = "",
                notSendEmail = true
            };

            var jsonBody = JsonConvert.SerializeObject(body);
            var response = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Post, "api/DichVu/add", jsonBody);

            if (response.IsSuccessStatusCode)
            {
                string msg = "Thêm mới dịch vụ thành công. Để làm thủ tục, bạn phải viết đơn xin mượn và mang theo sổ hộ khẩu gia đình bản gốc đến phòng CTCT&QLSV.";
                divThongBao.InnerHtml = msg;
                divThongBaoCapNhat.InnerHtml = msg;
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