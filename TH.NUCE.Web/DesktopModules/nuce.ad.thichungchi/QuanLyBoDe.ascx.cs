using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.thichungchi
{
    public partial class QuanLyBoDe : PortalModuleBase
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
                    if (strType == "edit" || strType == "insert")
                    {
                        //Lấy dữ liệu từ server để nhét vào các control
                        //THCore_CM_GetTag
                        int iNguoiDung_MonHocID = int.Parse(Request.QueryString["NguoiDung_MonHocid"]);
                        string strNguoiDung_MonHoc = Request.QueryString["NguoiDung_MonHocname"];
                        txtNguoiDung_MonHocID.Text = iNguoiDung_MonHocID.ToString();
                        divNameNguoiDung_MonHoc.InnerText = strNguoiDung_MonHoc;
                        if (strType == "insert")
                        {
                            divAnnouce.InnerHtml = "Thêm mới bộ câu hỏi";
                        }
                        else
                        {
                            divAnnouce.InnerHtml = "Chỉnh sửa bộ câu hỏi";
                            int iID = int.Parse(Request.QueryString["id"]);
                            DataTable tblTable = data.dnn_NuceThi_BoDe.get(iID);
                            if (tblTable.Rows.Count > 0)
                            {
                                txtMa.Text = tblTable.Rows[0]["Ma"].ToString();
                                txtTen.Text = tblTable.Rows[0]["Ten"].ToString();
                                txtMoTa.Text = tblTable.Rows[0]["MoTa"].ToString();
                                txtThoiGianThi.Text = tblTable.Rows[0]["ThoiGianThi"].ToString();
                                txtSoDe.Text= tblTable.Rows[0].IsNull("SoDe")?"100" : tblTable.Rows[0]["SoDe"].ToString();
                                txtID.Text = iID.ToString();
                            }
                        }

                    }
                    else if (strType == "delete")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceThi_BoDe.updateStatus(iID, 4);
                        returnMain(Request.QueryString["NguoiDung_MonHocid"]);
                    }
                }
                else
                {
                    //Bindata
                    DataTable tblTable = data.dnn_NuceCommon_MonHoc.getByNguoiDung(this.UserId);
                    ddlNguoiDung_MonHoc.DataSource = tblTable;
                    ddlNguoiDung_MonHoc.DataTextField = "Ten";
                    ddlNguoiDung_MonHoc.DataValueField = "NguoiDung_MonHocID";
                    ddlNguoiDung_MonHoc.DataBind();
                    if (tblTable.Rows.Count > 0)
                    {
                        string strNguoiDung_MonHocID = tblTable.Rows[0]["NguoiDung_MonHocID"].ToString();
                        string strNguoiDung_MonHocName = tblTable.Rows[0]["Ten"].ToString();
                        if (Request.QueryString["NguoiDung_MonHocid"] != null && Request.QueryString["NguoiDung_MonHocid"] != "")
                        {
                            strNguoiDung_MonHocID = Request.QueryString["NguoiDung_MonHocid"];
                            DataRow[] drRs = tblTable.Select(string.Format("NguoiDung_MonHocID={0}", strNguoiDung_MonHocID));
                            strNguoiDung_MonHocName = drRs[0]["Ten"].ToString();
                            //strNguoiDung_MonHocName = drRs.Length.ToString();
                        }
                        ddlNguoiDung_MonHoc.SelectedValue = strNguoiDung_MonHocID;
                        txtNguoiDung_MonHocID.Text = strNguoiDung_MonHocID;
                        divNameNguoiDung_MonHoc.InnerText = strNguoiDung_MonHocName;
                        divAnnouce.InnerHtml = "";
                        divEdit.Visible = false;
                        displayList(int.Parse(strNguoiDung_MonHocID), strNguoiDung_MonHocName);
                    }
                    else
                    {
                        divEdit.Visible = false;
                        divAnnouce.InnerHtml = "Bạn không phụ trách môn học nào cả !!!";
                    }
                }
            }
        }

        private void displayList(int NguoiDung_MonHocID, string NguoiDung_MonHocName)
        {
            DataTable tblTable = data.dnn_NuceThi_BoDe.getByNguoiDung_MonHoc(NguoiDung_MonHocID);
            string strTable =  "<table class=\"table\">";
            strTable += "<thead><tr>";
            strTable += string.Format("<th>{0}</th>", "#");
            strTable += string.Format("<th>{0}</th>", "Mã");
            strTable += string.Format("<th>{0}</th>", "Tên");
            strTable += string.Format("<th>{0}</th>", "T/G thi (phút)");
            strTable += string.Format("<th>{0}</th>", "Cấu trúc đề");
            strTable += string.Format("<th>{0}</th>", "Trạng thái");
            strTable += string.Format("<th>{0}</th>", "Danh sách đề");
            strTable += string.Format("<th>{0}</th>", "Chỉnh sửa");
            strTable += string.Format("<th>{0}</th>", "Xóa");
            //strTable += string.Format("<th><a href='/tabid/{0}/default.aspx?EditType=insert&&NguoiDung_MonHocid={1}&&NguoiDung_MonHocname={2}'>Thêm mới</a></th>", this.TabId, NguoiDung_MonHocID, NguoiDung_MonHocName);
            strTable += "</tr></thead>";
            strTable += "<tbody id=\"tbContent\">";
            for (int i = 0; i < tblTable.Rows.Count; i++)
            {
                string BoDeID = tblTable.Rows[i]["BoDeID"].ToString();
                strTable += " <tr>";
                strTable += string.Format("<td>{0}</td>", (i + 1));
                strTable += string.Format("<td>{0}</td>", tblTable.Rows[i]["Ma"].ToString());
                strTable += string.Format("<td>{0}</td>", tblTable.Rows[i]["Ten"].ToString());
                strTable += string.Format("<td>{0}</td>", tblTable.Rows[i]["ThoiGianThi"].ToString());
                strTable += string.Format("<td><a href='/tabid/43/default.aspx?NguoiDung_MonHocid={0}&&bodeid={1}'>{2}</a></td>",NguoiDung_MonHocID, BoDeID, "Cấu trúc đề");


                int iStatus = int.Parse(tblTable.Rows[i]["Status"].ToString());
                switch(iStatus)
                {
                    case 1:
                        strTable += string.Format("<td>{0}</td>", "Bắt đầu");
                        strTable += string.Format("<td>{0}</td>", "---");
                        strTable += string.Format("<td><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&&NguoiDung_MonHocid={2}&&NguoiDung_MonHocname={3}'>Sửa</a></td>", this.TabId, BoDeID, NguoiDung_MonHocID, NguoiDung_MonHocName);
                        strTable += string.Format("<td><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}&NguoiDung_MonHocid={2}'>Xóa</a></td>", this.TabId, BoDeID, NguoiDung_MonHocID);
                        break;
                    case 2:
                        strTable += string.Format("<td>{0}</td>", "Đã tạo đề");
                        strTable += string.Format("<td><a href='/tabid/44/default.aspx?NguoiDung_MonHocid={0}&&bodeid={1}'>Danh sách đề</a></td>", NguoiDung_MonHocID,BoDeID);
                        strTable += string.Format("<td><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&&NguoiDung_MonHocid={2}&&NguoiDung_MonHocname={3}'>Sửa</a></td>", this.TabId, BoDeID, NguoiDung_MonHocID, NguoiDung_MonHocName);
                        strTable += string.Format("<td><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}&NguoiDung_MonHocid={2}'>Xóa</a></td>", this.TabId, BoDeID, NguoiDung_MonHocID);

                        break;
                    case 3:
                        strTable += string.Format("<td>{0}</td>", "Đã sử dụng bộ đề");
                        strTable += string.Format("<td><a href='/tabid/44/default.aspx?NguoiDung_MonHocid={0}&&bodeid={1}'>Danh sách đề</a></td>", NguoiDung_MonHocID, BoDeID);
                        strTable += string.Format("<td>{0}</td>", "Không sửa");
                        strTable += string.Format("<td>{0}</td>", "Không xóa");
                        break;
                }
                //strTable += string.Format("<td><a href='/tabid/{0}/default.aspx?EditType=insert&&NguoiDung_MonHocid={1}&&NguoiDung_MonHocname={2}'>Thêm mới</a></td>", this.TabId, NguoiDung_MonHocID, NguoiDung_MonHocName);
                strTable += "</tr>";
            }
            strTable += " <tr>";
            strTable += string.Format("<td colspan='9' style='text-align:right;'><a href='/tabid/{0}/default.aspx?EditType=insert&&NguoiDung_MonHocid={1}&&NguoiDung_MonHocname={2}'>Thêm mới</a></td>", this.TabId, NguoiDung_MonHocID, NguoiDung_MonHocName);
            strTable += "</tr>";
            strTable += "</tbody>";
            strTable += "</table>";
            divContent.InnerHtml = strTable;
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string strType = txtType.Text;
            string strMa = txtMa.Text;
            string strTen = txtTen.Text;
            string strMoTa = txtMoTa.Text;
            int iThoiGianThi =int.Parse(txtThoiGianThi.Text.Trim());
            int iSoDe = int.Parse(txtSoDe.Text.Trim());
            int iNguoiDung_MonHocID = int.Parse(txtNguoiDung_MonHocID.Text)
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
                data.dnn_NuceThi_BoDe.insert(iNguoiDung_MonHocID, strMa, strTen, strMoTa, iThoiGianThi,iSoDe, 1);
                //DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("THCore_TM_InsertTags", iGroupTags, strName, "","","");
                returnMain(txtNguoiDung_MonHocID.Text);
            }
            else
            {
                if (strType == "edit")
                {
                    int iID = int.Parse(txtID.Text);
                    data.dnn_NuceThi_BoDe.update(iID, iNguoiDung_MonHocID, strMa, strTen, strMoTa, iThoiGianThi,iSoDe, 1);
                    returnMain(txtNguoiDung_MonHocID.Text);
                }
            }
        }

        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            returnMain(txtNguoiDung_MonHocID.Text)
            ;
        }
        private void returnMain(string NguoiDung_MonHocID)
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?NguoiDung_MonHocid={1}", this.TabId, NguoiDung_MonHocID));
        }

        protected void ddlNguoiDung_MonHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            returnMain(ddlNguoiDung_MonHoc.SelectedValue)
           ;
        }
    }
}