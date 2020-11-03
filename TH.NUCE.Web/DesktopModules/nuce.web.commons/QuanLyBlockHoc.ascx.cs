using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.commons
{
    public partial class QuanLyBlockHoc : PortalModuleBase
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
            #endregion
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["EditType"] != null && Request.QueryString["EditType"] != "")
                {
                    divFilter.Visible = false;
                    string strType = Request.QueryString["EditType"];
                    txtType.Text = strType;
                    divAnnouce.InnerHtml = ""; 
                    if (strType == "edit" ||strType=="insert")
                    {
                        //Lấy dữ liệu từ server để nhét vào các control
                        //THCore_CM_GetTag
                        int iNamHocID = int.Parse(Request.QueryString["NamHocid"]);
                        string strNamHoc = Request.QueryString["NamHocname"];
                        txtNamHocID.Text = iNamHocID.ToString();
                        divNameNamHoc.InnerText = strNamHoc;
                        if (strType == "insert")
                        {
                            divAnnouce.InnerHtml = "Thêm mới block học";
                        }
                        else
                        {
                            divAnnouce.InnerHtml = "Chỉnh sửa thông tin block học";
                            int iID = int.Parse(Request.QueryString["id"]);
                            DataTable tblTable = data.dnn_NuceCommon_BlockHoc.get(iID);
                            if (tblTable.Rows.Count > 0)
                            {
                                txtTen.Text = tblTable.Rows[0]["Ten"].ToString();
                                txtMoTa.Text = tblTable.Rows[0]["MoTa"].ToString();
                                txtNgayBatDau.Text = DateTime.Parse(tblTable.Rows[0]["NgayBatDau"].ToString()).ToString("dd/MM/yyyy");
                                txtNgayKetThuc.Text = DateTime.Parse(tblTable.Rows[0]["NgayKetThuc"].ToString()).ToString("dd/MM/yyyy");
                                txtID.Text = iID.ToString();
                            }
                        }
                        
                    }
                    else if(strType == "delete")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceCommon_BlockHoc.updateStatus(iID, 4);
                        returnMain(Request.QueryString["NamHocid"]);
                    }
                    else if (strType == "changefinish")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceCommon_BlockHoc.updateStatus(iID, 3);
                        returnMain(Request.QueryString["NamHocid"]);
                    }
                    else if (strType == "changeactive")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceCommon_BlockHoc.updateStatus(iID, 1);
                        returnMain(Request.QueryString["NamHocid"]);
                    }
                }
                else
                {
                    //Bindata
                    DataTable tblTable = data.dnn_NuceCommon_NamHoc.getName(-1);
                    ddlNamHoc.DataSource = tblTable;
                    ddlNamHoc.DataTextField = "Ten";
                    ddlNamHoc.DataValueField = "NamHocID";
                    ddlNamHoc.DataBind();
                    string strNamHocID = tblTable.Rows[0]["NamHocID"].ToString();
                    string strNamHocName = tblTable.Rows[0]["Ten"].ToString();
                    if (Request.QueryString["NamHocid"] != null && Request.QueryString["NamHocid"] != "")
                    {
                        strNamHocID = Request.QueryString["NamHocid"];
                        DataRow[] drRs = tblTable.Select(string.Format("NamHocID={0}", strNamHocID));
                        strNamHocName = drRs[0]["Ten"].ToString();
                       //strNamHocName = drRs.Length.ToString();
                    }
                    ddlNamHoc.SelectedValue = strNamHocID;
                    txtNamHocID.Text = strNamHocID;
                    divNameNamHoc.InnerText = strNamHocName;
                    divAnnouce.InnerHtml = "";
                    divEdit.Visible = false;
                    displayList(int.Parse(strNamHocID), strNamHocName);
                }
            }
        }

        private void displayList(int NamHocID,string NamHocName)
        {
            DataTable tblTable = data.dnn_NuceCommon_BlockHoc.getByNamHoc(NamHocID);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách block học</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable +="<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Tên");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Ngày bắt đầu");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Ngày kết thúc");
            strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", "Mô tả");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Trạng thái");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chuyển ");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chỉnh sửa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Xóa");
            strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&NamHocid={1}&&NamHocname={2}'>Thêm mới</a></div>", this.TabId, NamHocID, NamHocName);
            strTable += "</div>";
                    strTable += "<div class='nd_b fl'>";
                    for (int i = 0; i < tblTable.Rows.Count; i++)
                    {
                        strTable += " <div class='nd_b1 fl'>";
                        strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", (i + 1));
                        strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", tblTable.Rows[i]["Ten"].ToString());
                        strTable += string.Format("<div class='nd_t33 fl'>{0:dd/MM/yyyy}</div>", DateTime.Parse(tblTable.Rows[i]["NgayBatDau"].ToString()));
                        strTable += string.Format("<div class='nd_t33 fl'>{0:dd/MM/yyyy}</div>", DateTime.Parse(tblTable.Rows[i]["NgayKetThuc"].ToString()));
                        strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", tblTable.Rows[i]["MoTa"].ToString());
                        int iStatus = int.Parse(tblTable.Rows[i]["status"].ToString());
                        if (iStatus.Equals(1))
                        {
                            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Hoạt động");
                            strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changefinish&&id={1}'>Kết thúc</a></div>", this.TabId, tblTable.Rows[i]["BlockHocID"].ToString());
                        }
                        else
                        {
                            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Kết thúc");
                            strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changeactive&&id={1}'>Hoạt động</a></div>", this.TabId, tblTable.Rows[i]["BlockHocID"].ToString());
                        }
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&&NamHocid={2}&&NamHocname={3}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["BlockHocID"].ToString(), NamHocID, NamHocName);
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}&NamHocid={2}'>Xóa</a></div>", this.TabId, tblTable.Rows[i]["BlockHocID"].ToString(), NamHocID);
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&NamHocid={1}&&NamHocname={2}'>Thêm mới</a></div>", this.TabId, NamHocID, NamHocName);
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
            string strTen = txtTen.Text;
            DateTime dtNgayBatDau;
            try
            {
                dtNgayBatDau = DateTime.ParseExact(txtNgayBatDau.Text, "dd/MM/yyyy", null);
            }
            catch (Exception)
            {
                dtNgayBatDau = DateTime.Now;
            }
            DateTime dtNgayKetThuc;
            try
            {
                dtNgayKetThuc = DateTime.ParseExact(txtNgayKetThuc.Text, "dd/MM/yyyy", null);
            }
            catch (Exception)
            {
                dtNgayKetThuc = DateTime.Now;
            }
            string strMoTa = txtMoTa.Text;
            int iNamHocID = int.Parse(txtNamHocID.Text)
            ;
            if (strTen.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để tên trắng";
                return;
            }

            if (strType == "insert")
            {
                data.dnn_NuceCommon_BlockHoc.insert(iNamHocID, strTen, strMoTa, dtNgayBatDau, dtNgayKetThuc);
               //DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("THCore_TM_InsertTags", iGroupTags, strName, "","","");
                returnMain(txtNamHocID.Text);
            }
            else
            {
                if (strType == "edit")
                {
                    int iID = int.Parse(txtID.Text);
                    data.dnn_NuceCommon_BlockHoc.update(iID, iNamHocID, strTen, strMoTa, dtNgayBatDau, dtNgayKetThuc);
                    returnMain(txtNamHocID.Text);
                }
            }
        }

        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            returnMain(txtNamHocID.Text)
            ;
        }
        private  void returnMain(string NamHocID)
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?NamHocid={1}", this.TabId, NamHocID));
        }

        protected void ddlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            returnMain(ddlNamHoc.SelectedValue)
           ;
        }
    }
}