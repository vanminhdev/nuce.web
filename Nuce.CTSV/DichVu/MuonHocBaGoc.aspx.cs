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
    public partial class MuonHocBaGoc : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string api = $"/api/DichVu/type/{(int)DichVu.MuonHocBaGoc}";
                var res = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Get, api, "");
                List<MuonHocBaGocModel> muonHocBaList = new List<MuonHocBaGocModel>();

                if (res.IsSuccessStatusCode)
                {
                    string strResponse = await res.Content.ReadAsStringAsync();
                    muonHocBaList = JsonConvert.DeserializeObject<List<MuonHocBaGocModel>>(strResponse);
                }

                if (muonHocBaList.Count > 0)
                {
                    string strContent = "";
                    for (int i = 0; i < muonHocBaList.Count; i++)
                    {
                        var muonHocBa = muonHocBaList[i];
                        string strPhanHoi = muonHocBa.PhanHoi;
                        strContent += "<tr>";
                        strContent += string.Format("<td style=\"text-align:center;\">{0}</td>", (i + 1));
                        strContent += string.Format("<td style=\"text-align:center;\">{0}</td>", muonHocBa.CreatedTime?.ToString("dd/MM/yyyy"));

                        int status = muonHocBa.Status ?? -1;

                        string thoiGianMuon = muonHocBa.NgayMuon != null ? (muonHocBa.NgayMuon.Value.Hour + " giờ - " + muonHocBa.NgayMuon.Value.Minute + " phút - Ngày " + muonHocBa.NgayMuon.Value.ToString("dd/MM/yyyy")) : "";
                        string thoiGianTra = muonHocBa.NgayTraDuKien != null ? (muonHocBa.NgayTraDuKien.Value.Hour + " giờ - " + muonHocBa.NgayTraDuKien.Value.Minute + " phút - Ngày " + muonHocBa.NgayTraDuKien.Value.ToString("dd/MM/yyyy")) : "";
                        string noiDung = $@"
                                            <div><span class='font-weight-bold'>Thời gian mượn: {thoiGianMuon}</span></div>
                                            <div><span class='font-weight-bold'>Thời gian trả (dự kiến): </span>{thoiGianTra}</div>
                                            <div><span class='font-weight-bold'>Mô tả: </span>{muonHocBa.Description ?? ""}</div>
                                            <div><span class='font-weight-bold'>Ghi chú: </span>{muonHocBa.Notice ?? ""}</div>
                                        ";
                        DateTime dtNgayHenBatDau = muonHocBa.NgayHenTuNgay ?? DateTime.Now;
                        DateTime dtNgayHenDenNgay = muonHocBa.NgayHenDenNgay ?? DateTime.Now.AddDays(7);
                        // tiếp
                        switch (status)
                        {
                            case 1:
                                strContent += $"<td>{noiDung}</td>";
                                strContent += string.Format("<td style='color:green;text-align:center;'>Đang chờ xác nhận</td>");
                                strContent += $"<td><></td>";
                                break;
                            case 2:
                                strContent += $"<td>{noiDung}</td>";
                                strContent += string.Format("<td style='color:blue;text-align:center;'>Đã gửi lên nhà trường</td>");
                                break;
                            case 3:
                                strContent += $"<td>{noiDung}</td>";
                                strContent += string.Format("<td style='color:darkgreen;text-align:center;'>Đã tiếp nhận và đang xử lý</td>");
                                break;
                            case 4:
                                strContent += string.Format(@"<td><div>{6}</div><div style='color:blue;'>* Ngày hẹn: Từ <b>{0}</b> giờ - <b>{1} </b> phút - Ngày <b>{2:dd/MM/yyyy}</b> </br>Đến <b>
                                    {3}</b> giờ - <b>{4} </b> phút - Ngày <b>{5:dd/MM/yyyy}</b></div></td>", dtNgayHenBatDau.Hour, dtNgayHenBatDau.Minute, dtNgayHenBatDau, dtNgayHenDenNgay.Hour, dtNgayHenDenNgay.Minute, dtNgayHenDenNgay, noiDung);
                                strContent += string.Format("<td style='color:red;text-align:center;'>Đã xử lý và có lịch hẹn</td>");
                                break;
                            case 5:
                                strContent += string.Format("<td><div>{0}<div><div style='color:red;'>- Phản hồi: {1}</div></td>", noiDung, strPhanHoi);
                                strContent += string.Format("<td style='color:red;text-align:center;'>Từ chối dịch vụ</td>");
                                break;
                            case 6:
                                strContent += $"<td>{noiDung}</td>";
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