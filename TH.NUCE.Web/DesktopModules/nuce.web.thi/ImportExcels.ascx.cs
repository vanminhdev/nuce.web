using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace nuce.web.thi
{
    public partial class ImportExcels : CoreModule
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string strImagePath;
            if (upload_Image(FU1, "/Portals/0/FileExcelUploads/", out strImagePath))
            {
                divAnnounce.InnerHtml = "Upload file thành công với đường dẫn: " + strImagePath;

                //Save the uploaded Excel file.
                string filePath = Server.MapPath("~" + strImagePath);
                //Open the Excel file using ClosedXML.
                DataSet ds = new DataSet();
                using (XLWorkbook workBook = new XLWorkbook(filePath))
                {
                    //Read the first Sheet from Excel file.
                    IXLWorksheet workSheet = workBook.Worksheet(1);
                    ds.Tables.Add(getTableFromWorkSheet(workSheet));
                    workSheet = workBook.Worksheet(2);
                    ds.Tables.Add(getTableFromWorkSheet(workSheet));
                    //Create a new DataTable.
                }
            }
            else
            {
                divAnnounce.InnerHtml = "Upload file không thành công với lỗi: " + strImagePath;
                return;
            }

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
                        dt.Columns.Add(cell.Value.ToString());
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
    }
}