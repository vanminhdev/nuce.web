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
    public partial class VayVonNganHang : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var dien = new Dictionary<string, string>
                {
                    { "1", "Không miễn giảm" },
                    { "2", "Giảm học phí" },
                    { "3", "Miễn học phí" }
                };
                var doiTuong = new Dictionary<string, string>
                {
                    { "1", "Mồ côi" },
                    { "2", "Không mồ côi" }
                };
                var api = $"/api/DichVu/type/{(int)ApiModels.DichVu.VayVonNganHang}";
                var res = await CustomizeHttp.SendRequest(Request, HttpMethod.Get, api, "");
                List<VayVonModel> vayVonList = new List<VayVonModel>();

                if (res.IsSuccessStatusCode)
                {
                    string strResponse = await res.Content.ReadAsStringAsync();
                    vayVonList = JsonConvert.DeserializeObject<List<VayVonModel>>(strResponse);
                }
                if (vayVonList.Count > 0)
                {
                    string strContent = "";
                    for (int i = 0; i < vayVonList.Count; i++)
                    {
                        var vayVon = vayVonList[i];
                        string strPhanHoi = vayVon.PhanHoi ?? "";
                        strContent += "<tr>";
                        strContent += string.Format("<td style=\"text-align:center;\">{0}</td>", (i + 1));
                        strContent += string.Format("<td style=\"text-align:center;\">{0}</td>", vayVon.CreatedTime?.ToString("dd/MM/yyyy"));
                        //if(strPhanHoi.Trim()=="")
                        //    strContent += string.Format("<td>{0}</td>", dt.Rows[i]["LyDo"].ToString());
                        //else
                        //    strContent += string.Format("<td><div>{0}<div><div style='color:red;'>- Phản hồi: {1}</div></td>", dt.Rows[i]["LyDo"].ToString(),strPhanHoi);
                        int status = vayVon.Status ?? 0;
                        var ID = vayVon.Id;
                        DateTime dtNgayHenBatDau = vayVon.NgayHenTuNgay ?? DateTime.Now;
                        DateTime dtNgayHenDenNgay = vayVon.NgayHenDenNgay ?? DateTime.Now.AddDays(7);
                        //dtThuocDien dt.Rows[i]["ThuocDoiTuong"].ToString()
                        string dtThuocDien = "";
                        string dtThuocDoiTuong = "";
                        if (!string.IsNullOrEmpty(vayVon.ThuocDien))
                        {
                            dtThuocDien = dien[vayVon.ThuocDien];
                        }
                        if (!string.IsNullOrEmpty(vayVon.ThuocDoiTuong))
                        {
                            dtThuocDoiTuong = doiTuong[vayVon.ThuocDoiTuong];
                        }
                        switch (status)
                        {
                            case 1:
                                strContent += $"<td>Thuộc diện: {dtThuocDien}</br>Thuộc đối tượng: {dtThuocDoiTuong}</td>";
                                strContent += string.Format("<td style='color:green;text-align:center;'>Đang chờ xác nhận</td>");
                                // strContent += string.Format("<td><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"GioiThieu.fillData({0});\">Cập nhật mã xác nhận</button></td>", ID);
                                //  strContent += string.Format("<td><button type=\"button\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#myModal1\" onclick=\"GioiThieu.initDelete({0},'Bạn có chắc chắn muốn hủy dịch vụ đã yêu cầu?');\">Hủy yêu cầu</button></td>", ID);
                                break;
                            case 2:
                                strContent += $"<td>Thuộc diện: {dtThuocDien}</br>Thuộc đối tượng: {dtThuocDoiTuong}</td>";
                                strContent += string.Format("<td style='color:blue;text-align:center;'>Đã gửi lên nhà trường</td>");
                                // strContent += string.Format("<td></td>");
                                // strContent += string.Format("<td></td>");
                                break;
                            case 3:
                                strContent += $"<td>Thuộc diện: {dtThuocDien}</br>Thuộc đối tượng: {dtThuocDoiTuong}</td>";
                                strContent += string.Format("<td style='color:darkgreen;text-align:center;'>Đã tiếp nhận và đang xử lý</td>");
                                //  strContent += string.Format("<td></td>");
                                //  strContent += string.Format("<td></td>");
                                break;
                            case 4:
                                strContent += $"<td><div>Thuộc diện: {dtThuocDien}</br>Thuộc đối tượng: {dtThuocDoiTuong}</div>";
                                strContent += $"<div style='color:blue;'>* Ngày hẹn: Từ <b>{dtNgayHenBatDau.Hour}</b> giờ - <b>{dtNgayHenBatDau.Minute} </b> phút - Ngày <b>{dtNgayHenBatDau.ToString("dd/MM/yyyy")}</b> </br>";
                                strContent += $"Đến <b>{dtNgayHenDenNgay.Hour}</b> giờ - <b>{dtNgayHenDenNgay.Minute} </b> phút - Ngày <b>{dtNgayHenDenNgay.ToString("dd/MM/yyyy")}</b></div></td>";
                                strContent += string.Format("<td style='color:red;text-align:center;'>Đã xử lý và có lịch hẹn</td>");
                                // strContent += string.Format("<td><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal2\" onclick=\"GioiThieu.initDelete({0},'Bạn có chắc chắn muốn xác nhận hoàn thành dịch vụ?');\">Xác nhận hoàn thành</button></td>", ID);
                                // strContent += string.Format("<td></td>");
                                break;
                            case 5:
                                strContent += $"<td><div>Thuộc diện: {dtThuocDien}</br>Thuộc đối tượng: {dtThuocDoiTuong}</div><div style='color:red;'>- Phản hồi: {strPhanHoi}</div></td>";
                                strContent += string.Format("<td style='color:red;text-align:center;'>Từ chối dịch vụ</td>");
                                // strContent += string.Format("<td></td>");
                                // strContent += string.Format("<td></td>");
                                break;
                            case 6:
                                strContent += $"<td>Thuộc diện: {dtThuocDien}</br>Thuộc đối tượng: {dtThuocDoiTuong}</td>";
                                strContent += string.Format("<td style='color:black;text-align:center;'>Hoàn thành</td>");
                                // strContent += string.Format("<td></td>");
                                // strContent += string.Format("<td></td>");
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