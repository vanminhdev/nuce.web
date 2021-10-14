using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using nuce.web.data;
using Nuce.CTSV.ApiModels;

namespace Nuce.CTSV
{
    public partial class VayVonNganHang_YeuCauMoi : BasePage
    {
        private ApiModels.StudentModel student;
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divBtnContainer.Visible = false;
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

                    if (string.IsNullOrEmpty(student.BaoTinDiaChiNhanChuyenPhatNhanh?.Trim()))
                    {
                        thongBao += $"{(thongBao != "" ? "," : "")} địa chỉ nhận chuyển phát nhanh";
                    }

                    if (!string.IsNullOrEmpty(thongBao))
                    {
                        divBtnContainer.Visible = false;
                        divThongBao.InnerHtml = $"Yêu cầu cập nhật<a href=\"/capnhathoso.aspx\">{thongBao}</a>";
                        return;
                    } else
                    {
                        divBtnContainer.Visible = true;
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
            

            student = (ApiModels.StudentModel)ViewState["student"];

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

            string thuocDien = slThuocDien.Value;
            string thuocDoiTuong = slThuocDoiTuong.Value;

            string strRandom = nuce.web.tienich.email.RandomString(6, false);

            var body = new ApiModels.AddDichVuModel()
            {
                type = (int)ApiModels.DichVu.VayVonNganHang,
                maXacNhan = strRandom,
                thuocDien = thuocDien,
                thuocDoiTuong = thuocDoiTuong
            };

            var jsonBody = JsonConvert.SerializeObject(body);
            var response = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Post, ApiModels.ApiEndPoint.AddDichVu, jsonBody);


            if (response.IsSuccessStatusCode)
            {
                divThongBao.InnerHtml = "Thêm mới dịch vụ thành công";
                divThongBaoCapNhat.InnerHtml = "Thêm mới dịch vụ thành công";
                //Response.Redirect("/dichvu/GioiThieu");
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