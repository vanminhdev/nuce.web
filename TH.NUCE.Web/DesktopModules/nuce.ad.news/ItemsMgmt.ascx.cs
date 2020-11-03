using DotNetNuke.Entities.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using nuce.web.data;
using System.IO;
using nuce.web;
using System.Globalization;
using System.Text;

namespace nuce.ad.news
{
    public partial class ItemsMgmt : PortalModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
            string sql = "select * from AS_News_Cats where Status<>4 order by Count";
            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
            ddlDanhMuc.DataTextField = "Name";
            ddlDanhMuc.DataValueField = "ID";
            ddlDanhMuc.DataSource = dt;
            ddlDanhMuc.DataBind();
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strJsThamSo = string.Format(@"<script>var link_root='/tabid/{0}/default.aspx'; var strCtl='{1}';</script>", this.TabId, txtTieuDe.ClientID.Replace("txtTieuDe", ""));
                txtID.Text = "-1";
                spThamSo.InnerHtml = strJsThamSo;
                //Lay id
                int iID = -1;
                if (Request.QueryString["ID"] != null)
                {
                    if (int.TryParse(Request.QueryString["ID"].ToString(), out iID))
                    {
                        //Lay du lieu tu datat base
                        string sql = "select * from [dbo].[AS_News_Items] where ID=" + iID;
                        DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            txtID.Text = dt.Rows[0]["ID"].ToString();
                            ddlDanhMuc.SelectedValue = dt.Rows[0]["CatID"].ToString();
                            txtTieuDe.Text= dt.Rows[0]["title"].ToString();
                            txtMoTa.Text= dt.Rows[0]["description"].ToString();
                            txtNoiDung.Text= dt.Rows[0]["new_content"].ToString();
                            imgAnh.Src = dt.Rows[0]["avatar"].ToString();
                            string myScript = string.Format(@"<script>
            ItemsMgmt.message('myModal','Cập nhật','');
            </script>");
                            
                            spScript.InnerHtml = myScript;


                        }
                        else
                            Response.Redirect(string.Format("/tabid/{0}/default.aspx", this.TabId));
                    }
                    else
                        Response.Redirect(string.Format("/tabid/{0}/default.aspx", this.TabId));
                }

            }
        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            int iID = int.Parse(txtID.Text.Trim());
            int iDanhMucID = int.Parse(ddlDanhMuc.SelectedValue);
            string strType = "";
            if (iID.Equals(-1))
                strType = "Thêm mới";
            else
                strType = "Cập nhật";

            string strTieuDe = txtTieuDe.Text.Trim();
            string strMota = txtMoTa.Text;
            string strNoiDung = txtNoiDung.Text.Trim();
            string strLinkFile = imgAnh.Src;
            string myScript = "";

            if (strTieuDe.Equals(""))
            {
                myScript = string.Format(@"<script>
            ItemsMgmt.message('myModal','{0}','{0} thất bại. {1}');
            </script>", strType, "Không để tiêu đề trắng");
                spScript.InnerHtml = myScript;
                return;
            }
            if (FileUploadControl.HasFile)
            {
                try
                {
                    string strFileGioiHan = "image/bmp;image/gif;image/jpeg;image/jpg;image/png;image/tif;";
                    if (strFileGioiHan.Contains(FileUploadControl.PostedFile.ContentType))
                    {
                        if (FileUploadControl.PostedFile.ContentLength / 1048576.0< 5)
                        {
                            string strMoRongFile = Path.GetExtension(FileUploadControl.FileName);
                            string filename = Path.GetFileName(FileUploadControl.FileName);
                            string strTenCu = filename;
                            string strDanhMuc = Utils.RemoveUnicode(ddlDanhMuc.SelectedItem.Text).Replace(" ","_");
                            string strTenMoi = string.Format("{0}", Utils.RemoveUnicode(filename).Replace(" ", "_"));
                            string directoryPath = Server.MapPath("~/NuceDataUpload/" + strDanhMuc);
                            if (!System.IO.Directory.Exists(directoryPath))
                                System.IO.Directory.CreateDirectory(directoryPath);

                            string path = Server.MapPath("~/NuceDataUpload/" + strDanhMuc + "/" + strTenMoi);
                            strLinkFile = path;
                            FileInfo file = new FileInfo(path);
                            if (file.Exists)//check file exsit or not
                            {
                                file.Delete();
                            }
                            FileUploadControl.SaveAs(strLinkFile);
                            strLinkFile = "/NuceDataUpload/" + strDanhMuc + "/" + strTenMoi;
                            imgAnh.Src = strLinkFile;
                        }
                        else
                        {
                            myScript = string.Format(@"<script>
            ItemsMgmt.message('myModal','{0}','{0} thất bại. Ảnh đính kèm không được Vượt quá 5MB');
            </script>", strType);
                            spScript.InnerHtml = myScript;
                            return;
                        }
                    }
                    else
                    {
                        myScript = string.Format(@"<script>
            ItemsMgmt.message('myModal','{0}','{0} thất bại. File upload không phải là ảnh ');
            </script>", strType);
                        spScript.InnerHtml = myScript;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    myScript = string.Format(@"<script>
            ItemsMgmt.message('myModal','{0}','{0} thất bại. {1}');
            </script>", strType, "Lỗi hệ thống");
                    spScript.InnerHtml = myScript;
                    spThongBao.InnerHtml = ex.ToString();
                    return;
                }
            }
            if (iID.Equals(-1))
            {
                object objReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(Nuce_Common.ConnectionString, "AS_News_Items_Insert",
                             strTieuDe, strLinkFile, "", strMota, strNoiDung, this.UserInfo.Username, ddlDanhMuc.SelectedItem.Value);
                if (objReturn.ToString().Equals("-1"))
                {
                    myScript = string.Format(@"<script>
            ItemsMgmt.message('myModal','{0}','{0} thất bại. {1}');
            </script>", strType, "Mã đã tồn tại");
                }
                else
                {
                    myScript = string.Format(@"<script>
            ItemsMgmt.message('myModal','{0}','{0} thành công');
            </script>", strType);
                }
                spScript.InnerHtml = myScript;
            }
            else
            {
                // cap nhat
                object objReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(Nuce_Common.ConnectionString, "AS_News_Items_Update"
                               , iID, strTieuDe, strLinkFile, "", strMota, strNoiDung, this.UserInfo.Username, ddlDanhMuc.SelectedItem.Value);
             
                if (objReturn.ToString().Equals("-1"))
                {
                    myScript = string.Format(@"<script>
            ItemsMgmt.message('myModal','{0}','{0} thất bại. {1}');
            </script>", strType, "Mã không tồn tại");
                }
                else
                {
                    myScript = string.Format(@"<script>
            ItemsMgmt.message('myModal','{0}','{0} thành công');
            </script>", strType);
                }
                spScript.InnerHtml = myScript;
            }
            
        }
    }
}