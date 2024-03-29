﻿using Newtonsoft.Json;
using Nuce.CTSV.ApiModels;
using System;
using System.Net.Http;

namespace Nuce.CTSV
{
    public partial class VeXeBus_YeuCauMoi : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
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

            string selectedNoiNhan = slNoiNhan.Items[slNoiNhan.SelectedIndex].Text;
            string strNoiNhan = selectedNoiNhan.Split('-')[1];

            int intLoaiThe = Convert.ToInt32(radioLoaiThe.SelectedValue);
            string strMaTuyen = "";
            string strTenTuyen = "";

            if ((DichVuXeBusLoaiTuyen)intLoaiThe == DichVuXeBusLoaiTuyen.MotTuyen)
            {
                var tuyen = slTuyen.Items[slTuyen.SelectedIndex];
                strMaTuyen = tuyen.Value;
                strTenTuyen = tuyen.Text.Split('-')[1];
            }

            string strRandom = nuce.web.tienich.email.RandomString(6, false);

            var body = new AddDichVuModel()
            {
                type = (int)DichVu.VeXeBus,
                veBusTuyenType = intLoaiThe,
                veBusTuyenCode = strMaTuyen,
                veBusTuyenName = strTenTuyen,
                veBusNoiNhanThe = strNoiNhan,
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