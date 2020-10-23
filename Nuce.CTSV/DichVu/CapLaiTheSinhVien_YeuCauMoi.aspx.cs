using nuce.web.data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class CapLaiTheSinhVien_YeuCauMoi : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (m_SinhVien.IMG.Contains("noimage_human.png"))
                    Response.Redirect("/dichvusinhvien");
            }
        }
        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            //Kiem tra captcha
            //if(!(txtCaptcha.Text.ToLower() == Session["CaptchaVerify"].ToString()))
            //{
            //    divThongBao.InnerHtml = "Bạn nhập sai mã Captcha";
            //    return;
            //}
            string EncodedResponse = Request.Form["g-Recaptcha-Response"];
            bool IsCaptchaValid = (ReCaptchaClass.Validate(EncodedResponse) == "true" ? true : false);

            if (!IsCaptchaValid)
            {
                divThongBao.InnerHtml = "Bạn chưa xác thực Captcha";
                return;
            }

            string strLyDo = txtLyDoXacNhan.Text;
            if(strLyDo.Trim()=="")
            {
                divThongBao.InnerHtml = "Không được để trống lý do";
                return;
            }
            else
            {
                string strRandom = nuce.web.tienich.email.RandomString(6, false);
                string sql = string.Format(@"INSERT INTO [dbo].[AS_Academy_Student_SV_CapLaiTheSinhVien]
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
                sqlParams[2] = new SqlParameter("@Param2", strLyDo);
                sqlParams[3] = new SqlParameter("@Param3", DateTime.Now);
                sqlParams[4] = new SqlParameter("@Param4", m_SinhVien.Ho);
                sqlParams[5] = new SqlParameter("@Param5", strRandom);
                int iReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(Nuce_Common.ConnectionString, CommandType.Text, sql, sqlParams);
                if (iReturn > 0)
                {
                    divThongBao.InnerHtml = "Thêm mới dịch vụ thành công";
                    divThongBaoCapNhat.InnerHtml= "Thêm mới dịch vụ thành công";
                    #region Gui email
                    string strEmail = m_SinhVien.Email;
                    int iIDSV = m_SinhVien.SinhVienID;
                    string strCode = m_SinhVien.MaSV;
                    // gui email
                    string strTieuDe = string.Format("Xác nhận yêu cầu dịch vụ cấp lại thẻ sinh viên");
                    string strNoiDung = string.Format("<div style='color:black;'><div style='padding:5px;'>Xin chào {0}, ", m_SinhVien.Ho);
                    strNoiDung += string.Format("Hệ thống Quản lý thông tin sinh viên xác nhận bạn đã gửi thành công yêu cầu \"dịch vụ cấp lại thẻ sinh viên\". Yêu cầu được tạo lúc {0:dd/MM/yyyy HH:mm}.</div>",DateTime.Now);
                    strNoiDung += "<div style='padding:5px;'>Nhà trường sẽ sớm xử lý và gửi thông tin phản hồi cho bạn qua email.</div>";
                    strNoiDung += "<div style='padding:5px;'>Trân trọng.</div>";
                    strNoiDung += "<div style='padding:5px;'>---------------------------</div>";
                    strNoiDung += "<div style='padding:5px;'>Phòng Công tác Chính trị và Quản lý Sinh viên Trường Đại học Xây dựng</div>";
                    strNoiDung += "<div style='padding:5px;'>Phòng 302 - 303 Tòa nhà A1 Trường đại học Xây Dựng, 55 Giải Phóng - Hai Bà Trưng - Hà Nội</div>";
                    strNoiDung += "<div style='padding:5px;'> </div>";
                    strNoiDung += "<div style='padding:5px;'>TEL: 02438697004</div>";
                    strNoiDung += "<div style='padding:5px;'>Email: ctsv@nuce.edu.vn</div>";
                    strNoiDung += "</div>";
                    string strReturn = nuce.web.tienich.email.Send_Email(strTieuDe, strNoiDung, strEmail, "thanghn.nuce@gmail.com", "226226thang");
                    nuce.web.data.Nuce_CTSV.AS_Logs_Insert(iIDSV, strCode, 1, "AS_Academy_Student_SV_GioiThieu_GuiMailXacNhan", strReturn);
                    #endregion
                    // Response.Redirect("/dichvu/caplaithesinhvien");
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