using Newtonsoft.Json;
using nuce.web.data;
using Nuce.CTSV.ApiModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class ThueNhaSinhVien : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var api = $"/api/DichVu/type/{(int)ApiModels.DichVu.ThueNha}";
                var studentResponse = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Get, api, "");
                List<ThueNhaModel> thueNhaList = new List<ThueNhaModel>();

                if (studentResponse.IsSuccessStatusCode)
                {
                    string strResponse = await studentResponse.Content.ReadAsStringAsync();
                    thueNhaList = JsonConvert.DeserializeObject<List<ThueNhaModel>>(strResponse);
                }
                if (thueNhaList.Count > 0)
                {
                    string strContent = "";
                    for (int i = 0; i < thueNhaList.Count; i++)
                    {
                        var thueNha = thueNhaList[i];
                        string strPhanHoi = thueNha.PhanHoi ?? "";
                        strContent += "<tr>";
                        strContent += string.Format("<td style=\"text-align:center;\">{0}</td>", (i+1));
                        strContent += string.Format("<td style=\"text-align:center;\">{0}</td>", thueNha.CreatedTime?.ToString("dd/MM/yyyy"));
                        //if(strPhanHoi.Trim()=="")
                        //    strContent += string.Format("<td>{0}</td>", dt.Rows[i]["LyDo"].ToString());
                        //else
                        //    strContent += string.Format("<td><div>{0}<div><div style='color:red;'>- Phản hồi: {1}</div></td>", dt.Rows[i]["LyDo"].ToString(),strPhanHoi);
                        int status = thueNha.Status ?? 0;
                        long ID = thueNha.Id;
                        DateTime dtNgayHenBatDau = thueNha.NgayHenTuNgay ?? DateTime.Now;
                        DateTime dtNgayHenDenNgay = thueNha.NgayHenDenNgay ?? DateTime.Now.AddDays(7);

                        string ThongBaoChuyenPhatNhanh = UpdateDiaChiChuyenPhatNhanh.Enabled && (thueNha.ChuyenPhatNhanh ?? false) ? UpdateDiaChiChuyenPhatNhanh.NoiDungThongBao : "";
                        switch (status)
                        {
                            case 1:
                                strContent += "<td></td>";
                                strContent += string.Format("<td style='color:green;text-align:center;'>Đang chờ xác nhận</td>");
                               // strContent += string.Format("<td><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"ThueNhaSinhVien.fillData({0});\">Cập nhật mã xác nhận</button></td>", ID);
                               // strContent += string.Format("<td><button type=\"button\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#myModal1\" onclick=\"ThueNhaSinhVien.initDelete({0},'Bạn có chắc chắn muốn hủy dịch vụ đã yêu cầu?');\">Hủy yêu cầu</button></td>", ID);
                                break;
                            case 2:
                                strContent += "<td></td>";
                                strContent += string.Format("<td style='color:blue;text-align:center;'>Đã gửi lên nhà trường</td>");
                                //strContent += string.Format("<td></td>");
                                //strContent += string.Format("<td></td>");
                                break;
                            case 3:
                                strContent += "<td></td>";
                                strContent += string.Format("<td style='color:darkgreen;text-align:center;'>Đã tiếp nhận và đang xử lý</td>");
                               // strContent += string.Format("<td></td>");
                               // strContent += string.Format("<td></td>");
                                break;
                            case 4:
                                strContent += string.Format(@"<td>{6}<div style='color:blue;'>* Ngày hẹn: Từ <b>{0}</b> giờ - <b>{1} </b> phút - Ngày <b>{2:dd/MM/yyyy}</b> </br>Đến <b>
                                    {3}</b> giờ - <b>{4} </b> phút - Ngày <b>{5:dd/MM/yyyy}</b></div></td>", dtNgayHenBatDau.Hour, dtNgayHenBatDau.Minute, dtNgayHenBatDau,dtNgayHenDenNgay.Hour, dtNgayHenDenNgay.Minute, dtNgayHenDenNgay, ThongBaoChuyenPhatNhanh);
                                strContent += string.Format("<td style='color:red;text-align:center;'>Đã xử lý và có lịch hẹn</td>");
                               // strContent += string.Format("<td><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal2\" onclick=\"ThueNhaSinhVien.initDelete({0},'Bạn có chắc chắn muốn xác nhận hoàn thành dịch vụ?');\">Xác nhận hoàn thành</button></td>", ID);
                               // strContent += string.Format("<td></td>");
                                break;
                            case 5:
                                strContent += string.Format("<td><div style='color:red;'>- Phản hồi: {0}</div></td>", strPhanHoi);
                                strContent += string.Format("<td style='color:red;text-align:center;'>Từ chối dịch vụ</td>");
                               // strContent += string.Format("<td></td>");
                               // strContent += string.Format("<td></td>");
                                break;
                            case 6:
                                strContent += "<td></td>";
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