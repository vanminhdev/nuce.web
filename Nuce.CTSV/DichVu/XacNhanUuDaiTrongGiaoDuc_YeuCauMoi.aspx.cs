using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using nuce.web.data;
using Nuce.CTSV.ApiModels;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.Http;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class XacNhanUuDaiTrongGiaoDuc_YeuCauMoi : BasePage
    {
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
            
            string strKyLuat = txtKyLuat.Text;
            if (string.IsNullOrEmpty(strKyLuat.Trim()))
            {
                divThongBao.InnerHtml = "Không được để trống kỷ luật";
                return;
            }
            else
            {
                string strRandom = nuce.web.tienich.email.RandomString(6, false);
                var body = new ApiModels.AddDichVuModel()
                {
                    type = (int)ApiModels.DichVu.UuDaiGiaoDuc,
                    kyLuat = strKyLuat.Trim(),
                    maXacNhan = strRandom
                };

                var jsonBody = JsonConvert.SerializeObject(body);
                var response = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Post, ApiModels.ApiEndPoint.AddDichVu, jsonBody);

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
}