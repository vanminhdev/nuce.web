using DotNetNuke.Entities.Modules;
using nuce.web.data;
using System;
using System.Data;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;

namespace nuce.ad.ctsv
{
    public partial class StudentProfileEdit : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ID"] == null)
                    Response.Redirect("/tabid/21/default.aspx");
                else
                {
                    int iID = -1;
                    if (int.TryParse(Request.QueryString["ID"].Trim(), out iID))
                    {
                        string sql = "select * from AS_Academy_Student where Status<>4 and ID=" + iID;
                        DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            txtID.Text = iID.ToString();
                            txtCode.Text = dt.Rows[0]["Code"].ToString();
                             divSinhVien.InnerHtml = string.Format("Chỉnh sửa thông tin Cơ bản của sinh viên {0} - {1}", dt.Rows[0]["FulName"].ToString(), dt.Rows[0]["Code"].ToString());
                            // Xử lý dữ liệu sinh viên
                            string Email = dt.Rows[0].IsNull("Email") ? "" : dt.Rows[0]["Email"].ToString();
                            string Mobile = dt.Rows[0].IsNull("Mobile") ? "" : dt.Rows[0]["Mobile"].ToString();
                            string NgaySinh = dt.Rows[0].IsNull("NgaySinh") ? "1/1/1900" : DateTime.Parse(dt.Rows[0]["NgaySinh"].ToString()).ToString("dd/MM/yyyy");
                            string BirthPlace = dt.Rows[0].IsNull("BirthPlace") ? "" : dt.Rows[0]["BirthPlace"].ToString();
                            string DanToc = dt.Rows[0].IsNull("DanToc") ? "" : dt.Rows[0]["DanToc"].ToString();
                            string TonGiao = dt.Rows[0].IsNull("TonGiao") ? "" : dt.Rows[0]["TonGiao"].ToString();
                            string HKTT_SoNha = dt.Rows[0].IsNull("HKTT_SoNha") ? "" : dt.Rows[0]["HKTT_SoNha"].ToString();
                            string HKTT_Pho = dt.Rows[0].IsNull("HKTT_Pho") ? "" : dt.Rows[0]["HKTT_Pho"].ToString();
                            string HKTT_Phuong = dt.Rows[0].IsNull("HKTT_Phuong") ? "" : dt.Rows[0]["HKTT_Phuong"].ToString();
                            string HKTT_Quan = dt.Rows[0].IsNull("HKTT_Quan") ? "" : dt.Rows[0]["HKTT_Quan"].ToString();
                            string HKTT_Tinh = dt.Rows[0].IsNull("HKTT_Tinh") ? "" : dt.Rows[0]["HKTT_Tinh"].ToString();
                            string CMT = dt.Rows[0].IsNull("CMT") ? "" : dt.Rows[0]["CMT"].ToString();
                            string CMT_NoiCap = dt.Rows[0].IsNull("CMT_NoiCap") ? "" : dt.Rows[0]["CMT_NoiCap"].ToString();
                            string CMT_NgayCap = dt.Rows[0].IsNull("CMT_NgayCap") ? "1/1/1900" : DateTime.Parse(dt.Rows[0]["CMT_NgayCap"].ToString()).ToString("dd/MM/yyyy");
                            string NamTotNghiepPTTH = dt.Rows[0].IsNull("NamTotNghiepPTTH") ? "" : dt.Rows[0]["NamTotNghiepPTTH"].ToString();
                            string NgayVaoDoan = dt.Rows[0].IsNull("NgayVaoDoan") ? "1/1/1900" : DateTime.Parse(dt.Rows[0]["NgayVaoDoan"].ToString()).ToString("dd/MM/yyyy");
                            string NgayVaoDang = dt.Rows[0].IsNull("NgayVaoDang") ? "1/1/1900" : DateTime.Parse(dt.Rows[0]["NgayVaoDang"].ToString()).ToString("dd/MM/yyyy");
                            string DiemThiPTTH = dt.Rows[0].IsNull("DiemThiPTTH") ? "" : dt.Rows[0]["DiemThiPTTH"].ToString();
                            string KhuVucHKTT = dt.Rows[0].IsNull("KhuVucHKTT") ? "" : dt.Rows[0]["KhuVucHKTT"].ToString();
                            string DoiTuongUuTien = dt.Rows[0].IsNull("DoiTuongUuTien") ? "" : dt.Rows[0]["DoiTuongUuTien"].ToString();
                            bool DaTungLamCanBoLop = dt.Rows[0].IsNull("DaTungLamCanBoLop") ? false : bool.Parse(dt.Rows[0]["DaTungLamCanBoLop"].ToString());
                            bool DaTungLamCanBoDoan = dt.Rows[0].IsNull("DaTungLamCanBoLop") ? false : bool.Parse(dt.Rows[0]["DaTungLamCanBoDoan"].ToString());
                            bool DaThamGiaDoiTuyenThiHSG = dt.Rows[0].IsNull("DaThamGiaDoiTuyenThiHSG") ? false : bool.Parse(dt.Rows[0]["DaThamGiaDoiTuyenThiHSG"].ToString());
                            string BaoTin_DiaChi = dt.Rows[0].IsNull("BaoTin_DiaChi") ? "" : dt.Rows[0]["BaoTin_DiaChi"].ToString();
                            string BaoTin_HoVaTen = dt.Rows[0].IsNull("BaoTin_HoVaTen") ? "" : dt.Rows[0]["BaoTin_HoVaTen"].ToString();
                            string BaoTin_DiaChiNguoiNhan = dt.Rows[0].IsNull("BaoTin_DiaChiNguoiNhan") ? "" : dt.Rows[0]["BaoTin_DiaChiNguoiNhan"].ToString();
                            string BaoTin_SoDienThoai = dt.Rows[0].IsNull("BaoTin_SoDienThoai") ? "" : dt.Rows[0]["BaoTin_SoDienThoai"].ToString();
                            string BaoTin_Email = dt.Rows[0].IsNull("BaoTin_Email") ? "" : dt.Rows[0]["BaoTin_Email"].ToString();
                            bool LaNoiTru = dt.Rows[0].IsNull("LaNoiTru") ? false : bool.Parse(dt.Rows[0]["LaNoiTru"].ToString());
                            string DiaChiCuThe = dt.Rows[0].IsNull("DiaChiCuThe") ? "" : dt.Rows[0]["DiaChiCuThe"].ToString();

                            string EmailNhaTruong = dt.Rows[0].IsNull("EmailNhaTruong") ? "" : dt.Rows[0]["EmailNhaTruong"].ToString();
                            string GioiTinh = dt.Rows[0].IsNull("GioiTinh") ? "" : dt.Rows[0]["GioiTinh"].ToString();
                            bool DaXacThucEmailNhaTruong = dt.Rows[0].IsNull("DaXacThucEmailNhaTruong") ? false : bool.Parse(dt.Rows[0]["DaXacThucEmailNhaTruong"].ToString());

                            string File1 = dt.Rows[0].IsNull("File1") ? "" : dt.Rows[0]["File1"].ToString();
                            string File2 = dt.Rows[0].IsNull("File2") ? "" : dt.Rows[0]["File2"].ToString();

                            txtEmail.Text = Email;
                            txtMobile.Text = Mobile;
                            txtNgaySinh.Text = NgaySinh;
                            txtNoiSinh.Text = BirthPlace;
                            txtDanToc.Text = DanToc;
                            txtTonGiao.Text = TonGiao;
                            txtHKTT_SoNha.Text = HKTT_SoNha;
                            txtHKTT_Pho.Text = HKTT_Pho;
                            txtHKTT_Phuong.Text = HKTT_Phuong;
                            txtHKTT_Huyen.Text = HKTT_Quan;
                            txtHKTT_Tinh.Text = HKTT_Tinh;
                            txtCMT.Text = CMT;
                            txtCMTNGayCap.Text = CMT_NgayCap;
                            txtCMTNoiCap.Text = CMT_NoiCap;
                            txtNamTotNghiepPTTH.Text = NamTotNghiepPTTH;
                            txtNgayVaoDoan.Text = NgayVaoDoan;
                            txtNgayVaoDang.Text = NgayVaoDang;
                            txtDiemThiPTTH.Text = DiemThiPTTH;
                            txtKhuVucHKTT.Text = KhuVucHKTT;
                            txtDoiTuongUuTien.Text = DoiTuongUuTien;
                            chkDaTungLamCanBoLop.Checked = DaTungLamCanBoLop;
                            chkDatTungLamCanBoDoan.Checked = DaTungLamCanBoDoan;
                            chkDaThamGiaDoiTuyenThiHSG.Checked = DaThamGiaDoiTuyenThiHSG;
                            txtBaoTin_DiaChi.Text = BaoTin_DiaChi;
                            txtBaoTin_DiaChiNguoiNhan.Text = BaoTin_DiaChiNguoiNhan;
                            txtBaoTin_HoVaTen.Text = BaoTin_HoVaTen;
                            txtBaoTin_SoDienThoai.Text = BaoTin_SoDienThoai;
                            txtBaoTin_Email.Text = BaoTin_Email;
                            chkLaNoiTru.Checked = LaNoiTru;
                            txtDiaChiCuThe.Text = DiaChiCuThe;
                            if(!File1.Trim().Equals(""))
                            {
                                imgAnh.Src = File1;
                            }
                            if (!File2.Trim().Equals(""))
                            {
                                spHoSoDinhKem.InnerHtml = string.Format("<a href='{0}'>File đính kèm</a>",File2);
                            }

                            txtEmailNhaTruong.Text = EmailNhaTruong;
                            txtGioiTinh.Text = GioiTinh;
                            chkDaXacThucEmailNhaTruong.Checked = DaXacThucEmailNhaTruong;
                        }
                        else
                        {
                            Response.Redirect("/tabid/21/default.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("/tabid/21/default.aspx");
                    }
                }
            }
        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            divThongBao.InnerText = "";
            string Email = txtEmail.Text.Trim();
            if (!IsValidEmail(Email))
            {
                divThongBao.InnerText = "Email sai quy cách";
                return;
            }
            string Mobile = txtMobile.Text.Trim();
            if(!IsMobile(Mobile))
            {
                divThongBao.InnerText = "Mobile sai quy cách";
                return;
            }
            DateTime dtDefault = DateTime.Parse("1/1/1900");
            DateTime dtNgaySinh = dtDefault;
            DateTime.TryParseExact(txtNgaySinh.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtNgaySinh);
            DateTime dtCMTNgayCap = dtDefault;
            DateTime.TryParseExact(txtCMTNGayCap.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtCMTNgayCap);
            DateTime dtNgayVaoDoan = dtDefault;
            DateTime.TryParseExact(txtNgayVaoDoan.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtNgayVaoDoan);
            DateTime dtNgayVaoDang = dtDefault;
            DateTime.TryParseExact(txtNgayVaoDang.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtNgayVaoDang);
            if (dtNgaySinh < dtDefault)
                dtNgaySinh = dtDefault;
            if (dtCMTNgayCap < dtDefault)
                dtCMTNgayCap = dtDefault;
            if (dtNgayVaoDoan < dtDefault)
                dtNgayVaoDoan = dtDefault;
            if (dtNgayVaoDang < dtDefault)
                dtNgayVaoDang = dtDefault;
            string sql = string.Format(@"UPDATE [dbo].[AS_Academy_Student]
   SET [DateOfBirth] = '{0}',[BirthPlace] = N'{1}',[Email] = N'{2}'
      ,[Mobile] = N'{3}',[UpdatedDate] = N'{4:}',[NgaySinh] = N'{5}',[DanToc] = N'{6}',[TonGiao] = N'{7}'
      ,[HKTT_SoNha] = N'{8}',[HKTT_Pho] = N'{9}',[HKTT_Phuong] = N'{10}',[HKTT_Quan] = N'{11}'
      ,[HKTT_Tinh] =N'{12}',[CMT] = N'{13}',[CMT_NgayCap] = '{14}',[CMT_NoiCap] = N'{15}'
      ,[NamTotNghiepPTTH] =N'{16}',[NgayVaoDoan] = N'{17}',[NgayVaoDang] = N'{18}'
      ,[DiemThiPTTH] = N'{19}' ,[KhuVucHKTT] =N'{20}',[DoiTuongUuTien] = N'{21}',[DaTungLamCanBoLop] = {22}
      ,[DaTungLamCanBoDoan] = {23},[DaThamGiaDoiTuyenThiHSG] = {24},[BaoTin_DiaChi] =  N'{25}'
      ,[BaoTin_HoVaTen] = N'{26}',[BaoTin_DiaChiNguoiNhan] =  N'{27}',[BaoTin_SoDienThoai] =  N'{28}'
      ,[BaoTin_Email] =  N'{29}',[LaNoiTru] = {30},[DiaChiCuThe] =  N'{31}',[File1] =  N'{32}'
      ,[File2] =  N'{33}',[File3] =  N'{34}',[Mobile1] =  N'{35}',[Email1] =  N'{36}',[GioiTinh] =  N'{37}',[EmailNhaTruong] =  N'{38}',[DaXacThucEmailNhaTruong] =  N'{39}' WHERE ID={40}",
      dtNgaySinh, txtNoiSinh.Text.Trim(), Email, Mobile,DateTime.Now, dtNgaySinh, txtDanToc.Text.Trim(),txtTonGiao.Text.Trim(),
      txtHKTT_SoNha.Text.Trim(),txtHKTT_Pho.Text.Trim(),txtHKTT_Phuong.Text.Trim(),txtHKTT_Huyen.Text.Trim(),
      txtHKTT_Tinh.Text.Trim(),txtCMT.Text.Trim(), dtCMTNgayCap,txtCMTNoiCap.Text.Trim(),
      txtNamTotNghiepPTTH.Text.Trim(), dtNgayVaoDoan,dtNgayVaoDang,
      txtDiemThiPTTH.Text.Trim(),txtKhuVucHKTT.Text.Trim(),txtDoiTuongUuTien.Text.Trim(),chkDaTungLamCanBoLop.Checked?1:0,
      chkDatTungLamCanBoDoan.Checked?1:0,chkDaThamGiaDoiTuyenThiHSG.Checked?1:0,txtBaoTin_DiaChi.Text.Trim()
      ,txtBaoTin_HoVaTen.Text.Trim(),txtBaoTin_DiaChiNguoiNhan.Text.Trim(),txtBaoTin_SoDienThoai.Text.Trim()
      ,txtBaoTin_Email.Text.Trim(),chkLaNoiTru.Checked?1:0,txtDiaChiCuThe.Text.Trim(),"",
      "","","","", txtGioiTinh.Text.Trim(), txtEmailNhaTruong.Text.Trim(), chkDaXacThucEmailNhaTruong.Checked ? 1 : 0, txtID.Text.Trim());
            int iReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(Nuce_Common.ConnectionString, CommandType.Text, sql);
            if(iReturn>0)
            {
                //divThongBao.InnerText = "Cập nhật thành công";
                //Send_Email("ThangHNTest", "ThangHN", Email);
                //return;
                //Cap nhat file 
                if (FileUploadControl.HasFile)
                {
                    try
                    {
                        string strFileGioiHan = "image/bmp;image/gif;image/jpeg;image/jpg;image/png;image/tif;";
                        if (strFileGioiHan.Contains(FileUploadControl.PostedFile.ContentType))
                        {
                            if (FileUploadControl.PostedFile.ContentLength / 1048576.0 < 5)
                            {
                                string strMoRongFile = Path.GetExtension(FileUploadControl.FileName);
                                string filename = Path.GetFileName(FileUploadControl.FileName);
                                string strTenMoi = txtCode.Text.Trim();
                                string directoryPath = Server.MapPath("~/Data/images");
                                if (!System.IO.Directory.Exists(directoryPath))
                                    System.IO.Directory.CreateDirectory(directoryPath);

                                string path = directoryPath + "\\" + strTenMoi +  strMoRongFile;
                                FileInfo file = new FileInfo(path);
                                if (file.Exists)//check file exsit or not
                                {
                                    file.Delete();
                                }
                                FileUploadControl.SaveAs(path);
                                string link = "/Data/images/" + strTenMoi +  strMoRongFile;
                                imgAnh.Src = link;
                                sql = string.Format(@"UPDATE [dbo].[AS_Academy_Student] SET File1=N'{0}' where ID={1}", link, txtID.Text.Trim());
                                iReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(Nuce_Common.ConnectionString, CommandType.Text, sql);
                                if (iReturn > 0)
                                {
                                    // Cap nhat file dinh kem
                                    if (fuHoSoDinhKem.HasFile)
                                    {
                                        try
                                        {
                                            strFileGioiHan = "image/bmp;image/gif;image/jpeg;image/jpg;image/png;image/tif;application/zip;application/zar;";
                                            if (strFileGioiHan.Contains(fuHoSoDinhKem.PostedFile.ContentType))
                                            {
                                                if (fuHoSoDinhKem.PostedFile.ContentLength / 1048576.0 < 5)
                                                {
                                                    strMoRongFile = Path.GetExtension(fuHoSoDinhKem.FileName);
                                                    filename = Path.GetFileName(fuHoSoDinhKem.FileName);
                                                    strTenMoi = "file2";
                                                    directoryPath = Server.MapPath("~/Data/FileDinhKem/" + txtCode.Text.Trim());
                                                    if (!System.IO.Directory.Exists(directoryPath))
                                                        System.IO.Directory.CreateDirectory(directoryPath);

                                                    path = directoryPath + "\\" + strTenMoi  + strMoRongFile;
                                                    file = new FileInfo(path);
                                                    if (file.Exists)//check file exsit or not
                                                    {
                                                        file.Delete();
                                                    }
                                                    fuHoSoDinhKem.SaveAs(path);
                                                    link = "/Data/FileDinhKem/" + txtCode.Text.Trim() + "/" + strTenMoi + strMoRongFile;
                                                    spHoSoDinhKem.InnerHtml = string.Format("<a href='{0}'>File đính kèm</a>", link);
                                                    sql = string.Format(@"UPDATE [dbo].[AS_Academy_Student] SET File2=N'{0}' where ID={1}", link, txtID.Text.Trim());
                                                    iReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(Nuce_Common.ConnectionString, CommandType.Text, sql);
                                                    if (iReturn > 0)
                                                    {
                                                        // Cap nhat file dinh kem
                                                        divThongBao.InnerHtml = "Cập nhật thông tin thành công";
                                                        return;

                                                    }
                                                    else
                                                    {
                                                        divThongBao.InnerHtml = "Cập nhật thông tin cơ bản,ảnh thành công, File không cập nhật vì Lỗi hệ thống";
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    divThongBao.InnerHtml = "Cập nhật thông tin cơ bản,ảnh thành công, File không cập nhật vì File đính kèm không được Vượt quá 5MB";
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                divThongBao.InnerHtml = "Cập nhật thông tin cơ bản,ảnh thành công, File không cập nhật vì File upload Không đúng định dạng";
                                                return;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            divThongBao.InnerHtml = "Cập nhật thông tin cơ bản,ảnh thành công, File không cập nhật vì Lỗi hệ thống";
                                            return;
                                        }

                                    }
                                    else
                                    {
                                        divThongBao.InnerHtml = "Cập nhật thông tin cơ bản thành công, ảnh không cập nhật vì Lỗi hệ thống";
                                        return;
                                    }
                                }
                                else
                                {
                                    divThongBao.InnerHtml = "Cập nhật thông tin cơ bản thành công, ảnh không cập nhật vì Ảnh đính kèm không được Vượt quá 5MB";
                                    return;
                                }
                            }
                            else
                            {
                                divThongBao.InnerHtml = "Cập nhật thông tin cơ bản thành công, ảnh không cập nhật vì File upload không phải là ảnh";
                                return;
                            }
                        }
                        else
                        {
                            divThongBao.InnerHtml = "Cập nhật thông tin thành công";
                            return;
                        }
                    }
                    catch 
                    {
                        divThongBao.InnerHtml = "Cập nhật thông tin cơ bản thành công, có lỗi hệ thống cập nhạt file";
                        return;
                    }
                }
                else
                {
                    divThongBao.InnerHtml = "Cập nhật thông tin thành công";
                    return;
                }
            }
            else
            {
                divThongBao.InnerText = "Cập nhật thất bại - lỗi hệ thống";
                return;
            }
           
        }
        #region email
        public static string Send_Email(string Subject, string Message, string EmailTo)
        {
            try
            {
                // Gui Mail
                string strSubject = Subject;
                string strMessage = Message;
                string strEmailTo = EmailTo;

                string strEmailSend = "ks.ktdb@nuce.edu.vn";
                string strSmpt = "smtp.gmail.com";
                int iPort = 587;
                //int iIsSSL = 1;
                bool isSSL = true;
                string strUserNameSend = "ks.ktdb@nuce.edu.vn";
                string strPasswordSend = "khaosatktdb@123";
                //khaosatktdb@123
                return Send_Email(strEmailSend, strEmailTo, strSubject, strMessage, strSmpt, iPort, isSSL, strUserNameSend, strPasswordSend);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public static string Send_Email(string SendFrom, string SendTo, string Subject, string Body, string smtpAdr, int port, bool isSSL, string UserName, string Password)
        {
            try
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");


                bool result = regex.IsMatch(SendTo);
                if (result == false)
                {
                    return "Địa chỉ email không hợp lệ.";
                }
                else
                {
                    System.Net.Mail.SmtpClient smtp = new SmtpClient();
                    System.Net.Mail.MailMessage msg = new MailMessage(SendFrom, SendTo, Subject, Body);
                    msg.IsBodyHtml = true;
                    smtp.Host = smtpAdr;//"smtp.gmail.com";//Sử dụng SMTP của gmail 
                    smtp.Port = port;
                    smtp.Credentials = new System.Net.NetworkCredential(UserName, Password);
                    smtp.EnableSsl = isSSL;
                    smtp.Send(msg);
                    return "Email đã được gửi đến: " + SendTo + ".";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public static string Send_Email(string SendFrom, string SendTo, string Subject, string Body, bool isHtmlBody, string smtpAdr, int port, bool isSSL, string UserName, string Password)
        {
            try
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");


                bool result = regex.IsMatch(SendTo);
                if (result == false)
                {
                    return "Địa chỉ email không hợp lệ.";
                }
                else
                {
                    System.Net.Mail.SmtpClient smtp = new SmtpClient();
                    System.Net.Mail.MailMessage msg = new MailMessage(SendFrom, SendTo, Subject, Body);
                    msg.IsBodyHtml = isHtmlBody;
                    smtp.Host = smtpAdr;//"smtp.gmail.com";//Sử dụng SMTP của gmail 
                    smtp.Port = port;
                    smtp.Credentials = new System.Net.NetworkCredential(UserName, Password);
                    smtp.EnableSsl = isSSL;
                    smtp.Send(msg);
                    return "Email đã được gửi đến: " + SendTo + ".";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public static string Send_Email_With_Attachment(string SendTo, string SendFrom, string Subject, string Body, string AttachmentPath, string smtpAdr, int port, bool isSSL, string UserName, string Password)
        {
            try
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                string from = SendFrom;
                string to = SendTo;
                string subject = Subject;
                string body = Body;
                bool result = regex.IsMatch(to);
                if (result == false)
                {
                    return "Địa chỉ email không hợp lệ.";
                }
                else
                {
                    try
                    {
                        MailMessage em = new MailMessage(from, to, subject, body);
                        Attachment attach = new Attachment(AttachmentPath);
                        em.Attachments.Add(attach);
                        em.Bcc.Add(from);
                        System.Net.Mail.SmtpClient smtp = new SmtpClient();
                        smtp.Host = smtpAdr;//"smtp.gmail.com";//Ví dụ xử dụng SMTP của gmail 
                        smtp.Port = port;
                        smtp.EnableSsl = isSSL;
                        smtp.Credentials = new System.Net.NetworkCredential(UserName, Password);
                        smtp.Send(em);
                        return "";
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string Send_Email_With_BCC_Attachment(string SendTo, string SendBCC, string SendFrom, string Subject, string Body, string AttachmentPath, string smtpAdr, int port, bool isSSL, string UserName, string Password)
        {
            try
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                string from = SendFrom;
                string to = SendTo; //Danh sách email được ngăn cách nhau bởi dấu ";" 
                string subject = Subject;
                string body = Body;
                string bcc = SendBCC;
                bool result = true;
                String[] ALL_EMAILS = to.Split(';');
                foreach (string emailaddress in ALL_EMAILS)
                {
                    result = regex.IsMatch(emailaddress);
                    if (result == false)
                    {
                        return "Địa chỉ email không hợp lệ.";
                    }
                }
                if (result == true)
                {
                    try
                    {
                        MailMessage em = new MailMessage(from, to, subject, body);
                        Attachment attach = new Attachment(AttachmentPath);
                        em.Attachments.Add(attach);
                        em.Bcc.Add(bcc);


                        System.Net.Mail.SmtpClient smtp = new SmtpClient();
                        smtp.Host = smtpAdr;//"smtp.gmail.com";//Ví dụ xử dụng SMTP của gmail 
                        smtp.Port = port;
                        smtp.EnableSsl = isSSL;
                        smtp.Credentials = new System.Net.NetworkCredential(UserName, Password);
                        smtp.Send(em);

                        return "";
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
        #region validate
        public  bool IsValidEmail(string emailaddress)
        {
            if (emailaddress.Length == 0)
                return false;
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public  bool IsMobile(string mobile)
        {
            if (mobile.Length == 0)
                return false;
            try
            {

                Regex digitsOnly = new Regex(@"[^\d]");
                string _mobile = digitsOnly.Replace(mobile, "");
                if (_mobile[0] != '0')
                    return false;
                if (_mobile.Length < 10 || _mobile.Length > 11)
                    return false;
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        #endregion
    }
}