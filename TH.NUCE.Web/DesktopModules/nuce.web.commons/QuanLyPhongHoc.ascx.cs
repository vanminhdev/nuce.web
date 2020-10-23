using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.commons
{
    public partial class QuanLyPhongHoc : PortalModuleBase
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
                    if (strType == "edit" || strType == "insert")
                    {
                        //Lấy dữ liệu từ server để nhét vào các control
                        //THCore_CM_GetTag
                        int iToaNhaID = int.Parse(Request.QueryString["ToaNhaid"]);
                        string strToaNha = Request.QueryString["ToaNhaname"];
                        txtToaNhaID.Text = iToaNhaID.ToString();
                        divNameToaNha.InnerText = strToaNha;
                        if (strType == "insert")
                        {
                            divAnnouce.InnerHtml = "Thêm mới phòng học";
                        }
                        else
                        {
                            divAnnouce.InnerHtml = "Chỉnh sửa thông tin phòng học";
                            int iID = int.Parse(Request.QueryString["id"]);
                            DataTable tblTable = data.dnn_NuceCommon_PhongHoc.get(iID);
                            if (tblTable.Rows.Count > 0)
                            {
                                txtTen.Text = tblTable.Rows[0]["Ten"].ToString();
                                txtMoTa.Text = tblTable.Rows[0]["MoTa"].ToString();
                                txtIp1.Text = tblTable.Rows[0]["Ip1"].ToString();
                                txtSubnetMask1.Text = tblTable.Rows[0]["SubnetMask1"].ToString();
                                txtDefaultgateway1.Text = tblTable.Rows[0]["Defaultgateway1"].ToString();
                                txtProxy1.Text = tblTable.Rows[0]["Proxy1"].ToString();

                                txtIp2.Text = tblTable.Rows[0]["Ip2"].ToString();
                                txtSubnetMask2.Text = tblTable.Rows[0]["SubnetMask2"].ToString();
                                txtDefaultgateway2.Text = tblTable.Rows[0]["Defaultgateway2"].ToString();
                                txtProxy2.Text = tblTable.Rows[0]["Proxy2"].ToString();

                                txtID.Text = iID.ToString();
                            }
                        }

                    }
                    else if (strType == "delete")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceCommon_PhongHoc.updateStatus(iID, 4);
                        returnMain(Request.QueryString["ToaNhaid"]);
                    }
                    else if (strType == "changefinish")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceCommon_PhongHoc.updateStatus(iID, 3);
                        returnMain(Request.QueryString["ToaNhaid"]);
                    }
                    else if (strType == "changeactive")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceCommon_PhongHoc.updateStatus(iID, 1);
                        returnMain(Request.QueryString["ToaNhaid"]);
                    }
                }
                else
                {
                    //Bindata
                    DataTable tblTable = data.dnn_NuceCommon_ToaNha.get(-1);
                    ddlToaNha.DataSource = tblTable;
                    ddlToaNha.DataTextField = "Ten";
                    ddlToaNha.DataValueField = "ToaNhaID";
                    ddlToaNha.DataBind();
                    string strToaNhaID = tblTable.Rows[0]["ToaNhaID"].ToString();
                    string strToaNhaName = tblTable.Rows[0]["Ten"].ToString();
                    if (Request.QueryString["ToaNhaid"] != null && Request.QueryString["ToaNhaid"] != "")
                    {
                        strToaNhaID = Request.QueryString["ToaNhaid"];
                        DataRow[] drRs = tblTable.Select(string.Format("ToaNhaID={0}", strToaNhaID));
                        strToaNhaName = drRs[0]["Ten"].ToString();
                        //strToaNhaName = drRs.Length.ToString();
                    }
                    ddlToaNha.SelectedValue = strToaNhaID;
                    txtToaNhaID.Text = strToaNhaID;
                    divNameToaNha.InnerText = strToaNhaName;
                    divAnnouce.InnerHtml = "";
                    divEdit.Visible = false;
                    displayList(int.Parse(strToaNhaID), strToaNhaName);
                }
            }
        }

        private void displayList(int ToaNhaID, string ToaNhaName)
        {
            DataTable tblTable = data.dnn_NuceCommon_PhongHoc.getByToaNha(ToaNhaID);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách phòng học</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable += "<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Tên");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Danh sách máy");
            //strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Ip1");
            //strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "AddInfo1");
            strTable += string.Format("<div class='nd_t22 fl'>{0}</div>", "Mô tả");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Trạng thái");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chuyển ");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chỉnh sửa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Xóa");
            strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&ToaNhaid={1}&&ToaNhaname={2}'>Thêm mới</a></div>", this.TabId, ToaNhaID, ToaNhaName);
            strTable += "</div>";
            strTable += "<div class='nd_b fl'>";
            for (int i = 0; i < tblTable.Rows.Count; i++)
            {
                strTable += " <div class='nd_b1 fl'>";
                strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", (i + 1));
                strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", tblTable.Rows[i]["Ten"].ToString());
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/2123/default.aspx?phongid={0}&&ten={1}'>danh sách máy</a></div>", tblTable.Rows[i]["PhongHocID"].ToString(), tblTable.Rows[i]["Ten"].ToString());
                strTable += string.Format("<div class='nd_t22 fl'>{0}</div>", tblTable.Rows[i]["MoTa"].ToString());
                int iStatus = int.Parse(tblTable.Rows[i]["status"].ToString());
                if (iStatus.Equals(1))
                {
                    strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Hoạt động");
                    strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changefinish&&id={1}'>Kết thúc</a></div>", this.TabId, tblTable.Rows[i]["PhongHocID"].ToString());
                }
                else
                {
                    strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Kết thúc");
                    strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=changeactive&&id={1}'>Hoạt động</a></div>", this.TabId, tblTable.Rows[i]["PhongHocID"].ToString());
                }
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&&ToaNhaid={2}&&ToaNhaname={3}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["PhongHocID"].ToString(), ToaNhaID, ToaNhaName);
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}&ToaNhaid={2}'>Xóa</a></div>", this.TabId, tblTable.Rows[i]["PhongHocID"].ToString(), ToaNhaID);
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&ToaNhaid={1}&&ToaNhaname={2}'>Thêm mới</a></div>", this.TabId, ToaNhaID, ToaNhaName);
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

            string strIp1 = txtIp1.Text.Trim();
            string strIp2 = txtIp2.Text.Trim();
            string strSubnetmask1 = txtSubnetMask1.Text.Trim();
            string strSubnetmask2 = txtSubnetMask2.Text.Trim();
            string strDefaultgateway1 = txtDefaultgateway1.Text.Trim();
            string strDefaultgateway2 = txtDefaultgateway2.Text.Trim();
            string strProxy1 = txtProxy1.Text;
            string strProxy2 = txtProxy2.Text;

            string strMoTa = txtMoTa.Text;
            int iToaNhaID = int.Parse(txtToaNhaID.Text)
            ;
            if (strTen.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để tên trắng";
                return;
            }

            if (strType == "insert")
            {
                data.dnn_NuceCommon_PhongHoc.insert(iToaNhaID, strTen, strIp1, strSubnetmask1, strDefaultgateway1, strProxy1, strIp2, strSubnetmask2, strDefaultgateway2, strProxy2, strMoTa);
                returnMain(txtToaNhaID.Text);
            }
            else
            {
                if (strType == "edit")
                {
                    int iID = int.Parse(txtID.Text);
                    data.dnn_NuceCommon_PhongHoc.update(iID, iToaNhaID, strTen, strIp1, strSubnetmask1, strDefaultgateway1, strProxy1, strIp2, strSubnetmask2, strDefaultgateway2, strProxy2, strMoTa);
                    returnMain(txtToaNhaID.Text);
                }
            }
        }

        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            returnMain(txtToaNhaID.Text)
            ;
        }
        private void returnMain(string ToaNhaID)
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?ToaNhaid={1}", this.TabId, ToaNhaID));
        }

        protected void ddlToaNha_SelectedIndexChanged(object sender, EventArgs e)
        {
            returnMain(ddlToaNha.SelectedValue)
           ;
        }
    }
}