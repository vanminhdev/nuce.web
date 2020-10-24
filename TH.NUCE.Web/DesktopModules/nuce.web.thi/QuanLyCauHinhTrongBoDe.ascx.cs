using System;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace nuce.web.thi
{
    public partial class QuanLyCauHinhTrongBoDe : CoreModule
    {
        #region bang chua du lieu thong tin cau hoi
        public static DataTable m_dtThongkeSoCauHoiDaDung;
        public static DataTable m_dtThongkeSoCauHoi;
        public static DataTable m_dtBoCauHoi;
        #endregion
        public static Dictionary<string, DataTable> m_dcCauHois;
        public static Dictionary<string, DataTable> m_dcDapAns;
        public static Dictionary<string, List<model.CauHoi>> m_dcCauHoiPhans;
        public static Dictionary<string, List<int>> m_iCauHoiPhans;
        public static Dictionary<string, List<model.CauHoi>> m_dcCauHoiCauHinhs;
        public static Dictionary<string, List<model.CauHoi>> m_dcCauHoiBoDes;
        public static Dictionary<string, List<model.DapAn>> m_dcDapAnPhans;
        public static Dictionary<string, List<model.DapAn>> m_dcDapAnCauHinhs;
        public static Dictionary<string, List<model.DapAn>> m_dcDapAnBoDes;
        protected override void OnInit(EventArgs e)
        {
            m_dcCauHois = new Dictionary<string, DataTable>();
            m_dcDapAns = new Dictionary<string, DataTable>();
            m_dcCauHoiPhans = new Dictionary<string, List<model.CauHoi>>();
            m_iCauHoiPhans = new Dictionary<string, List<int>>();
            m_dcCauHoiCauHinhs = new Dictionary<string, List<model.CauHoi>>();
            m_dcCauHoiBoDes = new Dictionary<string, List<model.CauHoi>>();

            m_dcDapAnPhans = new Dictionary<string, List<model.DapAn>>();
            m_dcDapAnCauHinhs = new Dictionary<string, List<model.DapAn>>();
            m_dcDapAnBoDes = new Dictionary<string, List<model.DapAn>>();

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["NguoiDung_MonHocid"] != null && Request.QueryString["NguoiDung_MonHocid"] != "")
                {
                    int NguoiDung_MonHocID = -1;
                    if (int.TryParse(Request.QueryString["NguoiDung_MonHocid"].ToString().Trim(), out NguoiDung_MonHocID))
                    {
                        if (Request.QueryString["bodeid"] != null && Request.QueryString["bodeid"] != "")
                        {

                            int BoDeID = -1;
                            if (int.TryParse(Request.QueryString["bodeid"].ToString().Trim(), out BoDeID))
                            {
                                #region
                                m_dtThongkeSoCauHoiDaDung = data.dnn_NuceThi_BoDe.thongKeSoCauHoiDaDung(BoDeID);
                                m_dtThongkeSoCauHoi = data.dnn_NuceThi_CauHoi.thongKeSoCauHoiTheoNguoiDungMonHoc(NguoiDung_MonHocID);
                                m_dtBoCauHoi = data.dnn_NuceThi_BoCauHoi.getByNguoiDung_MonHoc(NguoiDung_MonHocID);
                                hienThiThongTinThongKe();
                                #endregion
                                divDanhSach.Visible = false;
                                divEditPhan.Visible = false;
                                divEditPhanCauHinh.Visible = false;
                                txtBoDeID.Text = BoDeID.ToString();
                                txtNguoiDung_MonHocID.Text = NguoiDung_MonHocID.ToString();
                                int iStatusBode = data.dnn_NuceThi_BoDe.getStatus(BoDeID);
                                if (iStatusBode.Equals(1))
                                {

                                    //divAnnouce.InnerHtml = "Quản lý cấu hình trong bộ đề";


                                    if (Request.QueryString["EditType"] != null && Request.QueryString["EditType"] != "")
                                    {
                                        string strType = Request.QueryString["EditType"];
                                        txtType.Text = strType;
                                        divAnnouce.InnerHtml = "";
                                        int iBode_PhanID = -1;
                                        int iBode_Phan_CauhinhID = -1;
                                        switch (strType)
                                        {
                                            case "insert":
                                                tdAnnouncePhan.InnerHtml = "Thêm mới phần trong cấu trúc đề";
                                                divEditPhan.Visible = true;
                                                divEditPhanCauHinh.Visible = false;
                                                break;
                                            case "update":
                                                divEditPhan.Visible = true;
                                                divEditPhanCauHinh.Visible = false;
                                                if (int.TryParse(Request.QueryString["bode_phanid"].ToString().Trim(), out iBode_PhanID))
                                                {
                                                    tdAnnouncePhan.InnerHtml = "Sửa thông tin phần trong cấu trúc đề";
                                                    DataTable dtBode_Phan = data.dnn_NuceThi_BoDe_Phan.get(iBode_PhanID);
                                                    txtTen.Text = dtBode_Phan.Rows[0]["Name"].ToString();
                                                    txtMoTa.Text = dtBode_Phan.Rows[0]["MoTa"].ToString();
                                                    txtID.Text = iBode_PhanID.ToString();
                                                }
                                                else
                                                    Response.Redirect(string.Format("/tabid/{0}/default.aspx?NguoiDung_MonHocid={1}", this.TabId, NguoiDung_MonHocID));
                                                break;
                                            case "delete":
                                                if (int.TryParse(Request.QueryString["bode_phanid"].ToString().Trim(), out iBode_PhanID))
                                                {
                                                    data.dnn_NuceThi_BoDe_Phan.updateStatus(iBode_PhanID, 4);
                                                    returnMain(NguoiDung_MonHocID.ToString(), BoDeID.ToString());
                                                }
                                                break;
                                            case "insertphan":

                                                divEditPhan.Visible = false;
                                                divEditPhanCauHinh.Visible = true;
                                                if (int.TryParse(Request.QueryString["bode_phanid"].ToString().Trim(), out iBode_PhanID))
                                                {
                                                    txtID.Text = "-1";
                                                    DataTable dtBode_Phan = data.dnn_NuceThi_BoDe_Phan.get(iBode_PhanID);
                                                    tdAnnounceBoDePhan.InnerHtml = "Thêm mới bộ câu hỏi trong phần " + dtBode_Phan.Rows[0]["Name"].ToString() + " của cấu trúc đề";
                                                    txtBoDe_PhanID.Text = iBode_PhanID.ToString();
                                                    initDdlDoKho();
                                                    initDdlBoCauHoi(NguoiDung_MonHocID);

                                                }
                                                else
                                                    Response.Redirect(string.Format("/tabid/{0}/default.aspx?NguoiDung_MonHocid={1}", this.TabId, NguoiDung_MonHocID));
                                                break;
                                            case "updatephan":
                                                divEditPhan.Visible = false;
                                                divEditPhanCauHinh.Visible = true;
                                                if (int.TryParse(Request.QueryString["bode_phanid"].ToString().Trim(), out iBode_PhanID))
                                                {

                                                    if (int.TryParse(Request.QueryString["bode_phan_cauhinhid"].ToString().Trim(), out iBode_Phan_CauhinhID))
                                                    {
                                                        DataTable dtBode_Phan = data.dnn_NuceThi_BoDe_Phan.get(iBode_PhanID);
                                                        tdAnnounceBoDePhan.InnerHtml = "Sửa thông tin bộ câu hỏi trong phần " + dtBode_Phan.Rows[0]["Name"].ToString() + " của cấu trúc đề";

                                                        txtBoDe_PhanID.Text = iBode_PhanID.ToString();
                                                        txtID.Text = iBode_Phan_CauhinhID.ToString();
                                                        initDdlDoKho();
                                                        initDdlBoCauHoi(NguoiDung_MonHocID);
                                                        DataTable dtBode_Phan_CauHinh = data.dnn_NuceThi_BoDe_Phan_CauHinh.get(iBode_Phan_CauhinhID);
                                                        ddlBoCauHoi.SelectedValue = dtBode_Phan_CauHinh.Rows[0]["BoCauHoiID"].ToString();
                                                        ddlDoKho.SelectedValue = dtBode_Phan_CauHinh.Rows[0]["DoKhoID"].ToString();
                                                        txtSoCauHoi.Text = dtBode_Phan_CauHinh.Rows[0]["SoCauHoi"].ToString();
                                                    }
                                                    else
                                                        Response.Redirect(string.Format("/tabid/{0}/default.aspx?NguoiDung_MonHocid={1}", this.TabId, NguoiDung_MonHocID));
                                                }
                                                else
                                                    Response.Redirect(string.Format("/tabid/{0}/default.aspx?NguoiDung_MonHocid={1}", this.TabId, NguoiDung_MonHocID));
                                                break;
                                            case "deletephan":
                                                if (int.TryParse(Request.QueryString["bode_phan_cauhinhid"].ToString().Trim(), out iBode_Phan_CauhinhID))
                                                {
                                                    data.dnn_NuceThi_BoDe_Phan_CauHinh.updateStatus(iBode_Phan_CauhinhID, 4);
                                                    returnMain(NguoiDung_MonHocID.ToString(), BoDeID.ToString());
                                                }
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        //Danh sách
                                        divDanhSach.Visible = true;
                                        displayList(NguoiDung_MonHocID, BoDeID);
                                    }
                                }
                                else
                                {
                                    btnTaoDe.Visible = false;
                                    btnTaoDe.Enabled = false;
                                    //Danh sách
                                    divDanhSach.Visible = true;
                                    displayList(NguoiDung_MonHocID, BoDeID);
                                }
                            }
                            else
                            {
                                Response.Redirect(string.Format("/tabid/{0}/default.aspx?NguoiDung_MonHocid={1}", this.TabId, NguoiDung_MonHocID));
                            }
                        }
                        else
                        {
                            Response.Redirect(string.Format("/tabid/{0}/default.aspx?NguoiDung_MonHocid={1}", this.TabId, NguoiDung_MonHocID));
                        }
                    }
                    else
                    {
                        Response.Redirect(string.Format("/tabid/{0}/default.aspx", this.TabId));
                    }
                }
                else
                {
                    Response.Redirect(string.Format("/tabid/{0}/default.aspx", this.TabId));
                }
            }
        }
        #region HienThiThongTinThongKe
        private string getHtmlThongTinThongKe(int s, int l)
        {
            string strTable1 = "";
            int iSoCauHoi = 0;
            int iSoCauHoiDaChon = 0;
            int iSoCauHoiToiDaConDuocChon = 0;

            DataRow[] drThongKeSoCauHoi;
            int iLengthRows = 0;
            if (m_dtBoCauHoi.Rows.Count > 0)
            {
                for (int i = s; i < l; i++)
                {
                    iLengthRows = 0;
                    if (m_dtThongkeSoCauHoi != null)
                    {
                        drThongKeSoCauHoi = m_dtThongkeSoCauHoi.Select(string.Format("BoCauHoiID={0}", m_dtBoCauHoi.Rows[i]["BoCauHoiID"].ToString()));
                        iLengthRows = drThongKeSoCauHoi.Length;
                        if (iLengthRows > 0)
                        {
                            strTable1 += "<tr style='vertical-align: top;'>";
                            strTable1 += string.Format("<td rowspan='{0}' style='color:blue;font-weight: bold; padding-left:3px;'>{1}</td>", iLengthRows, m_dtBoCauHoi.Rows[i]["Ten"].ToString());
                            strTable1 += string.Format("<td style='color:red;text-align:right;padding-right:3px;'>Cấp {0}</td>", drThongKeSoCauHoi[0]["DoKhoID"].ToString());
                            iSoCauHoi = int.Parse(drThongKeSoCauHoi[0]["SoCauHoi"].ToString());
                            iSoCauHoiDaChon = getSoCauHoiDaChon(drThongKeSoCauHoi[0]["BoCauHoiID"].ToString(), drThongKeSoCauHoi[0]["DoKhoID"].ToString());
                            strTable1 += string.Format("<td style='color:red;text-align:right;padding-right:3px;'>{0}</td>", iSoCauHoi);
                            strTable1 += string.Format("<td style='color:red;text-align:right;padding-right:3px;'>{0}</td>", iSoCauHoiDaChon);
                            iSoCauHoiToiDaConDuocChon = iSoCauHoi - iSoCauHoiDaChon;
                            strTable1 += string.Format("<td style='color:red;text-align:right;padding-right:3px;font-weight: bold;'>{0}</td>", iSoCauHoiToiDaConDuocChon);
                            strTable1 += "</tr>";

                            if (iLengthRows > 1)
                            {
                                for (int j = 1; j < iLengthRows; j++)
                                {
                                    strTable1 += "<tr style='vertical-align: top;'>";
                                    strTable1 += string.Format("<td style='color:red;text-align:right;padding-right:3px;'>Cấp {0}</td>", drThongKeSoCauHoi[j]["DoKhoID"].ToString());
                                    iSoCauHoi = int.Parse(drThongKeSoCauHoi[j]["SoCauHoi"].ToString());
                                    iSoCauHoiDaChon = getSoCauHoiDaChon(drThongKeSoCauHoi[j]["BoCauHoiID"].ToString(), drThongKeSoCauHoi[j]["DoKhoID"].ToString());
                                    strTable1 += string.Format("<td style='color:red;text-align:right;padding-right:3px;'>{0}</td>", iSoCauHoi);
                                    strTable1 += string.Format("<td style='color:red;text-align:right;padding-right:3px;'>{0}</td>", iSoCauHoiDaChon);
                                    iSoCauHoiToiDaConDuocChon = iSoCauHoi - iSoCauHoiDaChon;
                                    strTable1 += string.Format("<td style='color:red;text-align:right;padding-right:3px;font-weight: bold;'>{0}</td>", iSoCauHoiToiDaConDuocChon);
                                    strTable1 += "</tr>";
                                }
                            }
                        }
                    }
                }
            }
            return strTable1;
        }
        private void hienThiThongTinThongKe()
        {
            string strTable = "<div>" +
                  "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Thống kê các bộ câu hỏi</div>" +
                  "<div class='welcome_b'></div>" +
                  "<div class='nd fl'>" +
                  "<div class='noidung'>";
            strTable += "<table style='width:100%;'>";
            strTable += "<tr style='vertical-align: top;'>";
            string strTable1 = "";
            strTable1 += "<table border='1px' style='width:100%;'>";
            strTable1 += "<tr style='text-align: center; font-weight: bold;'>";
            strTable1 += "<td style='width:60%;'>Bộ câu hỏi</td>";
            strTable1 += "<td style='width:10%;'>Độ khó</td>";
            strTable1 += "<td style='width:10%;'>Tổng</td>";
            strTable1 += "<td style='width:10%;'>Đã chọn</td>";
            strTable1 += "<td style='width:10%;'>Còn lại</td>";
            strTable1 += "</tr>";
            string strTable2 = strTable1;
            int l = m_dtBoCauHoi.Rows.Count / 2;
            l = l + 1;
            strTable1 += getHtmlThongTinThongKe(0, l);
            strTable2 += getHtmlThongTinThongKe(l, m_dtBoCauHoi.Rows.Count);
            strTable1 += "</table>";
            strTable2 += "</table>";
            strTable += string.Format("<td style='width:50%; padding-right:1px;'>{0}<td>", strTable1);
            strTable += string.Format("<td style='width:50%;'>{0}<td>", strTable2);
            strTable += "</tr>";
            strTable += "</table>";
            strTable += "</div>";
            strTable += "</div>";
            strTable += "</div>";
            divThongKe.InnerHtml = strTable;
        }
        private string getTenBoCauHoiByID(string ID)
        {
            for (int i = 0; i < m_dtBoCauHoi.Rows.Count; i++)
            {
                if (m_dtBoCauHoi.Rows[i]["BoCauHoiID"].ToString().Equals(ID))
                    return m_dtBoCauHoi.Rows[i]["Ten"].ToString();
            }
            return "";
        }
        private int getSoCauHoiDaChon(string BoCauHoiID, string DoKhoID)
        {
            for (int i = 0; i < m_dtThongkeSoCauHoiDaDung.Rows.Count; i++)
            {
                if (m_dtThongkeSoCauHoiDaDung.Rows[i]["BoCauHoiID"].ToString().Equals(BoCauHoiID)
                    && m_dtThongkeSoCauHoiDaDung.Rows[i]["DoKhoID"].ToString().Equals(DoKhoID))
                    return int.Parse(m_dtThongkeSoCauHoiDaDung.Rows[i]["SoCauHoi"].ToString());
            }
            return 0;
        }
        #endregion
        private void initDdlBoCauHoi(int NguoiDung_MonHocID)
        {
            ddlBoCauHoi.DataSource = m_dtBoCauHoi;
            ddlBoCauHoi.DataTextField = "Ten";
            ddlBoCauHoi.DataValueField = "BoCauHoiID";
            ddlBoCauHoi.DataBind();

            tinhToanSoCauHoiToiDa();
        }
        private void initDdlDoKho()
        {
            DataTable tblDoKho = InitCacheDoKho();
            ddlDoKho.DataSource = tblDoKho;
            ddlDoKho.DataTextField = "Ten";
            ddlDoKho.DataValueField = "DoKhoID";
            ddlDoKho.DataBind();
        }
        private void returnMain(string NguoiDung_MonHocID, string BoDeID)
        {
            //Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "DanhSachCauHoi", "mid/" + this.ModuleId.ToString()));
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "CauHinh", "mid/" + this.ModuleId.ToString() + "/NguoiDung_MonHocid/" + NguoiDung_MonHocID + "/bodeid/" + BoDeID));
        }
        private string getUrlReturn(int NguoiDung_MonHocID, int BoDeID)
        {
            return DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "CauHinh", "mid/" + this.ModuleId.ToString() + "/NguoiDung_MonHocid/" + NguoiDung_MonHocID + "/bodeid/" + BoDeID);
        }
        private void displayList(int NguoiDung_MonHocID, int BoDeID)
        {
            DataTable tblTable = data.dnn_NuceThi_BoDe_Phan.getByBoDe(BoDeID);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách các phần</div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable += "<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t3 fl'>{0}</div>", "Tên");
            strTable += string.Format("<div class='nd_t2 fl'>{0}</div>", "Mô tả");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chỉnh sửa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Xóa");
            strTable += string.Format("<div class='nd_t4 fl'><a href='{0}/EditType/insert'>Thêm mới</a></div>", getUrlReturn(NguoiDung_MonHocID, BoDeID));
            strTable += "</div>";
            strTable += "<div class='nd_b fl'>";
            int iTongSoCauHoi = 0;
            int iTongSoCauHoiTungPhan = 0;
            int iSoCauHoiTungThanhPhan = 0;
            for (int i = 0; i < tblTable.Rows.Count; i++)
            {
                iTongSoCauHoiTungPhan = 0;
                string BoDe_PhanID = tblTable.Rows[i]["BoDe_PhanID"].ToString();
                strTable += " <div style='color: red;font-weight:bold; font-size:12px;' class='nd_b1 fl'>";
                strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", (i + 1));
                strTable += string.Format("<div class='nd_t3 fl'>{0}</div>", tblTable.Rows[i]["Name"].ToString());
                strTable += string.Format("<div class='nd_t2 fl'>{0}</div>", tblTable.Rows[i]["MoTa"].ToString());
                strTable += string.Format("<div class='nd_t4 fl'><a href='{0}/EditType/update/bode_phanid/{1}'>Sửa</a></div>", getUrlReturn(NguoiDung_MonHocID, BoDeID), BoDe_PhanID);
                strTable += string.Format("<div class='nd_t4 fl'><a href='{0}/EditType/delete/bode_phanid/{1}'>Xóa</a></div>", getUrlReturn(NguoiDung_MonHocID, BoDeID), BoDe_PhanID);
                strTable += string.Format("<div class='nd_t4 fl'><a href='{0}/EditType/insert'>Thêm mới</a></div>", getUrlReturn(NguoiDung_MonHocID, BoDeID));
                strTable += "</div>";
                DataTable tblTableCauHinh = data.dnn_NuceThi_BoDe_Phan_CauHinh.getByBoDe_Phan(int.Parse(BoDe_PhanID));
                if (tblTableCauHinh.Rows.Count > 0)
                {
                    int iLenghthCauHinh = tblTableCauHinh.Rows.Count;


                    strTable += " <div style='margin-left:50px;padding-left: 50px;height:" + ((iLenghthCauHinh + 1) * 25 + 25) + "px;' class='nd_b1 fl' >";
                    strTable += "<table style='padding:5px;width: 90%'>";
                    strTable += "<tr style='background: #F5F5F5;font-weight:bold; font-size:12px; color:blue;'>";
                    strTable += "<td style='padding:5px;width: 60%;'>Bộ câu hỏi </td>";
                    strTable += "<td style='padding:5px;width: 8%;'> Độ khó </td>";
                    strTable += "<td style='padding:5px;width: 8%;'>Số câu hỏi </td> ";
                    strTable += "<td style='padding:5px;width: 8%;'>Sửa</td> ";
                    strTable += "<td style='padding:5px;width: 8%;'>Xóa</td> ";
                    strTable += string.Format("<td style='padding:5px;width: 8%;'><a href='{0}/EditType/insertphan/bode_phanid/{1}'>Thêm mới</a></td> ", getUrlReturn(NguoiDung_MonHocID, BoDeID), BoDe_PhanID);
                    strTable += "</tr>";
                    for (int j = 0; j < iLenghthCauHinh; j++)
                    {
                        iSoCauHoiTungThanhPhan = 0;
                        string BoDe_Phan_CauHinhID = tblTableCauHinh.Rows[j]["BoDe_Phan_CauHinhID"].ToString();
                        strTable += "<tr>";
                        strTable += string.Format("<td style='padding:5px;'>{0}</td>", tblTableCauHinh.Rows[j]["TenBoCauHoi"]);
                        strTable += string.Format("<td style='padding:5px;'>{0}</td>", tblTableCauHinh.Rows[j]["TenDoKho"]);
                        iSoCauHoiTungThanhPhan = int.Parse(tblTableCauHinh.Rows[j]["SoCauHoi"].ToString());
                        iTongSoCauHoiTungPhan += iSoCauHoiTungThanhPhan;
                        strTable += string.Format("<td style='padding:5px;'>{0}</td>", iSoCauHoiTungThanhPhan);
                        strTable += string.Format("<td style='padding:5px;width: 8%;'><a href='{0}/EditType/updatephan/bode_phanid/{1}/bode_phan_cauhinhid/{2}'>Sửa</a></td> ", getUrlReturn(NguoiDung_MonHocID, BoDeID), BoDe_PhanID, BoDe_Phan_CauHinhID);
                        strTable += string.Format("<td style='padding:5px;width: 8%;'><a href='{0}/EditType/deletephan/bode_phanid/{1}/bode_phan_cauhinhid/{2}'>Xóa</a></td> ", getUrlReturn(NguoiDung_MonHocID, BoDeID), BoDe_PhanID, BoDe_Phan_CauHinhID);
                        strTable += string.Format("<td style='padding:5px;width: 8%;'><a href='{0}/EditType/insertphan/bode_phanid/{1}'>Thêm mới</a></td> ", getUrlReturn(NguoiDung_MonHocID, BoDeID), BoDe_PhanID);
                        strTable += "</tr>";
                    }
                    strTable += string.Format("<tr><td colspan='6' style='padding:5px;text-align:right;'>Tổng số câu hỏi trong phần là:<span style='color:red;'>{0}</span> </td></tr>", iTongSoCauHoiTungPhan);
                    strTable += "</table>";
                    strTable += "</div>";
                }
                else
                {
                    strTable += " <div style='margin-left:50px;padding-left: 50px;height:50px;' class='nd_b1 fl' >";
                    strTable += string.Format("<a href='{0}/EditType/insertphan/bode_phanid/{1}'>Thêm mới cấu hình câu hỏi trong phần</a>", getUrlReturn(NguoiDung_MonHocID, BoDeID), BoDe_PhanID);
                    strTable += "</div>";
                }
                iTongSoCauHoi += iTongSoCauHoiTungPhan;
            }
            strTable += string.Format("<div class='nd_t fl' style='height: 20px;text-align:right;font-weight:bold;'>Tổng số câu hỏi của cả bộ đề là: <span style='color:red;'>{0}</span></div>", iTongSoCauHoi);
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
            string strMoTa = txtMoTa.Text;
            int iOrder = int.Parse(txtOrder.Text.Trim());
            int iBoDeID = int.Parse(txtBoDeID.Text)
            ;

            if (strTen.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để tên trắng";
                return;
            }

            if (strType == "insert")
            {
                data.dnn_NuceThi_BoDe_Phan.insert(iBoDeID, strTen, strMoTa, iBoDeID, 1);
                returnMain(txtNguoiDung_MonHocID.Text, txtBoDeID.Text);
            }
            else
            {
                if (strType == "update")
                {
                    int iID = int.Parse(txtID.Text);
                    data.dnn_NuceThi_BoDe_Phan.update(iID, iBoDeID, strTen, strMoTa, iBoDeID, 1);
                    returnMain(txtNguoiDung_MonHocID.Text, txtBoDeID.Text);
                }
            }
        }
        protected void btnCapNhatBoDe_Phan_Click(object sender, EventArgs e)
        {
            string strType = txtType.Text;
            int iBoCauHoi = int.Parse(ddlBoCauHoi.SelectedValue);
            int iDoKho = int.Parse(ddlDoKho.SelectedValue);
            int iSoCauHoi = int.Parse(txtSoCauHoi.Text.Trim());
            int iBoDe_PhanID = int.Parse(txtBoDe_PhanID.Text)
            ;

            //Kiểm tra trước khi cập nhật
            //int iCount = data.dnn_NuceThi_CauHoi.countByBoCauHoiAndDoKho(iBoCauHoi, iDoKho,1);\
            int iCount = int.Parse(txtSoCauHoiToiDaDuocChon.Text);
            if (iCount == 0)
            {
                divAnnouce.InnerHtml = string.Format("Số câu hỏi chọn lớn hơn 0 ");
                return;
            }
            if (iCount < iSoCauHoi)
            {
                divAnnouce.InnerHtml = string.Format("Số câu hỏi không đủ (Bạn hãy chọn số câu hỏi nhỏ hơn {0})", iCount);
                return;
            }
            //Kiểm tra số lượng câu hỏi
            if (strType == "insertphan")
            {
                data.dnn_NuceThi_BoDe_Phan_CauHinh.insert(iBoDe_PhanID, iBoCauHoi, iDoKho, iSoCauHoi, 1);
                returnMain(txtNguoiDung_MonHocID.Text, txtBoDeID.Text);
            }
            else
            {
                if (strType == "updatephan")
                {
                    int iID = int.Parse(txtID.Text);
                    data.dnn_NuceThi_BoDe_Phan_CauHinh.update(iID, iBoDe_PhanID, iBoCauHoi, iDoKho, iSoCauHoi, 1);
                    returnMain(txtNguoiDung_MonHocID.Text, txtBoDeID.Text);
                }
            }
        }
        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            returnMain(txtNguoiDung_MonHocID.Text, txtBoDeID.Text);
        }

        protected void btnTaoDe_Click(object sender, EventArgs e)
        {
            DateTime dtStart = DateTime.Now;
            //Lay danh sach cau hinh
            int iBoDe = int.Parse(txtBoDeID.Text);
            DataTable tblTableBoDeInfo = data.dnn_NuceThi_BoDe.get(iBoDe);
            int iStatus = int.Parse(tblTableBoDeInfo.Rows[0]["Status"].ToString());
            if (iStatus > 2)
            {
                divAnnouce.InnerHtml = "Bộ đề đã được sử dụng nên không thể tạo lại, bạn hãy dùng chức năng nhân bản để tạo ra bộ đề có cấu hình tương tự.";
                return;
            }
            int iTypeBoDe= int.Parse(tblTableBoDeInfo.Rows[0]["Type"].ToString());
            int iSoDe = tblTableBoDeInfo.Rows[0].IsNull("SoDe") ? 100 : int.Parse(tblTableBoDeInfo.Rows[0]["SoDe"].ToString());
            DataTable tblTable = data.dnn_NuceThi_BoDe_Phan.getByBoDe(iBoDe);
            float fDiemToiDa = 0;
            if(iTypeBoDe.Equals(1))
            { 
            #region voi truong hop binh thuong type=1
            for (int i = 0; i < tblTable.Rows.Count; i++)
            {
                string BoDe_PhanID = tblTable.Rows[i]["BoDe_PhanID"].ToString();
                DataTable tblTableCauHinh = data.dnn_NuceThi_BoDe_Phan_CauHinh.getByBoDe_Phan(int.Parse(BoDe_PhanID));
                if (tblTableCauHinh.Rows.Count > 0)
                {
                    //Chen du lieu
                    #region chen du lieu
                    for (int j = 0; j < tblTableCauHinh.Rows.Count; j++)
                    {
                        string BoDe_Phan_CauHinhID = tblTableCauHinh.Rows[j]["BoDe_Phan_CauHinhID"].ToString();
                        int BoCauHoiID = int.Parse(tblTableCauHinh.Rows[j]["BoCauHoiID"].ToString());
                        int SoCauHoi = int.Parse(tblTableCauHinh.Rows[j]["SoCauHoi"].ToString());
                        int DoKhoID = int.Parse(tblTableCauHinh.Rows[j]["DoKhoID"].ToString());
                        // Xu ly lay ra iSoDe voi SoCauHoi chuoi json tuong ung voi cac cau hoi
                        // Lay du lieu cac cau hoi 
                        // lay dap an cac cau hoi

                        DataTable dtCauHois = GetDataFromCauHois(BoCauHoiID, DoKhoID);
                        DataRow[] drCauHois = dtCauHois.Select(string.Format("Level={0}", 1));
                        int iRCount = drCauHois.Length;
                        if (SoCauHoi > iRCount)
                        {
                            divAnnouce.InnerHtml = "Không đủ câu hỏi để tạo đề ";
                            return;
                        }
                        DataTable dtDapAns = GetDataFromDapAns(BoCauHoiID, DoKhoID);

                        int k = 0;
                        while (k < iSoDe)
                        {
                            string strKey_temp = string.Format("{0}_{1}", BoDe_Phan_CauHinhID, k);
                            string strKey_temp1 = string.Format("{0}_{1}_{2}", BoCauHoiID, DoKhoID, k);
                            List<int> lRand = new List<int>();
                            if (m_iCauHoiPhans.ContainsKey(strKey_temp1))
                            {
                                List<int> lSITemps = m_iCauHoiPhans[strKey_temp1];
                                if (SoCauHoi > iRCount - lSITemps.Count)
                                {
                                    divAnnouce.InnerHtml = "Không đủ câu hỏi để tạo đề ";
                                    return;
                                }
                                lRand = GetRandom(iRCount, SoCauHoi, lSITemps);
                                lSITemps.AddRange(lRand);
                                m_iCauHoiPhans[strKey_temp1] = lSITemps;
                            }
                            else
                            {
                                lRand = GetRandom(iRCount, SoCauHoi);
                                m_iCauHoiPhans[strKey_temp1] = lRand;
                            }
                            // Danh dau de xoa
                            //List<DataRow> lDRTemps = new List<DataRow>();
                            List<model.CauHoi> lCauHoiTemp = new List<model.CauHoi>();
                            List<model.DapAn> lDapAnTemp = new List<model.DapAn>();
                            #region 1 Row
                            #region 1 Row new
                            model.CauHoi CauHoiTemp = new model.CauHoi();
                            model.DapAn DapAnTemp = new model.DapAn();
                            DataRow[] drTemps;

                            List<model.CauHoi> ChildCauHois = new List<model.CauHoi>();
                            model.CauHoi CauHoiTemp1 = new model.CauHoi();
                            model.DapAn DapAnTemp1 = new model.DapAn();

                            foreach (int iR in lRand)
                            {
                                CauHoiTemp = getRowCauHoi(drCauHois[iR], dtDapAns,1, out DapAnTemp);
                                if (!DapAnTemp.Equals(""))
                                {
                                    lDapAnTemp.Add(DapAnTemp);
                                }
                                drTemps = dtCauHois.Select(string.Format("ParentQuestionId={0}", CauHoiTemp.CauHoiID));
                                if (drTemps != null && drTemps.Length > 0)
                                {
                                    //ChildCauHois.Clear();
                                    ChildCauHois = new List<model.CauHoi>();
                                    for (int iR1 = 0; iR1 < drTemps.Length; iR1++)
                                    {
                                        CauHoiTemp1 = getRowCauHoi(drTemps[iR1], dtDapAns,1, out DapAnTemp1);
                                        ChildCauHois.Add(CauHoiTemp1);
                                        if (!DapAnTemp1.Equals(""))
                                        {
                                            lDapAnTemp.Add(DapAnTemp1);
                                        }
                                    }
                                    CauHoiTemp.ChildCauHois = ChildCauHois;
                                }
                                // Them moi vao list
                                lCauHoiTemp.Add(CauHoiTemp);
                            }
                            #endregion
                            #region comment
                            /*
                                                        foreach (int iR in lRand)
                                                        {
                                                            //lDRTemps.Add(dtCauHois.Rows[iR]);
                                                            model.CauHoi CauHoiTemp = new model.CauHoi();
                                                            model.DapAn DapAnTemp = new model.DapAn();
                                                            int iCauHoiTemp = int.Parse(dtCauHois.Rows[iR]["CauHoiID"].ToString());
                                                            CauHoiTemp.CauHoiID = iCauHoiTemp;
                                                            CauHoiTemp.Content = dtCauHois.Rows[iR]["Content"].ToString();
                                                            CauHoiTemp.DoKhoID = int.Parse(dtCauHois.Rows[iR]["DoKhoID"].ToString());
                                                            CauHoiTemp.Explain = dtCauHois.Rows[iR]["Explain"].ToString();
                                                            CauHoiTemp.Image = dtCauHois.Rows[iR]["Image"].ToString();
                                                            int iTypeCauHoiTemp = int.Parse(dtCauHois.Rows[iR]["Type"].ToString());
                                                            string strTypeCauHoiTemp = GetLoaiCauHoi(iTypeCauHoiTemp).Name;
                                                            CauHoiTemp.Type = strTypeCauHoiTemp;

                                                            int iDoKhoIDTemp = int.Parse(dtCauHois.Rows[iR]["DoKhoID"].ToString());

                                                            DataRow[] drTemps = GetDapAnFromCauHoi(iCauHoiTemp, dtDapAns);
                                                            int iLengthDrTemp = drTemps.Length;
                                                            // Xu ly du lieu dua vao
                                                            if (iLengthDrTemp <= 0)
                                                            {
                                                                divAnnouce.InnerHtml = string.Format("Câu hỏi có {0} không có đáp án", iCauHoiTemp);
                                                                return;
                                                            }

                                                            CauHoiTemp.SoCauTraLoi = iLengthDrTemp;
                                                            string strMatchTemp = "";
                                                            switch (strTypeCauHoiTemp)
                                                            {
                                                                case "SC":
                                                                case "MC":
                                                                case "TQ":
                                                                case "FQ":
                                                                    int l = 1;
                                                                    foreach (DataRow drTemp in drTemps)
                                                                    {
                                                                        bool blIsCheck1 = bool.Parse(drTemp["IsCheck"].ToString());
                                                                        int iDapAnID1 = int.Parse(drTemp["DapAnID"].ToString());
                                                                        if (blIsCheck1)
                                                                        {
                                                                            strMatchTemp = strMatchTemp + ";" + iDapAnID1.ToString();
                                                                        }
                                                                        switch (l)
                                                                        {
                                                                            case 1:
                                                                                CauHoiTemp.A1 = drTemp["Content"].ToString();
                                                                                CauHoiTemp.M1 = iDapAnID1.ToString();
                                                                                break;
                                                                            case 2:
                                                                                CauHoiTemp.A2 = drTemp["Content"].ToString();
                                                                                CauHoiTemp.M2 = iDapAnID1.ToString();
                                                                                break;
                                                                            case 3:
                                                                                CauHoiTemp.A3 = drTemp["Content"].ToString();
                                                                                CauHoiTemp.M3 = iDapAnID1.ToString();
                                                                                break;
                                                                            case 4:
                                                                                CauHoiTemp.A4 = drTemp["Content"].ToString();
                                                                                CauHoiTemp.M4 = iDapAnID1.ToString();
                                                                                break;
                                                                            case 5:
                                                                                CauHoiTemp.A5 = drTemp["Content"].ToString();
                                                                                CauHoiTemp.M5 = iDapAnID1.ToString();
                                                                                break;
                                                                            case 6:
                                                                                CauHoiTemp.A6 = drTemp["Content"].ToString();
                                                                                CauHoiTemp.M6 = iDapAnID1.ToString();
                                                                                break;
                                                                            case 7:
                                                                                CauHoiTemp.A7 = drTemp["Content"].ToString();
                                                                                CauHoiTemp.M7 = iDapAnID1.ToString();
                                                                                break;
                                                                            case 8:
                                                                                CauHoiTemp.A8 = drTemp["Content"].ToString();
                                                                                CauHoiTemp.M8 = iDapAnID1.ToString();
                                                                                break;
                                                                            case 9:
                                                                                CauHoiTemp.A9 = drTemp["Content"].ToString();
                                                                                CauHoiTemp.M9 = iDapAnID1.ToString();
                                                                                break;
                                                                            case 10:
                                                                                CauHoiTemp.A10 = drTemp["Content"].ToString();
                                                                                CauHoiTemp.M10 = iDapAnID1.ToString();
                                                                                break;
                                                                            case 11:
                                                                                CauHoiTemp.A11 = drTemp["Content"].ToString();
                                                                                CauHoiTemp.M11 = iDapAnID1.ToString();
                                                                                break;
                                                                            case 12:
                                                                                CauHoiTemp.A12 = drTemp["Content"].ToString();
                                                                                CauHoiTemp.M12 = iDapAnID1.ToString();
                                                                                break;
                                                                            case 13:
                                                                                CauHoiTemp.A13 = drTemp["Content"].ToString();
                                                                                CauHoiTemp.M13 = iDapAnID1.ToString();
                                                                                break;
                                                                            case 14:
                                                                                CauHoiTemp.A14 = drTemp["Content"].ToString();
                                                                                CauHoiTemp.M14 = iDapAnID1.ToString();
                                                                                break;
                                                                            case 15:
                                                                                CauHoiTemp.A15 = drTemp["Content"].ToString();
                                                                                CauHoiTemp.M15 = iDapAnID1.ToString();
                                                                                break;
                                                                            default: break;
                                                                        }
                                                                        l++;
                                                                    }
                                                                    break;
                                                                case "MA":
                                                                    int[] lITemp = GenRandomPermutation(iLengthDrTemp);
                                                                    foreach (int l2 in lITemp)
                                                                    {
                                                                        int l3 = l2;
                                                                        int l4 = l2 + 1;
                                                                        switch (l4)
                                                                        {
                                                                            case 1:
                                                                                CauHoiTemp.D1 = drTemps[l3]["Matching"].ToString();
                                                                                CauHoiTemp.M1 = drTemps[l3]["Matched"].ToString();
                                                                                break;
                                                                            case 2:
                                                                                CauHoiTemp.D2 = drTemps[l3]["Matching"].ToString();
                                                                                CauHoiTemp.M2 = drTemps[l3]["Matched"].ToString();
                                                                                break;
                                                                            case 3:
                                                                                CauHoiTemp.D3 = drTemps[l3]["Matching"].ToString();
                                                                                CauHoiTemp.M3 = drTemps[l3]["Matched"].ToString();
                                                                                break;
                                                                            case 4:
                                                                                CauHoiTemp.D4 = drTemps[l3]["Matching"].ToString();
                                                                                CauHoiTemp.M4 = drTemps[l3]["Matched"].ToString();
                                                                                break;
                                                                            case 5:
                                                                                CauHoiTemp.D5 = drTemps[l3]["Matching"].ToString();
                                                                                CauHoiTemp.M5 = drTemps[l3]["Matched"].ToString();
                                                                                break;
                                                                            case 6:
                                                                                CauHoiTemp.D6 = drTemps[l3]["Matching"].ToString();
                                                                                CauHoiTemp.M6 = drTemps[l3]["Matched"].ToString();
                                                                                break;
                                                                            case 7:
                                                                                CauHoiTemp.D7 = drTemps[l3]["Matching"].ToString();
                                                                                CauHoiTemp.M7 = drTemps[l3]["Matched"].ToString();
                                                                                break;
                                                                            case 8:
                                                                                CauHoiTemp.D8 = drTemps[l3]["Matching"].ToString();
                                                                                CauHoiTemp.M8 = drTemps[l3]["Matched"].ToString();
                                                                                break;
                                                                            case 9:
                                                                                CauHoiTemp.D9 = drTemps[l3]["Matching"].ToString();
                                                                                CauHoiTemp.M9 = drTemps[l3]["Matched"].ToString();
                                                                                break;
                                                                            case 10:
                                                                                CauHoiTemp.D10 = drTemps[l3]["Matching"].ToString();
                                                                                CauHoiTemp.M10 = drTemps[l3]["Matched"].ToString();
                                                                                break;
                                                                            case 11:
                                                                                CauHoiTemp.D11 = drTemps[l3]["Matching"].ToString();
                                                                                CauHoiTemp.M11 = drTemps[l3]["Matched"].ToString();
                                                                                break;
                                                                            case 12:
                                                                                CauHoiTemp.D12 = drTemps[l3]["Matching"].ToString();
                                                                                CauHoiTemp.M12 = drTemps[l3]["Matched"].ToString();
                                                                                break;
                                                                            case 13:
                                                                                CauHoiTemp.D13 = drTemps[l3]["Matching"].ToString();
                                                                                CauHoiTemp.M13 = drTemps[l3]["Matched"].ToString();
                                                                                break;
                                                                            case 14:
                                                                                CauHoiTemp.D14 = drTemps[l3]["Matching"].ToString();
                                                                                CauHoiTemp.M14 = drTemps[l3]["Matched"].ToString();
                                                                                break;
                                                                            case 15:
                                                                                CauHoiTemp.D15 = drTemps[l3]["Matching"].ToString();
                                                                                CauHoiTemp.M15 = drTemps[l3]["Matched"].ToString();
                                                                                break;
                                                                            default: break;
                                                                        }
                                                                    }
                                                                    int l1 = 1;
                                                                    foreach (DataRow drTemp in drTemps)
                                                                    {
                                                                        strMatchTemp = strMatchTemp + ";" + drTemp["Matched"].ToString();
                                                                        switch (l1)
                                                                        {
                                                                            case 1:
                                                                                CauHoiTemp.A1 = drTemp["Content"].ToString();
                                                                                break;
                                                                            case 2:
                                                                                CauHoiTemp.A2 = drTemp["Content"].ToString();
                                                                                break;
                                                                            case 3:
                                                                                CauHoiTemp.A3 = drTemp["Content"].ToString();
                                                                                break;
                                                                            case 4:
                                                                                CauHoiTemp.A4 = drTemp["Content"].ToString();
                                                                                break;
                                                                            case 5:
                                                                                CauHoiTemp.A5 = drTemp["Content"].ToString();
                                                                                break;
                                                                            case 6:
                                                                                CauHoiTemp.A6 = drTemp["Content"].ToString();
                                                                                break;
                                                                            case 7:
                                                                                CauHoiTemp.A7 = drTemp["Content"].ToString();
                                                                                break;
                                                                            case 8:
                                                                                CauHoiTemp.A8 = drTemp["Content"].ToString();
                                                                                break;
                                                                            case 9:
                                                                                CauHoiTemp.A9 = drTemp["Content"].ToString();
                                                                                break;
                                                                            case 10:
                                                                                CauHoiTemp.A10 = drTemp["Content"].ToString();
                                                                                break;
                                                                            case 11:
                                                                                CauHoiTemp.A11 = drTemp["Content"].ToString();
                                                                                break;
                                                                            case 12:
                                                                                CauHoiTemp.A12 = drTemp["Content"].ToString();
                                                                                break;
                                                                            case 13:
                                                                                CauHoiTemp.A13 = drTemp["Content"].ToString();
                                                                                break;
                                                                            case 14:
                                                                                CauHoiTemp.A14 = drTemp["Content"].ToString();
                                                                                break;
                                                                            case 15:
                                                                                CauHoiTemp.A15 = drTemp["Content"].ToString();
                                                                                break;
                                                                            default: break;
                                                                        }
                                                                        l1++;
                                                                    }
                                                                    break;
                                                                default: break;
                                                            }

                                                            DapAnTemp.CauHoiID = iCauHoiTemp;
                                                            DapAnTemp.Type = strTypeCauHoiTemp;
                                                            float fDiemTemp = dtCauHois.Rows[iR].IsNull("Diem") ? 1 : int.Parse(dtCauHois.Rows[iR]["Diem"].ToString());
                                                            CauHoiTemp.Mark = fDiemTemp;
                                                            DapAnTemp.Mark = fDiemTemp;
                                                            DapAnTemp.Match = strMatchTemp;
                                                            // Them moi vao list
                                                            lCauHoiTemp.Add(CauHoiTemp);
                                                            lDapAnTemp.Add(DapAnTemp);
                                                        }
                                                        */
                            #endregion
                            #endregion

                            m_dcCauHoiCauHinhs[strKey_temp] = lCauHoiTemp;
                            m_dcDapAnCauHinhs[strKey_temp] = lDapAnTemp;
                            k++;
                        }
                    }
                    #endregion
                    #region Sinh phan de
                    int k1 = 0;
                    while (k1 < iSoDe)
                    {
                        List<model.CauHoi> lCauHoiTemp1 = new List<model.CauHoi>();
                        List<model.DapAn> lDapAnTemp1 = new List<model.DapAn>();
                        List<model.CauHoi> lCauHoiTemp2 = new List<model.CauHoi>();
                        //List<model.DapAn> lDapAnTemp2 = new List<model.DapAn>();
                        for (int j = 0; j < tblTableCauHinh.Rows.Count; j++)
                        {
                            string BoDe_Phan_CauHinhID = tblTableCauHinh.Rows[j]["BoDe_Phan_CauHinhID"].ToString();
                            string strKey_temp = string.Format("{0}_{1}", BoDe_Phan_CauHinhID, k1);
                            List<model.CauHoi> lCauHoiTemp = new List<model.CauHoi>();
                            lCauHoiTemp = m_dcCauHoiCauHinhs[strKey_temp];
                            lCauHoiTemp1.AddRange(lCauHoiTemp);
                            List<model.DapAn> lDapAnTemp = new List<model.DapAn>();
                            lDapAnTemp = m_dcDapAnCauHinhs[strKey_temp];
                            lDapAnTemp1.AddRange(lDapAnTemp);
                        }
                        int iCountTemp = lCauHoiTemp1.Count;
                        int[] lRand = GenRandomPermutation(iCountTemp);
                        foreach (int iR in lRand)
                        {
                            lCauHoiTemp2.Add(lCauHoiTemp1[iR]);
                            //lDapAnTemp2.Add(lDapAnTemp1[iR]);
                        }
                        string strKey_temp1 = string.Format("{0}_{1}", BoDe_PhanID, k1);
                        m_dcCauHoiPhans[strKey_temp1] = lCauHoiTemp2;
                        m_dcDapAnPhans[strKey_temp1] = lDapAnTemp1;
                        k1++;
                    }
                    #endregion
                }
            }
            if (m_dcCauHoiPhans.Count < 1)
            {
                divAnnouce.InnerHtml = string.Format("Chưa định nghĩa bộ đề nên chưa tạo được đề !!!");
                return;
            }

            #region Sinh de
            int k2 = 0;
            while (k2 < iSoDe)
            {
                List<model.CauHoi> lCauHoiTemp3 = new List<model.CauHoi>();
                List<model.DapAn> lDapAnTemp3 = new List<model.DapAn>();
                for (int i = 0; i < tblTable.Rows.Count; i++)
                {
                    string BoDe_PhanID = tblTable.Rows[i]["BoDe_PhanID"].ToString();
                    string strKey_temp2 = string.Format("{0}_{1}", BoDe_PhanID, k2);
                    List<model.CauHoi> lCauHoiTemp = new List<model.CauHoi>();
                    lCauHoiTemp = m_dcCauHoiPhans[strKey_temp2];
                    lCauHoiTemp3.AddRange(lCauHoiTemp);
                    List<model.DapAn> lDapAnTemp = new List<model.DapAn>();
                    lDapAnTemp = m_dcDapAnPhans[strKey_temp2];
                    lDapAnTemp3.AddRange(lDapAnTemp);
                }
                string strKey_temp1 = string.Format("{0}_{1}", "deso", k2);
                m_dcCauHoiPhans[strKey_temp1] = lCauHoiTemp3;
                m_dcDapAnPhans[strKey_temp1] = lDapAnTemp3;
                k2++;
            }
            List<model.CauHoi> lCauHoiTemp4 = new List<model.CauHoi>();
            lCauHoiTemp4 = m_dcCauHoiPhans["deso_0"];
            foreach (model.CauHoi cauHoiTemp in lCauHoiTemp4)
            {
                fDiemToiDa += cauHoiTemp.Mark;
            }
            #endregion
            #region Cap nhat vao co so du lieu
            //Kiem tra xem neu trang thai =2 thi se xoa bo de cu de chen bo de moi vao
            if (iStatus.Equals(2))
            {
                data.dnn_NuceThi_DeThi.deleteByBoDe(iBoDe);
            }
            for (int i = 0; i < iSoDe; i++)
            {
                string strKey_temp1 = string.Format("{0}_{1}", "deso", i);
                List<model.CauHoi> lCauHoiTemp3 = new List<model.CauHoi>();
                List<model.DapAn> lDapAnTemp3 = new List<model.DapAn>();
                lCauHoiTemp3 = m_dcCauHoiPhans[strKey_temp1];
                lDapAnTemp3 = m_dcDapAnPhans[strKey_temp1];
                string strMa = string.Format("Bode_{0}_de_{1}", iBoDe, (i + 1));
                string strNoiDung = JsonConvert.SerializeObject(lCauHoiTemp3);
                string strDapAn = JsonConvert.SerializeObject(lDapAnTemp3);
                //Cap nhat vao co so du lieu
                data.dnn_NuceThi_DeThi.insert(iBoDe, strMa, strNoiDung, strDapAn, 1);
            }
            if (!iStatus.Equals(2))
            {
                data.dnn_NuceThi_BoDe.updateStatus(iBoDe, 2);
            }
            data.dnn_NuceThi_BoDe.update_diemtoida(iBoDe, fDiemToiDa);
                #endregion
                #endregion
            }
            else
            {
                if (iTypeBoDe.Equals(2))
                {
                    #region voi truong hop cho khao sat type=2
                    // lấy các câu hỏi o cac phan
                    // ghep vao list
                    // generate 
                    List<model.CauHoi> lCauHoiTemp = new List<model.CauHoi>();
                    List<model.DapAn> lDapAnTemp = new List<model.DapAn>();
                    for (int i = 0; i < tblTable.Rows.Count; i++)
                    {
                        string BoDe_PhanID = tblTable.Rows[i]["BoDe_PhanID"].ToString();
                        DataTable tblTableCauHinh = data.dnn_NuceThi_BoDe_Phan_CauHinh.getByBoDe_Phan(int.Parse(BoDe_PhanID));
                        if (tblTableCauHinh.Rows.Count > 0)
                        {
                            for (int j = 0; j < tblTableCauHinh.Rows.Count; j++)
                            {
                                string BoDe_Phan_CauHinhID = tblTableCauHinh.Rows[j]["BoDe_Phan_CauHinhID"].ToString();
                                int BoCauHoiID = int.Parse(tblTableCauHinh.Rows[j]["BoCauHoiID"].ToString());
                                int SoCauHoi = int.Parse(tblTableCauHinh.Rows[j]["SoCauHoi"].ToString());
                                int DoKhoID = int.Parse(tblTableCauHinh.Rows[j]["DoKhoID"].ToString());
                                // Xu ly lay ra iSoDe voi SoCauHoi chuoi json tuong ung voi cac cau hoi
                                // Lay du lieu cac cau hoi 
                                // lay dap an cac cau hoi

                                DataTable dtCauHois = GetDataFromCauHois(BoCauHoiID, DoKhoID);
                                DataRow[] drCauHois = dtCauHois.Select(string.Format("Level={0}", 1));
                                int iRCount = drCauHois.Length;
                                if (SoCauHoi > iRCount)
                                {
                                    divAnnouce.InnerHtml = "Không đủ câu hỏi để tạo đề ";
                                    return;
                                }
                                DataTable dtDapAns = GetDataFromDapAns(BoCauHoiID, DoKhoID);
                                model.CauHoi CauHoiTemp = new model.CauHoi();
                                model.DapAn DapAnTemp = new model.DapAn();
                                DataRow[] drTemps;

                                List<model.CauHoi> ChildCauHois = new List<model.CauHoi>();
                                model.CauHoi CauHoiTemp1 = new model.CauHoi();
                                model.DapAn DapAnTemp1 = new model.DapAn();

                                for (int iR = 0; iR < SoCauHoi; iR++)
                                {
                                    CauHoiTemp = getRowCauHoi(drCauHois[iR], dtDapAns,2, out DapAnTemp);
                                    if (!DapAnTemp.Equals(""))
                                    {
                                        lDapAnTemp.Add(DapAnTemp);
                                    }
                                    drTemps = dtCauHois.Select(string.Format("ParentQuestionId={0}", CauHoiTemp.CauHoiID));
                                    if (drTemps != null && drTemps.Length > 0)
                                    {
                                        //ChildCauHois.Clear();
                                        ChildCauHois = new List<model.CauHoi>();
                                        for (int iR1 = 0; iR1 < drTemps.Length; iR1++)
                                        {
                                            CauHoiTemp1 = getRowCauHoi(drTemps[iR1], dtDapAns,2, out DapAnTemp1);
                                            ChildCauHois.Add(CauHoiTemp1);
                                            if (!DapAnTemp1.Equals(""))
                                            {
                                                lDapAnTemp.Add(DapAnTemp1);
                                            }
                                        }
                                        CauHoiTemp.ChildCauHois = ChildCauHois;
                                    }
                                    // Them moi vao list
                                    lCauHoiTemp.Add(CauHoiTemp);
                                }


                            }
                        }
                    }

                    // Sau khi xong thi da duoc danh sach cau hoi
                    #region Cap nhat vao co so du lieu
                    //Kiem tra xem neu trang thai =2 thi se xoa bo de cu de chen bo de moi vao
                    if (iStatus.Equals(2))
                    {
                        data.dnn_NuceThi_DeThi.deleteByBoDe(iBoDe);
                    }

                        string strNoiDung = JsonConvert.SerializeObject(lCauHoiTemp);
                        string strDapAn = JsonConvert.SerializeObject(lDapAnTemp);
                    //Cap nhat vao co so du lieu
                    string strMa = string.Format("Bode_{0}_de_{1}", iBoDe, 1);
                    data.dnn_NuceThi_DeThi.insert(iBoDe, strMa, strNoiDung, strDapAn, 1);
                    if (!iStatus.Equals(2))
                    {
                        data.dnn_NuceThi_BoDe.updateStatus(iBoDe, 2);
                    }
                    //data.dnn_NuceThi_BoDe.update_diemtoida(iBoDe, fDiemToiDa);
                    #endregion

                    #endregion
                }
            }
            DateTime dtEnd = DateTime.Now;
            TimeSpan ts = dtEnd - dtStart;
            divAnnouce.InnerHtml = string.Format("Tạo đề thành công trong thời gian {0} phút - {1} giây", ts.Minutes, ts.Seconds);
        }
        #region processDataCauHois
        public DataTable GetDataFromCauHois(int BoCauHoiID, int DoKhoID)
        {
            string Key = string.Format("{0}_{1}", BoCauHoiID, DoKhoID);
            if (m_dcCauHois.ContainsKey(Key))
                return m_dcCauHois[Key];
            else
            {
                DataTable dtReturn = data.dnn_NuceThi_CauHoi.getByBoCauHoiAndDoKho(BoCauHoiID, DoKhoID);
                m_dcCauHois.Add(Key, dtReturn);
                return dtReturn;
            }
        }
        public DataTable GetDataFromDapAns(int BoCauHoiID, int DoKhoID)
        {
            string Key = string.Format("{0}_{1}", BoCauHoiID, DoKhoID);

            if (m_dcDapAns.ContainsKey(Key))
                return m_dcDapAns[Key];
            else
            {
                DataTable dtReturn = data.dnn_NuceThi_DapAn.getByBoCauHoiAndDoKho(BoCauHoiID, DoKhoID);
                m_dcDapAns.Add(Key, dtReturn);
                return dtReturn;
            }
        }
        public DataRow[] GetDapAnFromCauHoi(int CauHoiID, DataTable Dt)
        {
            DataRow[] drTemp = Dt.Select(string.Format("CauHoiID={0}", CauHoiID));
            DataRow[] drReturn = new DataRow[drTemp.Length];
            int[] arrRd = GenRandomPermutation(drTemp.Length);
            for (int i = 0; i < arrRd.Length; i++)
            {
                drReturn[i] = drTemp[arrRd[i]];
            }
            return drReturn;
        }
        public DataRow[] GetDapAnFromCauHoiNotRandom(int CauHoiID, DataTable Dt)
        {
            DataRow[] drTemp = Dt.Select(string.Format("CauHoiID={0}", CauHoiID), "Order asc");
            return drTemp;
        }
        public List<int> GetRandom(int max, int count, List<int> lSExcepts)
        {
            List<model.RandomInt> lM = new List<model.RandomInt>();
            List<int> lR = new List<int>();
            for (int j = 0; j < max; j++)
            {
                if (!lSExcepts.Contains(j))
                {
                    lM.Add(new model.RandomInt() { Key = Guid.NewGuid(), Value = j });
                }
            }
            //lM.Sort(delegate (model.RandomInt x, model.RandomInt y)
            //{
            //    if (x.Key == null && y.Key == null) return 0;
            //    else if (x.Key == null) return -1;
            //    else if (y.Key == null) return 1;
            //    else return x.Key.CompareTo(y.Key);
            //});
            lR = lM.OrderBy(x => x.Key).Take(count).Select(x => x.Value).ToList();
            return lR;
        }
        public List<int> GetRandom(int max, int count)
        {
            List<model.RandomInt> lM = new List<model.RandomInt>();
            List<int> lR = new List<int>();
            for (int j = 0; j < max; j++)
            {
                lM.Add(new model.RandomInt() { Key = Guid.NewGuid(), Value = j });
            }
            //lM.Sort(delegate (model.RandomInt x, model.RandomInt y)
            //{
            //    if (x.Key == null && y.Key == null) return 0;
            //    else if (x.Key == null) return -1;
            //    else if (y.Key == null) return 1;
            //    else return x.Key.CompareTo(y.Key);
            //});
            lR = lM.OrderBy(x => x.Key).Take(count).Select(x => x.Value).ToList();
            return lR;
        }
        public int[] GenRandomPermutation(int n)
        {
            Random r = new Random();
            int[] a = new int[n];
            for (int i = 0; i < n; i++)
            {
                a[i] = i;
            }
            for (int i = 0; i < n; i++)
            {
                int j = r.Next(n);
                int t = a[0];
                a[0] = a[j];
                a[j] = t;
            }
            return a;
        }
        #endregion
        protected void btnNhanBan_Click(object sender, EventArgs e)
        {
            int iBoDe = int.Parse(txtBoDeID.Text);
            data.dnn_NuceThi_BoDe.nhanban(iBoDe);
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?NguoiDung_MonHocid={1}", this.TabId, txtNguoiDung_MonHocID.Text));
        }
        protected void btnQuayLaiDanhSachBoDe_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?NguoiDung_MonHocid={1}", this.TabId, txtNguoiDung_MonHocID.Text));
        }
        model.CauHoi getRowCauHoi(DataRow dr, DataTable dtDapAns,int type, out model.DapAn DapAnTemp)
        {
            // type=1 random
            //type=2 khong random
            model.CauHoi CauHoiTemp = new model.CauHoi();
            DapAnTemp = new model.DapAn();
            int iCauHoiTemp = int.Parse(dr["CauHoiID"].ToString());
            CauHoiTemp.CauHoiID = iCauHoiTemp;
            CauHoiTemp.Content = dr["Content"].ToString();
            CauHoiTemp.DoKhoID = int.Parse(dr["DoKhoID"].ToString());
            CauHoiTemp.Explain = dr["Explain"].ToString();
            CauHoiTemp.Image = dr["Image"].ToString();
            int iTypeCauHoiTemp = int.Parse(dr["Type"].ToString());
            string strTypeCauHoiTemp = GetLoaiCauHoi(iTypeCauHoiTemp).Name;
            CauHoiTemp.Type = strTypeCauHoiTemp;

            int iDoKhoIDTemp = int.Parse(dr["DoKhoID"].ToString());
            DataRow[] drTemps;
            if(type.Equals(1))
                drTemps = GetDapAnFromCauHoi(iCauHoiTemp, dtDapAns);
            else
                drTemps = GetDapAnFromCauHoiNotRandom(iCauHoiTemp, dtDapAns);
            int iLengthDrTemp = drTemps.Length;
            CauHoiTemp.SoCauTraLoi = iLengthDrTemp;
            string strMatchTemp = "";
            if (iLengthDrTemp > 0)
            {
                switch (strTypeCauHoiTemp)
                {
                    case "SC":
                    case "MC":
                    case "TQ":
                    case "FQ":
                        int l = 1;
                        foreach (DataRow drTemp in drTemps)
                        {
                            bool blIsCheck1 = bool.Parse(drTemp["IsCheck"].ToString());
                            int iDapAnID1 = int.Parse(drTemp["DapAnID"].ToString());
                            if (blIsCheck1)
                            {
                                strMatchTemp = strMatchTemp + ";" + iDapAnID1.ToString();
                            }
                            switch (l)
                            {
                                case 1:
                                    CauHoiTemp.A1 = drTemp["Content"].ToString();
                                    CauHoiTemp.M1 = iDapAnID1.ToString();
                                    break;
                                case 2:
                                    CauHoiTemp.A2 = drTemp["Content"].ToString();
                                    CauHoiTemp.M2 = iDapAnID1.ToString();
                                    break;
                                case 3:
                                    CauHoiTemp.A3 = drTemp["Content"].ToString();
                                    CauHoiTemp.M3 = iDapAnID1.ToString();
                                    break;
                                case 4:
                                    CauHoiTemp.A4 = drTemp["Content"].ToString();
                                    CauHoiTemp.M4 = iDapAnID1.ToString();
                                    break;
                                case 5:
                                    CauHoiTemp.A5 = drTemp["Content"].ToString();
                                    CauHoiTemp.M5 = iDapAnID1.ToString();
                                    break;
                                case 6:
                                    CauHoiTemp.A6 = drTemp["Content"].ToString();
                                    CauHoiTemp.M6 = iDapAnID1.ToString();
                                    break;
                                case 7:
                                    CauHoiTemp.A7 = drTemp["Content"].ToString();
                                    CauHoiTemp.M7 = iDapAnID1.ToString();
                                    break;
                                case 8:
                                    CauHoiTemp.A8 = drTemp["Content"].ToString();
                                    CauHoiTemp.M8 = iDapAnID1.ToString();
                                    break;
                                case 9:
                                    CauHoiTemp.A9 = drTemp["Content"].ToString();
                                    CauHoiTemp.M9 = iDapAnID1.ToString();
                                    break;
                                case 10:
                                    CauHoiTemp.A10 = drTemp["Content"].ToString();
                                    CauHoiTemp.M10 = iDapAnID1.ToString();
                                    break;
                                case 11:
                                    CauHoiTemp.A11 = drTemp["Content"].ToString();
                                    CauHoiTemp.M11 = iDapAnID1.ToString();
                                    break;
                                case 12:
                                    CauHoiTemp.A12 = drTemp["Content"].ToString();
                                    CauHoiTemp.M12 = iDapAnID1.ToString();
                                    break;
                                case 13:
                                    CauHoiTemp.A13 = drTemp["Content"].ToString();
                                    CauHoiTemp.M13 = iDapAnID1.ToString();
                                    break;
                                case 14:
                                    CauHoiTemp.A14 = drTemp["Content"].ToString();
                                    CauHoiTemp.M14 = iDapAnID1.ToString();
                                    break;
                                case 15:
                                    CauHoiTemp.A15 = drTemp["Content"].ToString();
                                    CauHoiTemp.M15 = iDapAnID1.ToString();
                                    break;
                                default: break;
                            }
                            l++;
                        }
                        break;
                    case "MA":
                        int[] lITemp = GenRandomPermutation(iLengthDrTemp);
                        foreach (int l2 in lITemp)
                        {
                            int l3 = l2;
                            int l4 = l2 + 1;
                            switch (l4)
                            {
                                case 1:
                                    CauHoiTemp.D1 = drTemps[l3]["Matching"].ToString();
                                    CauHoiTemp.M1 = drTemps[l3]["Matched"].ToString();
                                    break;
                                case 2:
                                    CauHoiTemp.D2 = drTemps[l3]["Matching"].ToString();
                                    CauHoiTemp.M2 = drTemps[l3]["Matched"].ToString();
                                    break;
                                case 3:
                                    CauHoiTemp.D3 = drTemps[l3]["Matching"].ToString();
                                    CauHoiTemp.M3 = drTemps[l3]["Matched"].ToString();
                                    break;
                                case 4:
                                    CauHoiTemp.D4 = drTemps[l3]["Matching"].ToString();
                                    CauHoiTemp.M4 = drTemps[l3]["Matched"].ToString();
                                    break;
                                case 5:
                                    CauHoiTemp.D5 = drTemps[l3]["Matching"].ToString();
                                    CauHoiTemp.M5 = drTemps[l3]["Matched"].ToString();
                                    break;
                                case 6:
                                    CauHoiTemp.D6 = drTemps[l3]["Matching"].ToString();
                                    CauHoiTemp.M6 = drTemps[l3]["Matched"].ToString();
                                    break;
                                case 7:
                                    CauHoiTemp.D7 = drTemps[l3]["Matching"].ToString();
                                    CauHoiTemp.M7 = drTemps[l3]["Matched"].ToString();
                                    break;
                                case 8:
                                    CauHoiTemp.D8 = drTemps[l3]["Matching"].ToString();
                                    CauHoiTemp.M8 = drTemps[l3]["Matched"].ToString();
                                    break;
                                case 9:
                                    CauHoiTemp.D9 = drTemps[l3]["Matching"].ToString();
                                    CauHoiTemp.M9 = drTemps[l3]["Matched"].ToString();
                                    break;
                                case 10:
                                    CauHoiTemp.D10 = drTemps[l3]["Matching"].ToString();
                                    CauHoiTemp.M10 = drTemps[l3]["Matched"].ToString();
                                    break;
                                case 11:
                                    CauHoiTemp.D11 = drTemps[l3]["Matching"].ToString();
                                    CauHoiTemp.M11 = drTemps[l3]["Matched"].ToString();
                                    break;
                                case 12:
                                    CauHoiTemp.D12 = drTemps[l3]["Matching"].ToString();
                                    CauHoiTemp.M12 = drTemps[l3]["Matched"].ToString();
                                    break;
                                case 13:
                                    CauHoiTemp.D13 = drTemps[l3]["Matching"].ToString();
                                    CauHoiTemp.M13 = drTemps[l3]["Matched"].ToString();
                                    break;
                                case 14:
                                    CauHoiTemp.D14 = drTemps[l3]["Matching"].ToString();
                                    CauHoiTemp.M14 = drTemps[l3]["Matched"].ToString();
                                    break;
                                case 15:
                                    CauHoiTemp.D15 = drTemps[l3]["Matching"].ToString();
                                    CauHoiTemp.M15 = drTemps[l3]["Matched"].ToString();
                                    break;
                                default: break;
                            }
                        }
                        int l1 = 1;
                        foreach (DataRow drTemp in drTemps)
                        {
                            strMatchTemp = strMatchTemp + ";" + drTemp["Matched"].ToString();
                            switch (l1)
                            {
                                case 1:
                                    CauHoiTemp.A1 = drTemp["Content"].ToString();
                                    break;
                                case 2:
                                    CauHoiTemp.A2 = drTemp["Content"].ToString();
                                    break;
                                case 3:
                                    CauHoiTemp.A3 = drTemp["Content"].ToString();
                                    break;
                                case 4:
                                    CauHoiTemp.A4 = drTemp["Content"].ToString();
                                    break;
                                case 5:
                                    CauHoiTemp.A5 = drTemp["Content"].ToString();
                                    break;
                                case 6:
                                    CauHoiTemp.A6 = drTemp["Content"].ToString();
                                    break;
                                case 7:
                                    CauHoiTemp.A7 = drTemp["Content"].ToString();
                                    break;
                                case 8:
                                    CauHoiTemp.A8 = drTemp["Content"].ToString();
                                    break;
                                case 9:
                                    CauHoiTemp.A9 = drTemp["Content"].ToString();
                                    break;
                                case 10:
                                    CauHoiTemp.A10 = drTemp["Content"].ToString();
                                    break;
                                case 11:
                                    CauHoiTemp.A11 = drTemp["Content"].ToString();
                                    break;
                                case 12:
                                    CauHoiTemp.A12 = drTemp["Content"].ToString();
                                    break;
                                case 13:
                                    CauHoiTemp.A13 = drTemp["Content"].ToString();
                                    break;
                                case 14:
                                    CauHoiTemp.A14 = drTemp["Content"].ToString();
                                    break;
                                case 15:
                                    CauHoiTemp.A15 = drTemp["Content"].ToString();
                                    break;
                                default: break;
                            }
                            l1++;
                        }
                        break;
                    default: break;
                }
            }
            DapAnTemp.CauHoiID = iCauHoiTemp;
            DapAnTemp.Type = strTypeCauHoiTemp;
            float fDiemTemp = dr.IsNull("Diem") ? 1 : int.Parse(dr["Diem"].ToString());
            CauHoiTemp.Mark = fDiemTemp;
            DapAnTemp.Mark = fDiemTemp;
            DapAnTemp.Match = strMatchTemp;
            return CauHoiTemp;
        }

        protected void ddlBoCauHoi_SelectedIndexChanged(object sender, EventArgs e)
        {
            tinhToanSoCauHoiToiDa();
        }

        protected void ddlDoKho_SelectedIndexChanged(object sender, EventArgs e)
        {
            tinhToanSoCauHoiToiDa();
        }
        private void tinhToanSoCauHoiToiDa()
        {
            //int iBoCauHoiID = int.Parse(ddlBoCauHoi.SelectedValue);
            //int iDoKho = int.Parse(ddlDoKho.SelectedValue);
            //int iBoDe = int.Parse(txtBoDeID.Text);
            //int iBoDePhanCauHinh = -1;
            //int.TryParse(txtID.Text, out iBoDePhanCauHinh);


            //DataTable dtTemp = data.dnn_NuceThi_BoDe_Phan_CauHinh.tinhToanSoCauHoi(iBoDe, iBoCauHoiID, iDoKho, iBoDePhanCauHinh);

            int iSoCauHoiDaDung = getSoCauHoiDaDung(ddlBoCauHoi.SelectedValue, ddlDoKho.SelectedValue);
            int iSoCauHoiToiDaDuocDung = getSoCauHoiToiDa(ddlBoCauHoi.SelectedValue, ddlDoKho.SelectedValue, "1");

            int iSoCauHoiConDuocDung = iSoCauHoiToiDaDuocDung - iSoCauHoiDaDung;

            txtSoCauHoiToiDaDuocChon.Text = iSoCauHoiConDuocDung.ToString();
            tdSoCauHoiToiDa.InnerHtml = string.Format("Số câu hỏi đã dùng: {0} - Số câu hỏi trong bộ câu hỏi: {1} - Số câu hỏi còn được dùng tối đa: {2}", iSoCauHoiDaDung, iSoCauHoiToiDaDuocDung, iSoCauHoiConDuocDung);
        }
        private int getSoCauHoiDaDung(string BoCauHoi, string DoKho)
        {
            for (int i = 0; i < m_dtThongkeSoCauHoiDaDung.Rows.Count; i++)
            {
                if (m_dtThongkeSoCauHoiDaDung.Rows[i]["BoCauHoiID"].ToString().Equals(BoCauHoi) &&
                    m_dtThongkeSoCauHoiDaDung.Rows[i]["DoKhoID"].ToString().Equals(DoKho))
                    return int.Parse(m_dtThongkeSoCauHoiDaDung.Rows[i]["SoCauHoi"].ToString());
            }
            return 0;
        }
        private int getSoCauHoiToiDa(string BoCauHoi, string DoKho, string Level)
        {
            for (int i = 0; i < m_dtThongkeSoCauHoi.Rows.Count; i++)
            {
                if (m_dtThongkeSoCauHoi.Rows[i]["BoCauHoiID"].ToString().Equals(BoCauHoi) &&
                    m_dtThongkeSoCauHoi.Rows[i]["DoKhoID"].ToString().Equals(DoKho) &&
                     m_dtThongkeSoCauHoi.Rows[i]["Level"].ToString().Equals(Level))
                    return int.Parse(m_dtThongkeSoCauHoi.Rows[i]["SoCauHoi"].ToString());
            }
            return 0;
        }
    }
}