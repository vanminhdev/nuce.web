using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace nuce.web.sinhvien
{
    public partial class Hoc : CoreModule
    {
        DataTable dtDotTraoDoiActive;
        DataTable dtDotTraoDoi;
        DataTable dtCaItems;

        int m_iCaLopHocSinhVien;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["calophoc"] == null || Request.QueryString["calophocsinhvien"] == null)
            {
                writeLog("Canh Bao", "Url bi sai " + Request.QueryString["calophoc"]);
                Response.Redirect(string.Format("/tabid/{0}/default.aspx", 1119));
                return;
            }
            int iCaLopHoc = int.Parse(Request.QueryString["calophoc"]);
            m_iCaLopHocSinhVien = int.Parse(Request.QueryString["calophocsinhvien"]);
            Dictionary<int, model.CaLopHocSinhVien> m_CaLopHocSinhViens = (Dictionary<int, model.CaLopHocSinhVien>)Session[Utils.session_ca_lophoc_sinhvien];
            model.CaLopHocSinhVien CaLopHocSinhVien = m_CaLopHocSinhViens[m_iCaLopHocSinhVien];
            if (!CaLopHocSinhVien.Status.Equals(2))
            {
                //return lai trang chu
                Response.Redirect(string.Format("/tabid/{0}/default.aspx", 1119));
            }
            dtDotTraoDoi = data.dnn_NuceQLHPM_CaHoc_DotTraoDoi.getByCaLopHoc(iCaLopHoc);
            dtCaItems = data.dnn_NuceQLHPM_CaHoc_Items.getGetByCaLopHocSinhVien(m_iCaLopHocSinhVien);
            if (!IsPostBack)
            {
                dtDotTraoDoiActive = new DataTable();
                dtDotTraoDoiActive.Columns.Add("id");
                dtDotTraoDoiActive.Columns.Add("value");
                string strHtml = "";
                strHtml = "<table border='1px' style='width: 100%;'>";
                strHtml += "<tr>";
                strHtml += "<td style='width: 40px;text-align: center;font-weight: bold;'>STT</td>";
                strHtml += "<td style='width: 12%;text-align: center;font-weight: bold;'>Tên đợt trao đổi</td>";
                strHtml += "<td style='width: 18%;text-align: center;font-weight: bold;'>Thời gian</td>";
                strHtml += "<td style='width: 18%;text-align: center;font-weight: bold;'>Nội dung</td>";
                strHtml += "<td style='width: 18%;text-align: center;font-weight: bold;'>FileUpload</td>";
                strHtml += "<td style='text-align: center;font-weight: bold;'>Trạng thái</td>";
                strHtml += "<td style='text-align: center;font-weight: bold;'></td>";
                strHtml += "<td style='text-align: center;font-weight: bold;'></td>";
                strHtml += "</tr>";
                // Lay du lieu de xe ly

                for (int i = 0; i < dtDotTraoDoi.Rows.Count; i++)
                {
                    int ID = int.Parse(dtDotTraoDoi.Rows[i]["CaHoc_DotTraoDoiID"].ToString());
                    string TenDotTraoDoi = dtDotTraoDoi.Rows[i]["Ten"].ToString();
                    strHtml += "<tr>";
                    strHtml += string.Format("<td style='width: 40px;text-align: center;font-weight: bold;'>{0}</td>", i + 1);
                    strHtml += string.Format("<td style='width: 12%;text-align: center;'>{0}</td>", TenDotTraoDoi);
                    strHtml += string.Format("<td style='width: 18%;text-align: center;'>Từ {0} giờ {1} phút đến {2} giờ {3} phút</td>", dtDotTraoDoi.Rows[i]["GioBatDau"].ToString(), dtDotTraoDoi.Rows[i]["PhutBatDau"].ToString(), dtDotTraoDoi.Rows[i]["GioKetThuc"].ToString(), dtDotTraoDoi.Rows[i]["PhutKetThuc"].ToString());

                    int iStatus = int.Parse(dtDotTraoDoi.Rows[i]["Status"].ToString());
                    string strTrangThai = "";
                    switch (iStatus)
                    {
                        case 1:
                            strTrangThai = "Gốc";
                            break;
                        case 2:
                            strTrangThai = "Thực hiện";
                            break;
                        case 3:
                            strTrangThai = "Hoàn thành";
                            break;
                        default: break;
                    }
                    DataRow[] drs = getItemRow(dtCaItems, int.Parse(dtDotTraoDoi.Rows[i]["CaHoc_DotTraoDoiID"].ToString()));
                    string strLinkFile = "";
                    string strFile = "";
                    int iStatusItem = -1;
                    string strNoiDung = "";
                    int iCaItemID = -1;
                    if (drs.Length > 0)
                    {
                        strLinkFile = drs[0]["File_Link"].ToString();
                        strFile = string.Format("{0} (Loại File {1} - Dung lượng {2}B)",
                            drs[0]["File_TenGoc"].ToString()
                            , drs[0]["File_Loai"].ToString()
                            , drs[0]["File_DungLuong"].ToString());
                        iStatusItem = int.Parse(drs[0]["Status"].ToString());
                        strNoiDung = drs[0]["NoiDung"].ToString();
                        iCaItemID = int.Parse(drs[0]["CaHoc_ItemID"].ToString());
                    }
                    strHtml += string.Format("<td style='width: 20%;text-align: center;'>{0}</td>", strNoiDung);
                    if (strLinkFile == "")
                    {
                        strHtml += "<td style='width: 20%;text-align: center;font-weight: bold;'></td>";
                        strHtml += "<td style='text-align: center;font-weight: bold;'>" + strTrangThai + "</td>";
                        strHtml += "<td style='text-align: center;font-weight: bold;'>Chưa nộp</td>";
                        strHtml += "<td style='text-align: center;font-weight: bold;'></td>";
                    }
                    else
                    {
                        strHtml += "<td style='width: 25%;text-align: center;font-weight: bold;'>" + strFile + "</td>";
                        strHtml += "<td style='text-align: center;font-weight: bold;'>" + strTrangThai + "</td>";
                        if (iStatusItem.Equals(1))
                        {
                            strHtml += "<td style='text-align: center;font-weight: bold;'>Chưa nộp bài</td>";
                            if(iStatus.Equals(3))
                            {
                                strHtml += "<td style='text-align: center;font-weight: bold;'></td>";
                            }
                            else
                                strHtml += string.Format("<td style='text-align: center;font-weight: bold;'><a href='javascript:nopbai({0});'>Nộp bài</a></td>", iCaItemID);
                        }
                        else
                        {
                            strHtml += "<td style='text-align: center;font-weight: bold;'>Đã nộp bài</td>";
                            strHtml += "<td style='text-align: center;font-weight: bold;'></td>";
                        }
                    }
                    strHtml += "</tr>";
                    //Them vao datatable
                    if (iStatus.Equals(2) && !iStatusItem.Equals(2))
                    {
                        dtDotTraoDoiActive.Rows.Add(ID.ToString(), TenDotTraoDoi);
                    }
                }
                strHtml += "</table>";
                divContent.InnerHtml = strHtml;
                if (dtDotTraoDoiActive.Rows.Count > 0)
                {
                    ddlDotTraoDoi.DataValueField = "ID";
                    ddlDotTraoDoi.DataTextField = "Value";
                    ddlDotTraoDoi.DataSource = dtDotTraoDoiActive;
                    ddlDotTraoDoi.DataBind();
                }
                else
                {
                    divUpload.Visible = false;
                }

            }

        }
        private DataRow[] getItemRow(DataTable dt, int CaHoc_DotTraoDoiID)
        {
            DataRow[] dr = dt.Select(string.Format("CaHoc_DotTraoDoiID={0}", CaHoc_DotTraoDoiID));
            return dr;
        }
        protected void UploadButton_Click(object sender, EventArgs e)
        {
            int iDotTraoDoi = int.Parse(ddlDotTraoDoi.SelectedValue);
            string strFileGioiHan = "";
            int iDungLuongGioiHan = 102400;
            DataRow[] drs = dtDotTraoDoi.Select(string.Format("CaHoc_DotTraoDoiID={0}", iDotTraoDoi));
            if (drs.Length > 0)
            {
                strFileGioiHan = drs[0]["FileChoPhep"].ToString();
                iDungLuongGioiHan = int.Parse(drs[0]["DungLuongToiDa"].ToString());
            }

            string strMoTa = txtMoTa.Text;

            string strLinkFile = "";
            string strTenCu = "";
            string strTenMoi = "";
            string strMoRongFile = "";
            int iDungLuongFile = -1;
            if (FileUploadControl.HasFile)
            {
                try
                {
                    if (strFileGioiHan.Contains(FileUploadControl.PostedFile.ContentType))
                    {
                        if (FileUploadControl.PostedFile.ContentLength > iDungLuongFile)
                        {
                            strMoRongFile = Path.GetExtension(FileUploadControl.FileName);
                            iDungLuongFile = FileUploadControl.PostedFile.ContentLength;
                            string filename = Path.GetFileName(FileUploadControl.FileName);
                            strTenCu = filename;
                            strTenMoi = string.Format("{0}_{1}_{2}_{3}_{4}.{5}", Utils.RemoveUnicode(m_SinhVien.Ten), Utils.RemoveUnicode(m_SinhVien.Ho).Replace(" ", "_"), m_SinhVien.MaSV, m_iCaLopHocSinhVien, iDotTraoDoi, strMoRongFile);
                            string directoryPath = Server.MapPath("~/NuceDataUpload/" + iDotTraoDoi.ToString());
                            if (!System.IO.Directory.Exists(directoryPath))
                                System.IO.Directory.CreateDirectory(directoryPath);

                            string path = Server.MapPath("~/NuceDataUpload/"+ iDotTraoDoi.ToString()+"/" + strTenMoi);
                            strLinkFile = path;
                            FileInfo file = new FileInfo(path);
                            if (file.Exists)//check file exsit or not
                            {
                                file.Delete();
                            }
                            FileUploadControl.SaveAs(strLinkFile);
                            data.dnn_NuceQLHPM_CaHoc_Items.insert(iDotTraoDoi, m_iCaLopHocSinhVien, strMoTa, strLinkFile, strTenCu, strTenMoi, strMoRongFile, strMoRongFile, iDungLuongFile);
                            StatusLabel.Text = "Cập nhật dữ liệu thành công !";
                            Response.Redirect(Request.RawUrl);
                        }
                        else
                            StatusLabel.Text = "Dung lượng file upload phải nhỏ hơn " + iDungLuongGioiHan.ToString() + " b!";
                    }
                    else
                        StatusLabel.Text = "Loại file không đúng: " + strFileGioiHan;
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }
            else
            {
                data.dnn_NuceQLHPM_CaHoc_Items.insert(iDotTraoDoi, m_iCaLopHocSinhVien, strMoTa, strLinkFile, strTenCu, strTenMoi, strMoRongFile, strMoRongFile, iDungLuongFile);
                StatusLabel.Text = "Cập nhật dữ liệu thành công !";
                Response.Redirect(Request.RawUrl);
            }
        }
    }
}