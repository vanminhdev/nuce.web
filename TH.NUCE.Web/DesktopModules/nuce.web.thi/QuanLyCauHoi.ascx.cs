using System;
using System.Data;
using DotNetNuke.Entities.Modules;
using System.Web;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.Text;

namespace nuce.web.thi
{
    public partial class QuanLyCauHoi : CoreModule
    {
        public static DataSet m_DSDataImport;
        protected override void OnInit(EventArgs e)
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
                    DataTable dtLopQuanLy = data.dnn_NuceThi_BoCauHoi.getByMaAndUser(this.UserId, mabocauhoi);
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
                                if(tukhoa!="")
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
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sác câu hỏi " +
                              "<a href='/tabid/100/default.aspx?NguoiDung_MonHocid=" + txtNguoiDungMonHoc.Text + "'> (Quay lại danh sách bộ câu hỏi)</a>" +
                              "</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable += "<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t3 fl'>{0}</div>", "Mã");
            strTable += string.Format("<div class='nd_t2222 fl'>{0}</div>", "Nội dung");
            strTable += string.Format("<div class='nd_t41 fl'>{0}</div>", "Trạng thái");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Danh sách đáp án");
            strTable += string.Format("<div class='nd_t44 fl'>{0}</div>", "Độ khó");
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "Thứ tự");
            strTable += string.Format("<div class='nd_t44 fl'>{0}</div>", "Chỉnh sửa");
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "Xóa");
            strTable += string.Format("<div class='nd_t44 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&ma={1}&&tukhoa={2}'>Thêm mới</a></div>", this.TabId, txtMaBoCauHoi.Text, txtTuKhoa.Text);
            strTable += "</div>";
            strTable += "<div class='nd_b fl'>";
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
            strTable += "</div>";
                strTable += "<div style='float:right;'>";
                strTable += string.Format("<a href='/tabid/{0}/default.aspx?EditType=deleteall&&ma={1}&&tukhoa={2}'>Xóa hết</a>", this.TabId, txtMaBoCauHoi.Text, txtTuKhoa.Text);
            // strTable += string.Format(" - - - <a href='/tabid/{0}/default.aspx?EditType=statistic&&ma={1}&&tukhoa={2}'>Thông kê</a>", this.TabId, txtMaBoCauHoi.Text, txtTuKhoa.Text);
                strTable += string.Format(" - - - <a href='/ExportExcel.aspx?type=5&&bocauhoiid={0}' target='_blank'> Thống kê </a>", BoCauHoiID);
            strTable += "</div>";
            strTable += "</div>";
            strTable += "</div>";
            strTable += "</div>";
            divContent.InnerHtml = strTable;
        }
        public string getDataHtmlFromRow(DataRow dr, string index, string mabocauhoi)
        {
            string strTable = "";
            string strParent = dr["ParentQuestionId"].ToString();
            int iLevel = int.Parse(dr["Level"].ToString());
            if (iLevel > 1)
                strTable += " <div class='nd_b3 fl'>";
            else
                strTable += " <div class='nd_b1 fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", index);
            strTable += string.Format("<div class='nd_t3 fl' style='text-align: left'>{0}</div>", dr["Ma"].ToString());
            strTable += string.Format("<div class='nd_t2222l fl'>{0}</div>", HttpUtility.HtmlDecode(dr["Content"].ToString()));
            int iStatus = int.Parse(dr["Status"].ToString());
            if (iStatus.Equals(1))
            {
                strTable += string.Format("<div class='nd_t41 fl'>{0} ", "Hoạt động");
                strTable += string.Format(" - <a href='/tabid/{0}/default.aspx?EditType=changefinish&&id={1}&&ma={2}&&parent={4}&&tukhoa={3}'>(Chuyển)</a></div>", this.TabId, dr["CauHoiID"].ToString(), txtMaBoCauHoi.Text, txtTuKhoa.Text, strParent);
            }
            else
            {
                strTable += string.Format("<div class='nd_t41 fl'>{0}", "Dừng hoạt động");
                strTable += string.Format(" - <a href='/tabid/{0}/default.aspx?EditType=changeactive&&id={1}&&ma={2}&&parent={4}&&tukhoa={3}'>(Chuyển)</a></div>", this.TabId, dr["CauHoiID"].ToString(), txtMaBoCauHoi.Text, txtTuKhoa.Text, strParent);
            }
            strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?id={1}&&ma={2}&&mabocauhoi={3}&&tukhoa={4}'>Danh sách đáp án</a></div>", 113, dr["CauHoiID"].ToString(), dr["Ma"].ToString(), mabocauhoi, txtTuKhoa.Text);
            strTable += string.Format("<div class='nd_t44 fl'>{0}</div>", GetDoKho(dr["DoKhoID"].ToString()).Ten);
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", dr["Order"].ToString());

            strTable += string.Format("<div class='nd_t44 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&&ma={2}&&parent={4}&&tukhoa={3}'>Sửa</a></div>", this.TabId, dr["CauHoiID"].ToString(), txtMaBoCauHoi.Text, txtTuKhoa.Text, strParent);
            strTable += string.Format("<div class='nd_t1 fl'><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}&ma={2}&&tukhoa={3}'>Xóa</a></div>", this.TabId, dr["CauHoiID"].ToString(), txtMaBoCauHoi.Text, txtTuKhoa.Text);
            if (iLevel.Equals(1))
                strTable += string.Format("<div class='nd_t44 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&ma={1}&&parent={3}&&tukhoa={2}'>Thêm mới</a></div>", this.TabId, txtMaBoCauHoi.Text, txtTuKhoa.Text, dr["CauHoiID"].ToString());
            else
                strTable += string.Format("<div class='nd_t44 fl'></div>");
            strTable += "</div>";
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
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?ma={1}&&tukhoa={2}", this.TabId, mabocauhoi, txtTuKhoa.Text));
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
                            if(strDapAn.Equals("S"))
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

                List<int> liColumnDapAn=new List<int>();

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
            FileUpload fu = fileupload;
            imagepath = "";
            if (fileupload.HasFile)
            {
                string filepath = Server.MapPath(ImageSavedPath);
                String fileExtension = System.IO.Path.GetExtension(fu.FileName).ToLower();
                List<string> allowedExtensions = new List<string> { ".xlsx" };
                if (allowedExtensions.Contains(fileExtension))
                {
                    try
                    {
                        string s_newfilename = DateTime.Now.Year.ToString() +
                            DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                            DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + fileExtension;
                        fu.PostedFile.SaveAs(filepath + s_newfilename);

                        imagepath = ImageSavedPath + s_newfilename;
                    }
                    catch (Exception ex)
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
    }
}