using DotNetNuke.Entities.Modules;
using System;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Data;

namespace nuce.web.commons
{
    public partial class ImportExcelInLopHoc : PortalModuleBase
    {
        public static DataSet m_DSDataImport;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!(this.UserInfo.IsInRole(web.Utils.role_Admin) || this.UserInfo.IsInRole(web.Utils.role_QuanTriThongTinChung)))
                {  
                    int iLopHocID = int.Parse(Request.QueryString["lophocid"].Trim());
                    if(data.dnn_NuceCommon_LopHoc.checkLopHocAndNguoiDung(iLopHocID,this.UserId)<1)
                    {
                        quayLai();
                    }
                }
            }
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
        DataTable getTableFromWorkSheet(IXLWorksheet workSheet)
        {
            DataTable dt = new DataTable();
            //Loop through the Worksheet rows.
            bool firstRow = true;
            int i = 0;
            int j = 0;
            foreach (IXLRow row in workSheet.Rows())
            {
                //Use the first row to add columns to DataTable.
                if (j >=8)
                {
                    i = 0;
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

                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }
                }
                j++;
                //GridView1.DataSource = dt;
                //GridView1.DataBind();
            }
            return dt;
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
        protected void btnImport_Click(object sender, EventArgs e)
        {
            // Du lieu cua datatable da co
            // Cap nhat vao co so du lieu
            int iLopHocID = int.Parse(Request.QueryString["lophocid"].Trim());
            int iSTT = -1;
            string strMaSV = "";
            string strHo = "";
            string strTen = "";
            string strMaLopQL = "";
            string strGhiChu = "";
            DataTable dtContent = m_DSDataImport.Tables[0];
            string strReturn = "";
            for(int i=0;i< dtContent.Rows.Count;i++)
            {
                try
                {
                    iSTT = int.Parse(dtContent.Rows[i][0].ToString());
                    strMaSV = dtContent.Rows[i][2].ToString();
                    strHo = dtContent.Rows[i][3].ToString();
                    strTen = dtContent.Rows[i][6].ToString();
                    strMaLopQL= dtContent.Rows[i][8].ToString();
                    strGhiChu = dtContent.Rows[i][17].ToString();
                    // Cap nhat vao csdl
                    data.dnn_NuceCommon_LopHoc_SinhVien.insert(iLopHocID, strMaSV, strHo, strTen, strGhiChu, iSTT, strMaLopQL);
                }
                catch(Exception ex)
                {
                    strReturn += string.Format("Loi tai dong {0}: {1};",(i+1), ex.ToString());
                }
            }
            divAnnouce.InnerHtml = strReturn;
        }
        protected void btnQuayTroLaiMain_Click(object sender, EventArgs e)
        {
            quayLai();
        }
        private void quayLai()
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?lophocid={1}", this.TabId, Request.QueryString["lophocid"]));
        }
    }
}