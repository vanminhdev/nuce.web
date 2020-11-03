using System;
using System.Data;
using DotNetNuke.Entities.Modules;
using System.Web;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.Text;
using nuce.web.thi;
using DotNetNuke.Services.FileSystem;
using System.Text.RegularExpressions;

namespace nuce.web.thichungchi
{
    public partial class QuanLyCauHoi : PortalModuleBase
    {
        public static DataSet m_DSDataImport;
        protected override void OnInit(EventArgs e)
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int NguoiDung_MonHocID = -1;
                if (Request.QueryString["NguoiDung_MonHocid"] != null&&int.TryParse(Request.QueryString["NguoiDung_MonHocid"].ToString().Trim(), out NguoiDung_MonHocID))
                {
                    if (Request.QueryString["ma"] != null && Request.QueryString["ma"] != "")
                    {
                        string mabocauhoi = Request.QueryString["ma"].Trim();
                        string tukhoa = "";
                        string parent = "-1";
                        if (Request.QueryString["tukhoa"] != null && Request.QueryString["tukhoa"] != "")
                            tukhoa = Request.QueryString["tukhoa"].Trim();
                        txtTuKhoa.Text = tukhoa;

                        if (Request.QueryString["parent"] != null && Request.QueryString["parent"] != "")
                            parent = Request.QueryString["parent"].Trim();

                        txtParent.Text = parent;

                        txtMaBoCauHoi.Text = mabocauhoi;
                        // Kiểm tra xem có lớp
                        DataTable dtLopQuanLy = data.dnn_NuceThi_BoCauHoi.getByMaAndUser(NguoiDung_MonHocID, mabocauhoi);
                        if (dtLopQuanLy.Rows.Count > 0)
                        {
                            txtNguoiDungMonHoc.Text = dtLopQuanLy.Rows[0]["NguoiDung_MonHocID"].ToString();
                            int iBoCauHoiID = int.Parse(dtLopQuanLy.Rows[0]["BoCauHoiID"].ToString())
                            ;
                            int iType = int.Parse(dtLopQuanLy.Rows[0]["Type"].ToString())
                           ;
                            txtBoCauHoiID.Text = iBoCauHoiID.ToString();
                            txtTypeCauHoi.Text = iType.ToString();
                            //txtLopQuanLyID.Text = iLopQuanLyID.ToString();
                            if (Request.QueryString["EditType"] != null && Request.QueryString["EditType"] != "")
                            {
                                divFilter.Visible = false;
                                string strType = Request.QueryString["EditType"];
                                txtType.Text = strType;
                                divAnnouce.InnerHtml = "";
                                DataTable tblDoKho = InitCacheDoKho();


                                ddlDoKho.DataSource = tblDoKho;
                                ddlDoKho.DataTextField = "Ten";
                                ddlDoKho.DataValueField = "DoKhoID";
                                ddlDoKho.DataBind();
                                if (strType == "edit" || strType == "insert")
                                {

                                    txtLevel.Text = "1";

                                    if (strType == "insert")
                                    {
                                        divAnnouce.InnerHtml = string.Format("Thêm mới câu hỏi trong bộ câu hỏi {0} (loại {1})", mabocauhoi, this.GetLoaiCauHoi(iType).Description);
                                    }
                                    else
                                    {
                                        divAnnouce.InnerHtml = string.Format("Chỉnh sửa thông tin của câu hỏi trong bộ câu hỏi {0} (loại {1})", mabocauhoi, this.GetLoaiCauHoi(iType).Description);
                                        int iID = int.Parse(Request.QueryString["id"]);
                                        DataTable dtCauHoi = data.dnn_NuceThi_CauHoi.get(iID);
                                        if (dtCauHoi.Rows.Count > 0)
                                        {
                                            txtID.Text = iID.ToString();
                                            txtMa.Text = dtCauHoi.Rows[0]["Ma"].ToString();
                                            txtNoiDung.Text = dtCauHoi.Rows[0]["Content"].ToString(); ;
                                            txtGiaiThich.Text = dtCauHoi.Rows[0]["Explain"].ToString();
                                            //txtBoCauHoiID.Text= dtCauHoi.Rows[0]["BoCauHoiID"].ToString();
                                            ddlDoKho.SelectedValue = dtCauHoi.Rows[0]["DoKhoID"].ToString();
                                            txtOrder.Text = dtCauHoi.Rows[0]["Order"].ToString();
                                            //txtLevel.Text= dtCauHoi.Rows[0]["Level"].ToString();
                                            //txtTypeCauHoi.Text= dtCauHoi.Rows[0]["Type"].ToString();
                                            string strImageOld = dtCauHoi.Rows[0]["Image"].ToString();

                                            urlImage.Url = strImageOld;
                                            this.imgImage.Src = string.Format("/Portals/{0}/{1}", this.PortalId, strImageOld);
                                            this.spImage.InnerHtml = strImageOld;
                                            txtImageOld.Text = strImageOld;


                                        }
                                        else
                                        {
                                            returnMain(mabocauhoi);
                                        }
                                    }
                                    //divDapAn.InnerHtml = strTableDapAn;
                                }
                                else if (strType == "delete")
                                {
                                    int iID = int.Parse(Request.QueryString["id"]);
                                    data.dnn_NuceThi_CauHoi.updateStatus(iID, 4);
                                    returnMain(Request.QueryString["ma"]);
                                }
                                else if (strType == "changefinish")
                                {
                                    int iID = int.Parse(Request.QueryString["id"]);
                                    data.dnn_NuceThi_CauHoi.updateStatus(iID, 3);
                                    returnMain(Request.QueryString["ma"]);
                                }
                                else if (strType == "changeactive")
                                {
                                    int iID = int.Parse(Request.QueryString["id"]);
                                    data.dnn_NuceThi_CauHoi.updateStatus(iID, 1);
                                    returnMain(Request.QueryString["ma"]);
                                }
                                else if (strType == "deleteall")
                                {
                                    if (tukhoa != "")
                                        data.dnn_NuceThi_CauHoi.updateAllStatus(iBoCauHoiID, tukhoa, 4);
                                    else
                                        data.dnn_NuceThi_CauHoi.updateAllStatus(iBoCauHoiID, 4);
                                    returnMain(Request.QueryString["ma"]);
                                }
                                else if (strType == "statistic")
                                {
                                    Utils.thongKe();
                                    returnMain(Request.QueryString["ma"]);
                                }
                            }
                            else
                            {
                                divAnnouce.InnerHtml = "";
                                divEdit.Visible = false;
                                displayList(iBoCauHoiID, mabocauhoi);
                            }
                        }
                        else
                        {
                            divAnnouce.InnerHtml = "Không tồn tại mã bộ câu hỏi này đề nghị bạn chọn lại !";
                            divEdit.Visible = false;
                        }
                    }
                    else
                    {
                        divAnnouce.InnerHtml = "Không có mã !";
                        divEdit.Visible = false;
                    }
                }
                else
                {
                    divAnnouce.InnerHtml = "Không có mã !";
                    divEdit.Visible = false;
                }
            }
        }

        private void displayList(int BoCauHoiID, string mabocauhoi)
        {
            divInportExcel.Visible = true;
            divInportExcel_Action.Visible = true;
            divInportExcel_Upload.Visible = false;
            DataTable tblTable;
            if (txtTuKhoa.Text != "")
                tblTable = data.dnn_NuceThi_CauHoi.getByBoCauHoi(BoCauHoiID, txtTuKhoa.Text);
            else
                tblTable = data.dnn_NuceThi_CauHoi.getByBoCauHoi(BoCauHoiID);
            string strTable = "<table class=\"table\">";
            strTable += "<thead><tr>";
            strTable += string.Format("<th style=\"width: 5%;vertical-align: top;\">{0}</th>", "STT");
            strTable += string.Format("<th style=\"vertical-align: top;\">{0}</th>", "Mã");
            strTable += string.Format("<th style=\"vertical-align: top;\">{0}</th>", "Nội dung");
            strTable += string.Format("<th style=\"vertical-align: top;\">{0}</th>", "Trạng thái");
            strTable += string.Format("<th style=\"vertical-align: top;\">{0}</th>", "Danh sách đáp án");
            strTable += string.Format("<th style=\"vertical-align: top;\">{0}</th>", "Độ khó");
            strTable += string.Format("<th style=\"vertical-align: top;\">{0}</th>", "Thứ tự");
            strTable += string.Format("<th style=\"vertical-align: top;\">{0}</th>", "Chỉnh sửa");
            strTable += string.Format("<th style=\"vertical-align: top;\">{0}</th>", "Xóa");
            strTable += string.Format("<th style=\"vertical-align: top;\" class='nd_t44 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&ma={1}&&tukhoa={2}&&NguoiDung_MonHocid={3}'>Thêm mới</a></th>", this.TabId, txtMaBoCauHoi.Text, txtTuKhoa.Text,txtNguoiDungMonHoc.Text);
            strTable += "</tr></thead>";
            strTable += "<tbody id=\"tbContent\">";
            DataRow[] drs = tblTable.Select(string.Format("Level=1"));
            for (int i = 0; i < drs.Length; i++)
            {

                strTable += getDataHtmlFromRow(drs[i], (i + 1).ToString(), mabocauhoi);
                DataRow[] drs1 = tblTable.Select(string.Format("ParentQuestionId={0}", drs[i]["CauHoiID"].ToString()));
                for (int j = 0; j < drs1.Length; j++)
                {
                    strTable += getDataHtmlFromRow(drs1[j], string.Format("{0}.{1}", (i + 1), (j + 1)), mabocauhoi);
                }
            }
            strTable += "</tbody>";
            strTable += "</table>";
            strTable += "<div style='float:right;'>";
            strTable += string.Format("<a href='/tabid/{0}/default.aspx?EditType=deleteall&&ma={1}&&tukhoa={2}&&NguoiDung_MonHocid={3}'>Xóa hết</a>", this.TabId, txtMaBoCauHoi.Text, txtTuKhoa.Text,txtNguoiDungMonHoc.Text);
            strTable += "<div";
            divContent.InnerHtml = strTable;
        }
        public string getDataHtmlFromRow(DataRow dr, string index, string mabocauhoi)
        {
            int iLevel = int.Parse(dr["Level"].ToString());
            string strTable = "<tr style='color:blue;'>";
            if (iLevel.Equals(1))
                strTable = "<tr>";

            string strParent = dr["ParentQuestionId"].ToString();

            strTable += string.Format("<td>{0}</td>", index);
            strTable += string.Format("<td>{0}</td>", dr["Ma"].ToString());
            strTable += string.Format("<td>{0}</td>", Regex.Replace(HttpUtility.HtmlDecode(dr["Content"].ToString()), "<.*?>", String.Empty));
            int iStatus = int.Parse(dr["Status"].ToString());
            if (iStatus.Equals(1))
            {
                strTable += string.Format("<td>{0} ", "Hoạt động");
                strTable += string.Format(" - <a href='/tabid/{0}/default.aspx?EditType=changefinish&&id={1}&&ma={2}&&parent={4}&&tukhoa={3}&&NguoiDung_MonHocid={5}'>(Chuyển)</a></td>", this.TabId, dr["CauHoiID"].ToString(), txtMaBoCauHoi.Text, txtTuKhoa.Text, strParent,txtNguoiDungMonHoc.Text);
            }
            else
            {
                strTable += string.Format("<td>{0}", "Dừng hoạt động");
                strTable += string.Format(" - <a href='/tabid/{0}/default.aspx?EditType=changeactive&&id={1}&&ma={2}&&parent={4}&&tukhoa={3}&&NguoiDung_MonHocid={5}'>(Chuyển)</a></td>", this.TabId, dr["CauHoiID"].ToString(), txtMaBoCauHoi.Text, txtTuKhoa.Text, strParent,txtNguoiDungMonHoc.Text);
            }
            strTable += string.Format("<td><a href='/tabid/{0}/default.aspx?id={1}&&ma={2}&&mabocauhoi={3}&&tukhoa={4}&&NguoiDung_MonHocid={5}'>Danh sách đáp án</a></td>", 40, dr["CauHoiID"].ToString(), dr["Ma"].ToString(), mabocauhoi, txtTuKhoa.Text,txtNguoiDungMonHoc.Text,txtNguoiDungMonHoc.Text);
            strTable += string.Format("<td>{0}</td>", GetDoKho(dr["DoKhoID"].ToString()).Ten);
            strTable += string.Format("<td>{0}</td>", dr["Order"].ToString());

            strTable += string.Format("<td><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&&ma={2}&&parent={4}&&tukhoa={3}&&NguoiDung_MonHocid={5}'>Sửa</a></td>", this.TabId, dr["CauHoiID"].ToString(), txtMaBoCauHoi.Text, txtTuKhoa.Text, strParent,txtNguoiDungMonHoc.Text);
            strTable += string.Format("<td><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}&ma={2}&&tukhoa={3}&&NguoiDung_MonHocid={4}'>Xóa</a></td>", this.TabId, dr["CauHoiID"].ToString(), txtMaBoCauHoi.Text, txtTuKhoa.Text,txtNguoiDungMonHoc.Text);
            if (iLevel.Equals(1))
                strTable += string.Format("<td><a href='/tabid/{0}/default.aspx?EditType=insert&&ma={1}&&parent={3}&&tukhoa={2}&&NguoiDung_MonHocid={4}'>Thêm mới</a></td>", this.TabId, txtMaBoCauHoi.Text, txtTuKhoa.Text, dr["CauHoiID"].ToString(),txtNguoiDungMonHoc.Text);
            else
                strTable += string.Format("<td></td>");
            strTable += "</tr>";
            return strTable;
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string strType = txtType.Text;
            string strMa = txtMa.Text;
            string strNoiDung = txtNoiDung.Text;
            string strGiaiThich = txtGiaiThich.Text;
            int iBoCauHoiID = int.Parse(txtBoCauHoiID.Text);
            int iDoKho = int.Parse(ddlDoKho.SelectedValue);
            int iOrder = int.Parse(txtOrder.Text);
            int iLevel = int.Parse(txtLevel.Text);
            int iType = int.Parse(txtTypeCauHoi.Text);
            string strImage = "";
            strImage = GetFilePath(urlImage.Url, this.PortalId);
            if (strImage == "")
                strImage = txtImageOld.Text;

            int iParent = int.Parse(txtParent.Text.Trim());
            if (iParent > 0) iLevel = 2;
            //int imabocauhoiQuanLy = int.Parse(txtLopQuanLyID.Text)
            if (strMa.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để mã trắng";
                return;
            }
            //int iLopQuanLy = int.Parse(txtLopQuanLyID.Text);
            if (strType == "insert")
            {
                int iReturnID = data.dnn_NuceThi_CauHoi.insert(iBoCauHoiID, iDoKho, strMa, strNoiDung, strImage, "", "", -1, -1, true, -1, strGiaiThich, iParent, -1, DateTime.Now, -1, this.UserId, this.UserId, DateTime.Now, DateTime.Now, iOrder, iLevel, iType, 1);
                if (iReturnID > 0)
                {
                    returnMain(txtMaBoCauHoi.Text);
                }
                else
                {
                    divAnnouce.InnerHtml = divAnnouce.InnerHtml + " --- Thêm mới thất bại !!!";
                }
            }
            else
            {
                if (strType == "edit")
                {
                    int iID = int.Parse(txtID.Text);
                    data.dnn_NuceThi_CauHoi.update(iID, iBoCauHoiID, iDoKho, strMa, strNoiDung, strImage, "", "", -1, -1, true, -1, strGiaiThich, iParent, -1, DateTime.Now, -1, this.UserId, this.UserId, DateTime.Now, DateTime.Now, iOrder, iLevel, iType);
                    returnMain(txtMaBoCauHoi.Text);
                }
            }
        }
        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            returnMain(txtMaBoCauHoi.Text)
            ;
        }
        private void returnMain(string mabocauhoi)
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?ma={1}&&tukhoa={2}&&NguoiDung_MonHocid={3}", this.TabId, mabocauhoi, txtTuKhoa.Text,txtNguoiDungMonHoc.Text));
        }
        protected void btnTimBoCauHoi_Click(object sender, EventArgs e)
        {
            returnMain(txtMaBoCauHoi.Text.Trim());
        }
        protected void btnQuayTroLaiMain_Click(object sender, EventArgs e)
        {
            returnMain(txtMaBoCauHoi.Text)
          ;
        }
        protected void btnShowFormImport_Click(object sender, EventArgs e)
        {
            divInportExcel.Visible = true;
            divInportExcel_Action.Visible = false;
            divInportExcel_Upload.Visible = true;
            divFilter.Visible = false;
            divContent.Visible = false;
            divEdit.Visible = false;

            divAnnouce.Visible = true;
            divAnnouce.InnerHtml = string.Format("import file vào bộ câu hỏi có mã <b style='color:blue;'>{0}</b> loại <b style='color:blue;'>{1}</b>", txtMaBoCauHoi.Text, GetLoaiCauHoi(int.Parse(txtTypeCauHoi.Text)).Description);
        }
        protected void btnImport_Click(object sender, EventArgs e)
        {
            //divContentExcel.InnerHtml = getHtmlFromDataTable(m_DSDataImport.Tables[0]);
            switch (GetLoaiCauHoi(int.Parse(txtTypeCauHoi.Text)).Name)
            {
                case "TL":
                    processTL(m_DSDataImport);
                    break;
                case "TQ":
                    processTQ(m_DSDataImport);
                    break;
                case "FQ":
                    processTQ(m_DSDataImport);
                    break;
                case "SC":
                    processSC(m_DSDataImport);
                    break;
                case "MC":
                    processMC(m_DSDataImport);
                    break;
                default: break;
            }

        }
        #region ProcessImportExcel
        void processTQ(DataSet ds)
        {
            try
            {
                if (ds.Tables.Count < 1)
                {
                    divAnnouce.InnerHtml = "Không đúng chuẩn format excel ";
                    return;
                }
                if (ds.Tables[0].Rows.Count < 1)
                {
                    divAnnouce.InnerHtml = "Không có dữ liệu import ";
                }

                string strMa = txtMaBoCauHoi.Text;
                int iBoCauHoiID = int.Parse(txtBoCauHoiID.Text);
                int iType = int.Parse(txtTypeCauHoi.Text);

                string strReport = "";
                int iSTT = -1;
                string strNoiDung = "";
                string strDapAn = "";
                int iDoKhoID = -1;
                bool isDapAnDung = false;
                bool isDapAnSai = false;

                int iIDCauHoi = -1;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        iSTT = int.Parse(ds.Tables[0].Rows[i][0].ToString().Trim());
                        strNoiDung = ds.Tables[0].Rows[i][1].ToString().Trim();
                        strDapAn = ds.Tables[0].Rows[i][2].ToString().Trim();
                        strDapAn = strDapAn.ToUpper();
                        iDoKhoID = int.Parse(ds.Tables[0].Rows[i][3].ToString().Trim());
                        iIDCauHoi = data.dnn_NuceThi_CauHoi.insert(iBoCauHoiID, iDoKhoID, strMa, strNoiDung, "", -1, Utils.StripUnicodeCharactersFromString(strNoiDung), iBoCauHoiID, 1, iType, 1);
                        if (iIDCauHoi > 0)
                        {
                            if (strDapAn.Equals("S"))
                            {
                                isDapAnDung = false;
                                isDapAnSai = true;
                            }
                            else
                            {
                                isDapAnDung = true;
                                isDapAnSai = false;
                            }
                            data.dnn_NuceThi_DapAn.insert(iIDCauHoi, -1, "Đúng", isDapAnDung, "", -1, -1, iIDCauHoi, 1);
                            data.dnn_NuceThi_DapAn.insert(iIDCauHoi, -1, "Sai", isDapAnSai, "", -1, -1, iIDCauHoi, 1);
                        }
                    }
                    catch (Exception ex)
                    {
                        strReport += string.Format("Loi dong {0}: {1} ;", i, ex.ToString());
                    }
                }
                divAnnouce.InnerHtml = strReport;
            }
            catch (Exception ex0)
            {
                divAnnouce.InnerHtml = ex0.ToString();
            }
        }
        void processSC(DataSet ds)
        {
            try
            {
                if (ds.Tables.Count < 1)
                {
                    divAnnouce.InnerHtml = "Không đúng chuẩn format excel ";
                    return;
                }
                if (ds.Tables[0].Rows.Count < 2)
                {
                    divAnnouce.InnerHtml = "Không có dữ liệu import ";
                }

                string strMa = txtMaBoCauHoi.Text;
                int iBoCauHoiID = int.Parse(txtBoCauHoiID.Text);
                int iType = int.Parse(txtTypeCauHoi.Text);

                string strReport = "";
                int iSTT = -1;
                string strNoiDung = "";
                string strDapAn = "";
                int iDoKhoID = -1;
                bool isDapAn = false;
                string strNoiDungDapAn = "";
                // Column dap an
                Dictionary<int, string> dcColumeDapAn = new Dictionary<int, string>();
                for (int i = 5; i < ds.Tables[0].Columns.Count; i++)
                {
                    dcColumeDapAn.Add(i, ds.Tables[0].Rows[0][i].ToString().Trim());
                }

                int iColumnDapAn = -1;

                int iIDCauHoi = -1;

                for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        iSTT = int.Parse(ds.Tables[0].Rows[i][0].ToString().Trim());
                        strNoiDung = ds.Tables[0].Rows[i][2].ToString().Trim();
                        strDapAn = ds.Tables[0].Rows[i][3].ToString().Trim();
                        strDapAn = strDapAn.ToUpper();
                        iDoKhoID = int.Parse(ds.Tables[0].Rows[i][4].ToString().Trim());
                        iIDCauHoi = data.dnn_NuceThi_CauHoi.insert(iBoCauHoiID, iDoKhoID, strMa, strNoiDung, "", -1, Utils.StripUnicodeCharactersFromString(strNoiDung), iBoCauHoiID, 1, iType, 1);
                        if (iIDCauHoi > 0)
                        {
                            for (int k = 5; k < ds.Tables[0].Columns.Count; k++)
                            {
                                if (dcColumeDapAn[k].ToUpper().Equals(strDapAn))
                                {
                                    iColumnDapAn = k;
                                    break;
                                }
                            }
                            // Cap nhat du lieu cau hoi con
                            for (int k = 5; k < ds.Tables[0].Columns.Count; k++)
                            {
                                if (iColumnDapAn.Equals(k))
                                    isDapAn = true;
                                else
                                    isDapAn = false;
                                strNoiDungDapAn = ds.Tables[0].Rows[i][k].ToString().Trim();
                                // Cap nhat du lieu dap an
                                data.dnn_NuceThi_DapAn.insert(iIDCauHoi, -1, strNoiDungDapAn, isDapAn, "", -1, -1, iIDCauHoi, 1);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        strReport += string.Format("Loi dong {0}: {1} ;", i, ex.ToString());
                    }
                }
                divAnnouce.InnerHtml = strReport;
            }
            catch (Exception ex0)
            {
                divAnnouce.InnerHtml = ex0.ToString();
            }
        }
        void processMC(DataSet ds)
        {
            try
            {
                if (ds.Tables.Count < 1)
                {
                    divAnnouce.InnerHtml = "Không đúng chuẩn format excel ";
                    return;
                }
                if (ds.Tables[0].Rows.Count < 2)
                {
                    divAnnouce.InnerHtml = "Không có dữ liệu import ";
                }

                string strMa = txtMaBoCauHoi.Text;
                int iBoCauHoiID = int.Parse(txtBoCauHoiID.Text);
                int iType = int.Parse(txtTypeCauHoi.Text);

                string strReport = "";
                int iSTT = -1;
                string strNoiDung = "";
                string strDapAn = "";
                int iDoKhoID = -1;
                bool isDapAn = false;
                string strNoiDungDapAn = "";
                // Column dap an
                Dictionary<int, string> dcColumeDapAn = new Dictionary<int, string>();
                for (int i = 5; i < ds.Tables[0].Columns.Count; i++)
                {
                    dcColumeDapAn.Add(i, ds.Tables[0].Rows[0][i].ToString().Trim());
                }

                List<int> liColumnDapAn = new List<int>();

                int iIDCauHoi = -1;

                for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
                {
                    liColumnDapAn.Clear();
                    liColumnDapAn = new List<int>();
                    try
                    {
                        iSTT = int.Parse(ds.Tables[0].Rows[i][0].ToString().Trim());
                        strNoiDung = ds.Tables[0].Rows[i][2].ToString().Trim();
                        strDapAn = ds.Tables[0].Rows[i][3].ToString().Trim();
                        iDoKhoID = int.Parse(ds.Tables[0].Rows[i][4].ToString().Trim());
                        iIDCauHoi = data.dnn_NuceThi_CauHoi.insert(iBoCauHoiID, iDoKhoID, strMa, strNoiDung, "", -1, Utils.StripUnicodeCharactersFromString(strNoiDung), iBoCauHoiID, 1, iType, 1);
                        if (iIDCauHoi > 0)
                        {
                            for (int k = 5; k < ds.Tables[0].Columns.Count; k++)
                            {
                                if (strDapAn.Contains(dcColumeDapAn[k]))
                                    liColumnDapAn.Add(k);
                            }
                            // Cap nhat du lieu cau hoi con
                            for (int k = 5; k < ds.Tables[0].Columns.Count; k++)
                            {
                                if (liColumnDapAn.Contains(k))
                                    isDapAn = true;
                                else
                                    isDapAn = false;
                                strNoiDungDapAn = ds.Tables[0].Rows[i][k].ToString().Trim();
                                // Cap nhat du lieu dap an
                                data.dnn_NuceThi_DapAn.insert(iIDCauHoi, -1, strNoiDungDapAn, isDapAn, "", -1, -1, iIDCauHoi, 1);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        strReport += string.Format("Loi dong {0}: {1} ;", i, ex.ToString());
                    }
                }
                divAnnouce.InnerHtml = strReport;
            }
            catch (Exception ex0)
            {
                divAnnouce.InnerHtml = ex0.ToString();
            }
        }
        void processTL(DataSet ds)
        {
            try
            {

                if (ds.Tables.Count < 2)
                {
                    divAnnouce.InnerHtml = "Không đúng chuẩn format excel ";
                    return;
                }
                if (ds.Tables[0].Rows.Count < 2)
                {
                    divAnnouce.InnerHtml = "Không có dữ liệu import ";
                    return;
                }

                string strMa = txtMaBoCauHoi.Text;
                int iBoCauHoiID = int.Parse(txtBoCauHoiID.Text);
                int iType = int.Parse(txtTypeCauHoi.Text);

                string strReport = "";
                string strNameColumeIDDuLieu = ds.Tables[0].Columns[1].ColumnName;
                int iIDCauHoi = -1;
                int iIDCauHoiChild = -1;
                int iID = -1;
                string strDuLieu = "";
                DataRow[] drsTemp;

                int iSTT = -1;
                string strLoaiCauHoi = "";
                string strNoiDung = "";
                int iDoKhoID = -1;
                string strDapAnDung = "";
                string strNoiDungDapAn = "";
                bool isDapAn = false;
                // Column dap an
                Dictionary<int, string> dcColumeDapAn = new Dictionary<int, string>();
                for (int i = 6; i < ds.Tables[0].Columns.Count; i++)
                {
                    dcColumeDapAn.Add(i, ds.Tables[0].Rows[0][i].ToString().Trim());
                }

                int iColumnDapAn = -1;
                // Lấy dữ liệu 

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    try
                    {
                        iID = int.Parse(ds.Tables[1].Rows[i][0].ToString().Trim());
                        strDuLieu = ds.Tables[1].Rows[i][1].ToString().Trim();
                        iDoKhoID = int.Parse(ds.Tables[1].Rows[i][2].ToString().Trim());
                        if (strDuLieu.Equals(""))
                        {
                            strReport += string.Format("Loi dong {0}: {1};", i, "Du lieu trang");
                            continue;
                        }
                        drsTemp = ds.Tables[0].Select(string.Format("{0}='{1}'", strNameColumeIDDuLieu, iID));
                        if (drsTemp.Length >= 1)
                        {
                            // Cap nhat du lieu cau hoi cha
                            iIDCauHoi = data.dnn_NuceThi_CauHoi.insert(iBoCauHoiID, iDoKhoID, strMa, strDuLieu, "", -1, Utils.StripUnicodeCharactersFromString(strDuLieu), iBoCauHoiID, 1, iType, 1);
                            if (iIDCauHoi > 0)
                            {
                                for (int j = 0; j < drsTemp.Length; j++)
                                {
                                    try
                                    {
                                        iSTT = int.Parse(drsTemp[j][0].ToString().Trim());
                                        strLoaiCauHoi = drsTemp[j][2].ToString().Trim();
                                        strNoiDung = drsTemp[j][3].ToString().Trim();
                                        iDoKhoID = int.Parse(drsTemp[j][4].ToString().Trim());
                                        strDapAnDung = drsTemp[j][5].ToString().Trim();
                                        for (int k = 6; k < ds.Tables[0].Columns.Count; k++)
                                        {
                                            if (dcColumeDapAn[k].Equals(strDapAnDung))
                                            {
                                                iColumnDapAn = k;
                                                break;
                                            }
                                        }
                                        // Cap nhat du lieu cau hoi con
                                        iIDCauHoiChild = data.dnn_NuceThi_CauHoi.insert(iBoCauHoiID, iDoKhoID, strMa, strNoiDung, "", iIDCauHoi, Utils.StripUnicodeCharactersFromString(strDuLieu + strNoiDung), iBoCauHoiID, 2, this.GetLoaiCauHoi(strLoaiCauHoi).ID, 1);
                                        for (int k = 6; k < ds.Tables[0].Columns.Count; k++)
                                        {
                                            if (iColumnDapAn.Equals(k))
                                                isDapAn = true;
                                            else
                                                isDapAn = false;
                                            strNoiDungDapAn = drsTemp[j][k].ToString().Trim();
                                            // Cap nhat du lieu dap an
                                            data.dnn_NuceThi_DapAn.insert(iIDCauHoiChild, -1, strNoiDungDapAn, isDapAn, "", -1, -1, iIDCauHoiChild, 1);
                                        }
                                    }
                                    catch (Exception ex1)
                                    {
                                        strReport += string.Format("Loi dong {0} - Khong chen duoc cau hoi tai dong {1} trong sheet CH voi loi: {2} ;", i, (j + 1), ex1.ToString());
                                    }
                                    //
                                }
                            }
                            else
                            {
                                strReport += string.Format("Loi dong {0}: {1};", i, "Dữ liệu bị trùng");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        strReport += string.Format("Loi dong {0}: {1} ;", i, ex.ToString());
                    }
                    divAnnouce.InnerHtml = strReport;
                }
            }
            catch (Exception ex0)
            {
                divAnnouce.InnerHtml = ex0.ToString();
            }

        }
        #endregion
        #region importExcel
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string strImagePath;
            if (upload_Image(FU1, "/Portals/0/FileExcelUploads/", out strImagePath))
            {
                divAnnouce.InnerHtml = "Upload file thành công với đường dẫn: " + strImagePath;

                //Save the uploaded Excel file.
                string filePath = Server.MapPath("~" + strImagePath);
                //Open the Excel file using ClosedXML.
                m_DSDataImport = new DataSet();
                using (XLWorkbook workBook = new XLWorkbook(filePath))
                {
                    string strHtml = "";
                    for (int i = 1; i <= workBook.Worksheets.Count; i++)
                    {
                        IXLWorksheet workSheet = workBook.Worksheet(i);
                        m_DSDataImport.Tables.Add(getTableFromWorkSheet(workSheet));
                        strHtml += string.Format("<div style='margin:10px;text-align: center;'>Sheet: <b>{0}</b></div><div style='margin:10px;text-align: center;'>{1}</div>", workSheet.Name, getHtmlFromDataTable(m_DSDataImport.Tables[i - 1]));
                    }
                    divContentExcel.InnerHtml = strHtml;
                }
            }
            else
            {
                divAnnouce.InnerHtml = "Upload file không thành công với lỗi: " + strImagePath;
                return;
            }

        }
        string getHtmlFromDataTable(DataTable dt)
        {
            //Building an HTML string.
            StringBuilder html = new StringBuilder();

            //Table start.
            html.Append("<table border = '1' style='width:100%;'>");

            //Building the Header row.
            html.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<th>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt.Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    html.Append("<td>");
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");
            }

            //Table end.
            html.Append("</table>");
            return html.ToString();
        }
        DataTable getTableFromWorkSheet(IXLWorksheet workSheet)
        {
            DataTable dt = new DataTable();
            //Loop through the Worksheet rows.
            bool firstRow = true;
            foreach (IXLRow row in workSheet.Rows())
            {
                //Use the first row to add columns to DataTable.
                if (firstRow)
                {
                    foreach (IXLCell cell in row.Cells())
                    {
                        dt.Columns.Add(Utils.StripUnicodeCharactersFromString(cell.Value.ToString()).Replace(" ", "_"));
                    }
                    firstRow = false;
                }
                else
                {
                    //Add rows to DataTable.
                    dt.Rows.Add();
                    int i = 0;
                    foreach (IXLCell cell in row.Cells())
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                        i++;
                    }
                }

                //GridView1.DataSource = dt;
                //GridView1.DataBind();
            }
            return dt;
        }
        bool upload_Image(FileUpload fileupload, string ImageSavedPath, out string imagepath)
        {
            FileUpload FileUploadControl = fileupload;
            imagepath = "";
            if (fileupload.HasFile)
            {
                //string filepath = Server.MapPath(ImageSavedPath);
                String fileExtension = System.IO.Path.GetExtension(FileUploadControl.FileName).ToLower();
                List<string> allowedExtensions = new List<string> { ".xlsx" };
                if (allowedExtensions.Contains(fileExtension))
                {
                    try
                    {
                        string strMoRongFile = System.IO.Path.GetExtension(FileUploadControl.FileName);
                        string filename = System.IO.Path.GetFileName(FileUploadControl.FileName);
                        string strTenCu = filename;
                        string strTenMoi = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Second, 0, 0, Utils.RemoveUnicode(filename).Replace(" ", "_"));
                        string directoryPath = Server.MapPath("~/NuceDataUpload/cauhoi");
                        if (!System.IO.Directory.Exists(directoryPath))
                            System.IO.Directory.CreateDirectory(directoryPath);

                        string path = Server.MapPath("~/NuceDataUpload/cauhoi/" + strTenMoi);
                        string strLinkFile = path;
                        System.IO.FileInfo file = new System.IO.FileInfo(path);
                        if (file.Exists)//check file exsit or not
                        {
                            file.Delete();
                        }
                        FileUploadControl.SaveAs(strLinkFile);
                        strLinkFile = "/NuceDataUpload/cauhoi/" + strTenMoi;

                        imagepath = strLinkFile;
                    }
                    catch (Exception)
                    {
                        imagepath = "File could not be uploaded.";
                    }
                    return true;
                }
                else
                {
                    imagepath = string.Format("Tệp mở rộng {0} không hỗ trợ ", fileExtension);
                }
            }
            else
            { imagepath = string.Format("file không upload được"); }
            return false;
        }
        #endregion

        public DataTable InitCacheDoKho()
        {
            string cacheKey;
            cacheKey = "nuce_web_thi_dokho";
            DataTable dt;
            if (Cache[cacheKey] == null)
            {
                dt = nuce.web.data.dnn_NuceThi_DoKho.get(-1);
                Cache.Insert(cacheKey, dt);
            }
            else
                dt = (DataTable)Cache[cacheKey];
            return dt;
        }
        #region LoaiCauHoi
        public LoaiCauHoi GetLoaiCauHoi(int Id)
        {
            string cacheKey;
            LoaiCauHoi objLoaiCauHoi = new LoaiCauHoi();
            cacheKey = "nuce_web_thi_loaicauhoi_" + Id.ToString();
            if (Cache[cacheKey] == null)
            {
                DataTable objTable = InitCacheLoaiCauHoi();
                DataRow[] drs = objTable.Select(string.Format("ID = {0}", Id));
                if (drs.Length > 0)
                {
                    objLoaiCauHoi.ID = Id;
                    objLoaiCauHoi.Description = drs[0]["Description"].ToString();
                    objLoaiCauHoi.Name = drs[0]["Name"].ToString();
                }
                else
                {
                    objLoaiCauHoi.ID = Id;
                    objLoaiCauHoi.Description = "";
                    objLoaiCauHoi.Name = "";
                }
                Cache.Insert(cacheKey, objLoaiCauHoi);
            }
            else
            {
                objLoaiCauHoi = (LoaiCauHoi)Cache[cacheKey];
            }
            return objLoaiCauHoi;
        }
        public LoaiCauHoi GetLoaiCauHoi(string Name)
        {
            DataTable objTable = InitCacheLoaiCauHoi();
            DataRow[] drs = objTable.Select(string.Format("Name = '{0}'", Name));
            LoaiCauHoi objLoaiCauHoi = new LoaiCauHoi();
            if (drs.Length > 0)
            {
                objLoaiCauHoi.ID = int.Parse(drs[0]["ID"].ToString());
                objLoaiCauHoi.Description = drs[0]["Description"].ToString();
                objLoaiCauHoi.Name = Name;
            }
            else
            {
                objLoaiCauHoi.ID = -1;
                objLoaiCauHoi.Description = "";
                objLoaiCauHoi.Name = Name;
            }
            return objLoaiCauHoi;
        }
        public DataTable InitCacheLoaiCauHoi()
        {
            string cacheKey;
            cacheKey = "nuce_web_thi_loaicauhoi";
            DataTable dt;
            if (Cache[cacheKey] == null)
            {
                dt = nuce.web.data.dnn_NuceThi_LoaiCauHoi.get(-1);
                Cache.Insert(cacheKey, dt);
            }
            else
                dt = (DataTable)Cache[cacheKey];
            return dt;
        }
        #endregion
        #region DoKho
        public DoKho GetDoKho(string Id)
        {
            return GetDoKho(int.Parse(Id));
        }
        public DoKho GetDoKho(int Id)
        {
            string cacheKey;
            DoKho objDoKho = new DoKho();
            cacheKey = "nuce_web_thi_dokho_" + Id.ToString();
            if (Cache[cacheKey] == null)
            {
                DataTable objTable = InitCacheDoKho();
                DataRow[] drs = objTable.Select(string.Format("DoKhoID={0}", Id));
                if (drs.Length > 0)
                {
                    objDoKho.DoKhoID = Id;
                    objDoKho.Ten = drs[0]["Ten"].ToString();
                }
                else
                {
                    objDoKho.DoKhoID = Id;
                    objDoKho.Ten = "";
                }
                Cache.Insert(cacheKey, objDoKho);
            }
            else
            {
                objDoKho = (DoKho)Cache[cacheKey];
            }
            return objDoKho;
        }
        #endregion
        public string GetFilePath(string strFile, int portalId)
        {
            if (strFile != "")
            {
                DotNetNuke.Services.FileSystem.IFileInfo objXSLFile = FileManager.Instance.GetFile((int.Parse(strFile.Replace("FileID=", ""))));
                return string.Format("{0}{1}", objXSLFile.Folder, objXSLFile.FileName);
            }
            else
            {
                return "";
            }
        }

        public string GetFileID(string fileUrl, int portalId)
        {
            if (fileUrl != "")
            {
                FileController objFileController = new FileController();
                int intFileID = objFileController.ConvertFilePathToFileId(string.Format("{0}", fileUrl), portalId);
                return string.Format("FileID={0}", intFileID);
            }
            else
            {
                return "";
            }
        }
    }
}