using nuce.web.data;
using System;
using System.Data;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class CapLaiTheSinhVien : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!m_SinhVien.IMG.Contains("noimage_human.png"))
                {
                    string sql = "select top 100 * from AS_Academy_Student_SV_CapLaiTheSinhVien where Deleted<>1 and StudentID=" + m_SinhVien.SinhVienID + " order by LastModifiedTime desc";
                    DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        string strContent = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string strPhanHoi = dt.Rows[i]["PhanHoi"].ToString();
                            strContent += "<tr>";
                            strContent += string.Format("<td style=\"text-align:center;\">{0}</td>", (i + 1));
                            strContent += string.Format("<td style=\"text-align:center;\">{0:dd/MM/yyyy}</td>", DateTime.Parse(dt.Rows[i]["CreatedTime"].ToString()));
                            //if(strPhanHoi.Trim()=="")
                            //    strContent += string.Format("<td>{0}</td>", dt.Rows[i]["LyDo"].ToString());
                            //else
                            //    strContent += string.Format("<td><div>{0}<div><div style='color:red;'>- Phản hồi: {1}</div></td>", dt.Rows[i]["LyDo"].ToString(),strPhanHoi);
                            int status = int.Parse(dt.Rows[i]["Status"].ToString());
                            int ID = int.Parse(dt.Rows[i]["ID"].ToString()); ;
                            DateTime dtNgayHenBatDau = dt.Rows[i].IsNull("NgayHen_TuNgay") ? DateTime.Now : DateTime.Parse(dt.Rows[i]["NgayHen_TuNgay"].ToString());
                            DateTime dtNgayHenDenNgay = dt.Rows[i].IsNull("NgayHen_DenNgay") ? DateTime.Now.AddDays(7) : DateTime.Parse(dt.Rows[i]["NgayHen_DenNgay"].ToString());
                            switch (status)
                            {
                                case 1:
                                    strContent += string.Format("<td>{0}</td>", dt.Rows[i]["LyDo"].ToString());
                                    strContent += string.Format("<td style='color:green;text-align:center;'>Đang chờ xác nhận</td>");
                                    //strContent += string.Format("<td><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"CapLaiTheSinhVien.fillData({0});\">Cập nhật mã xác nhận</button></td>", ID);
                                    //strContent += string.Format("<td><button type=\"button\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#myModal1\" onclick=\"CapLaiTheSinhVien.initDelete({0},'Bạn có chắc chắn muốn hủy dịch vụ đã yêu cầu?');\">Hủy yêu cầu</button></td>", ID);
                                    break;
                                case 2:
                                    strContent += string.Format("<td>{0}</td>", dt.Rows[i]["LyDo"].ToString());
                                    strContent += string.Format("<td style='color:blue;text-align:center;'>Đã gửi lên nhà trường</td>");
                                    //strContent += string.Format("<td></td>");
                                    //strContent += string.Format("<td></td>");
                                    break;
                                case 3:
                                    strContent += string.Format("<td>{0}</td>", dt.Rows[i]["LyDo"].ToString());
                                    strContent += string.Format("<td style='color:darkgreen;text-align:center;'>Đã tiếp nhận và đang xử lý</td>");
                                    //strContent += string.Format("<td></td>");
                                    //strContent += string.Format("<td></td>");
                                    break;
                                case 4:
                                    strContent += string.Format(@"<td><div>{6}</div><div style='color:blue;'>* Ngày hẹn: Từ <b>{0}</b> giờ - <b>{1} </b> phút - Ngày <b>{2:dd/MM/yyyy}</b> </br>Đến <b>
                                    {3}</b> giờ - <b>{4} </b> phút - Ngày <b>{5:dd/MM/yyyy}</b></div></td>", dtNgayHenBatDau.Hour, dtNgayHenBatDau.Minute, dtNgayHenBatDau, dtNgayHenDenNgay.Hour, dtNgayHenDenNgay.Minute, dtNgayHenDenNgay, dt.Rows[i]["LyDo"].ToString());
                                    strContent += string.Format("<td style='color:red;text-align:center;'>Đã xử lý và có lịch hẹn</td>");
                                    //strContent += string.Format("<td><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal2\" onclick=\"CapLaiTheSinhVien.initDelete({0},'Bạn có chắc chắn muốn xác nhận hoàn thành dịch vụ?');\">Xác nhận hoàn thành</button></td>", ID);
                                    //strContent += string.Format("<td></td>");
                                    break;
                                case 5:
                                    strContent += string.Format("<td><div>{0}<div><div style='color:red;'>- Phản hồi: {1}</div></td>", dt.Rows[i]["LyDo"].ToString(), strPhanHoi);
                                    strContent += string.Format("<td style='color:red;text-align:center;'>Từ chối dịch vụ</td>");
                                    //strContent += string.Format("<td></td>");
                                    //strContent += string.Format("<td></td>");
                                    break;
                                case 6:
                                    strContent += string.Format("<td>{0}</td>", dt.Rows[i]["LyDo"].ToString());
                                    strContent += string.Format("<td style='color:black;text-align:center;'>Hoàn thành</td>");
                                    //strContent += string.Format("<td></td>");
                                    //strContent += string.Format("<td></td>");
                                    break;
                                default: break;
                            }
                            strContent += "</tr>";

                        }
                        tbContent.InnerHtml = strContent;
                    }
                }
                else
                {
                    divThemMoi.Visible = false;
                    tbContent.InnerHtml = string.Format("<td colspan='4' style='text-align:center;color:red;'>Hiện tại bạn chưa cập nhật ảnh nên không được sử dụng dịch vụ này.</td>");
                }
            }
        }
    }
}