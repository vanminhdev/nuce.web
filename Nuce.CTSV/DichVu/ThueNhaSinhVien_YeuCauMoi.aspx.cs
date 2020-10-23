using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using nuce.web.data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class ThueNhaSinhVien_YeuCauMoi : BasePage
    {
        private string sqlPhuong = "HKTT_Phuong";
        private string sqlQuan = "HKTT_Quan";
        private string sqlTinh = "HKTT_Tinh";
        private string sqlCmt = "CMT";
        private string sqlCmtNgayCap = "CMT_NgayCap";
        private string sqlCmtNoiCap = "CMT_NoiCap";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //site key
                //6Lc_ocYZAAAAALAtyq0MiHsiZw9hjUbFmY2rKdQ0
                //secret key
                //6Lc_ocYZAAAAADbYasKFfBLQRJgxTAPWNYIDOaxQ
                string sql = $@"SELECT * FROM dbo.AS_Academy_Student AS AAS
                                WHERE AAS.ID = {m_SinhVien.SinhVienID}";
                DataSet ds = SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql);
                var student = ds.Tables[0];
                string phuong = Nuce_CTSV.firstOrDefault(student.Rows, 0, sqlPhuong);
                string quan = Nuce_CTSV.firstOrDefault(student.Rows, 0, sqlQuan);
                string tinh = Nuce_CTSV.firstOrDefault(student.Rows, 0, sqlTinh);
                string cmt = Nuce_CTSV.firstOrDefault(student.Rows, 0, sqlCmt);
                string cmtNgayCap = Nuce_CTSV.firstOrDefault(student.Rows, 0, sqlCmtNgayCap);
                string cmtNoiCap = Nuce_CTSV.firstOrDefault(student.Rows, 0, sqlCmtNoiCap);

                frmPhuong.Visible = string.IsNullOrEmpty(phuong);
                frmQuan.Visible = string.IsNullOrEmpty(quan);
                frmTinh.Visible = string.IsNullOrEmpty(tinh);
                frmCmnd.Visible = string.IsNullOrEmpty(cmt);
                frmNgayCap.Visible = string.IsNullOrEmpty(cmtNgayCap);
                frmNoiCap.Visible = string.IsNullOrEmpty(cmtNoiCap);
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
        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            //Kiem tra captcha
            //if (!(txtCaptcha.Text.ToLower() == Session["CaptchaVerify"].ToString()))
            //{
            //    divThongBao.InnerHtml = "Bạn nhập sai mã Captcha";
            //    return;
            //}
            // tmp
            string EncodedResponse = Request.Form["g-Recaptcha-Response"];
            bool IsCaptchaValid = (ReCaptchaClass.Validate(EncodedResponse) == "true" ? true : false);

            if (!IsCaptchaValid)
            {
                divThongBao.InnerHtml = "Bạn chưa xác thực Captcha";
                return;
            }

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
            if (frmCmnd.Visible)
            {
                tmpSql = updateRequiredValue(txtCmnd.Text.Trim(), "Không được để trống số chứng minh thư", sqlCmt);
                if (string.IsNullOrEmpty(tmpSql))
                {
                    return;
                }
                sql += tmpSql;
            }
            if (frmNoiCap.Visible)
            {
                tmpSql = updateRequiredValue(txtNoiCap.Text.Trim(), "Không được để trống nơi cấp chứng minh thư", sqlCmtNoiCap);
                if (string.IsNullOrEmpty(tmpSql))
                {
                    return;
                }
                sql += tmpSql;
            }
            if (frmNgayCap.Visible)
            {
                tmpSql = updateRequiredValue(txtNgayCap.Text.Trim(), "Không được để trống ngày cấp chứng minh thư", sqlCmtNgayCap);
                if (string.IsNullOrEmpty(tmpSql))
                {
                    return;
                }
                sql += tmpSql;
            }

            if (Nuce_CTSV.isDuplicated(7, m_SinhVien.SinhVienID))
            {
                divThongBao.InnerText = "Trùng yêu cầu dịch vụ";
                return;
            }
            else
            {
                string strRandom = nuce.web.tienich.email.RandomString(6, false);
                sql += string.Format(@"INSERT INTO [dbo].[AS_Academy_Student_SV_ThueNha]
           ([StudentID],[StudentCode],[Status],[LyDo],[PhanHoi],[Deleted]
           ,[CreatedBy],[LastModifiedBy],[DeletedBy],[CreatedTime],[DeletedTime],[LastModifiedTime],StudentName,MaXacNhan)
     VALUES
           (@Param0
           ,@Param1
           ,2
           ,@Param2
           ,''
           ,0
           ,@Param0
           ,@Param0
           ,-1
           ,@Param3
           ,@Param3
           ,@Param3,@Param4,@Param5)");
                SqlParameter[] sqlParams = new SqlParameter[6];
                sqlParams[0] = new SqlParameter("@Param0", m_SinhVien.SinhVienID);
                sqlParams[1] = new SqlParameter("@Param1", m_SinhVien.MaSV);
                sqlParams[2] = new SqlParameter("@Param2", null);
                sqlParams[3] = new SqlParameter("@Param3", DateTime.Now);
                sqlParams[4] = new SqlParameter("@Param4", m_SinhVien.Ho);
                sqlParams[5] = new SqlParameter("@Param5", strRandom);
                int iReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(Nuce_Common.ConnectionString, CommandType.Text, sql, sqlParams);
                if (iReturn > 0)
                {
                    divThongBao.InnerHtml = "Thêm mới dịch vụ thành công";
                    divThongBaoCapNhat.InnerHtml = "Thêm mới dịch vụ thành công";
                    #region Gui email
                    string strEmail = m_SinhVien.Email;
                    int iIDSV = m_SinhVien.SinhVienID;
                    string strCode = m_SinhVien.MaSV;
                    // gui email
                    string strTieuDe = string.Format("Xác nhận yêu cầu dịch vụ xin giấy xác nhận thuê nhà cho sinh viên");
                    string strNoiDung = string.Format("<div style='color:black;'><div style='padding:5px;'>Xin chào {0}. </div>", m_SinhVien.Ho);
                    strNoiDung += string.Format("<div style='padding:5px;'>Hệ thống Quản lý thông tin sinh viên xác nhận bạn đã gửi thành công yêu cầu \"dịch vụ xin giấy xác nhận sinh viên\". Yêu cầu được tạo lúc {0:dd/MM/yyyy HH:mm}.</div>", DateTime.Now);
                    strNoiDung += "<div style='padding:5px;'>Nhà trường sẽ sớm xử lý và gửi thông tin phản hồi cho bạn qua email.</div>";
                    strNoiDung += "<div style='padding:5px;'>Trân trọng.</div>";
                    strNoiDung += "<div style='padding:5px;'>---------------------------</div>";
                    strNoiDung += "<div style='padding:5px;'>Phòng Công tác Chính trị và Quản lý Sinh viên Trường Đại học Xây dựng</div>";
                    strNoiDung += "<div style='padding:5px;'>Phòng 302 - 303 Tòa nhà A1 Trường đại học Xây Dựng, 55 Giải Phóng - Hai Bà Trưng - Hà Nội</div>";
                    strNoiDung += "<div style='padding:5px;'> </div>";
                    strNoiDung += "<div style='padding:5px;'>TEL: 02438697004</div>";
                    strNoiDung += "<div style='padding:5px;'>Email: ctsv@nuce.edu.vn</div>";
                    strNoiDung += "</div>";
                    string strReturn = nuce.web.tienich.email.Send_Email(strTieuDe, strNoiDung, strEmail, "ctsv.hotro@nuce.edu.vn", "ctsv@123456a");
                    nuce.web.data.Nuce_CTSV.AS_Logs_Insert(iIDSV, strCode, 1, "AS_Academy_Student_SV_XacNhan_GuiMailXacNhan", strReturn);
                    nuce.web.data.Nuce_CTSV.AS_Academy_Student_TinNhan_Insert("GIOI_THIEU_THEM_MOI", -1, m_SinhVien.MaSV, m_SinhVien.Ho, m_SinhVien.MaSV, m_SinhVien.Email, strNoiDung, 1, m_SinhVien.SinhVienID,strTieuDe);
                    #endregion
                    //Response.Redirect("/dichvu/GioiThieu");
                    spScript.InnerHtml = string.Format("<script>$('#myModalThongBao').modal();</script>");
                    return;
                }
                else
                {
                    divThongBao.InnerText = "Thêm mới dịch vụ thất bại - lỗi hệ thống";
                    divThongBaoCapNhat.InnerHtml = "Cập nhật thất bại - lỗi hệ thống";
                    return;
                }
            }
        }
    }

}