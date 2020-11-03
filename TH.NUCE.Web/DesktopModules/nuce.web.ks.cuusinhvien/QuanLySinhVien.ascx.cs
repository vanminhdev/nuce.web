using System;
using System.Data;
using DotNetNuke.Entities.Modules;
using System.Web.UI.WebControls;
using System.Text;
using ClosedXML.Excel;
using System.Collections.Generic;

namespace nuce.web.ks.cuusinhvien
{
    public partial class QuanLySinhVien : PortalModuleBase
    {
        public static DataSet m_DSDataImport;
        public static DataSet m_DSDataImport2;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadGridData();
        }
        private void LoadGridData()
        {
            //I am adding dummy data here. You should bring data from your repository.
            DataTable dt = data.dnn_Nuce_KS_SinhVienRaTruong_SinhVien.getByDotKhaoSat(int.Parse(ddlDotKhaoSat.SelectedValue), txtMaSv.Text.Trim());
            grdData.DataSource = dt;
            grdData.DataBind();
        }
        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdData.PageIndex = e.NewPageIndex;
            LoadGridData();
        }

        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadGridData();
        }
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

        protected void btnCapNhatLan1_Click(object sender, EventArgs e)
        {
            string dottotnghiep; dottotnghiep = "";
            string sovaoso; sovaoso = "";
            string masv; masv = "";
            string noisiti; noisiti = "";
            string tbcht; tbcht = "";
            string xeploai; xeploai = "";
            string soqdtn; soqdtn = "";
            string sohieuba; sohieuba = "";
            string tinh; tinh = "";
            string truong; truong = "";
            string gioitinh; gioitinh = "";
            string ngaysinh; ngaysinh = "";
            string tkhau; tkhau = "";
            string lop12; lop12 = "";
            string namtn; namtn = "";
            string sobaodanh; sobaodanh = "";
            string tcong; tcong = "";
            string ghichu_thi; ghichu_thi = "";
            string lopqd; lopqd = "";
            string k; k = "";
            string dtoc; dtoc = "";
            string quoctich; quoctich = "";
            string bangclc; bangclc = "";
            string manganh; manganh = "";
            string tenchnga; tenchnga = "";
            string tennganh; tennganh = "";
            string hedaotao; hedaotao = "";
            string khoahoc; khoahoc = "";
            string tensinhvien; tensinhvien = "";
            string email; email = "";
            string email1; email1 = "";
            string email2; email2 = "";
            string mobile; mobile = "";
            string mobile1; mobile1 = "";
            string mobile2; mobile2 = "";
            string thongtinthem; thongtinthem = "";
            string thongtinthem1; thongtinthem1 = "";
            int dotkhoasat_id; dotkhoasat_id = 2;
            string psw = "123";
            int type; type = 1;
            int status; status = 1;
            string checksum = "";
            string ex_masv = "";
            string add0pass = "";

            for (int i = 0; i < m_DSDataImport.Tables[0].Rows.Count; i++)
            {
                try
                {
                    add0pass = "";
                    sovaoso = m_DSDataImport.Tables[0].Rows[i][1].ToString().Trim();
                    tensinhvien = m_DSDataImport.Tables[0].Rows[i][3].ToString().Trim();
                    masv = m_DSDataImport.Tables[0].Rows[i][2].ToString().Trim();
                    ex_masv = masv;
                    if (masv.Length < 7)
                    {
                        for (int j = 0; j < 7 - masv.Length; j++)
                        {
                            add0pass = add0pass + "0";
                        }
                    }
                    masv = string.Format("{0}{1}", add0pass, masv);
                    ngaysinh = m_DSDataImport.Tables[0].Rows[i][4].ToString().Trim();
                    gioitinh = m_DSDataImport.Tables[0].Rows[i][5].ToString().Trim();
                    manganh = m_DSDataImport.Tables[0].Rows[i][6].ToString().Trim();
                    tennganh = m_DSDataImport.Tables[0].Rows[i][7].ToString().Trim();
                    tenchnga = m_DSDataImport.Tables[0].Rows[i][8].ToString().Trim();
                    lopqd = m_DSDataImport.Tables[0].Rows[i][9].ToString().Trim();
                    khoahoc = m_DSDataImport.Tables[0].Rows[i][10].ToString().Trim();
                    xeploai = m_DSDataImport.Tables[0].Rows[i][11].ToString().Trim();
                    namtn = m_DSDataImport.Tables[0].Rows[i][12].ToString().Trim();
                    soqdtn = m_DSDataImport.Tables[0].Rows[i][13].ToString().Trim();
                    sohieuba = m_DSDataImport.Tables[0].Rows[i][14].ToString().Trim();
                    hedaotao = m_DSDataImport.Tables[0].Rows[i][15].ToString().Trim();

                    // k = m_DSDataImport.Tables[0].Rows[i][3].ToString().Trim();
                    checksum = Utils.RemoveUnicode(tensinhvien).Replace(" ", "").ToLower();


                    data.dnn_Nuce_KS_SinhVienRaTruong_SinhVien1.insert(dottotnghiep,
sovaoso,
masv,
noisiti,
tbcht,
xeploai,
soqdtn,
sohieuba,
tinh,
truong,
gioitinh,
ngaysinh,
tkhau,
lop12,
namtn,
sobaodanh,
tcong,
ghichu_thi,
lopqd,
k,
dtoc,
quoctich,
bangclc,
manganh,
tenchnga,
tennganh,
hedaotao,
khoahoc,
tensinhvien,
email,
email1,
email2,
mobile,
mobile1,
mobile2,
thongtinthem,
thongtinthem1,
dotkhoasat_id, checksum, checksum,ex_masv,
type,
status

);
                }
                catch (Exception ex)
                {

                }
            }

        }

        protected void btnUpload2_Click(object sender, EventArgs e)
        {
            string strImagePath;
            if (upload_Image(FU2, "/Portals/0/FileExcelUploads/", out strImagePath))
            {
                divAnnouce.InnerHtml = "Upload file thành công với đường dẫn: " + strImagePath;

                //Save the uploaded Excel file.
                string filePath = Server.MapPath("~" + strImagePath);
                //Open the Excel file using ClosedXML.
                m_DSDataImport2 = new DataSet();
                using (XLWorkbook workBook = new XLWorkbook(filePath))
                {
                    string strHtml = "";
                    for (int i = 1; i <= workBook.Worksheets.Count; i++)
                    {
                        IXLWorksheet workSheet = workBook.Worksheet(i);
                        m_DSDataImport2.Tables.Add(getTableFromWorkSheet(workSheet));
                        strHtml += string.Format("<div style='margin:10px;text-align: center;'>Sheet: <b>{0}</b></div><div style='margin:10px;text-align: center;'>{1}</div>", workSheet.Name, getHtmlFromDataTable(m_DSDataImport2.Tables[i - 1]));
                    }
                    divContentExcel2.InnerHtml = strHtml;
                }
            }
            else
            {
                divAnnouce.InnerHtml = "Upload file không thành công với lỗi: " + strImagePath;
                return;
            }
        }

        protected void btnCapNhatLan2_Click(object sender, EventArgs e)
        {
            string masv = "";
            string tensv = "";
            string email; email = "";
            string email1; email1 = "";
            string email2; email2 = "";
            string mobile; mobile = "";
            string mobile1; mobile1 = "";
            string mobile2; mobile2 = "";
            int iReturn = -1;

            for (int i = 0; i < m_DSDataImport2.Tables[0].Rows.Count; i++)
            {
                try
                {
                    masv = m_DSDataImport2.Tables[0].Rows[i][1].ToString().Trim();
                    tensv = m_DSDataImport2.Tables[0].Rows[i][2].ToString().Trim();
                    email = m_DSDataImport2.Tables[0].Rows[i][4].ToString().Trim();
                    mobile = m_DSDataImport2.Tables[0].Rows[i][5].ToString().Trim();
                    // k = m_DSDataImport.Tables[0].Rows[i][3].ToString().Trim();
                    iReturn = data.dnn_Nuce_KS_SinhVienRaTruong_SinhVien.update(masv,
email,
email1,
email2,
mobile,
mobile1,
mobile2, "cap nhat email bang masv", ""

);
                    if (!(iReturn > 0))
                    {
                        // cap nhat dua vao checksum
                        tensv = Utils.RemoveUnicode(tensv).Replace(" ", "").ToLower();
                        data.dnn_Nuce_KS_SinhVienRaTruong_SinhVien.updateByChecksum(tensv,
email,
email1,
email2,
mobile,
mobile1,
mobile2, "cap nhat email bang ten", ""

);
                    }
                }
                catch (Exception ex)
                {

                }
            }

        }

        protected void btnCapNhatLai_lan_1_Click(object sender, EventArgs e)
        {

            string dottotnghiep; dottotnghiep = "";
            string sovaoso; sovaoso = "";
            string masv; masv = "";
            string noisiti; noisiti = "";
            string tbcht; tbcht = "";
            string xeploai; xeploai = "";
            string soqdtn; soqdtn = "";
            string sohieuba; sohieuba = "";
            string tinh; tinh = "";
            string truong; truong = "";
            string gioitinh; gioitinh = "";
            string ngaysinh; ngaysinh = "";
            string tkhau; tkhau = "";
            string lop12; lop12 = "";
            string namtn; namtn = "";
            string sobaodanh; sobaodanh = "";
            string tcong; tcong = "";
            string ghichu_thi; ghichu_thi = "";
            string lopqd; lopqd = "";
            string k; k = "";
            string dtoc; dtoc = "";
            string quoctich; quoctich = "";
            string bangclc; bangclc = "";
            string manganh; manganh = "";
            string tenchnga; tenchnga = "";
            string tennganh; tennganh = "";
            string hedaotao; hedaotao = "";
            string khoahoc; khoahoc = "";
            string tensinhvien; tensinhvien = "";
            string email; email = "";
            string email1; email1 = "";
            string email2; email2 = "";
            string mobile; mobile = "";
            string mobile1; mobile1 = "";
            string mobile2; mobile2 = "";
            string thongtinthem; thongtinthem = "";
            string thongtinthem1; thongtinthem1 = "";
            int dotkhoasat_id; dotkhoasat_id = 1;
            string psw = "123";
            int type; type = 1;
            int status; status = 1;
            string checksum = "";
            string ex_masv = "";
            string add0pass = "";

            for (int i = 0; i < m_DSDataImport.Tables[0].Rows.Count; i++)
            {
                try
                {
                    add0pass = "";
                    sovaoso = m_DSDataImport.Tables[0].Rows[i][1].ToString().Trim();
                    tensinhvien = m_DSDataImport.Tables[0].Rows[i][3].ToString().Trim();
                    masv = m_DSDataImport.Tables[0].Rows[i][2].ToString().Trim();
                    ex_masv = masv;
                    if (masv.Length < 7)
                    {
                        for (int j = 0; j < 7 - masv.Length; j++)
                        {
                            add0pass = add0pass + "0";
                        }
                    }
                    masv = string.Format("{0}{1}", add0pass, masv);
                    ngaysinh = m_DSDataImport.Tables[0].Rows[i][4].ToString().Trim();
                    gioitinh = m_DSDataImport.Tables[0].Rows[i][5].ToString().Trim();
                    manganh = m_DSDataImport.Tables[0].Rows[i][6].ToString().Trim();
                    tennganh = m_DSDataImport.Tables[0].Rows[i][7].ToString().Trim();
                    tenchnga = m_DSDataImport.Tables[0].Rows[i][8].ToString().Trim();
                    lopqd = m_DSDataImport.Tables[0].Rows[i][9].ToString().Trim();
                    khoahoc = m_DSDataImport.Tables[0].Rows[i][10].ToString().Trim();
                    xeploai = m_DSDataImport.Tables[0].Rows[i][11].ToString().Trim();
                    namtn = m_DSDataImport.Tables[0].Rows[i][12].ToString().Trim();
                    soqdtn = m_DSDataImport.Tables[0].Rows[i][13].ToString().Trim();
                    sohieuba = m_DSDataImport.Tables[0].Rows[i][14].ToString().Trim();
                    hedaotao = m_DSDataImport.Tables[0].Rows[i][15].ToString().Trim();

                    // k = m_DSDataImport.Tables[0].Rows[i][3].ToString().Trim();
                    checksum = Utils.RemoveUnicode(tensinhvien).Replace(" ", "").ToLower();
                    //data.dnn_Nuce_KS_SinhVienRaTruong_SinhVien.updatelopQD(masv, lopqd);
                    data.dnn_Nuce_KS_SinhVienRaTruong_SinhVien.update_addInfo(masv, khoahoc, xeploai,
                        namtn, soqdtn, sohieuba, hedaotao);



                }
                catch (Exception ex)
                {

                }
            }
        }

        protected void btnExportExcelByClass_Click(object sender, EventArgs e)
        {

        }
    }
}