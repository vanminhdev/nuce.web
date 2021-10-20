﻿using Newtonsoft.Json;
using Nuce.CTSV.ApiModels;
using System;
using System.Net.Http;

namespace Nuce.CTSV
{
    public partial class HoTroHocTap_YeuCauMoi : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divBtnContainer.Visible = false;
                var studentResponse = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Get, $"{ApiModels.ApiEndPoint.GetStudentInfo}/{m_SinhVien.MaSV}", "");
                if (studentResponse.IsSuccessStatusCode)
                {
                    var strResponse = await studentResponse.Content.ReadAsStringAsync();
                    var student = JsonConvert.DeserializeObject<ApiModels.StudentModel>(strResponse);

                    string thongBao = "";

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

                    if (string.IsNullOrEmpty(student.DanToc?.Trim()))
                    {
                        thongBao += $"{(thongBao != "" ? "," : "")} dân tộc";
                    }

                    if (UpdateDiaChiChuyenPhatNhanh.Enabled && string.IsNullOrEmpty(student.BaoTinDiaChiNhanChuyenPhatNhanh?.Trim()))
                    {
                        thongBao += $"{(thongBao != "" ? "," : "")} địa chỉ nhận chuyển phát nhanh";
                    }

                    if (!string.IsNullOrEmpty(thongBao))
                    {
                        divThongBao.InnerHtml = $"Yêu cầu cập nhật<a href=\"/capnhathoso.aspx\">{thongBao}</a>";
                    }
                    else
                    {
                        divBtnContainer.Visible = true;
                    }
                }
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


            var body = new AddDichVuModel()
            {
                type = (int)DichVu.HoTroHocTap,
                notSendEmail = true
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