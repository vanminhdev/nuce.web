using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.commons
{
    public partial class QuanLyBoMon : PortalModuleBase
    {
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
                            DataTable tblTable = data.dnn_NuceCommon_BoMon.get(iID);
                            if (tblTable.Rows.Count > 0)
                            {
                                txtMa.Text = tblTable.Rows[0]["Ma"].ToString();
                                txtTen.Text = tblTable.Rows[0]["Ten"].ToString();
                                txtTenTiengAnh.Text = tblTable.Rows[0]["TenTiengAnh"].ToString();
                                txtDiaChi.Text = tblTable.Rows[0]["DiaChi"].ToString();
                                txtMoTa.Text = tblTable.Rows[0]["MoTa"].ToString();
                                txtID.Text = iID.ToString();
                            }
                        }
                        
                    }
                    else if(strType == "delete")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceCommon_BoMon.updateStatus(iID, 4);
                        returnMain(Request.QueryString["khoaid"]);
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
                    divEdit.Visible = false;
                    displayList(int.Parse(strKhoaID), strKhoaName);
                }
            }
        }

        private void displayList(int KhoaID,string KhoaName)
        {
            DataTable tblTable = data.dnn_NuceCommon_BoMon.getByKhoa(KhoaID);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách bộ môn</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable +="<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "Mã");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Tên");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Tên Tiếng anh");
            strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", "Địa chỉ");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Danh sách cán bộ");
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
                        strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", tblTable.Rows[i]["Ten"].ToString());
                        strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", tblTable.Rows[i]["TenTiengAnh"].ToString());
                        strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", tblTable.Rows[i]["DiaChi"].ToString());
                        strTable += string.Format("<div class='nd_t33 fl'><a href='javascript:quanlybomon.showdetailcanbotrongbomon({0},\"{1}\");'>Danh sách cán bộ</a></div>", tblTable.Rows[i]["BoMonID"].ToString(), tblTable.Rows[i]["Ten"].ToString());
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&&khoaid={2}&&khoaname={3}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["BoMonID"].ToString(), KhoaID, KhoaName);
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}&khoaid={2}'>Xóa</a></div>", this.TabId, tblTable.Rows[i]["BoMonID"].ToString(), KhoaID);
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&khoaid={1}&&khoaname={2}'>Thêm mới</a></div>", this.TabId, KhoaID, KhoaName);
                        strTable += "</div>";
                    }
                    strTable += "</div>";
                 strTable += "</div>";
                 strTable += "</div>";

                 strTable += "<div  class='nd fl' id='divGiaoVienTrongBoMon'>" +
                             "</div>";

            strTable += "</div>";
            divContent.InnerHtml = strTable;
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string strType = txtType.Text;
            string strMa = txtMa.Text;
            string strTen = txtTen.Text;
            string strTenTiengAnh = txtTenTiengAnh.Text;
            string strDiaChi = txtDiaChi.Text;
            string strMoTa = txtMoTa.Text;
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
                data.dnn_NuceCommon_BoMon.insert(iKhoaID, strMa, strTen, strTenTiengAnh, strDiaChi, strMoTa);
               //DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("THCore_TM_InsertTags", iGroupTags, strName, "","","");
                returnMain(txtKhoaID.Text);
            }
            else
            {
                if (strType == "edit")
                {
                    int iID = int.Parse(txtID.Text);
                    data.dnn_NuceCommon_BoMon.update(iID, iKhoaID, strMa, strTen, strTenTiengAnh, strDiaChi, strMoTa);
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
    }
}