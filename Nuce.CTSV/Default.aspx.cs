using nuce.web.data;
using System;
using System.Data;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class _Default : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Lấy dữ liệu từ CSDL
                DataTable dtData = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(nuce.web.data.Nuce_Common.ConnectionString, CommandType.Text, string.Format(@"SELECT [ID]
                          ,[CatID]
                          ,[title],update_datetime
                      FROM [dbo].[AS_News_Items]
                      where CatID in ({0}) order by update_datetime desc", "10,11,12,14")).Tables[0];
                string strThongBao = "";
                string strVanBan = "";
                string strTuyenDung = "";
                string strHocBong = "";
                string strTemplate = "<a href=\"[link]\" style=\"display: block\" class=\"main-color\">[title]: [updated_time]<img src = \"/style/public/images/icons/warning.png\" class=\"ml-3\"/></a>";
                for (int i = 0; i < dtData.Rows.Count; i++)
                {
                    string id = Nuce_CTSV.firstOrDefault(dtData.Rows, i, "ID");
                    string title = Nuce_CTSV.firstOrDefault(dtData.Rows, i, "title");
                    string updatedTime = Nuce_CTSV.firstOrDefault(dtData.Rows, i, "update_datetime");
                    updatedTime = DateTime.Parse(updatedTime).ToString("dd/MM/yyyy");

                    switch (dtData.Rows[i]["CatID"].ToString())
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