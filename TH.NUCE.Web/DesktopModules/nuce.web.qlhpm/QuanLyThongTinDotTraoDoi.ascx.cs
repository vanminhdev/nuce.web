using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.qlhpm
{
    public partial class QuanLyThongTinDotTraoDoi : PortalModuleBase
    {
        int iCaHoc_DotTraoDoiID;
        int iCaHoc;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["CaHoc_DotTraoDoiID"] != null && Request.QueryString["CaHoc_DotTraoDoiID"] != "" && int.TryParse(Request.QueryString["CaHoc_DotTraoDoiID"], out iCaHoc_DotTraoDoiID))
            {
                // Lấy thông tin của ca học

                DataTable dt = nuce.web.data.dnn_NuceQLHPM_CaHoc_DotTraoDoi.getName(iCaHoc_DotTraoDoiID);
                DataTable dtItems = nuce.web.data.dnn_NuceQLHPM_CaHoc_Items.getGetByCaHocDotTraoDoi(iCaHoc_DotTraoDoiID);
                iCaHoc = int.Parse(dt.Rows[0]["CaHocID"].ToString());

                string strThongBao= string.Format("Học kì: {0} - Môn học: {1} - Ca học: {2} - Đợt trao đổi: {8} - Ngày: {3} ({4}h:{5} phút --> {6}h:{7} phút)", dt.Rows[0]["TenHocKy"].ToString(), dt.Rows[0]["TenMonHoc"].ToString(), dt.Rows[0]["TenCa"].ToString()
                , DateTime.Parse(dt.Rows[0]["Ngay"].ToString()).ToString("dd/MM/yyyy"), dt.Rows[0]["GioBatDau"].ToString(), dt.Rows[0]["PhutBatDau"].ToString(), dt.Rows[0]["GioKetThuc"].ToString(), dt.Rows[0]["PhutKetThuc"].ToString(), dt.Rows[0]["Ten"].ToString());
                strThongBao += "</br>";
                strThongBao += string.Format("<a href='/tabid/{0}/default.aspx?cahoc={1}'>Quay lại</a>", 2126,iCaHoc);
                divDotTraoDoi.InnerHtml = strThongBao;


                // Hien thi thong tin cua cac lop hoc
                //Lay thong tin cua lop hoc
                //
                DataTable dtLopHoc = nuce.web.data.dnn_NuceQLHPM_Ca_LopHoc.getByCaHoc(iCaHoc);

                string strHtml = "";
                for (int i = 0; i < dtLopHoc.Rows.Count; i++)
                {

                    strHtml += string.Format("<div style='text-align:left; font-weight:bold; padding:5px;'>({0}) {1}</div>", (i + 1), dtLopHoc.Rows[i]["Ten1"].ToString());
                    strHtml += "<div style='width:100%;'>";
                    strHtml += "<table border='1px' style='width:100%;'>";
                    strHtml += "<tr style='text-align:center; font-weight:bold;'>";
                    strHtml += "<td style='width:70%;'>";
                    strHtml += "Sinh viên đã vào học";
                    strHtml += "</td>";
                    strHtml += "<td style='width:30%;'>";
                    strHtml += "Sinh viên chưa vào học";
                    strHtml += "</td>";
                    strHtml += "</tr>";
                    strHtml += "<tr>";
                    // Chen sinh vien
                    int iCaLopHocID = int.Parse(dtLopHoc.Rows[i]["Ca_LopHocID"].ToString());
                    DataTable dtSinhVien = nuce.web.data.dnn_NuceQLHPM_Ca_LopHoc.getSinhVien(iCaLopHocID);
                    strHtml += string.Format("<td style='width:70%;vertical-align:top;'>{0}</td>", getHtmlSinhVienDaHoc(dtSinhVien, dtItems));
                    strHtml += string.Format("<td style='width:30%;vertical-align:top;'>{0}</td>", getHtmlSinhVienChuaNhanMay(dtSinhVien));
                    strHtml += "</tr>";
                    strHtml += "<tr>";
                    strHtml += "<td colspan='2'>";
                    // Get may chua su dung
                    int iPhongID= int.Parse(dtLopHoc.Rows[i]["PhongHocID"].ToString());
                    DataTable dtMayChuaSuDung = nuce.web.data.dnn_NuceQLHPM_Ca_LopHoc.getMayNotUseByPhong(iCaLopHocID, iPhongID);
                    strHtml += getHtmlMayChuaDung(dtMayChuaSuDung);
                    strHtml += "</td>";
                    strHtml += "</tr>";
                    strHtml += "</table>";
                    strHtml += "</div>";
                }
                divContent.InnerHtml = strHtml;
                if (dt.Rows.Count > 0)
                {
                    if (!IsPostBack)
                    {

                    }
                }
                else
                {
                    returnMain();
                }
            }
            else
            {
                returnMain();
            }
        }
        private string getHtmlMayChuaDung(DataTable dt)
        {
            string strHtml = "";
            strHtml += "<table border='1px' style='width:100%;'>";
            strHtml += "<tr style='text-align:center; font-weight:bold;'>";
            strHtml += "<td colspan='10'>Máy chưa sử dụng</td>";
            strHtml += "</tr>";
            int count = 0;
            for(int i=0;i<dt.Rows.Count;i++)
            {
                if(count%10==0)
                {
                    if(count>0)
                    {
                        strHtml += "</tr>";
                    }
                    strHtml += "<tr>";
                }
                strHtml += string.Format("<td>{0}</td>", dt.Rows[i]["Ma"].ToString());
                count++;
            }
            if(count>0)
            {
                strHtml += "</tr>";
            }
            strHtml += "</table>";
            return strHtml;
        }
        private string getHtmlSinhVienDaHoc(DataTable dtSinhVien, DataTable dtItems)
        {
            string strHtml = "";
            strHtml += "<table border='1px' style='width:100%;'>";
            strHtml += "<tr style='text-align:center; font-weight:bold;'>";
            strHtml += "<td style='width:10%;'>STT</td>";
            strHtml += "<td style='width:30%;'>Họ và tên</td>";
            strHtml += "<td style='width:15%;'>Mã sinh viên</td>";
            strHtml += "<td style='width:15%;'>Máy</td>";
            strHtml += "<td>Trạng thái</td>";
            strHtml += "<td></td>";
            strHtml += "</tr>";
            int count = 0;
            for(int i=0;i< dtSinhVien.Rows.Count;i++)
            {
                if(!dtSinhVien.Rows[i].IsNull("MaMay"))
                {
                    count++;
                    strHtml += "<tr style='text-align:center;'>";
                    strHtml += string.Format("<td style='width:10%;'>{0}</td>", count);
                    strHtml += string.Format("<td style='width:30%;'>{0} {1}</td>", dtSinhVien.Rows[i]["Ho"].ToString(), dtSinhVien.Rows[i]["Ten"].ToString());
                    strHtml += string.Format("<td style='width:15%;'>{0}</td>", dtSinhVien.Rows[i]["MaSV"].ToString());
                    strHtml += string.Format("<td style='width:15%;'>{0}</td>", dtSinhVien.Rows[i]["MaMay"].ToString());
                    int Ca_LopHoc_SinhVienID= int.Parse(dtSinhVien.Rows[i]["Ca_LopHoc_SinhVienID"].ToString());
                    DataRow[] ItemRow = getItemRow(dtItems, Ca_LopHoc_SinhVienID);
                    int iStatus = -1;
                    int iItemID = -1;
                    if (ItemRow.Length > 0)
                    {
                        iStatus=int.Parse(ItemRow[0]["Status"].ToString());
                        iItemID= int.Parse(ItemRow[0]["CaHoc_ItemID"].ToString());
                        if (iStatus.Equals(2))
                        {
                            strHtml += "<td>Đã nộp bài</td>";
                            strHtml += string.Format("<td><a href='javascript:nopbai({0},{1});'>Huỷ nộp bài</a></td>", iItemID,this.UserId);
                        }
                        else
                        {
                            strHtml += "<td>Chưa nộp bài</td>";
                            strHtml += "<td></td>";
                        }
                    }
                    else
                    {
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                    }
                    strHtml += "</tr>";
                    if(iStatus.Equals(2))
                        {
                        strHtml += "<tr>";
                        strHtml += "<td colspan='6'>";
                        string strFile = string.Format("{0} (Loại File {1} - Dung lượng {2}B)",
    ItemRow[0]["File_TenGoc"].ToString()
    , ItemRow[0]["File_Loai"].ToString()
    , ItemRow[0]["File_DungLuong"].ToString());
                        string strNoiDung = ItemRow[0]["NoiDung"].ToString();
                        strHtml += strFile;
                        strHtml += "</br>";
                        strHtml += strNoiDung;
                        strHtml += "</td>";
                        strHtml += "</tr>";
                    }
                }
            }
            strHtml += "</table>";
            return strHtml;
        }
        private string getHtmlSinhVienChuaNhanMay(DataTable dtSinhVien)
        {
            string strHtml = "";
            strHtml += "<table border='1px' style='width:100%;'>";
            strHtml += "<tr style='text-align:center; font-weight:bold;'>";
            strHtml += "<td>STT</td>";
            strHtml += "<td style='width:70%;'>Họ và tên</td>";
            strHtml += "<td>Mã sinh viên</td>";
            strHtml += "</tr>";
            int count = 0;
            for (int i = 0; i < dtSinhVien.Rows.Count; i++)
            {
                if (dtSinhVien.Rows[i].IsNull("MaMay"))
                {
                    count++;
                    strHtml += "<tr style='text-align:center;'>";
                    strHtml += string.Format("<td>{0}</td>", count);
                    strHtml += string.Format("<td style='width:70%;'>{0} {1}</td>", dtSinhVien.Rows[i]["Ho"].ToString(), dtSinhVien.Rows[i]["Ten"].ToString());
                    strHtml += string.Format("<td>{0}</td>", dtSinhVien.Rows[i]["MaSV"].ToString());
                    strHtml += "</tr>";
                }
            }
            strHtml += "</table>";
            return strHtml;
        }
        private DataRow[] getItemRow(DataTable dt,int Ca_LopHoc_SinhVienID)
        {
            DataRow[] dr = dt.Select(string.Format("Ca_LopHoc_SinhVienID={0}", Ca_LopHoc_SinhVienID));
            return dr;
        }
        private void returnMain()
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?cahoc={1}", 2126, iCaHoc));
        }
    }
}