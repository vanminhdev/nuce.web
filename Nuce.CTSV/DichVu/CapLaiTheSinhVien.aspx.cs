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
    public partial class CapLaiTheSinhVien : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string api = $"/api/DichVu/type/{(int)DichVu.CapLaiThe}";
                var res = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Get, api, "");
                List<XacNhanModel> capLaiTheList = new List<XacNhanModel>();

                if (res.IsSuccessStatusCode)
                {
                    string strResponse = await res.Content.ReadAsStringAsync();
                    capLaiTheList = JsonConvert.DeserializeObject<List<XacNhanModel>>(strResponse);
                }

                if (capLaiTheList.Count > 0)
                {
                    string strContent = "";
                    for (int i = 0; i < capLaiTheList.Count; i++)
                    {
                        var capLaiThe = capLaiTheList[i];
                        string strPhanHoi = capLaiThe.PhanHoi;
                        strContent += "<tr>";
                        strContent += string.Format("<td style=\"text-align:center;\">{0}</td>", (i + 1));
                        strContent += string.Format("<td style=\"text-align:center;\">{0}</td>", capLaiThe.CreatedTime?.ToString("dd/MM/yyyy"));

                        int status = capLaiThe.Status ?? -1;

                        DateTime dtNgayHenBatDau = capLaiThe.NgayHenTuNgay ?? DateTime.Now;
                        DateTime dtNgayHenDenNgay = capLaiThe.NgayHenDenNgay ?? DateTime.Now.AddDays(7);
                        switch (status)
                        {
                            case 1:
                                strContent += "<td></td>";
                                strContent += string.Format("<td style='color:green;text-align:center;'>Đang chờ xác nhận</td>");
                                break;
                            case 2:
                                strContent += "<td></td>";
                                strContent += string.Format("<td style='color:blue;text-align:center;'>Đã gửi lên nhà trường</td>");
                                break;
                            case 3:
                                strContent += "<td></td>";
                                strContent += string.Format("<td style='color:darkgreen;text-align:center;'>Đã tiếp nhận và đang xử lý</td>");
                                break;
                            case 4:
                                strContent += string.Format(@"<td><div style='color:blue;'>* Ngày hẹn: Từ <b>{0}</b> giờ - <b>{1} </b> phút - Ngày <b>{2:dd/MM/yyyy}</b> </br>Đến <b>
                                    {3}</b> giờ - <b>{4} </b> phút - Ngày <b>{5:dd/MM/yyyy}</b></div></td>", dtNgayHenBatDau.Hour, dtNgayHenBatDau.Minute, dtNgayHenBatDau, dtNgayHenDenNgay.Hour, dtNgayHenDenNgay.Minute, dtNgayHenDenNgay);
                                strContent += string.Format("<td style='color:red;text-align:center;'>Đã xử lý và có lịch hẹn</td>");
                                break;
                            case 5:
                                strContent += string.Format("<td><div style='color:red;'>- Phản hồi: {0}</div></td>", strPhanHoi);
                                strContent += string.Format("<td style='color:red;text-align:center;'>Từ chối dịch vụ</td>");
                                break;
                            case 6:
                                strContent += "<td></td>";
                                strContent += string.Format("<td style='color:black;text-align:center;'>Hoàn thành</td>");
                                break;
                            default: break;
                        }
                        strContent += "</tr>";

                    }
                    tbContent.InnerHtml = strContent;
                }
            }
        }
    }
}