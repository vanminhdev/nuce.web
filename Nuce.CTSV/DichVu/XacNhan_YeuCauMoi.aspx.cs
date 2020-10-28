﻿using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using nuce.web.data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class XacNhan_YeuCauMoi : BasePage
    {
        private ApiModels.StudentModel student;
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var studentResponse = await CustomizeHttp.SendRequest(Request, HttpMethod.Get, $"{ApiModels.ApiEndPoint.GetStudentInfo}/{m_SinhVien.MaSV}", "");
                if (studentResponse.IsSuccessStatusCode)
                {
                    var strResponse = await studentResponse.Content.ReadAsStringAsync();
                    student = JsonConvert.DeserializeObject<ApiModels.StudentModel>(strResponse);
                    ViewState["student"] = student;

                    frmPhuong.Visible = string.IsNullOrEmpty(student.HkttPhuong?.Trim());
                    frmQuan.Visible = string.IsNullOrEmpty(student.HkttQuan?.Trim());
                    frmTinh.Visible = string.IsNullOrEmpty(student.HkttTinh?.Trim());
                    frmDiaChi.Visible = string.IsNullOrEmpty(student.DiaChiCuThe?.Trim());
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
            //string EncodedResponse = Request.Form["g-Recaptcha-Response"];
            //bool IsCaptchaValid = (ReCaptchaClass.Validate(EncodedResponse) == "true" ? true : false);

            //if (!IsCaptchaValid)
            //{
            //    divThongBao.InnerHtml = "Bạn chưa xác thực Captcha";
            //    return;
            //}

            student = (ApiModels.StudentModel)ViewState["student"];

            bool update = false;

            if (frmPhuong.Visible && string.IsNullOrEmpty(txtPhuong.Text.Trim()))
            {
                divThongBao.InnerHtml = "Không được để trống phường/xã";
                return;
            } else if (frmPhuong.Visible)
            {
                student.HkttPhuong = txtPhuong.Text.Trim();
                update = true;
            }

            if (frmQuan.Visible && string.IsNullOrEmpty(txtQuan.Text.Trim()))
            {
                divThongBao.InnerHtml = "Không được để trống quận/huyện";
                return;
            } else if (frmQuan.Visible)
            {
                student.HkttQuan = txtQuan.Text.Trim();
                update = true;
            }

            if (frmTinh.Visible && string.IsNullOrEmpty(txtTinh.Text.Trim()))
            {
                divThongBao.InnerHtml = "Không được để trống tỉnh/thành phố";
                return;
            } else if (frmTinh.Visible)
            {
                student.HkttTinh = txtTinh.Text.Trim();
                update = true;
            }

            if (frmDiaChi.Visible && string.IsNullOrEmpty(txtDiaChi.Text.Trim()))
            {
                divThongBao.InnerHtml = "Không được để trống địa chỉ tạm trú";
                return;
            } else if (frmDiaChi.Visible)
            {
                student.DiaChiCuThe = txtDiaChi.Text.Trim();
                update = true;
            }

            if (update)
            {
                string updateStudentContent = JsonConvert.SerializeObject(student);
                string endpoint = ApiModels.ApiEndPoint.PutStudentUpdate;
                var updateResponse = await CustomizeHttp.SendRequest(Request, HttpMethod.Put, endpoint, updateStudentContent);

                if (!updateResponse.IsSuccessStatusCode)
                {
                    divThongBao.InnerHtml = "Không cập nhật được thông tin sinh viên";
                    return;
                }
            }

            string strLyDo = txtLyDoXacNhan.Text;
            if (strLyDo.Trim() == "")
            {
                divThongBao.InnerHtml = "Không được để trống lý do";
                return;
            }
            else
            {
                string strRandom = nuce.web.tienich.email.RandomString(6, false);
                var body = new ApiModels.AddDichVuModel()
                {
                    type = (int)ApiModels.DichVu.XacNhan,
                    lyDo = strLyDo.Trim(),
                    maXacNhan = strRandom
                };

                var jsonBody = JsonConvert.SerializeObject(body);
                var response = await CustomizeHttp.SendRequest(Request, HttpMethod.Post, "api/DichVu/add", jsonBody);

                if (response.IsSuccessStatusCode)
                {
                    divThongBao.InnerHtml = "Thêm mới dịch vụ thành công";
                    divThongBaoCapNhat.InnerHtml = "Thêm mới dịch vụ thành công";
                    spScript.InnerHtml = string.Format("<script>$('#myModalThongBao').modal();</script>");
                } 
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    divThongBao.InnerText = "Trùng yêu cầu dịch vụ";
                }
                else
                {
                    divThongBao.InnerText = "Thêm mới dịch vụ thất bại - lỗi hệ thống";
                    divThongBaoCapNhat.InnerHtml = "Cập nhật thất bại - lỗi hệ thống";
                }
            }
        }
    }

}