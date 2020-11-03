using DotNetNuke.Entities.Modules;
using nuce.web.thi;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
namespace nuce.web.thichungchi
{
    public partial class QuanLyDapAn : PortalModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ma"] != null && Request.QueryString["ma"] != "")
                {
                    string tukhoa = "";
                    if (Request.QueryString["tukhoa"] != null && Request.QueryString["tukhoa"] != "")
                        tukhoa = Request.QueryString["tukhoa"].Trim();
                    txtTuKhoa.Text = tukhoa;

                    string macauhoi = Request.QueryString["ma"].Trim();
                    string mabocauhoi = Request.QueryString["mabocauhoi"].Trim();
                    txtMaCauHoi.Text = macauhoi;
                    txtMaBoCauHoi.Text = mabocauhoi;
                    txtMonHoc.Text = Request.QueryString["NguoiDung_MonHocid"].Trim();
                    // Kiểm tra xem có lớp
                    DataTable dtCauHoi = data.dnn_NuceThi_CauHoi.getByMa(macauhoi);
                    if (dtCauHoi.Rows.Count > 0)
                    {
                        int iCauHoiID = int.Parse(dtCauHoi.Rows[0]["CauHoiID"].ToString())
                        ;
                        int iType = int.Parse(dtCauHoi.Rows[0]["Type"].ToString())
                       ;
                        string strNameType = this.GetLoaiCauHoi(iType).Name;
                        string strNoiDungCauHoi = Regex.Replace(HttpUtility.HtmlDecode(dtCauHoi.Rows[0]["Content"].ToString()), "<.*?>", String.Empty);
                        strNoiDungCauHoi = string.Format("<b>Câu hỏi <span style='color:blue;'> (loại {1})</style></b> : {0} </br> <a href='/tabid/{2}/default.aspx?ma={3}&&tukhoa={4}&&NguoiDung_MonHocid={5}'>Quay lại danh sách</a>", strNoiDungCauHoi, this.GetLoaiCauHoi(iType).Description, 39, mabocauhoi, txtTuKhoa.Text,txtMonHoc.Text);
                        //txtLopQuanLyID.Text = iLopQuanLyID.ToString();
                        if (Request.QueryString["EditType"] != null && Request.QueryString["EditType"] != "")
                        {
                            string strType = Request.QueryString["EditType"];

                            txtType.Text = strType;
                            divAnnouce.InnerHtml = "";
                            if (strType == "edit" || strType == "insert")
                            {
                                switch (strNameType)
                                {
                                    case "MA":
                                        trMatching.Visible = true;
                                        trIsChecked.Visible = false;
                                        break;
                                    case "SC":
                                    case "MC":
                                    case "TQ":
                                    case "FQ":
                                    case "TL":
                                        trMatching.Visible = false;
                                        trIsChecked.Visible = true;
                                        break;
                                    default: break;
                                }


                                txtCauHoiID.Text = iCauHoiID.ToString();
                                if (strType == "insert")
                                {
                                    divAnnouce.InnerHtml = string.Format("Thêm mới đáp án trong {0}", strNoiDungCauHoi);
                                }
                                else
                                {
                                    divAnnouce.InnerHtml = string.Format("Chỉnh sửa thông tin của đáp án trong {0}", strNoiDungCauHoi);
                                    int iID = int.Parse(Request.QueryString["id"]);
                                    DataTable dtDapAn = data.dnn_NuceThi_DapAn.get(iID);
                                    if (dtCauHoi.Rows.Count > 0)
                                    {
                                        txtID.Text = iID.ToString();
                                        txtNoiDung.Text = dtDapAn.Rows[0]["Content"].ToString(); ;
                                        txtMatching.Text = dtDapAn.Rows[0]["Matching"].ToString();
                                        //txtBoCauHoiID.Text= dtCauHoi.Rows[0]["BoCauHoiID"].ToString();
                                        txtOrder.Text = dtDapAn.Rows[0]["Order"].ToString();
                                        chkIsChecked.Checked = bool.Parse(dtDapAn.Rows[0]["IsCheck"].ToString());
                                        //txtLevel.Text= dtCauHoi.Rows[0]["Level"].ToString();
                                        //txtTypeCauHoi.Text= dtCauHoi.Rows[0]["Type"].ToString();
                                    }
                                    else
                                    {
                                        returnMain(macauhoi);
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
                        }
                        else
                        {
                            divAnnouce.InnerHtml = strNoiDungCauHoi;
                            divEdit.Visible = false;
                            displayList(iCauHoiID, strNameType);
                        }
                    }
                    else
                    {
                        divAnnouce.InnerHtml = "Không tồn tại mã câu hỏi này đề nghị bạn chọn lại !";
                        divEdit.Visible = false;
                    }
                }
                else
                {
                    divAnnouce.InnerHtml = "Không có mã câu hỏi !";
                    divEdit.Visible = false;
                }
            }
        }

