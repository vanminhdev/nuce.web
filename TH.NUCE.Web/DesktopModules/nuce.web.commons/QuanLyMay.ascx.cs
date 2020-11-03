using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.commons
{
    public partial class QuanLyMay : PortalModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
            #region Datetime
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
                        int iPhongID = int.Parse(Request.QueryString["phongid"]);
                        string strTen = Request.QueryString["ten"];
                        txtPhongID.Text = iPhongID.ToString();
                        divNamePhong.InnerText = strTen;
                        if (strType == "insert")
                        {
                            divAnnouce.InnerHtml = "Thêm mới máy";
                        }
                        else
                        {
                            divAnnouce.InnerHtml = "Chỉnh sửa thông tin máy";
                            int iID = int.Parse(Request.QueryString["id"]);
                            DataTable tblTable = data.dnn_NuceCommon_May.get(iID);
                            if (tblTable.Rows.Count > 0)
                            {

                                txtMa.Text = tblTable.Rows[0]["Ma"].ToString();
                                txtMoTa.Text = tblTable.Rows[0]["MoTa"].ToString();
                                txtGhiChu.Text = tblTable.Rows[0]["GhiChu"].ToString();
                                txtMac.Text = tblTable.Rows[0]["MacAddress"].ToString();
                                
                                txtID.Text = iID.ToString();
                            }
                        }
                        
                    }
                    else if(strType == "delete")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceCommon_May.updateStatus(iID, 4);
                        returnMain(Request.QueryString["phongid"]);
                    }
                    else if (strType == "changefinish")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceCommon_May.updateStatus(iID, 3);
                        returnMain(Request.QueryString["phongid"]);
                    }
                    else if (strType == "changeactive")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceCommon_May.updateStatus(iID, 1);
                        returnMain(Request.QueryString["phongid"]);
                    }
                }
                else
                {
                    //Bindata
                    DataTable tblTable = data.dnn_NuceCommon_PhongHoc.getName(-1);

                    ddlPhong.DataSource = tblTable;
                    ddlPhong.DataTextField = "Ten";
                    ddlPhong.DataValueField = "PhongHocID";
                    ddlPhong.DataBind();
                    string strPhongID = tblTable.Rows[0]["PhongHocID"].ToString();
                    string strPhongName = tblTable.Rows[0]["Ten"].ToString();
                    if (Request.QueryString["phongid"] != null && Request.QueryString["phongid"] != "")
                    {
                        strPhongID = Request.QueryString["phongid"];
                        DataRow[] drRs = tblTable.Select(string.Format("PhongHocID={0}", strPhongID));
                        strPhongName = drRs[0]["Ten"].ToString();
                       //strToaNhaName = drRs.Length.ToString();
                    }
                    ddlPhong.SelectedValue = strPhongID;
                    txtPhongID.Text = strPhongID;
                    divNamePhong.InnerText = strPhongName;
                    divAnnouce.InnerHtml = "";
                    divEdit.Visible = false;
                    displayList(int.Parse(strPhongID), strPhongName);
                }
            }
        }

        private void displayList(int ToaNhaID,string ToaNhaName)
        {
            DataTable tblTable = data.dnn_NuceCommon_May.getByPhongHoc(ToaNhaID);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách máy</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable +="<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Mã");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "MacAddress");
            //strTable += string.Format("<div class='nd_t22 fl'>{0}</div>", "Mô tả");
            //strTable += string.Format("<div class='nd_t22 fl'>{0}</div>", "Ghi chú");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Trạng thái");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chuyển ");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chỉnh sửa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Xóa");
            strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&phongid={1}&&ten={2}'>Thêm mới</a></div>", this.TabId, ToaNhaID, ToaNhaName);
            strTable += "</div>";
                    strTable += "<div class='nd_b fl'>";
                    for (int i = 0; i < tblTable.Rows.Count; i++)
                    {
                        strTable += " <div class='nd_b1 fl'>";
                        strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", (i + 1));
                        strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", tblTable.Rows[i]["Ma"].ToString());
                        strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", tblTable.Rows[i]["MacAddress"].ToString().Length<15?"------------------": tblTable.Rows[i]["MacAddress"].ToString());
                        int iStatus = int.Parse(tblTable.Rows[i]["status"].ToString());
                        if (iStatus.Equals(1))
                        {
                            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Hoạt động");
                            strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changefinish&&id={1}'>Kết thúc</a></div>", this.TabId, tblTable.Rows[i]["MayID"].ToString());
                        }
                        else
                        {
                            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Kết thúc");
                            strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changeactive&&id={1}'>Hoạt động</a></div>", this.TabId, tblTable.Rows[i]["MayID"].ToString());
                        }
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&&phongid={2}&&ten={3}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["MayID"].ToString(), ToaNhaID, ToaNhaName);
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}&phongid={2}'>Xóa</a></div>", this.TabId, tblTable.Rows[i]["MayID"].ToString(), ToaNhaID);
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&phongid={1}&&ten={2}'>Thêm mới</a></div>", this.TabId, ToaNhaID, ToaNhaName);
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

            string strMac = txtMac.Text.Trim();
            string strGhiChu = txtGhiChu.Text.Trim();

            string strMoTa = txtMoTa.Text;
            int iPhongID = int.Parse(txtPhongID.Text)
            ;
            if (strMa.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để tên trắng";
                return;
            }

            if (strType == "insert")
            {
                data.dnn_NuceCommon_May.insert(iPhongID, strMa, strMac, strMoTa, strGhiChu, 1, 1);
                returnMain(txtPhongID.Text);
            }
            else
            {
                if (strType == "edit")
                {
                    int iID = int.Parse(txtID.Text);
                    data.dnn_NuceCommon_May.update(iID, iPhongID, strMa, strMac, strMoTa, strGhiChu);
                    returnMain(txtPhongID.Text);
                }
            }
        }

        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            returnMain(txtPhongID.Text)
            ;
        }
        private  void returnMain(string ToaNhaID)
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?phongid={1}", this.TabId, ToaNhaID));
        }

        protected void ddlPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            returnMain(ddlPhong.SelectedValue)
           ;
        }
    }
}