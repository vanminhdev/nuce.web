using nuce.web.data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class _Default : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Lấy dữ liệu từ CSDL
                var res = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Get, ApiModels.ApiEndPoint.GetTinTuc, "");
                if (res.IsSuccessStatusCode)
                {
                    var tinTucList = await CustomizeHttp.DeserializeAsync<List<ApiModels.TinTucModel>>(res.Content);

                    string strThongBao = "";
                    string strVanBan = "";
                    string strTuyenDung = "";
                    string strHocBong = "";
                    string strTemplate = "<a href=\"[link]\" style=\"display: block\" class=\"main-color\">[title]: [updated_time]<img src = \"/style/public/images/icons/warning.png\" class=\"ml-3\"/></a>";
                    for (int i = 0; i < tinTucList.Count; i++)
                    {
                        var tinTuc = tinTucList[i];

                        string id = tinTuc.Id.ToString();
                        string title = tinTuc.Title ?? "";
                        string updatedTime = tinTuc.UpdateDatetime.ToString("dd/MM/yyyy");

                        switch (tinTuc.CatId.ToString())
                        {
                            case "11":
                                //      strThongBao += string.Format(@"<div style='font-size: 1rem;'>
                                //     <i class='fas fa-exclamation'></i>
                                //  <a href = '/ChiTietBaiTin?ID={0}'>{1} - {2:dd/MM/yyyy}</a>
                                //</div>", dtData.Rows[i]["ID"].ToString(), dtData.Rows[i]["title"].ToString(), DateTime.Parse(dtData.Rows[i]["update_datetime"].ToString()));
                                strThongBao += strTemplate.Replace("[link]", $"/ChiTietBaiTin?ID={id}")
                                                    .Replace("[title]", title)
                                                    .Replace("[updated_time]", updatedTime);
                                break;
                            case "10":
                                //      strVanBan += string.Format(@"<div style='font-size: 1rem;'>
                                //     <i class='fas fa-check'></i>
                                //  <a href = '/ChiTietBaiTin?ID={0}'>{1} - {2:dd/MM/yyyy}</a>
                                //</div>", dtData.Rows[i]["ID"].ToString(), dtData.Rows[i]["title"].ToString(), DateTime.Parse(dtData.Rows[i]["update_datetime"].ToString()));
                                strVanBan += strTemplate.Replace("[link]", $"/ChiTietBaiTin?ID={id}")
                                                          .Replace("[title]", title)
                                                          .Replace("[updated_time]", updatedTime);
                                break;
                            case "12":
                                //      strTuyenDung += string.Format(@"<div style='font-size: 1rem;'>
                                //     <i class='fas fa-exclamation'></i>
                                //  <a href = '/ChiTietBaiTin?ID={0}'>{1} - {2:dd/MM/yyyy}</a>
                                //</div>", dtData.Rows[i]["ID"].ToString(), dtData.Rows[i]["title"].ToString(), DateTime.Parse(dtData.Rows[i]["update_datetime"].ToString()));
                                strTuyenDung += strTemplate.Replace("[link]", $"/ChiTietBaiTin?ID={id}")
                                                                  .Replace("[title]", title)
                                                                  .Replace("[updated_time]", updatedTime);
                                break;
                            case "13":
                                //      strHocBong += string.Format(@"<div style='font-size: 1rem;'>
                                //     <i class='fas fa-exclamation'></i>
                                //  <a href = '/ChiTietBaiTin?ID={0}'>{1} - {2:dd/MM/yyyy}</a>
                                //</div>", dtData.Rows[i]["ID"].ToString(), dtData.Rows[i]["title"].ToString(), DateTime.Parse(dtData.Rows[i]["update_datetime"].ToString()));
                                strHocBong += strTemplate.Replace("[link]", $"/ChiTietBaiTin?ID={id}")
                                                                  .Replace("[title]", title)
                                                                  .Replace("[updated_time]", updatedTime);
                                break;

                        }
                    }

                    divThongBaoSinhVien.InnerHtml = strThongBao;
                    divHocBong.InnerHtml = strHocBong;
                    divTuyenDung.InnerHtml = strTuyenDung;
                    divVanBan.InnerHtml = strVanBan;
                }
            }
        }
    }
}