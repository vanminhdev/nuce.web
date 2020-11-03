using ClosedXML.Excel;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace nuce.web.data
{
    public static class Nuce_CTSV
    {
        #region Conection
        private static string m_strConnectionString = System.Configuration.ConfigurationSettings.AppSettings["DB"];
        public static string ConnectionString
        {
            get
            {
                return m_strConnectionString;
            }
        }

        private static string m_strPoolingConnectionString = System.Configuration.ConfigurationSettings.AppSettings["DB"];
        public static string PoolingConnectionString
        {
            get
            {
                return m_strPoolingConnectionString;
            }
        }

        /// <summary>
        /// Return a database connection
        /// </summary>
        /// <returns>System.Data.SqlClient.SqlConnection</returns>
        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection objConnection = new SqlConnection();
                try
                {
                    objConnection.ConnectionString = PoolingConnectionString;
                    objConnection.Open();
                }
                catch (Exception)
                {
                    if (objConnection.State != ConnectionState.Closed)
                        objConnection.Close();
                    //Open new connection if all the connections are being used
                    objConnection.ConnectionString = ConnectionString;
                    objConnection.Open();
                }

                return objConnection;
            }
            catch
            {
                return null;
            }
        }
        #endregion
        #region Logs
        public static void AS_Logs_Insert(int userid, string usercode, int status, string code, string message)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"INSERT INTO [dbo].[AS_Logs]
           ([UserId]
           ,[UserCode]
           ,[Status]
           ,[Code]
           ,[Message]
           ,[CreatedTime])
     VALUES
           (@Param0
           ,@Param1
           ,@Param2
           ,@Param3
           ,@Param4
           ,@Param5)");
                    SqlParameter[] sqlParams = new SqlParameter[6];
                    sqlParams[0] = new SqlParameter("@Param0", userid);
                    sqlParams[1] = new SqlParameter("@Param1", usercode);
                    sqlParams[2] = new SqlParameter("@Param2", status);
                    sqlParams[3] = new SqlParameter("@Param3", code);
                    sqlParams[4] = new SqlParameter("@Param4", message);
                    sqlParams[5] = new SqlParameter("@Param5", DateTime.Now);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql, sqlParams);
                    return;
                }
                catch
                {
                    return;
                }
            }

        }

        public static void AS_Academy_Student_TinNhan_Insert(string Code, int SenderID, string StudentCode, string StudentName, string Receiver, string ReceiverEmail, string NoiDung
            , int Type, int UserID)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"INSERT INTO [dbo].[AS_Academy_Student_TinNhan]
           ([Code]
           ,[SenderID]
           ,[StudentCode]
           ,[StudentName]
           ,[receiver]
           ,[receiverEmail]
           ,[Content]
           ,[Type]
           ,[Status]
           ,[Deleted]
           ,[CreatedBy]
           ,[LastModifiedBy]
           ,[DeletedBy]
           ,[CreatedTime]
           ,[DeletedTime]
           ,[LastModifiedTime])
     VALUES
           (@Param0
           ,@Param1
           ,@Param2
           ,@Param3
           ,@Param4
           ,@Param5
           ,@Param6
           ,@Param7
           ,1
           ,0
           ,@Param8
           ,@Param8
           ,@Param8
           ,@Param9
           ,@Param9
           ,@Param9)");
                    SqlParameter[] sqlParams = new SqlParameter[10];
                    sqlParams[0] = new SqlParameter("@Param0", Code);
                    sqlParams[1] = new SqlParameter("@Param1", SenderID);
                    sqlParams[2] = new SqlParameter("@Param2", StudentCode);
                    sqlParams[3] = new SqlParameter("@Param3", StudentName);
                    sqlParams[4] = new SqlParameter("@Param4", Receiver);
                    sqlParams[5] = new SqlParameter("@Param5", ReceiverEmail);
                    sqlParams[6] = new SqlParameter("@Param6", NoiDung);
                    sqlParams[7] = new SqlParameter("@Param7", Type);
                    sqlParams[8] = new SqlParameter("@Param8", UserID);
                    sqlParams[9] = new SqlParameter("@Param9", DateTime.Now);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql, sqlParams);
                    return;
                }
                catch
                {
                    return;
                }
            }

        }
        public static void AS_Academy_Student_TinNhan_Insert(string Code, int SenderID, string StudentCode, string StudentName, string Receiver, string ReceiverEmail, string NoiDung
          , int Type, int UserID, string Title)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"INSERT INTO [dbo].[AS_Academy_Student_TinNhan]
           ([Code]
           ,[SenderID]
           ,[StudentCode]
           ,[StudentName]
           ,[receiver]
           ,[receiverEmail]
           ,[Content]
           ,[Type]
           ,[Status]
           ,[Deleted]
           ,[CreatedBy]
           ,[LastModifiedBy]
           ,[DeletedBy]
           ,[CreatedTime]
           ,[DeletedTime]
           ,[LastModifiedTime]
            ,[Title])
     VALUES
           (@Param0
           ,@Param1
           ,@Param2
           ,@Param3
           ,@Param4
           ,@Param5
           ,@Param6
           ,@Param7
           ,1
           ,0
           ,@Param8
           ,@Param8
           ,@Param8
           ,@Param9
           ,@Param9
           ,@Param9
            ,@Param10
            )");
                    SqlParameter[] sqlParams = new SqlParameter[11];
                    sqlParams[0] = new SqlParameter("@Param0", Code);
                    sqlParams[1] = new SqlParameter("@Param1", SenderID);
                    sqlParams[2] = new SqlParameter("@Param2", StudentCode);
                    sqlParams[3] = new SqlParameter("@Param3", StudentName);
                    sqlParams[4] = new SqlParameter("@Param4", Receiver);
                    sqlParams[5] = new SqlParameter("@Param5", ReceiverEmail);
                    sqlParams[6] = new SqlParameter("@Param6", NoiDung);
                    sqlParams[7] = new SqlParameter("@Param7", Type);
                    sqlParams[8] = new SqlParameter("@Param8", UserID);
                    sqlParams[9] = new SqlParameter("@Param9", DateTime.Now);
                    sqlParams[10] = new SqlParameter("@Param10", Title);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql, sqlParams);
                    return;
                }
                catch
                {
                    return;
                }
            }

        }
        #endregion
        #region Helper dịch vụ
        public static string UpdateRequestStatus(string strID, string strStatus, string strPhanHoi, string strNgayBatDau, string strNgayKetThuc, string strType)
        {
            string result = "";
            int iType = 1;
            int.TryParse(strType, out iType);
            DateTime dtNgayBatDau = DateTime.Parse("1/1/1990");
            DateTime.TryParseExact(strNgayBatDau, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtNgayBatDau);
            DateTime dtNgayKetThuc = DateTime.Parse("1/1/1990");
            DateTime.TryParseExact(strNgayKetThuc, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtNgayKetThuc);
            if (dtNgayBatDau < DateTime.Now)
            {
                dtNgayBatDau = DateTime.Parse("1/1/1990");
            }
            if (dtNgayKetThuc < DateTime.Now)
            {
                dtNgayKetThuc = DateTime.Parse("1/1/1990");
            }
            int status = 1;
            int.TryParse(strStatus, out status);
            string sql = "";
            if (nuce.web.data.DataUtils.LoaiDichVuSinhViens.ContainsKey(iType))
            {

                sql = string.Format(@"UPDATE [dbo].[{0}]
   SET  Status=@Param0, PhanHoi=@Param1, NgayHen_TuNgay=@Param2,NgayHen_DenNgay=@Param3
 WHERE ID=@Param4 ; 
select * from [dbo].[{0}]  
WHERE ID=@Param4;", nuce.web.data.DataUtils.LoaiDichVuSinhViens[iType].Param1);
                if (status.Equals(4))
                {
                    //xử lý ngày hẹn là thứ 6 và ngày kết thúc là 1 tháng sau thứ 6.
                    // (int)DateTime.Now.DayOfWeekime.Now.DayOfWeek;
                    int Day = (int)DateTime.Now.DayOfWeek;
                    int Hour = DateTime.Now.Hour;
                    //dtNgayBatDau = DateTime.Now;
                    if (Hour <= 13)
                    {
                        if (Day < 5)
                        {
                            dtNgayBatDau = DateTime.Parse(string.Format("{0}/{1}/{2} 10:00:00 AM", DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, DateTime.Now.AddDays(1).Year));
                        }
                        else
                        {
                            dtNgayBatDau = DateTime.Parse(string.Format("{0}/{1}/{2} 10:00:00 AM", DateTime.Now.AddDays(8 - Day).Month, DateTime.Now.AddDays(8 - Day).Day, DateTime.Now.AddDays(8 - Day).Year));
                        }
                    }
                    else
                    {
                        //Cap nhat vào buổi chiều
                        if (Day < 5)
                        {
                            dtNgayBatDau = DateTime.Parse(string.Format("{0}/{1}/{2} 4:00:00 PM", DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, DateTime.Now.AddDays(1).Year));
                        }
                        else
                        {
                            dtNgayBatDau = DateTime.Parse(string.Format("{0}/{1}/{2} 10:00:00 AM", DateTime.Now.AddDays(8 - Day).Month, DateTime.Now.AddDays(8 - Day).Day, DateTime.Now.AddDays(8 - Day).Year));
                        }
                    }
                    dtNgayKetThuc = dtNgayBatDau.AddMonths(1);
                }
                SqlParameter[] sqlParams = new SqlParameter[5];
                sqlParams[0] = new SqlParameter("@Param0", status);
                sqlParams[1] = new SqlParameter("@Param1", strPhanHoi);
                sqlParams[2] = new SqlParameter("@Param2", dtNgayBatDau);
                sqlParams[3] = new SqlParameter("@Param3", dtNgayKetThuc);
                sqlParams[4] = new SqlParameter("@Param4", strID);

                DataTable dtReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql, sqlParams).Tables[0];
                result = dtReturn.Rows.Count.ToString();

                if (dtReturn.Rows.Count > 0 && status < 7 && status > 2)
                {
                    // Lay thong tin sinh vien
                    sql = string.Format("select EmailNhaTruong,ID,Code from AS_Academy_Student where ID in (select StudentID from {0} where ID=@Param0)", nuce.web.data.DataUtils.LoaiDichVuSinhViens[iType].Param1);
                    sqlParams = new SqlParameter[1];
                    sqlParams[0] = new SqlParameter("@Param0", strID);
                    DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql, sqlParams).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        string strEmail = dt.Rows[0]["EmailNhaTruong"].ToString();
                        string strIDSV = dt.Rows[0]["ID"].ToString();
                        string strCode = dt.Rows[0]["Code"].ToString();
                        // gui email
                        string strTieuDe = string.Format("Thông báo về việc {0} {1}", DataUtils.TrangThaiDichVuSinhViens[status].Name, DataUtils.LoaiDichVuSinhViens[iType].Name);
                        string strNoiDung = string.Format("<div style='color:black;'><div style='padding:5px;'>Xin chào: {0}</div>", dtReturn.Rows[0]["StudentName"].ToString());
                        strNoiDung += string.Format("<div style='padding:5px;'>Hệ thống Quản lý thông tin sinh viên xin thống báo về việc {0} {1}, bạn đã tạo lúc {2:dd/MM/yyyy HH:mm}.</div>", DataUtils.TrangThaiDichVuSinhViens[status].Name, DataUtils.LoaiDichVuSinhViens[iType].Name, DateTime.Parse(dtReturn.Rows[0]["CreatedTime"].ToString()));
                        if (status.Equals(4))
                        {
                            //strNoiDung += string.Format("<div style='padding:5px;'>Anh/chị vui lòng qua phòng CTVS&QLSV từ <b>{0}</b> giờ <b>{1}</b> phút ngày {2:dd/MM/yyyy} đến <b>{3}</b> giờ <b>{4}</b> phút ngày {5:dd/MM/yyyy} để hoàn tất dịch vụ.</div>", dtNgayBatDau.Hour, dtNgayBatDau.Minute,  dtNgayBatDau, dtNgayKetThuc.Hour, dtNgayKetThuc.Minute, dtNgayKetThuc);
                            strNoiDung += string.Format("<div style='padding:5px;'>Anh/chị vui lòng qua phòng CTVS&QLSV từ <b>{0}</b> giờ <b>{1}</b> phút ngày {2:dd/MM/yyyy} để hoàn tất dịch vụ.</div>", dtNgayBatDau.Hour, dtNgayBatDau.Minute, dtNgayBatDau);
                            strNoiDung += "<div style='padding:5px;'><i>Chú ý: <b>Sinh viên kiểm tra lại thông tin của mình trên giấy tờ khi nhận để điều chỉnh kịp thời nếu có thay đổi.</b></i></div>";
                        }
                        strNoiDung += "<div style='padding:5px;'>Trân trọng</div>";
                        strNoiDung += "<div style='padding:5px;'>----------------------</div>";
                        strNoiDung += "<div style='padding:5px;'>Phòng Công tác Chính trị và Quản lý Sinh viên Trường Đại học Xây dựng</div>";
                        strNoiDung += "<div style='padding:5px;'>Phòng 302 - 303 Tòa nhà A1 Trường đại học Xây Dựng, 55 Giải Phóng - Hai Bà Trưng - Hà Nội</div>";
                        strNoiDung += "<div style='padding:5px;'></div>";
                        strNoiDung += "<div style='padding:5px;'>TEL: 02438697004</div>";
                        strNoiDung += "<div style='padding:5px;'>Email: ctsv@nuce.edu.vn</div>";
                        strNoiDung += "</div>";
                        nuce.web.data.Nuce_CTSV.AS_Academy_Student_TinNhan_Insert(DataUtils.LoaiDichVuSinhViens[iType].Code + "_CHUYENTRANGTHAI_" + status.ToString(), -1, strCode, dtReturn.Rows[0]["StudentName"].ToString(), strCode, strEmail, strNoiDung, 1, int.Parse(strIDSV), strTieuDe);
                        //string strReturn = nuce.web.tienich.email.Send_Email(strTieuDe, strNoiDung, strEmail, "ctsv.hotro@nuce.edu.vn", "ctsv@123456a");
                        //data.Nuce_CTSV.AS_Logs_Insert(int.Parse(strID), strCode, 1, nuce.web.data.DataUtils.LoaiDichVuSinhViens[iType].Param1 +"_status_"+ DataUtils.TrangThaiDichVuSinhViens[status].ID, strReturn);
                    }
                    //spThongBao.InnerHtml = "Cập nhật dữ liệu email thành công. </br> email xác thực đã được gửi.";
                }
            }
            return result;
        }
        public static string firstOrDefault(DataRowCollection rows, int row, string fieldName)
        {
            return rows[row].IsNull(fieldName) ? "" : rows[row][fieldName].ToString();
        }
        public static void setStyle(IXLWorksheet ws, int row, int col, string value)
        {
            initHeaderCell(ws, row, col, value);
            ws.Cell(row, col).Style.Fill.SetBackgroundColor(XLColor.Yellow);
        }
        public static void setStyleUniqueCol(IXLWorksheet ws, int row, int col, string value)
        {
            initHeaderCell(ws, row, col, value);
            ws.Cell(row, col).Style.Fill.SetBackgroundColor(XLColor.White);
        }
        public static void initHeaderCell(IXLWorksheet ws, int row, int col, string value)
        {
            ws.Cell(row, col).SetValue(value);
            ws.Cell(row, col).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell(row, col).Style.Font.Bold = true;
        }
        public static void updateStudent(JsonData student)
        {
            string sql = getSqlStringUpdate(student);
            SqlHelper.ExecuteNonQuery(Nuce_Common.ConnectionString, CommandType.Text, sql);
        }
        public static void updateStudentList(List<JsonData> students)
        {
            List<int> updatedStudent = new List<int>();
            string sql = "";
            foreach (var item in students)
            {
                if (!updatedStudent.Contains(item.StudentID))
                {
                    sql += getSqlStringUpdate(item);
                    updatedStudent.Add(item.StudentID);
                }
            }
            SqlHelper.ExecuteNonQuery(Nuce_Common.ConnectionString, CommandType.Text, sql);
        }
        public static string getNamThu(string NienKhoa)
        {
            string[] strNamhocs = NienKhoa.Split(new char[] { '-' });
            string nambatdau = strNamhocs[0].Trim();
            int iNamBatDau = int.Parse(nambatdau);
            int iNamHienTai = DateTime.Now.Year;
            if (DateTime.Now.Month > 8)
                iNamHienTai++;
            int iReturn = iNamHienTai - iNamBatDau;
            iReturn = iReturn > 5 ? 5 : iReturn;
            return iReturn.ToString();
        }
        public static string getThoiGianKhoaHoc(string NienKhoa)
        {
            string[] strNamhocs = NienKhoa.Split(new char[] { '-' });
            string nambatdau = strNamhocs[0].Trim();
            string namKetThuc = strNamhocs[1].Trim();
            int iNamBatDau = int.Parse(nambatdau);
            int iNamKetThuc = int.Parse(namKetThuc);
            return (iNamKetThuc - iNamBatDau).ToString();
        }
        public static string getNamNhapHoc(string NienKhoa)
        {
            string[] strNamhocs = NienKhoa.Split(new char[] { '-' });
            return strNamhocs[0].Trim();
        }
        public static string getNamRaTruong(string NienKhoa)
        {
            string[] strNamhocs = NienKhoa.Split(new char[] { '-' });
            return strNamhocs[1].Trim();
        }
        public static bool isDuplicated(int dichvuType, int svId, string lydo = "")
        {
            string tblName = nuce.web.data.DataUtils.LoaiDichVuSinhViens[dichvuType].Param1;
            string sqlDuplicate = $@"SELECT * FROM {tblName} AS AASSXN
                                WHERE AASSXN.Status < 4 AND StudentID = {svId} ";
            if (dichvuType == 1)
            {
                sqlDuplicate += $"AND AASSXN.LyDo = N'{lydo}'";
            }
            DataSet ds = SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sqlDuplicate);
            bool result = ds.Tables[0].Rows.Count > 0;
            return result;
        }
        private static string getSqlStringUpdate(JsonData item)
        {
            return $@"UPDATE [dbo].[AS_Academy_Student]
                           SET  HKTT_Pho=N'{item.HKTT_Pho}', 
                                HKTT_Phuong=N'{item.HKTT_Phuong}', 
                                HKTT_Quan=N'{item.HKTT_Quan}',
                                HKTT_Tinh=N'{item.HKTT_Tinh}'
                         WHERE ID={item.StudentID}; ";
        }
        public static string getGender(string gender)
        {
            if (string.IsNullOrEmpty(gender.Trim()))
            {
                return "Nam";
            }
            if (gender.Trim() == "N")
            {
                return "Nữ";
            }
            return "";
        }
        #endregion
    }
    public class JsonData
    {
        //public int RowNum { get; set; }
        //public int Total { get; set; }
        //public string Ngay { get; set; }
        //public object NgayHen_BatDau_Ngay { get; set; }
        //public object NgayHen_BatDau_Gio { get; set; }
        //public object NgayHen_BatDau_Phut { get; set; }
        //public object NgayHen_KetThuc_Ngay { get; set; }
        //public object NgayHen_KetThuc_Gio { get; set; }
        //public object NgayHen_KetThuc_Phut { get; set; }
        public int ID { get; set; }
        public int StudentID { get; set; }
        //public string StudentCode { get; set; }
        //public string StudentName { get; set; }
        public int Status { get; set; }
        //public string LyDo { get; set; }
        //public string PhanHoi { get; set; }
        //public bool Deleted { get; set; }
        //public int CreatedBy { get; set; }
        //public int LastModifiedBy { get; set; }
        //public int DeletedBy { get; set; }
        //public DateTime CreatedTime { get; set; }
        //public DateTime DeletedTime { get; set; }
        //public DateTime LastModifiedTime { get; set; }
        //public object NgayGui { get; set; }
        //public object NgayHen_TuNgay { get; set; }
        //public object NgayHen_DenNgay { get; set; }
        //public string MaXacNhan { get; set; }
        //public object HKTT_SoNha { get; set; }
        public string HKTT_Pho { get; set; }
        public string HKTT_Phuong { get; set; }
        public string HKTT_Quan { get; set; }
        public string HKTT_Tinh { get; set; }
    }
}
