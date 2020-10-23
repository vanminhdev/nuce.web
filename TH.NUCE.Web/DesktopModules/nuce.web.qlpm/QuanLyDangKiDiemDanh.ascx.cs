using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.qlpm
{
    public partial class QuanLyDangKiDiemDanh : PortalModuleBase
    {
        public DataTable m_dtPhongHoc;
        public DataTable m_dtCaHoc;
        public DataTable m_dtLopHoc;

        public int m_TypeFix = 2;

        public string getPhongHoc(string PhongHocID)
        {
            for (int i = 0; i < m_dtPhongHoc.Rows.Count;i++)
            {
                if (m_dtPhongHoc.Rows[i]["PhongHocID"].ToString().Equals(PhongHocID))
                    return m_dtPhongHoc.Rows[i]["Ten"].ToString();
            }
            return "";
        }
        public string getCaHoc(string CaHocID)
        {
            for (int i = 0; i < m_dtCaHoc.Rows.Count; i++)
            {
                if (m_dtCaHoc.Rows[i]["CaHocID"].ToString().Equals(CaHocID))
                    return m_dtCaHoc.Rows[i]["Ten"].ToString();
            }
            return "";
        }
        public string getLopHoc(string LopHocID)
        {
            for (int i = 0; i < m_dtLopHoc.Rows.Count; i++)
            {
                if (m_dtLopHoc.Rows[i]["LopHocID"].ToString().Equals(LopHocID))
                    return m_dtLopHoc.Rows[i]["Ten"].ToString();
            }
            return "";
        }
        protected override void OnInit(EventArgs e)
        {
            m_dtLopHoc=data.dnn_NuceCommon_LopHoc.get1(-1);
            m_dtCaHoc = data.dnn_NuceCommon_CaHoc.get(-1);
            m_dtPhongHoc = data.dnn_NuceCommon_PhongHoc.getName(-1);

            ddlPhongHoc.DataTextField = "Ten";
            ddlPhongHoc.DataValueField = "PhongHocID";
            ddlPhongHoc.DataSource = m_dtPhongHoc;
            ddlPhongHoc.DataBind();
            ddlPhongHoc.SelectedValue = m_dtPhongHoc.Rows[0]["PhongHocID"].ToString();

            ddlLopHoc.DataTextField = "Ten";
            ddlLopHoc.DataValueField = "LopHocID";
            ddlLopHoc.DataSource = m_dtLopHoc;
            ddlLopHoc.DataBind();
            ddlLopHoc.SelectedValue = m_dtLopHoc.Rows[0]["LopHocID"].ToString();


            ddlCaHoc.DataTextField = "Ten";
            ddlCaHoc.DataValueField = "CaHocID";
            ddlCaHoc.DataSource = m_dtCaHoc;
            ddlCaHoc.DataBind();
            ddlCaHoc.SelectedValue = m_dtCaHoc.Rows[0]["CaHocID"].ToString();


            lnkNgayBatDau.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtNgay).Replace("M/d/yyyy", "MM/dd/yyyy").Replace("d/MM/yyyy", "dd/MM/yyyy").Replace("MM/dd/yyyy", "dd/MM/yyyy");
            //Điền dữ liệu vào các control
            imgNgayBatDau.ImageUrl = string.Format("{0}/images/calendar.png", this.ModulePath);

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["EditType"] != null && Request.QueryString["EditType"] != "")
                {
                    string strType = Request.QueryString["EditType"];
                    txtType.Text = strType;
                    divAnnouce.InnerHtml = "";
                    if (strType == "edit" || strType == "insert")
                    {
                        //Lấy dữ liệu từ server để nhét vào các control
                        if (strType == "insert")
                        {
                            divAnnouce.InnerHtml = "Thêm mới điểm danh";
                        }
                        else
                        {
                            divAnnouce.InnerHtml = "Chỉnh sửa thông tin điểm danh";
                            int iID = int.Parse(Request.QueryString["id"]);
                            DataTable tblTable = data.dnn_NuceQLPM_TuanHienTai.getByType(iID, m_TypeFix);
                            if (tblTable.Rows.Count > 0)
                            {
                                txtTen.Text = tblTable.Rows[0]["TenTuan"].ToString();
                                txtNgay.Text = DateTime.Parse(tblTable.Rows[0]["Ngay"].ToString()).ToString("dd/MM/yyyy");
                                ddlPhongHoc.SelectedValue = tblTable.Rows[0]["PhongHocID"].ToString();
                                ddlCaHoc.SelectedValue = tblTable.Rows[0]["CaHocID"].ToString();
                                ddlLopHoc.SelectedValue = tblTable.Rows[0]["LopHocID"].ToString();
                                txtGhiChu.Text = tblTable.Rows[0]["GhiChu"].ToString();
                                txtID.Text = iID.ToString();
                            }
                        }
                    }
                    else if(strType == "delete")
                    {
                        int iID = int.Parse(Request.QueryString["id"]);
                        data.dnn_NuceQLPM_TuanHienTai.updateStatus(iID, 4);
                        returnMain();
                    }
                }
                else
                {
                    divAnnouce.InnerHtml = "";
                    divEdit.Visible = false;
                    displayList();
                }
            }
        }

        private void displayList()
        {
            DataTable tblTable = data.dnn_NuceQLPM_TuanHienTai.getByType(-1,m_TypeFix);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách đăng ki khoa</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable +="<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Tên");
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Phòng học");
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "Ca học");
            strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", "Lớp học");
            strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", "Mô tả");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chỉnh sửa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Xóa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Thêm mới");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Điểm danh");
            strTable += "</div>";
                    strTable += "<div class='nd_b fl'>";
                    for (int i = 0; i < tblTable.Rows.Count; i++)
                    {
                        strTable += " <div class='nd_b1 fl'>";
                        strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", (i+1));
                        strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", tblTable.Rows[i]["TenTuan"].ToString());
                        strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", getPhongHoc(tblTable.Rows[i]["PhongHocID"].ToString()));
                        strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", getCaHoc(tblTable.Rows[i]["CaHocID"].ToString()));
                        strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", getLopHoc(tblTable.Rows[i]["LopHocID"].ToString()));
                        strTable += string.Format("<div class='nd_t222 fl'>{0}</div>", tblTable.Rows[i]["GhiChu"].ToString());
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["TuanHienTaiID"].ToString());
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}'>Xóa</a></div>", this.TabId, tblTable.Rows[i]["TuanHienTaiID"].ToString());
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert'>Thêm mới</a></div>", this.TabId);
                        strTable += string.Format("<div class='nd_t4 fl'><a href='/Quan-ly-phong-may/Diem-Danh?tuanhientaiid={0}&popUp=true'>Điểm danh</a></div>", tblTable.Rows[i]["TuanHienTaiID"].ToString());
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

            DateTime dtNgay;
            try
            {
                dtNgay = DateTime.ParseExact(txtNgay.Text, "dd/mm/yyyy", null);
            }
            catch (Exception)
            {
                dtNgay = DateTime.Now;
            }

            int iPhongHoc = int.Parse(ddlPhongHoc.SelectedValue.ToString());
            int iCaHoc = int.Parse(ddlCaHoc.SelectedValue.ToString());
            int iLopHoc = int.Parse(ddlLopHoc.SelectedValue.ToString());

            string strGhiChu = txtGhiChu.Text;

            bool blCheck = true;
            if (strTen.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để tên trắng";
                blCheck = false;
            }
            if (blCheck)
            {
                if (strType == "insert")
                {
                    data.dnn_NuceQLPM_TuanHienTai.insert(strTen, dtNgay, iPhongHoc, iCaHoc, iLopHoc, strGhiChu);
                    //DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("THCore_TM_InsertTags", iGroupTags, strName, "","","");
                    returnMain();
                }
                else
                {
                    if (strType == "edit")
                    {
                        int iID = int.Parse(txtID.Text);
                        data.dnn_NuceQLPM_TuanHienTai.update1(iID, strTen, dtNgay, iPhongHoc, iCaHoc, iLopHoc, strGhiChu);
                        returnMain();
                    }
                }
            }
        }

        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            returnMain()
            ;
        }
        private  void returnMain()
        {
              Response.Redirect(string.Format("/tabid/{0}/default.aspx", this.TabId));
        }
    }
}