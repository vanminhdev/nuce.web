using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

using iTextSharp.text.html;

using nuce.web;
using System.Web;

namespace TH.NUCE.Web
{
    public partial class ExportPdf : System.Web.UI.Page
    {

        protected override void OnInit(EventArgs e)
        {
            int iType = int.Parse(Request.QueryString["type"]);
            switch (iType)
            {
                case 1:
                    ProcessDeThi1();
                    break;
            }
            base.OnInit(e);
        }
        protected void ProcessDeThi()
        {
            string strDeThiID = Request.QueryString["dethiid"];
            string strMaDe = Request.QueryString["made"];
            int PortalID = int.Parse(Request.QueryString["portalid"]);
            string strHtml = UtilsDisplayDe.displayDe(strDeThiID, strMaDe, PortalID);

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + "PDFfile.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //strHtml= "<font face='arial unicode ms'>Hoàng Nam Thắng</font>";

            //strHtml += "<div><font face='arial unicode ms'>Thử coi thế nào</font></div>";
            Response.BinaryWrite(GetPDF(strHtml));
            Response.End();
        }
        protected void ProcessDeThi1()
        {
            Document oDoc = new Document(PageSize.A4, 10f, 10f, 5f, 0f);
            System.IO.MemoryStream msReport = new System.IO.MemoryStream();
            string ImagePath = Server.MapPath("Portals\\0\\Users\\002\\02\\2\\anh_day_tien_trinh_an_toan_1.jpg");

            string FilePath = "";

            FilePath = Server.MapPath("Fonts\\ARIALUNI.TTF");
            string fontpath = FilePath;
            BaseFont bf = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font fontCompany = new Font(bf, 10, Font.BOLD);
            Font fontHeader = new Font(bf, 9, Font.BOLD);
            Font fontSubHeader = new Font(bf, 8);
            Font fontTableHeader = new Font(bf, 8, Font.BOLD);
            Font fontContent = new Font(bf, 8, Font.NORMAL);


            try
            {
                PdfWriter writer = PdfWriter.GetInstance(oDoc, msReport);
                oDoc.AddAuthor("ThangHN");
                oDoc.AddSubject("Export to PDF");
                oDoc.Open();

                Chunk cBreak = new Chunk(Environment.NewLine);
                Phrase pBreak = new Phrase();
                Paragraph paBreak = new Paragraph();

                string sText = "TRƯỜNG ĐẠI HỌC XÂY DỰNG";
                Chunk beginning = new Chunk(sText, fontCompany);
                Phrase p1 = new Phrase(beginning);
                Paragraph pCompanyName = new Paragraph();
                pCompanyName.IndentationLeft = 30;
                pCompanyName.Add(p1);
                oDoc.Add(pCompanyName);
                #region subheader
                /*
                //Website
                string sWebsite = "Website: http://www.laptrinhdotnet.com";
                sText = "";
                if (!string.IsNullOrEmpty(sWebsite))
                {
                    sText = sWebsite;
                }

                if (!string.IsNullOrEmpty(sText))
                {
                    sText = sText.Replace(Environment.NewLine, string.Empty).Replace("  ", string.Empty);
                    beginning = new Chunk(sText, fontSubHeader);
                    p1 = new Phrase(beginning);
                    Paragraph pAddresse = new Paragraph();
                    pAddresse.IndentationLeft = 30;
                    pAddresse.Add(p1);
                    oDoc.Add(pAddresse);
                }

                string sEmail = "Email: kenhphanmemviet@gmail.com";
                if (!string.IsNullOrEmpty(sEmail))
                {
                    sText = sEmail;
                }

                if (!string.IsNullOrEmpty(sText))
                {
                    sText = sText.Replace(Environment.NewLine, string.Empty).Replace("  ", string.Empty);
                    beginning = new Chunk(sText, fontSubHeader);
                    p1 = new Phrase(beginning);
                    Paragraph pAddresse = new Paragraph();
                    pAddresse.IndentationLeft = 30;
                    pAddresse.Add(p1);
                    oDoc.Add(pAddresse);
                }*/
                #endregion
                //=================End Header =====================

                //Title
                sText = "ĐỀ THI CUỐI KÌ MÔN HỆ ĐIỀU HÀNH" + Environment.NewLine;
                if (!string.IsNullOrEmpty(sText))
                {
                    beginning = new Chunk(sText, fontHeader);
                    p1 = new Phrase(beginning);
                    Paragraph pAddresse = new Paragraph();
                    //pAddresse.IndentationLeft = 10;
                    pAddresse.Alignment = 1;
                    pAddresse.Add(p1);
                    oDoc.Add(pAddresse);
                }

                sText = "Mã đề: 0001" + Environment.NewLine;
                if (!string.IsNullOrEmpty(sText))
                {
                    beginning = new Chunk(sText, fontHeader);
                    p1 = new Phrase(beginning);
                    Paragraph pAddresse = new Paragraph();
                    //pAddresse.IndentationLeft = 10;
                    pAddresse.Alignment = 1;
                    pAddresse.Add(p1);
                    oDoc.Add(pAddresse);
                }

                #region image

                //Image
                //float PositionX = 35f;
                //float PositionY = 780f;
                for (int i = 0; i < 50; i++)
                {

                    sText = "Ảnh thứ:" + (i + 1)+"---"+oDoc.Top.ToString() + Environment.NewLine;
                    if (!string.IsNullOrEmpty(sText))
                    {
                        beginning = new Chunk(sText, fontHeader);
                        p1 = new Phrase(beginning);
                        Paragraph pAddresse = new Paragraph();
                        //pAddresse.IndentationLeft = 10;
                        pAddresse.Alignment = 1;
                        pAddresse.Add(p1);
                        oDoc.Add(pAddresse);
                    }
                    
                    if (File.Exists(ImagePath))
                    {
                        Image pdfImage = Image.GetInstance(ImagePath);
                        pdfImage.ScaleToFit(300, 150);
                        pdfImage.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                        //pdfImage.SetAbsolutePosition(PositionX, PositionY);
                        oDoc.Add(pdfImage);
                    }
                }
                #endregion
            }
            catch
            {
            }
            oDoc.Close();
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + "test" + ".pdf");
            Response.ContentType = "application/pdf";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(msReport.ToArray());
            Response.End();
        }
        public byte[] GetPDF(string pHTML)
        {
            byte[] bPDF = null;

            MemoryStream ms = new MemoryStream();
            TextReader txtReader = new StringReader(pHTML);

            // 1: create object of a itextsharp document class
            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file
            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

            // 3: we create a worker parse the document
            HTMLWorker htmlWorker = new HTMLWorker(doc);

            // 4: we open document and start the worker on the document
            doc.Open();

            // step 4.1: register a unicode font and assign it an allias
            FontFactory.Register("C:\\Windows\\Fonts\\ARIALUNI.TTF", "arial unicode ms");

            // step 4.2: create a style sheet and set the encoding to Identity-H
            StyleSheet ST = new StyleSheet();
            ST.LoadTagStyle("body", "encoding", "Identity-H");

            // step 4.3: assign the style sheet to the html parser
            htmlWorker.SetStyleSheet(ST);

            htmlWorker.StartDocument();

            // 5: parse the html into the document
            htmlWorker.Parse(txtReader);

            // 6: close the document and the worker
            htmlWorker.EndDocument();
            htmlWorker.Close();
            doc.Close();

            bPDF = ms.ToArray();

            return bPDF;
        }
    }


}