﻿using Microsoft.ApplicationBlocks.Data;
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
    public partial class GioiThieu_YeuCauMoi : BasePage
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
            
            string strDenGap = txtDenGap.Text;
            if (strDenGap.Trim() == "")
            {
                divThongBao.InnerHtml = "Không được để trống trường Đến gặp";
                return;
            }
            string strVeViec = txtVeViec.Text;
            if (strVeViec.Trim() == "")
            {
                divThongBao.InnerHtml = "Không được để trống trường Về việc";
                return;
            }

            string strDonVi = txtDonVi.Text;
            if(strDonVi.Trim()=="")
            {
                divThongBao.InnerHtml = "Không được để trống trường Đơn vị";
                return;
            }

            string strRandom = nuce.web.tienich.email.RandomString(6, false);
            var body = new AddDichVuModel()
            {
                type = (int)DichVu.GioiThieu,
                donVi = strDonVi.Trim(),
                denGap = strDenGap.Trim(),
                veViec = strVeViec.Trim(),
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