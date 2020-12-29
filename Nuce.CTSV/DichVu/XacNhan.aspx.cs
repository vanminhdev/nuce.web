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
    public partial class XacNhan : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string api = $"/api/DichVu/type/{(int)DichVu.XacNhan}";
                var res = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Get, api, "");
                List<XacNhanModel> xacNhanList = new List<XacNhanModel>();

                if (res.IsSuccessStatusCode)
                {
                    string strResponse = await res.Content.ReadAsStringAsync();
                    xacNhanList = JsonConvert.DeserializeObject<List<XacNhanModel>>(strResponse);
                }

                if (xacNhanList.Count > 0)
                {
                    string strContent = "";
                    for (int i = 0; i < xacNhanList.Count; i++)
                    {
                        var xacNhan = xacNhanList[i];
                        string strPhanHoi = xacNhan.PhanHoi;
                        strContent += "<tr>";
                        strContent += string.Format("<td style=\"text-align:center;\">{0}</td>", (i+1));
                        strContent += string.Format("<td style=\"text-align:center;\">{0}</td>", xacNhan.CreatedTime?.ToString("dd/MM/yyyy"));

                        int status = xacNhan.Status ?? -1;
                        
                        DateTime dtNgayHenBatDau = xacNhan.NgayHenTuNgay ?? DateTime.Now;
                        DateTime dtNgayHenDenNgay = xacNhan.NgayHenDenNgay ?? DateTime.Now.AddDays(7);
                        switch (status)
                        {
                            case 1:
                                strContent += string.Format("<td>{0}</td>", xacNhan.LyDo);
                                strContent += string.Format("<td style='color:green;text-align:center;'>Đang chờ xác nhận</td>");
                                break;
                            case 2:
                                strContent += string.Format("<td>{0}</td>", xacNhan.LyDo);
                                strContent += string.Format("<td style='color:blue;text-align:center;'>Đã gửi lên nhà trường</td>");
                                break;
                            case 3:
                                strContent += string.Format("<td>{0}</td>", xacNhan.LyDo);
                                strContent += string.Format("<td style='color:darkgreen;text-align:center;'>Đã tiếp nhận và đang xử lý</td>");
                                break;
                            case 4:
                                strContent += string.Format(@"<td><div>{6}</div><div style='color:blue;'>* Ngày hẹn: Từ <b>{0}</b> giờ - <b>{1} </b> phút - Ngày <b>{2:dd/MM/yyyy}</b> </br>Đến <b>
                                    {3}</b> giờ - <b>{4} </b> phút - Ngày <b>{5:dd/MM/yyyy}</b></div></td>", dtNgayHenBatDau.Hour, dtNgayHenBatDau.Minute, dtNgayHenBatDau,dtNgayHenDenNgay.Hour, dtNgayHenDenNgay.Minute, dtNgayHenDenNgay, xacNhan.LyDo);
                                strContent += string.Format("<td style='color:red;text-align:center;'>Đã xử lý và có lịch hẹn</td>");
                                break;
                            case 5:
                                strContent += string.Format("<td><div>{0}<div><div style='color:red;'>- Phản hồi: {1}</div></td>", xacNhan.LyDo, strPhanHoi);
                                strContent += string.Format("<td style='color:red;text-align:center;'>Từ chối dịch vụ</td>");
                                break;
                            case 6:
                                strContent += string.Format("<td>{0}</td>", xacNhan.LyDo);
                                strContent += string.Format("<td style='color:black;text-align:center;'>Hoàn thành</td>");
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