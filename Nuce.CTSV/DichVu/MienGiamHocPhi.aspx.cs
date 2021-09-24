using Newtonsoft.Json;
using nuce.web.data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.UI;
using Nuce.CTSV;
using Nuce.CTSV.ApiModels;

namespace Nuce.CTSV
{
    public partial class MienGiamHocPhi : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string api = $"/api/DichVu/type/{(int)DichVu.MienGiamHocPhi}";
                var res = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Get, api, "");
                List<XinMienGiamHocPhiModel> listData = new List<XinMienGiamHocPhiModel>();

                if (res.IsSuccessStatusCode)
                {
                    string strResponse = await res.Content.ReadAsStringAsync();
                    listData = JsonConvert.DeserializeObject<List<XinMienGiamHocPhiModel>>(strResponse);
                }

                if (listData != null && listData.Count > 0)
                {
                    string strContent = "";
                    for (int i = 0; i < listData.Count; i++)
                    {
                        var dkChoO = listData[i];
                        string strPhanHoi = dkChoO.PhanHoi;
                        strContent += "<tr>";
                        strContent += string.Format("<td style=\"text-align:center;\">{0}</td>", (i+1));
                        strContent += string.Format("<td style=\"text-align:center;\">{0}</td>", dkChoO.CreatedTime?.ToString("dd/MM/yyyy"));
                        //if(strPhanHoi.Trim()=="")
                        //    strContent += string.Format("<td>{0}</td>", dt.Rows[i]["LyDo"].ToString());
                        //else
                        //    strContent += string.Format("<td><div>{0}<div><div style='color:red;'>- Phản hồi: {1}</div></td>", dt.Rows[i]["LyDo"].ToString(),strPhanHoi);
                        int status = dkChoO.Status ?? -1;
                        DateTime dtNgayHenBatDau = dkChoO.NgayHenTuNgay ?? DateTime.Now;
                        DateTime dtNgayHenDenNgay = dkChoO.NgayHenDenNgay ?? DateTime.Now.AddDays(7);

                        string content = "";
                        string doiTuong = "";
                        switch(dkChoO.DoiTuongHuong)
                        {
                            case "CO_CONG_CACH_MANG":
                                doiTuong = "Là con của người có công với cách mạng được hưởng ưu đãi";
                                break;
                            case "SV_VAN_BANG_1":
                                doiTuong = "Là sinh viên học đại học văn bằng thứ nhất từ 16 tuổi đến 22 tuổi thuộc một trong các trường hợp quy định tại Khoản 1 Điều 5 Nghị định số 136/2013/NĐ-CP ngày 21/10/2013 của Chính phủ quy định chính sách trợ giúp xã hội";
                                break;
                            case "TAN_TAT_KHO_KHAN_KINH_TE":
                                doiTuong = "Bản thân bị khuyết tật, tàn tật có khó khăn về kinh tế";
                                break;
                            case "DAN_TOC_HO_NGHEO":
                                doiTuong = "Bản thân là người dân tộc thiểu số thuộc hộ nghèo, hộ cận nghèo";
                                break;
                            case "DAN_TOC_IT_NGUOI_VUNG_KHO_KHAN":
                                doiTuong = "Bản thân là người dân tộc thiểu số rất ít người và ở vùng có điều kiện kinh tế - xã hội khó khăn hoặc đặc biệt khó khăn";
                                break;
                            case "DAN_TOC_VUNG_KHO_KHAN":
                                doiTuong = "Bản thân là người dân tộc thiểu số và ở vùng có điều kiện kinh tế - xã hội đặc biệt khó khăn";
                                break;
                            case "CHA_ME_TAI_NAN_DUOC_TRO_CAP":
                                doiTuong = "Là con của cán bộ, công nhân, viên chức mà cha hoặc mẹ bị tai nạn lao động hoặc mắc bệnh nghề nghiệp được hưởng trợ cấp thường xuyên";
                                break;
                            default:
                                break;
                        }
                        content += $"<div>Đối tượng: {doiTuong}</div>";
                        switch (status)
                        {
                            case 1:
                                strContent += string.Format("<td>{0}</td>", content);
                                strContent += string.Format("<td style='color:green;text-align:center;'>Đang chờ xác nhận</td>");
                               // strContent += string.Format("<td><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"XacNhan.fillData({0});\">Cập nhật mã xác nhận</button></td>", ID);
                               // strContent += string.Format("<td><button type=\"button\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#myModal1\" onclick=\"XacNhan.initDelete({0},'Bạn có chắc chắn muốn hủy dịch vụ đã yêu cầu?');\">Hủy yêu cầu</button></td>", ID);
                                break;
                            case 2:
                                strContent += string.Format("<td>{0}</td>", content);
                                strContent += string.Format("<td style='color:blue;text-align:center;'>Đã gửi lên nhà trường</td>");
                                //strContent += string.Format("<td></td>");
                                //strContent += string.Format("<td></td>");
                                break;
                            case 3:
                                strContent += string.Format("<td>{0}</td>", content);
                                strContent += string.Format("<td style='color:darkgreen;text-align:center;'>Đã tiếp nhận và đang xử lý</td>");
                               // strContent += string.Format("<td></td>");
                               // strContent += string.Format("<td></td>");
                                break;
                            case 4:
                                strContent += string.Format(@"<td><div>{6}</div><div style='color:blue;'>* Ngày hẹn: Từ <b>{0}</b> giờ - <b>{1} </b> phút - Ngày <b>{2:dd/MM/yyyy}</b> </br>Đến <b>
                                    {3}</b> giờ - <b>{4} </b> phút - Ngày <b>{5:dd/MM/yyyy}</b></div></td>", dtNgayHenBatDau.Hour, dtNgayHenBatDau.Minute, dtNgayHenBatDau,dtNgayHenDenNgay.Hour, dtNgayHenDenNgay.Minute, dtNgayHenDenNgay, content);
                                strContent += string.Format("<td style='color:red;text-align:center;'>Đã xử lý và có lịch hẹn</td>");
                               // strContent += string.Format("<td><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal2\" onclick=\"XacNhan.initDelete({0},'Bạn có chắc chắn muốn xác nhận hoàn thành dịch vụ?');\">Xác nhận hoàn thành</button></td>", ID);
                               // strContent += string.Format("<td></td>");
                                break;
                            case 5:
                                strContent += string.Format("<td><div>{0}<div><div style='color:red;'>- Phản hồi: {1}</div></td>", content, strPhanHoi);
                                strContent += string.Format("<td style='color:red;text-align:center;'>Từ chối dịch vụ</td>");
                               // strContent += string.Format("<td></td>");
                               // strContent += string.Format("<td></td>");
                                break;
                            case 6:
                                strContent += string.Format("<td>{0}</td>", content);
                                strContent += string.Format("<td style='color:black;text-align:center;'>Hoàn thành</td>");
                               // strContent += string.Format("<td></td>");
                               // strContent += string.Format("<td></td>");
                                break;
                            default:break;
                        }
                        strContent += "</tr>";
                       
                    }
                    tbContent.InnerHtml = strContent;
                }
            }
        }
    }

    
}