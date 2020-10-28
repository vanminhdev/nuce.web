using nuce.web.data;
using Nuce.CTSV.ApiModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Web.UI;
using Newtonsoft.Json;

namespace Nuce.CTSV
{
    public partial class GioiThieu : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var api = $"/api/DichVu/type/{(int)ApiModels.DichVu.GioiThieu}";
                var studentResponse = await CustomizeHttp.SendRequest(Request, HttpMethod.Get, api, "");
                List<GioiThieuModel> gioiThieuList = new List<GioiThieuModel>();

                if (studentResponse.IsSuccessStatusCode)
                {
                    string strResponse = await studentResponse.Content.ReadAsStringAsync();
                    gioiThieuList = JsonConvert.DeserializeObject<List<GioiThieuModel>>(strResponse);
                }

                if (gioiThieuList.Count > 0)
                {
                    string strContent = "";
                    for (int i = 0; i < gioiThieuList.Count; i++)
                    {
                        var gioiThieu = gioiThieuList[i];
                        string strPhanHoi = gioiThieu.PhanHoi;
                        strContent += "<tr>";
                        strContent += string.Format("<td style=\"text-align:center;\">{0}</td>", (i+1));
                        strContent += string.Format("<td style=\"text-align:center;\">{0}</td>", gioiThieu.CreatedTime?.ToString("dd/MM/yyyy"));
                        //if(strPhanHoi.Trim()=="")
                        //    strContent += string.Format("<td>{0}</td>", dt.Rows[i]["LyDo"].ToString());
                        //else
                        //    strContent += string.Format("<td><div>{0}<div><div style='color:red;'>- Phản hồi: {1}</div></td>", dt.Rows[i]["LyDo"].ToString(),strPhanHoi);
                        int status = gioiThieu.Status ?? 0;
                        int ID = (int)gioiThieu.Id;
                        DateTime dtNgayHenBatDau = gioiThieu.NgayHenTuNgay ?? DateTime.Now;
                        DateTime dtNgayHenDenNgay = gioiThieu.NgayHenDenNgay ?? DateTime.Now.AddDays(7);
                        switch (status)
                        {
                            case 1:
                                strContent += string.Format("<td>Kính gửi: {0}</br>Đên gặp: {1} </br>Về việc: {2}</td>", gioiThieu.DonVi ?? "", gioiThieu.DenGap ?? "", gioiThieu.VeViec ?? "");
                                strContent += string.Format("<td style='color:green;text-align:center;'>Đang chờ xác nhận</td>");
                               // strContent += string.Format("<td><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"GioiThieu.fillData({0});\">Cập nhật mã xác nhận</button></td>", ID);
                              //  strContent += string.Format("<td><button type=\"button\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#myModal1\" onclick=\"GioiThieu.initDelete({0},'Bạn có chắc chắn muốn hủy dịch vụ đã yêu cầu?');\">Hủy yêu cầu</button></td>", ID);
                                break;
                            case 2:
                                strContent += string.Format("<td>Kính gửi: {0}</br>Đên gặp: {1} </br>Về việc: {2}</td>", gioiThieu.DonVi ?? "", gioiThieu.DenGap ?? "", gioiThieu.VeViec ?? "");
                                strContent += string.Format("<td style='color:blue;text-align:center;'>Đã gửi lên nhà trường</td>");
                               // strContent += string.Format("<td></td>");
                               // strContent += string.Format("<td></td>");
                                break;
                            case 3:
                                strContent += string.Format("<td>Kính gửi: {0}</br>Đên gặp: {1} </br>Về việc: {2}</td>", gioiThieu.DonVi ?? "", gioiThieu.DenGap ?? "", gioiThieu.VeViec ?? "");
                                strContent += string.Format("<td style='color:darkgreen;text-align:center;'>Đã tiếp nhận và đang xử lý</td>");
                              //  strContent += string.Format("<td></td>");
                              //  strContent += string.Format("<td></td>");
                                break;
                            case 4:
                                strContent += string.Format(@"<td><div>Kính gửi: {6}</br>Đên gặp: {7} </br>Về việc: {8}</div><div style='color:blue;'>* Ngày hẹn: Từ <b>{0}</b> giờ - <b>{1} </b> phút - Ngày <b>{2:dd/MM/yyyy}</b> </br>Đến <b>
                                    {3}</b> giờ - <b>{4} </b> phút - Ngày <b>{5:dd/MM/yyyy}</b></div></td>", dtNgayHenBatDau.Hour, dtNgayHenBatDau.Minute, dtNgayHenBatDau,dtNgayHenDenNgay.Hour, dtNgayHenDenNgay.Minute, dtNgayHenDenNgay, gioiThieu.DonVi ?? "", gioiThieu.DenGap ?? "", gioiThieu.VeViec ?? "");
                                strContent += string.Format("<td style='color:red;text-align:center;'>Đã xử lý và có lịch hẹn</td>");
                               // strContent += string.Format("<td><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal2\" onclick=\"GioiThieu.initDelete({0},'Bạn có chắc chắn muốn xác nhận hoàn thành dịch vụ?');\">Xác nhận hoàn thành</button></td>", ID);
                               // strContent += string.Format("<td></td>");
                                break;
                            case 5:
                                strContent += string.Format("<td><div>Kính gửi: {0}</br>Đên gặp: {1} </br>Về việc: {2}<div><div style='color:red;'>- Phản hồi: {3}</div></td>", dtNgayHenDenNgay, gioiThieu.DonVi ?? "", gioiThieu.DenGap ?? "", gioiThieu.VeViec ?? "", strPhanHoi);
                                strContent += string.Format("<td style='color:red;text-align:center;'>Từ chối dịch vụ</td>");
                               // strContent += string.Format("<td></td>");
                               // strContent += string.Format("<td></td>");
                                break;
                            case 6:
                                strContent += string.Format("<td>Kính gửi: {0}</br>Đên gặp: {1} </br>Về việc: {2}</td>", gioiThieu.DonVi ?? "", gioiThieu.DenGap ?? "", gioiThieu.VeViec ?? "");
                                strContent += string.Format("<td style='color:black;text-align:center;'>Hoàn thành</td>");
                               // strContent += string.Format("<td></td>");
                               // strContent += string.Format("<td></td>");
                                break;
                            default:
                                break;
                        }
                        strContent += "</tr>";
                       
                    }
                    tbContent.InnerHtml = strContent;
                }
            }
        }
    }
}