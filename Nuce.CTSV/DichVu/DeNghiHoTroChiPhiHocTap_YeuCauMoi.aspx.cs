using Newtonsoft.Json;
using Nuce.CTSV.ApiModels;
using System;
using System.Net.Http;

namespace Nuce.CTSV
{
    public partial class DeNghiHoTroChiPhiHocTap_YeuCauMoi : BasePage
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

                    if (!string.IsNullOrEmpty(thongBao))
                    {
                        divThongBao.InnerHtml = $"Yêu cầu cập nhật<a href=\"/capnhathoso.aspx?dantoc=1\">{thongBao}</a>";
                    }
                    else
                    {
                        divBtnContainer.Visible = true;
                    }
                }

                this.radioDoiTuong.Attributes.Add("class", "radio-list");
                this.textBoxSdt.Attributes.Add("class", "form-control col-md-3 col-12");
                this.textBoxSdt.Attributes.Add("type", "tel");
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

            string doiTuong = radioDoiTuong.SelectedValue;

            var body = new AddDichVuModel()
            {
                type = (int)DichVu.DeNghiHoTroChiPhiHocTap,
                doiTuongDeNghiHoTro = doiTuong,
                sdt = this.textBoxSdt.Text,
                notSendEmail = true
            };

            var jsonBody = JsonConvert.SerializeObject(body);
            var response = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Post, "api/DichVu/add", jsonBody);

            if (response.IsSuccessStatusCode)
            {
                divThongBao.InnerHtml = "Thêm mới thành công";
                divThongBaoCapNhat.InnerHtml = "Thêm mới thành công";
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
                divThongBao.InnerText = "Thêm mới thất bại - lỗi hệ thống";
                divThongBaoCapNhat.InnerHtml = "Cập nhật thất bại - lỗi hệ thống";
                return;
            }
        }
    }
}