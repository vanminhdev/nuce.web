using DotNetNuke.Entities.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using nuce.web.data;
using System.IO;
using nuce.web;
using System.Globalization;
using System.Text;

namespace nuce.ad.thichungchi
{
    public partial class QLNguoiThi : PortalModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
            string sql = "select * from Nuce_ThiChungChi_DanhMuc where Status<>4";
            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_ThiChungChi.ConnectionString, CommandType.Text, sql).Tables[0];
            ddlDanhMuc.DataTextField = "Ten";
            ddlDanhMuc.DataValueField = "ID";
            ddlDanhMuc.DataSource = dt;
            ddlDanhMuc.DataBind();
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strJsThamSo = string.Format(@"<script>var link_root='/tabid/{0}/default.aspx'; var strCtl='{1}';</script>", this.TabId, txtCMT.ClientID.Replace("txtCMT", ""));
                txtID.Text = "-1";
                txtLinkUpload.Text= DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "import", "mid/" + this.ModuleId.ToString() + "/danhmuc/");
                spThamSo.InnerHtml = strJsThamSo;
                //Lay id
                int iID = -1;
                if (Request.QueryString["ID"] != null)
                {
                    if (int.TryParse(Request.QueryString["ID"].ToString(), out iID))
                    {
                        //Lay du lieu tu datat base
                        string sql = "select * from [dbo].[NUCE_ThiChungChi_NguoiThi] where ID=" + iID;
                        DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_ThiChungChi.ConnectionString, CommandType.Text, sql).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            txtID.Text = dt.Rows[0]["ID"].ToString();
                            ddlDanhMuc.SelectedValue = dt.Rows[0]["DanhMucID"].ToString();

                            txtMa.Text = dt.Rows[0]["Ma"].ToString();
                            txtHo.Text = dt.Rows[0]["Ho"].ToString();
                            txtTen.Text = dt.Rows[0]["Ten"].ToString();
                            txtNgaySinh.Text = DateTime.Parse(dt.Rows[0]["NgaySinh"].ToString()).ToString("dd/MM/yyyy");
                            txtNoiSinh.Text = dt.Rows[0]["NoiSinh"].ToString();
                            rblGioiTinh.SelectedValue = dt.Rows[0]["GioiTinh"].ToString();
                            txtCMT.Text = dt.Rows[0]["CMT"].ToString();
                            txtNgayCap.Text = DateTime.Parse(dt.Rows[0]["NgayCap"].ToString()).ToString("dd/MM/yyyy");
                            txtNoiCap.Text = dt.Rows[0]["NoiCap"].ToString();
                            txtMobile.Text = dt.Rows[0]["Mobile"].ToString();
                            txtEmail.Text = dt.Rows[0]["Email"].ToString();
                            txtDiaChi.Text = dt.Rows[0]["DiaChi"].ToString();
                            imgAnh.Src = dt.Rows[0]["Anh"].ToString();
                            string myScript = string.Format(@"<script>
            QuanLyNguoiThi.message('myModal','Cập nhật','');
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
            string strMa = txtMa.Text.Trim();
            string strMatKhau = RandomString(6, true);
            string strHo = txtHo.Text.Trim();
            string strTen = txtTen.Text.Trim();
            string strNgaySinh = txtNgaySinh.Text.Trim();
            DateTime dtNgaySinh = DateTime.Now;
            try
            {
                dtNgaySinh = DateTime.ParseExact(strNgaySinh, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch
            {

            }
            string strNoiSinh = txtNoiSinh.Text.Trim();
            string strGioiTinh = rblGioiTinh.SelectedValue.Trim();
            string strCMT = txtCMT.Text.Trim();
            string strNgayCap = txtNgayCap.Text.Trim();
            DateTime dtNgayCap = DateTime.Now;
            try
            {
                dtNgayCap = DateTime.ParseExact(strNgayCap, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch
            {

            }
            string strNoiCap = txtNoiCap.Text.Trim();
            string strMobile = txtMobile.Text.Trim();
            string strEmail = txtEmail.Text.Trim();
            string strDiaChi = txtDiaChi.Text.Trim();
            string strLinkFile = imgAnh.Src;
            string myScript = "";

            if (strMa.Equals(""))
            {
                myScript = string.Format(@"<script>
            QuanLyNguoiThi.message('myModal','{0}','{0} thất bại. {1}');
            </script>", strType, "Không để mã trắng");
                spScript.InnerHtml = myScript;
                return;
            }
            if (strTen.Equals(""))
            {
                myScript = string.Format(@"<script>
            QuanLyNguoiThi.message('myModal','{0}','{0} thất bại. {1}');
            </script>", strType, "Không để tên trắng");
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
                        if (FileUploadControl.PostedFile.ContentLength > 5000)
                        {
                            string strMoRongFile = Path.GetExtension(FileUploadControl.FileName);
                            string filename = Path.GetFileName(FileUploadControl.FileName);
                            string strTenCu = filename;
                            string strTenMoi = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", Utils.RemoveUnicode(strHo).Replace(" ", "_"), Utils.RemoveUnicode(strTen).Replace(" ", "_"), strMa, 0, 0, Utils.RemoveUnicode(filename).Replace(" ", "_"));
                            string directoryPath = Server.MapPath("~/NuceDataUpload/" + strMa);
                            if (!System.IO.Directory.Exists(directoryPath))
                                System.IO.Directory.CreateDirectory(directoryPath);

                            string path = Server.MapPath("~/NuceDataUpload/" + strMa + "/" + strTenMoi);
                            strLinkFile = path;
                            FileInfo file = new FileInfo(path);
                            if (file.Exists)//check file exsit or not
                            {
                                file.Delete();
                            }
                            FileUploadControl.SaveAs(strLinkFile);
                            strLinkFile = "/NuceDataUpload/" + strMa + "/" + strTenMoi;
                            imgAnh.Src = strLinkFile;
                        }
                        else
                        {
                            myScript = string.Format(@"<script>
            QuanLyNguoiThi.message('myModal','{0}','{0} thất bại. Ảnh đính kèm không được Vượt quá 5MB');
            </script>", strType);
                            spScript.InnerHtml = myScript;
                            return;
                        }
                    }
                    else
                    {
                        myScript = string.Format(@"<script>
            QuanLyNguoiThi.message('myModal','{0}','{0} thất bại. File upload không phải là ảnh ');
            </script>", strType);
                        spScript.InnerHtml = myScript;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    myScript = string.Format(@"<script>
            QuanLyNguoiThi.message('myModal','{0}','{0} thất bại. {1}');
            </script>", strType, "Lỗi hệ thống");
                    spScript.InnerHtml = myScript;
                    spThongBao.InnerHtml = ex.ToString();
                    return;
                }
            }
            if (iID.Equals(-1))
            {
                object objReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(Nuce_ThiChungChi.ConnectionString, "NUCE_ThiChungChi_NguoiThi_Insert"
                                , strMa, strMatKhau, strHo, strTen, dtNgaySinh, strNoiSinh, strGioiTinh, strCMT, dtNgayCap, strNoiCap, strMobile, strEmail, strDiaChi,
                                strLinkFile, 1, 1, iDanhMucID);
                if (objReturn.ToString().Equals("-1"))
                {
                    myScript = string.Format(@"<script>
            QuanLyNguoiThi.message('myModal','{0}','{0} thất bại. {1}');
            </script>", strType, "Mã đã tồn tại");
                }
                else
                {
                    myScript = string.Format(@"<script>
            QuanLyNguoiThi.message('myModal','{0}','{0} thành công');
            </script>", strType);
                }
                spScript.InnerHtml = myScript;
            }
            else
            {
                // cap nhat
                object objReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(Nuce_ThiChungChi.ConnectionString, "NUCE_ThiChungChi_NguoiThi_Update",
                               iID, strHo, strTen, dtNgaySinh, strNoiSinh, strGioiTinh, strCMT, dtNgayCap, strNoiCap, strMobile, strEmail, strDiaChi,
                               strLinkFile, iDanhMucID);
                if (objReturn.ToString().Equals("-1"))
                {
                    myScript = string.Format(@"<script>
            QuanLyNguoiThi.message('myModal','{0}','{0} thất bại. {1}');
            </script>", strType, "Mã không tồn tại");
                }
                else
                {
                    myScript = string.Format(@"<script>
            QuanLyNguoiThi.message('myModal','{0}','{0} thành công');
            </script>", strType);
                }
                spScript.InnerHtml = myScript;
            }
        }
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }
}