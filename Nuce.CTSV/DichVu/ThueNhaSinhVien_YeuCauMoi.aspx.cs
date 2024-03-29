﻿using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using nuce.web.data;
using Nuce.CTSV.ApiModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class ThueNhaSinhVien_YeuCauMoi : BasePage
    {
        private StudentModel student;
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var studentResponse = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Get, $"{ApiModels.ApiEndPoint.GetStudentInfo}/{m_SinhVien.MaSV}", "");
                if (studentResponse.IsSuccessStatusCode)
                {
                    var strResponse = await studentResponse.Content.ReadAsStringAsync();
                    student = JsonConvert.DeserializeObject<ApiModels.StudentModel>(strResponse);
                    ViewState["student"] = student;

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

                    if (!string.IsNullOrEmpty(thongBao))
                    {
                        divBtnContainer.Visible = false;
                        divThongBao.InnerHtml = $"Yêu cầu cập nhật<a href=\"/capnhathoso.aspx\">{thongBao}</a>";
                        return;
                    }

                    frmCmnd.Visible = string.IsNullOrEmpty(student.Cmt?.Trim());
                    frmNgayCap.Visible = student.CmtNgayCap == null;
                    frmNoiCap.Visible = string.IsNullOrEmpty(student.CmtNoiCap);
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


            student = (StudentModel)ViewState["student"];

            bool update = false;

            if (frmCmnd.Visible && string.IsNullOrEmpty(txtCmnd.Text.Trim()))
            {
                divThongBao.InnerHtml = "Không được để trống chứng minh nhân dân";
                return;
            }
            else if (frmCmnd.Visible)
            {
                student.Cmt = txtCmnd.Text.Trim();
                update = true;
            }

            if (frmNoiCap.Visible && string.IsNullOrEmpty(txtNoiCap.Text.Trim()))
            {
                divThongBao.InnerHtml = "Không được để trống nơi cấp chứng minh nhân dân";
                return;
            }
            else if (frmNoiCap.Visible)
            {
                student.CmtNoiCap = txtNoiCap.Text.Trim();
                update = true;
            }

            if (frmNgayCap.Visible && string.IsNullOrEmpty(txtNgayCap.Text.Trim()))
            {
                divThongBao.InnerHtml = "Không được để trống ngày cấp chứng minh nhân dân";
                return;
            }
            else if (frmNgayCap.Visible)
            {
                student.CmtNgayCap = DateTime.Parse(txtNgayCap.Text.Trim());
                update = true;
            }

            if (update)
            {
                string updateStudentContent = JsonConvert.SerializeObject(student);
                string endpoint = ApiModels.ApiEndPoint.PutStudentUpdate;
                var updateResponse = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Put, endpoint, updateStudentContent);

                if (!updateResponse.IsSuccessStatusCode)
                {
                    divThongBao.InnerHtml = "Không cập nhật được thông tin sinh viên";
                    return;
                }
            }

            string strRandom = nuce.web.tienich.email.RandomString(6, false);

            var body = new ApiModels.AddDichVuModel()
            {
                type = (int)ApiModels.DichVu.ThueNha,
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