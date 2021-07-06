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
    public partial class DangKyChoO : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string api = $"/api/DichVu/type/{(int)DichVu.DangKyChoO}";
                var res = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Get, api, "");
                List<KhaoSatNhaOModel> ksNhaOList = new List<KhaoSatNhaOModel>();

                if (res.IsSuccessStatusCode)
                {
                    string strResponse = await res.Content.ReadAsStringAsync();
                    ksNhaOList = JsonConvert.DeserializeObject<List<KhaoSatNhaOModel>>(strResponse);
                }

                if (ksNhaOList != null && ksNhaOList.Count > 0)
                {
                    string strContent = "";
                    for (int i = 0; i < ksNhaOList.Count; i++)
                    {
                        var khaoSat = ksNhaOList[i];
                        string strPhanHoi = khaoSat.PhanHoi;
                        strContent += "<tr>";
                        strContent += string.Format("<td style=\"text-align:center;\">{0}</td>", (i+1));
                        strContent += string.Format("<td style=\"text-align:center;\">{0}</td>", khaoSat.CreatedTime?.ToString("dd/MM/yyyy"));
                        //if(strPhanHoi.Trim()=="")
                        //    strContent += string.Format("<td>{0}</td>", dt.Rows[i]["LyDo"].ToString());
                        //else
                        //    strContent += string.Format("<td><div>{0}<div><div style='color:red;'>- Phản hồi: {1}</div></td>", dt.Rows[i]["LyDo"].ToString(),strPhanHoi);
                        int status = khaoSat.Status ?? -1;
                        DateTime dtNgayHenBatDau = khaoSat.NgayHenTuNgay ?? DateTime.Now;
                        DateTime dtNgayHenDenNgay = khaoSat.NgayHenDenNgay ?? DateTime.Now.AddDays(7);

                        string content = "";
                        switch ((DichVuXeBusLoaiTuyen)khaoSat.TuyenType)
                        {
                            case DichVuXeBusLoaiTuyen.MotTuyen:
                                content = $"<div>Loại tuyến: Một tuyến</div><div>Số tuyến: {khaoSat.TuyenCode ?? ""}</div>";
                                break;
                            case DichVuXeBusLoaiTuyen.LienTuyen:
                                content = $"<div>Loại tuyến: Liên tuyến</div>";
                                break;
                            default:
                                break;
                        }
                        content += $"<div>Nơi nhận thẻ: {khaoSat.NoiNhanThe ?? ""}</div>";

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