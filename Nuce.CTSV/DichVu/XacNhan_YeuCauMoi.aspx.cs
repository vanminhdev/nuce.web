using Microsoft.ApplicationBlocks.Data;
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
        private string sqlPhuong = "HKTT_Phuong";
        private string sqlQuan = "HKTT_Quan";
        private string sqlTinh = "HKTT_Tinh";
        private string sqlDiaChi = "DiaChiCuThe";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sql = $@"SELECT {sqlPhuong}, {sqlQuan}, {sqlTinh}, {sqlDiaChi} FROM dbo.AS_Academy_Student AS AAS
                                WHERE AAS.ID = {m_SinhVien.SinhVienID}";
                DataSet ds = SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql);
                var student = ds.Tables[0];
                string phuong = Nuce_CTSV.firstOrDefault(student.Rows, 0, sqlPhuong);
                string quan = Nuce_CTSV.firstOrDefault(student.Rows, 0, sqlQuan);
                string tinh = Nuce_CTSV.firstOrDefault(student.Rows, 0, sqlTinh);
                string diachi = Nuce_CTSV.firstOrDefault(student.Rows, 0, sqlDiaChi);

                frmPhuong.Visible = string.IsNullOrEmpty(phuong);
                frmQuan.Visible = string.IsNullOrEmpty(quan);
                frmTinh.Visible = string.IsNullOrEmpty(tinh);
                frmDiaChi.Visible = string.IsNullOrEmpty(diachi);
            }
        }
        private string updateRequiredValue(string value, string msg, string col)
        {
            if (string.IsNullOrEmpty(value.Trim()))
            {
                divThongBao.InnerHtml = msg;
                return null;
            }
            return $@"UPDATE dbo.AS_Academy_Student
                    SET	{col} = N'{value}'
                    WHERE ID = {m_SinhVien.SinhVienID}; ";
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

            string sql = "";
            string tmpSql = "";

            if (frmPhuong.Visible)
            {
                tmpSql = updateRequiredValue(txtPhuong.Text.Trim(), "Không được để trống phường/xã", sqlPhuong);
                if (string.IsNullOrEmpty(tmpSql))
                {
                    return;
                }
                sql += tmpSql;
            }
            if (frmQuan.Visible)
            {
                tmpSql = updateRequiredValue(txtQuan.Text.Trim(), "Không được để trống quận/huyện", sqlQuan);
                if (string.IsNullOrEmpty(tmpSql))
                {
                    return;
                }
                sql += tmpSql;
            }
            if (frmTinh.Visible)
            {
                tmpSql = updateRequiredValue(txtTinh.Text.Trim(), "Không được để trống tỉnh/thành phố", sqlTinh);
                if (string.IsNullOrEmpty(tmpSql))
                {
                    return;
                }
                sql += tmpSql;
            }
            if (frmDiaChi.Visible)
            {
                tmpSql = updateRequiredValue(txtDiaChi.Text.Trim(), "Không được để trống địa chỉ tạm trú", sqlDiaChi);
                if (string.IsNullOrEmpty(tmpSql))
                {
                    return;
                }
                sql += tmpSql;
            }

            string strLyDo = txtLyDoXacNhan.Text;
            if(strLyDo.Trim()=="")
            {
                divThongBao.InnerHtml = "Không được để trống lý do";
                return;
            }
            else
            {
                if (Nuce_CTSV.isDuplicated(1, m_SinhVien.SinhVienID, strLyDo))
                {
                    divThongBao.InnerText = "Trùng yêu cầu dịch vụ";
                    return;
                }

                var body = new
                {
                    type = 1,
                    lyDo = strLyDo,
                };
                string strRandom = nuce.web.tienich.email.RandomString(6, false);

                var jsonBody = JsonConvert.SerializeObject(body);
                var response = await CustomizeHttp.SendRequest(Request, HttpMethod.Post, "api/DichVu/add", jsonBody);

                if (response.IsSuccessStatusCode)
                {
                    divThongBao.InnerHtml = "Thêm mới dịch vụ thành công";
                    divThongBaoCapNhat.InnerHtml = "Thêm mới dịch vụ thành công";
                    spScript.InnerHtml = string.Format("<script>$('#myModalThongBao').modal();</script>");
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