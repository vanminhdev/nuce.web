using System;
using System.Data;
using DotNetNuke.Entities.Modules;
using System.Globalization;
namespace nuce.web.commons
{
    public partial class QuanLyLopQuanLy : PortalModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
            #region Datetime

            lnkNgayBatDau.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtNgayBatDau).Replace("M/d/yyyy", "MM/dd/yyyy").Replace("d/MM/yyyy", "dd/MM/yyyy").Replace("MM/dd/yyyy", "dd/MM/yyyy");
            //Điền dữ liệu vào các control
            imgNgayBatDau.ImageUrl = string.Format("{0}/images/calendar.png", this.ModulePath);

            lnkNgayKetThuc.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtNgayKetThuc).Replace("M/d/yyyy", "MM/dd/yyyy").Replace("d/MM/yyyy", "dd/MM/yyyy").Replace("MM/dd/yyyy", "dd/MM/yyyy");
            //Điền dữ liệu vào các control
            imgNgayKetThuc.ImageUrl = string.Format("{0}/images/calendar.png", this.ModulePath);

            //Bindata

            DataTable tblTable = data.dnn_NuceCommon_NamHoc.getName(-1);

            ddlNamHoc.DataSource = tblTable;
            ddlNamHoc.DataTextField = "Ten";
            ddlNamHoc.DataValueField = "NamHocID";
            ddlNamHoc.DataBind();

            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divFilter.Visible = false;
                divChangeKhoa.Visible = false;
                divEdit.Visible = false;
                divContent.Visible = false;
                if (Request.QueryString["EditType"] != null && Request.QueryString["EditType"] != "")
                {
                    string strType = Request.QueryString["EditType"];
                    txtType.Text = strType;
                    divAnnouce.InnerHtml = ""; 
                    if (strType == "edit" ||strType=="insert")
                    {
                        divEdit.Visible = true;
                        //Lấy dữ liệu từ server để nhét vào các control
                        //THCore_CM_GetTag
                        int iKhoaID = int.Parse(Request.QueryString["khoaid"]);
                        string strKhoa = Request.QueryString["khoaname"];
                        txtKhoaID.Text = iKhoaID.ToString();
                        divNameKhoa.InnerText = strKhoa;
                        if (strType == "insert")
                        {
                            divAnnouce.InnerHtml = "Thêm mới bộ môn";
                        }
                        else
                        {
                            divAnnouce.InnerHtml = "Chỉnh sửa thông tin bộ môn";
                            int iID = int.Parse(Request.QueryString["id"]);
                            DataTable tblTable = data.dnn_NuceCommon_LopQuanLy.get(iID);
                            if (tblTable.Rows.Count > 0)
                            {
                                txtMa.Text = tblTable.Rows[0]["Ma"].ToString();
                                txtTen.Text = tblTable.Rows[0]["Ten"].ToString();
                                ddlNamHoc.SelectedValue = tblTable.Rows[0]["NamHocID"].ToString();
                                txtNgayBatDau.Text = DateTime.Parse(tblTable.Rows[0]["NgayBatDau"].ToString()).ToString("dd/MM/yyyy");
                                txtNgayKetThuc.Text = DateTime.Parse(tblTable.Rows[0]["NgayKetThuc"].ToString()).ToString("dd/MM/yyyy");
                                txtMoTa.Text = tblTable.Rows[0]["MoTa"].ToString();
                                txtID.Text = iID.ToString();
                            }
                        }
                        
                    }
                    else if(strType == "delete")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceCommon_LopQuanLy.updateStatus(iID, 4);
                        returnMain(Request.QueryString["khoaid"]);
                    }
                    else if (strType == "changefinish")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceCommon_LopQuanLy.updateStatus(iID, 3);
                        returnMain(Request.QueryString["khoaid"]);
                    }
                    else if (strType == "changeactive")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceCommon_LopQuanLy.updateStatus(iID, 1);
                        returnMain(Request.QueryString["khoaid"]);
                    }

                    else if (strType == "changekhoa")
                    {
                        DataTable tblTable = data.dnn_NuceCommon_Khoa.getName(-1);
                        ddlChangeKhoa.DataSource = tblTable;
                        ddlChangeKhoa.DataTextField = "Ten";
                        ddlChangeKhoa.DataValueField = "KhoaID";
                        ddlChangeKhoa.DataBind();
                    
                        divChangeKhoa.Visible = true;
                        int iID = int.Parse(Request.QueryString["id"]);
                        ddlChangeKhoa.SelectedValue = Request.QueryString["khoaid"];
                        txtID.Text = iID.ToString();
                    }
                }
                else
                {
                    //Bindata
                    DataTable tblTable = data.dnn_NuceCommon_Khoa.getName(-1);
                    ddlKhoa.DataSource = tblTable;
                    ddlKhoa.DataTextField = "Ten";
                    ddlKhoa.DataValueField = "KhoaID";
                    ddlKhoa.DataBind();
                    string strKhoaID = tblTable.Rows[0]["KhoaID"].ToString();
                    string strKhoaName = tblTable.Rows[0]["Ten"].ToString();
                    if (Request.QueryString["khoaid"] != null && Request.QueryString["khoaid"] != "")
                    {
                        strKhoaID = Request.QueryString["khoaid"];
                        DataRow[] drRs = tblTable.Select(string.Format("KhoaID={0}", strKhoaID));
                        strKhoaName = drRs[0]["Ten"].ToString();
                       //strKhoaName = drRs.Length.ToString();
                    }
                    ddlKhoa.SelectedValue = strKhoaID;
                    txtKhoaID.Text = strKhoaID;
                    divNameKhoa.InnerText = strKhoaName;
                    divAnnouce.InnerHtml = "";
                    divFilter.Visible = true;
                    divContent.Visible = true;
                    displayList(int.Parse(strKhoaID), strKhoaName);
                }
            }
        }

        private void displayList(int KhoaID,string KhoaName)
        {
            DataTable tblTable = data.dnn_NuceCommon_LopQuanLy.getByKhoa(KhoaID);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách lớp</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable +="<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "Mã");
            strTable += string.Format("<div class='nd_t3 fl'>{0}</div>", "Tên");
            strTable += string.Format("<div class='nd_t3 fl'>{0}</div>", "Danh sách sinh viên");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Trạng Thái");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chuyển");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chuyển khoa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chỉnh sửa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Xóa");
            strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&khoaid={1}&&khoaname={2}'>Thêm mới</a></div>", this.TabId, KhoaID, KhoaName);
            strTable += "</div>";
                    strTable += "<div class='nd_b fl'>";
                    for (int i = 0; i < tblTable.Rows.Count; i++)
                    {
                        strTable += " <div class='nd_b1 fl'>";
                        strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", (i+1));
                        strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", tblTable.Rows[i]["Ma"].ToString());
                        strTable += string.Format("<div class='nd_t3 fl'>{0}</div>", tblTable.Rows[i]["Ten"].ToString());
                        strTable += string.Format("<div class='nd_t3 fl'><a href='/tabid/{0}/default.aspx?ma={1}'>Danh sách sinh viên</a></div>", 97, tblTable.Rows[i]["Ma"].ToString());
                        int iStatus = int.Parse(tblTable.Rows[i]["status"].ToString());
                        if (iStatus.Equals(1))
                        {
                            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Hoạt động");
                            strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changefinish&&id={1}&&khoaid={2}'>Kết thúc</a></div>", this.TabId, tblTable.Rows[i]["LopQuanLyID"].ToString(), KhoaID);
                        }
                        else
                        {
                            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Kết thúc");
                            strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changeactive&&id={1}&&khoaid={2}'>Hoạt động</a></div>", this.TabId, tblTable.Rows[i]["LopQuanLyID"].ToString(), KhoaID);
                        }
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changekhoa&&id={1}&&khoaid={2}&&khoaname={3}'>Chuyển khoa</a></div>", this.TabId, tblTable.Rows[i]["LopQuanLyID"].ToString(), KhoaID, KhoaName);
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&&khoaid={2}&&khoaname={3}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["LopQuanLyID"].ToString(), KhoaID, KhoaName);
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}&khoaid={2}'>Xóa</a></div>", this.TabId, tblTable.Rows[i]["LopQuanLyID"].ToString(), KhoaID);
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&khoaid={1}&&khoaname={2}'>Thêm mới</a></div>", this.TabId, KhoaID, KhoaName);
                        strTable += "</div>";
                    }
                    strTable += "</div>";
                 strTable += "</div>";
                 strTable += "</div>";
            strTable += "</div>";
            divContent.InnerHtml = strTable;
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string strType = txtType.Text;
            string strMa = txtMa.Text;
            string strTen = txtTen.Text;
            string strMoTa = txtMoTa.Text;

            DateTime dtNgayBatDau;
            try
            {
                dtNgayBatDau = DateTime.ParseExact(txtNgayBatDau.Text, "dd/mm/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                dtNgayBatDau = DateTime.Now;
            }
            DateTime dtNgayKetThuc;
            try
            {
                dtNgayKetThuc = DateTime.ParseExact(txtNgayKetThuc.Text, "dd/mm/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                dtNgayKetThuc = DateTime.Now;
            }

            int iNamHoc = int.Parse(ddlNamHoc.SelectedValue.ToString());
            int iKhoaID = int.Parse(txtKhoaID.Text)
            ;
            if (strMa.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để mã trắng";
                return;
            }
            if (strTen.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để tên trắng";
                return;
            }

            if (strType == "insert")
            {
                data.dnn_NuceCommon_LopQuanLy.insert(iKhoaID,iNamHoc, strMa, strTen, strMoTa,dtNgayBatDau,dtNgayKetThuc);
               //DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("THCore_TM_InsertTags", iGroupTags, strName, "","","");
                returnMain(txtKhoaID.Text);
            }
            else
            {
                if (strType == "edit")
                {
                    int iID = int.Parse(txtID.Text);
                    data.dnn_NuceCommon_LopQuanLy.update(iID, iKhoaID, iNamHoc, strMa, strTen, strMoTa, dtNgayBatDau, dtNgayKetThuc);
                    returnMain(txtKhoaID.Text);
                }
            }
        }

        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            returnMain(txtKhoaID.Text)
            ;
        }
        private  void returnMain(string KhoaID)
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?khoaid={1}", this.TabId, KhoaID));
        }

        protected void ddlKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            returnMain(ddlKhoa.SelectedValue)
           ;
        }

        protected void btnChuyenKhoa_Click(object sender, EventArgs e)
        {
            int iKhoaID = int.Parse(ddlChangeKhoa.SelectedValue);
            int iID = int.Parse(txtID.Text);
            data.dnn_NuceCommon_LopQuanLy.updateKhoa(iID, iKhoaID);
            returnMain(ddlChangeKhoa.SelectedValue)
           ;
        }
    }
}