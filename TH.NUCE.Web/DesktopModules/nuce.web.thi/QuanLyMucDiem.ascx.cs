using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.commons
{
    public partial class QuanLyMucDiem : PortalModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
            
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
                        //Bindata
                        DataTable tblDoKho = data.dnn_NuceThi_DoKho.get(-1);

                        ddlDoKho.DataSource = tblDoKho;
                        ddlDoKho.DataTextField = "Ten";
                        ddlDoKho.DataValueField = "DoKhoID";
                        ddlDoKho.DataBind();
                        //Lấy dữ liệu từ server để nhét vào các control
                        //THCore_CM_GetTag
                        int iThangDiemID = int.Parse(Request.QueryString["ThangDiemid"]);
                        string strThangDiem = Request.QueryString["ThangDiemname"];
                        txtThangDiemID.Text = iThangDiemID.ToString();
                        divNameThangDiem.InnerText = strThangDiem;
                        if (strType == "insert")
                        {
                            divAnnouce.InnerHtml = "Thêm mới mức điểm";
                        }
                        else
                        {
                            divAnnouce.InnerHtml = "Chỉnh sửa thông tin mức điểm";
                            int iID = int.Parse(Request.QueryString["id"]);
                            DataTable tblTable = data.dnn_NuceThi_MucDiem.get(iID);
                            if (tblTable.Rows.Count > 0)
                            {
                                //txtDoKho.Text = tblTable.Rows[0]["DoKho"].ToString();
                                ddlDoKho.SelectedValue= tblTable.Rows[0]["DoKho"].ToString();
                                txtDiem.Text = tblTable.Rows[0]["Diem"].ToString();
                                txtID.Text = iID.ToString();
                            }
                        }
                        
                    }
                    else if(strType == "delete")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceThi_MucDiem.delete(iID);
                        returnMain(Request.QueryString["ThangDiemid"]);
                    }
                }
                else
                {
                   
                    //BindData
                    DataTable tblTable = data.dnn_NuceThi_ThangDiem.getName(-1);
                    ddlThangDiem.DataSource = tblTable;
                    ddlThangDiem.DataTextField = "Ten";
                    ddlThangDiem.DataValueField = "ThangDiemID";
                    ddlThangDiem.DataBind();
                    string strThangDiemID = tblTable.Rows[0]["ThangDiemID"].ToString();
                    string strThangDiemName = tblTable.Rows[0]["Ten"].ToString();
                    if (Request.QueryString["ThangDiemid"] != null && Request.QueryString["ThangDiemid"] != "")
                    {
                        strThangDiemID = Request.QueryString["ThangDiemid"];
                        DataRow[] drRs = tblTable.Select(string.Format("ThangDiemID={0}", strThangDiemID));
                        strThangDiemName = drRs[0]["Ten"].ToString();
                       //strThangDiemName = drRs.Length.ToString();
                    }
                    ddlThangDiem.SelectedValue = strThangDiemID;
                    txtThangDiemID.Text = strThangDiemID;
                    divNameThangDiem.InnerText = strThangDiemName;
                    divAnnouce.InnerHtml = "";
                    divEdit.Visible = false;
                    displayList(int.Parse(strThangDiemID), strThangDiemName);
                }
            }
        }

        private void displayList(int ThangDiemID,string ThangDiemName)
        {
            DataTable tblTable = data.dnn_NuceThi_MucDiem.getByThangDiem(ThangDiemID);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách các mức điểm của thang điểm "+ThangDiemName+"</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable +="<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t3 fl'>{0}</div>", "Độ khó");
            strTable += string.Format("<div class='nd_t3 fl'>{0}</div>", "Điểm");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Xóa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Sửa");
            strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&ThangDiemid={1}&&ThangDiemname={2}'>Thêm mới</a></div>", this.TabId, ThangDiemID, ThangDiemName);
            strTable += "</div>";
                    strTable += "<div class='nd_b fl'>";
                    for (int i = 0; i < tblTable.Rows.Count; i++)
                    {
                        strTable += " <div class='nd_b1 fl'>";
                        strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", (i + 1));
                        strTable += string.Format("<div class='nd_t3 fl'>{0}</div>", tblTable.Rows[i]["TenDoKho"].ToString());
                        strTable += string.Format("<div class='nd_t3 fl'>{0}</div>", tblTable.Rows[i]["Diem"].ToString());
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}&ThangDiemid={2}'>Xóa</a></div>", this.TabId, tblTable.Rows[i]["MucDiemID"].ToString(), ThangDiemID);
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&ThangDiemid={2}&&ThangDiemname={3}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["MucDiemID"].ToString(), ThangDiemID, ThangDiemName);
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&ThangDiemid={1}&&ThangDiemname={2}'>Thêm mới</a></div>", this.TabId, ThangDiemID, ThangDiemName);
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
            
            int iDoKho = int.Parse(ddlDoKho.SelectedValue);
            float fDiem = float.Parse(txtDiem.Text.Trim().Replace(",","."));
            int iThangDiemID = int.Parse(txtThangDiemID.Text)
            ;

            if (strType == "insert")
            {
                data.dnn_NuceThi_MucDiem.insert(iThangDiemID, iDoKho, fDiem);
               //DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("THCore_TM_InsertTags", iGroupTags, strName, "","","");
                returnMain(txtThangDiemID.Text);
            }
            else
            {
                if (strType == "edit")
                {
                    int iID = int.Parse(txtID.Text);
                    data.dnn_NuceThi_MucDiem.update(iID, iThangDiemID, iDoKho, fDiem);
                    returnMain(txtThangDiemID.Text);
                }
            }
        }

        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            returnMain(txtThangDiemID.Text)
            ;
        }
        private  void returnMain(string ThangDiemID)
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?ThangDiemid={1}", this.TabId, ThangDiemID));
        }

        protected void ddlThangDiem_SelectedIndexChanged(object sender, EventArgs e)
        {
            returnMain(ddlThangDiem.SelectedValue)
           ;
        }
    }
}