        private void displayList(int CauHoiID, string NameType)
        {
            DataTable tblTable = data.dnn_NuceThi_DapAn.getByCauHoi(CauHoiID);
            string strTable = "<table class=\"table\">";
            strTable += "<thead><tr>";
            strTable += string.Format("<th>{0}</th>", "STT");
            strTable += string.Format("<th>{0}</th>", "Nội dung");
            switch (NameType)
            {
                case "MA":
                    strTable += string.Format("<th>{0}</th>", "Nội dung ghép");
                    break;
                case "SC":
                case "MC":
                case "TQ":
                case "FQ":
                    strTable += string.Format("<th>{0}</th>", "Là đáp án");
                    break;
                default: break;
            }


            strTable += string.Format("<th>{0}</th>", "Trạng thái");
            strTable += string.Format("<th>{0}</th>", "Chỉnh sửa");
            strTable += string.Format("<th>{0}</th>", "Xóa");
            strTable += string.Format("<th><a href='/tabid/{0}/default.aspx?EditType=insert&&ma={1}&&mabocauhoi={2}&&tukhoa={3}&&NguoiDung_MonHocid={4}'>Thêm mới</a></th>", this.TabId, txtMaCauHoi.Text, txtMaBoCauHoi.Text, txtTuKhoa.Text,txtMonHoc.Text);
            strTable += "</tr></thead>";
            strTable += "<tbody id=\"tbContent\">";
            for (int i = 0; i < tblTable.Rows.Count; i++)
            {
                strTable += "<tr>";
                strTable += string.Format("<td>{0}</td>", (i + 1));
                strTable += string.Format("<td>{0}</td>", HttpUtility.HtmlDecode(tblTable.Rows[i]["Content"].ToString()));
                switch (NameType)
                {
                    case "MA":
                        strTable += string.Format("<td>{0}</td>", HttpUtility.HtmlDecode(tblTable.Rows[i]["Matching"].ToString()));
                        break;
                    case "SC":
                    case "MC":
                    case "TQ":
                    case "FQ":
                        strTable += string.Format("<td>{0}</td>", bool.Parse(tblTable.Rows[i]["IsCheck"].ToString()) ? "Là đáp án" : "không là đáp án");
                        break;
                    default: break;
                }
                int iStatus = int.Parse(tblTable.Rows[i]["Status"].ToString());
                if (iStatus.Equals(1))
                {
                    strTable += string.Format("<td>{0} ", "Hoạt động");
                    strTable += string.Format(" - <a href='/tabid/{0}/default.aspx?EditType=changefinish&&id={1}&&ma={2}&&mabocauhoi={3}&&tukhoa={4}&&NguoiDung_MonHocid={5}'>(Chuyển)</a></td>", this.TabId, tblTable.Rows[i]["DapAnID"].ToString(), txtMaCauHoi.Text, txtMaBoCauHoi.Text, txtTuKhoa.Text,txtMonHoc.Text);
                }
                else
                {
                    strTable += string.Format("<td>{0}", "Dừng hoạt động");
                    strTable += string.Format(" - <a href='/tabid/{0}/default.aspx?EditType=changeactive&&id={1}&&ma={2}&&mabocauhoi={3}&&tukhoa={4}&&NguoiDung_MonHocid={5}'>(Chuyển)</a></td>", this.TabId, tblTable.Rows[i]["DapAnID"].ToString(), txtMaCauHoi.Text, txtMaBoCauHoi.Text, txtTuKhoa.Text,txtMonHoc.Text);
                }
                strTable += string.Format("<td><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&&ma={2}&&mabocauhoi={3}&&tukhoa={4}&&NguoiDung_MonHocid={5}'>Sửa</a></td>", this.TabId, tblTable.Rows[i]["DapAnID"].ToString(), txtMaCauHoi.Text, txtMaBoCauHoi.Text, txtTuKhoa.Text,txtMonHoc.Text);
                strTable += string.Format("<td><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}&ma={2}&&mabocauhoi={3}&&tukhoa={4}&&NguoiDung_MonHocid={5}'>Xóa</a></td>", this.TabId, tblTable.Rows[i]["DapAnID"].ToString(), txtMaCauHoi.Text, txtMaBoCauHoi.Text, txtTuKhoa.Text,txtMonHoc.Text);
                strTable += string.Format("<td><a href='/tabid/{0}/default.aspx?EditType=insert&&ma={1}&&mabocauhoi={2}&&tukhoa={3}&&NguoiDung_MonHocid={4}'>Thêm mới</a></td>", this.TabId, txtMaCauHoi.Text, txtMaBoCauHoi.Text, txtTuKhoa.Text,txtMonHoc.Text);
                strTable += "</tr>";
            }
            strTable += "</tbody>";
            strTable += "</table>";
            divContent.InnerHtml = strTable;
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string strType = txtType.Text;
            string strMa = txtMaCauHoi.Text;
            string strNoiDung = txtNoiDung.Text;
            bool isCheck = chkIsChecked.Checked;
            string strGhepNoiDung = txtMatching.Text;
            int iCauHoiID = int.Parse(txtCauHoiID.Text);
            int iOrder = int.Parse(txtOrder.Text);

            //int imabocauhoiQuanLy = int.Parse(txtLopQuanLyID.Text)
            if (strMa.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để mã trắng";
                return;
            }
            //int iLopQuanLy = int.Parse(txtLopQuanLyID.Text);
            if (strType == "insert")
            {
                int iReturnID = data.dnn_NuceThi_DapAn.insert(iCauHoiID, -1, strNoiDung, isCheck, strGhepNoiDung, 0, 0, iOrder, 1);
                if (iReturnID > 0)
                {
                    returnMain(txtMaCauHoi.Text);
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
                    data.dnn_NuceThi_DapAn.update(iID, iCauHoiID, -1, strNoiDung, isCheck, strGhepNoiDung, 0, 0, iOrder);
                    returnMain(txtMaCauHoi.Text);
                }
            }
        }
        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            returnMain(txtMaCauHoi.Text)
            ;
        }
        private void returnMain(string mabocauhoi)
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?ma={1}&&mabocauhoi={2}&&tukhoa={3}&&NguoiDung_MonHocid={4}", this.TabId, mabocauhoi, txtMaBoCauHoi.Text, txtTuKhoa.Text,txtMonHoc.Text));
        }
        protected void btnTimBoCauHoi_Click(object sender, EventArgs e)
        {
            returnMain(txtMaCauHoi.Text.Trim());
        }


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
    }
}