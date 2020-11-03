using System;
using System.Data;
using System.Data.SqlClient;

namespace nuce.web.data
{
    public static class Nuce_Survey
    {
        #region Conection
        private static string m_strConnectionString = System.Configuration.ConfigurationSettings.AppSettings["NUCESURVEY"];
        public static string ConnectionString
        {
            get
            {
                return m_strConnectionString;
            }
        }

        private static string m_strPoolingConnectionString = System.Configuration.ConfigurationSettings.AppSettings["NUCESURVEY"];
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
                catch (Exception ex)
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
        #region Survey
        public static DataTable getAcademy_ClassRoom()
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT * FROM [dbo].[AS_Academy_ClassRoom]");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];
                }
                catch
                {
                    return null;
                }
            }

        }
        public static DataTable getAcademy_C_ClassRoom()
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT * FROM [dbo].[AS_Academy_C_ClassRoom]");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];
                }
                catch
                {
                    return null;
                }
            }

        }
        public static DataTable getAcademy_Lecturer()
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT * FROM [dbo].[AS_Academy_Lecturer]");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];
                }
                catch
                {
                    return null;
                }
            }

        }
        public static DataTable getAcademy_Class()
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT * FROM [dbo].[AS_Academy_Class]");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable spAcademy_Lecturer_GetByDepartmentCode(string code)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"spAcademy_Lecturer_GetByDepartmentCode");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, code).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getAcademy_Student_ByCode(string code)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT [ID]
      ,[Code]
      ,[FulName]
      ,[ClassID]
      ,[ClassCode]
      ,[DateOfBirth]
      ,[BirthPlace],Email,Mobile
  FROM [dbo].[AS_Academy_Student] where code='{0}'", code);
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getAcademy_Lecturer_ByCode(string code)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT a.*,b.Name as DepartmentName
  FROM [dbo].[AS_Academy_Lecturer] a inner join [dbo].[AS_Academy_Department] b
  on a.DepartmentCode=b.Code where a.code='{0}'", code);
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getAS_Edu_Survey_BaiKhaoSat_SinhVien(int sinhvienID)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"
select a.*
  FROM [dbo].[AS_Edu_Survey_BaiKhaoSat_SinhVien] a
  where SinhVienID={0}  and BaiKhaoSatID in (
select ID from [dbo].[AS_Edu_Survey_BaiKhaoSat]
where [DotKhaoSatID] in (
SELECT ID
  FROM [dbo].[AS_Edu_Survey_DotKhaoSat]
  where Status=3))", sinhvienID);
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public static void InsertAS_Edu_Survey_Log_Access(int UserId, string UserCode, int Status, string Message)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"spSurvey_insertLogAccesses");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, UserId, UserCode, Status, Message);
                }
                catch
                {
                    return;
                }
            }
        }
        public static void InsertAS_Edu_Survey_Ext_BaiLam(int DotKhaoSatID, string DotKhaoSatName, string Name,
            int GioiTinh,string Email,string Mobile,string TenCoQuan, string DoiTuongTraLoi,int Template,string BaiLam)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"sp_AS_Edu_Survey_Ext_BaiLam_insert");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, DotKhaoSatID, DotKhaoSatName, Name,
                        GioiTinh, Email, Mobile, TenCoQuan, DoiTuongTraLoi, Template, BaiLam);
                }
                catch
                {
                    return;
                }
            }
        }
        #region Student
        public static void UpdateEmailStudent(string Code, string Email, string Mobile)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"UPDATE [dbo].[AS_Academy_Student] SET Mobile='{1}', Email='{2}'  WHERE [Code] = '{0}' ", Code, Mobile, Email);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, CommandType.Text, strSql);
                }
                catch
                {
                    return;
                }
            }
        }
        public static DataTable UpdateEmailStudent1(string Code, string Email, string Mobile)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, "AS_Academy_Student_update_Email", Code, Email,Mobile).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public static int authen_email(string masv, string key, int status)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return -1;
                try
                {
                    //Execute select command
                      return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, "AS_Academy_Student__authen_email", masv, key, status);
                }
                catch
                {
                    return -1;
                }
            }
        }
        #endregion
        #region Subject_Extent
        public static void InsertAS_Academy_Subject_Extent(string Code, string Name, int Type)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"spSubject_Extent_Insert");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, Code, Name, Type);
                }
                catch
                {
                    return;
                }
            }
        }
        #endregion
        #region ClassRoom
        public static void InsertAcademy_Lecturer_ClassRoom(string MaDK, string MaCB, int SemesterID)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"spAcademy_Lecturer_ClassRoom_Insert");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, MaDK, MaCB, SemesterID);
                }
                catch
                {
                    return;
                }
            }
        }
        public static void InsertAcademy_C_Lecturer_ClassRoom(string MaDK, string MaCB, int SemesterID)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"spAcademy_C_Lecturer_ClassRoom_Insert");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, MaDK, MaCB, SemesterID);
                }
                catch
                {
                    return;
                }
            }
        }
        public static void InsertAcademy_C_ClassRoom(int SemesterID, string Code, string GroupCode, string ClassCode,
            string SubjectCode, string ExamAttemptDate)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"spAcademy_C_ClassRoom_Insert");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, SemesterID, Code, GroupCode, ClassCode,
                        SubjectCode, ExamAttemptDate);
                }
                catch
                {
                    return;
                }
            }
        }
        public static void UpdateAcademy_C_ClassRoom_FEDate(string Code, DateTime From, DateTime End)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"spAcademy_C_ClassRoom_UpdateFEDate");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, Code, From, End);
                }
                catch
                {
                    return;
                }
            }
        }
        public static void InsertAcademy_C_Student_ClassRoom(int SemesterID, string MaSV, string MaDK)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"spAcademy_C_Student_ClassRoom_Insert");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, SemesterID, MaSV, MaDK);
                }
                catch
                {
                    return;
                }
            }
        }
        public static void UpdateAS_Edu_QA_Week(int ID, int Count,int Total,DateTime from,DateTime end)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"AS_Edu_QA_WeekUpdate");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, ID, Count, Total, from,end);
                }
                catch
                {
                    return;
                }
            }
        }
        public static DataTable getC_ClassRoomByLecturer(int LeID)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"select a.ID,a.GroupCode + ' - '+b.Name as ClassRoomName  from [dbo].[AS_Academy_C_ClassRoom] a inner join [dbo].[AS_Academy_Subject] b
on a.SubjectID=b.ID where a.ID in (select [ClassRoomID] from [dbo].[AS_Academy_C_Lecturer_ClassRoom] where [LecturerID]={0})", LeID);
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getAS_Academy_C_ClassRoomWithFromDateNotNull()
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT *
  FROM [dbo].[AS_Academy_C_ClassRoom]
  where ISNULL(FromDate,getDate())!=getDate()");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion
        #region BaiKhaoSat
        public static string getEdu_Survey_BaiKhaoSat_GetNoiDungDeThi(int id)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"select NoiDungDeThi from [dbo].[AS_Edu_Survey_BaiKhaoSat] where ID={0}", id);
                    return (string)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, CommandType.Text, strSql);
                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getEdu_Survey_BaiKhaoSat_GetNoiDungDeThi()
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"select * from [dbo].[AS_Edu_Survey_BaiKhaoSat]");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion
        #region BaiLamSinhVien
        public static void Edu_Survey_BaiKhaoSat_SinhVien_update_bailam(int iID, string BaiLam, DateTime NgayGioNopBai, string LogIP, int Status)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"spEdu_Survey_BaiKhaoSat_SinhVien_update_bailam");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, strSql, iID, BaiLam, NgayGioNopBai, LogIP, Status);
                }
                catch
                {
                    return;
                }
            }
        }
        public static void Edu_Survey_BaiKhaoSat_SinhVien_update_giangvien(int iID, string Code, string Name)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"spEdu_Survey_BaiKhaoSat_SinhVien_update_giangvien");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, strSql, iID, Code, Name);
                }
                catch
                {
                    return;
                }
            }
        }
        #endregion
        #region TongHopKetQua
        public static DataTable getByStatus(int Status, int Type)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"spEdu_Survey_BaiKhaoSat_SinhVien_GetByStatus");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, Status, Type).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }

        public static void InsertReportTotal(int SemesterId, int CampaignId, int CampaignTemplateId,
          int ClassRoom, int Lecturer, int QuestionType, int QuestionID, int QuestionIndex, int AnswerId, int Total, string Value)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"spSurvey_reportTotal_Insert");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, strSql, SemesterId, SemesterId, CampaignTemplateId
                        , ClassRoom, Lecturer, QuestionType, QuestionID, QuestionIndex, AnswerId, Total, Value);

                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }
        #endregion
        #region QA
        public static void InsertAS_Edu_QA_Log_Access(int UserId, string UserCode, int Status, string Message)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"spSurvey_insertLogAccesses");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, UserId, UserCode, Status, Message);
                }
                catch
                {
                    return;
                }
            }
        }
        public static void InsertQuestion(int StudentID, string StudentCode, int LecturerClassRoomID, int Type, string Question)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"spQA_insertQuestion");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, StudentID, StudentCode, LecturerClassRoomID, Type, Question);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }
        public static void InsertQuestion(int StudentID, string StudentCode, int LecturerClassRoomID, int Type, string Question,int Status, string File)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"spQA_insertQuestion1");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, StudentID, StudentCode, LecturerClassRoomID, Type, Question,Status,File);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }
        public static void UpdateQuestion(int ID, int StudentID, string StudentCode, int LecturerClassRoomID, int Type, string Question)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"spQA_updateQuestion");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, ID, StudentID, StudentCode, LecturerClassRoomID, Type, Question);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }
        public static void UpdateQuestion(int ID, int StudentID, string StudentCode, int LecturerClassRoomID, int Type, string Question,string File)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"spQA_updateQuestion1");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, ID, StudentID, StudentCode, LecturerClassRoomID, Type, Question,File);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }
        public static void UpdateQuestion(int ID, int StudentID, int Status)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = "";
                    if (Status < 3)
                        strSql = string.Format(@"UPDATE [dbo].[AS_Edu_QA_Question] SET status={0},CreatedTime=getDate() WHERE ID={1} and StudentID={2}", Status, ID, StudentID);
                    else
                        strSql = string.Format(@"UPDATE [dbo].[AS_Edu_QA_Question] SET status={0},UpdatedTime=getDate() WHERE ID={1} and StudentID={2}", Status, ID, StudentID);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, CommandType.Text, strSql);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }
        public static void UpdateQuestion(int ID, int Status)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = "";
                    if (Status < 3)
                        strSql = string.Format(@"UPDATE [dbo].[AS_Edu_QA_Question] SET status={0},CreatedTime=getDate() WHERE ID={1}", Status, ID);
                    else
                        strSql = string.Format(@"UPDATE [dbo].[AS_Edu_QA_Question] SET status={0},UpdatedTime=getDate() WHERE ID={1}", Status, ID);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, CommandType.Text, strSql);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }
        public static void UpdateQuestion_IsLecturer(int ID, int  IsLecturer)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = "";
                        strSql = string.Format(@"UPDATE [dbo].[AS_Edu_QA_Question] SET IsLecturer={0} WHERE ID={1}", IsLecturer, ID);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, CommandType.Text, strSql);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }
        public static void UpdateQuestion_IsStudent(int ID, int IsStudent)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = "";
                    strSql = string.Format(@"UPDATE [dbo].[AS_Edu_QA_Question] SET IsStudent={0} WHERE ID={1}", IsStudent, ID);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, CommandType.Text, strSql);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }
        public static void UpdateQuestion(int ID, int LecturerID, string strAnswer, int Status)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = "";
                    strSql = string.Format(@"UPDATE [dbo].[AS_Edu_QA_Question] SET status={0},UpdatedTime=getDate(),Answare=N'{1}' WHERE ID={2} and LecturerID={3}", Status,strAnswer, ID, LecturerID);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, CommandType.Text, strSql);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }
        public static void UpdateQuestion(int ID, int LecturerID, string strAnswer, int Status,string File)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = "";
                    strSql = string.Format(@"UPDATE [dbo].[AS_Edu_QA_Question] SET status={0},UpdatedTime=getDate(),Answare=N'{1}',Le_Attachment=N'{4}' WHERE ID={2} and LecturerID={3}", Status, strAnswer, ID, LecturerID, File);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, CommandType.Text, strSql);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        public static DataTable getCNameLecturerByStudentCode(string code)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"select x.ID,x.Name+' - '+x.GroupCode+ ' - '+y.FullName+' - ' as Name from (
select * from (
select a.ID as ClassRoomID,a.code,b.Name,b.Code as SubjectCode,c.LecturerCode,c.LecturerID,c.ID,a.GroupCode  from [dbo].[AS_Academy_C_ClassRoom] a inner join [dbo].[AS_Academy_Subject] b
on a.SubjectID=b.ID 
left join [dbo].[AS_Academy_C_Lecturer_ClassRoom] c on a.ID=c.ClassRoomID) x where IsNull(LecturerCode, '') !='') x inner join [dbo].[AS_Academy_Lecturer] y
on x.LecturerID=y.ID where x.ClassRoomID in (select [ClassRoomID] from [dbo].[AS_Academy_C_Student_ClassRoom] where StudentCode='{0}')", code);
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataSet getQA_QuestionByStudent(int StudentID)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT a.*,b.FullName,d.Name,c.GroupCode
  FROM [dbo].[AS_Edu_QA_Question] a left join [dbo].[AS_Academy_Lecturer] b on a.LecturerID=b.ID
  inner join [dbo].[AS_Academy_C_ClassRoom] c on a.ClassRoomID=c.ID inner join [dbo].[AS_Academy_Subject] d on c.SubjectID=d.ID
  where StudentID={0} and a.Status<10;
  select ID,Name,Template from [dbo].[AS_Edu_QA_Type] where Status=1;
select a.ClassRoomID,b.ID as LecturerClassRoomID,b.Template, b.LecturerID,b.LecturerCode,c.GroupCode,d.Name,e.FullName from (select StudentID,StudentCode,ClassRoomID from [dbo].[AS_Academy_C_Student_ClassRoom] where StudentID={0}) a
inner join [dbo].[AS_Academy_C_Lecturer_ClassRoom] b on a.ClassRoomID=b.ClassRoomID 
 inner join [dbo].[AS_Academy_C_ClassRoom] c on a.ClassRoomID=c.ID inner join [dbo].[AS_Academy_Subject] d on c.SubjectID=d.ID
 inner join [dbo].[AS_Academy_Lecturer] e on b.LecturerID=e.ID order by Name;", StudentID);
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql);
                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataSet getQA_QuestionByLecturer(int LeID)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT a.*,b.FulName,d.Name,c.GroupCode,e.Name as NameType
  FROM [dbo].[AS_Edu_QA_Question] a left join [dbo].AS_Academy_Student b on a.StudentID=b.ID
  inner join [dbo].[AS_Academy_C_ClassRoom] c on a.ClassRoomID=c.ID inner join [dbo].[AS_Academy_Subject] d on c.SubjectID=d.ID
  inner join [dbo].[AS_Edu_QA_Type] e on a.Type=e.ID
  where LecturerID={0} and a.Status>1 and a.Status<10; 
  select ID,Name,Template from [dbo].[AS_Edu_QA_Type] where Status=1;
  select a.ClassRoomID,a.ID as LecturerClassRoomID,a.Template, a.LecturerID,a.LecturerCode,c.GroupCode,d.Name,e.FullName from 
(select * from [dbo].[AS_Academy_C_Lecturer_ClassRoom] where LecturerID={0}) a
 inner join [dbo].[AS_Academy_C_ClassRoom] c on a.ClassRoomID=c.ID inner join [dbo].[AS_Academy_Subject] d on c.SubjectID=d.ID
 inner join [dbo].[AS_Academy_Lecturer] e on a.LecturerID=e.ID order by Name;", LeID);
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql);
                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getQA_Type()
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT * from [dbo].[AS_Edu_QA_Type] ");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public static string getQA_Type(int ID)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return "";
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT Name from [dbo].[AS_Edu_QA_Type] where ID={0}",ID);
                    return (string)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, CommandType.Text, strSql);
                }
                catch
                {
                    return "";
                }
            }
        }
        public static DataTable getQA_TypeByLecturClass(int ID)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT * from [dbo].[AS_Edu_QA_Type]
where Template in (select template from [dbo].[AS_Academy_C_Lecturer_ClassRoom] where ID={0}) ",ID);
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getQA_QuestionByID(int ID)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT a.*
  FROM [dbo].[AS_Edu_QA_Question] a where a.ID={0} or -1={0}", ID);
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getQA_QuestionDetailByID(int ID)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT a.*,b.FulName,d.Name,c.GroupCode
  FROM [dbo].[AS_Edu_QA_Question] a left join [dbo].AS_Academy_Student b on a.StudentID=b.ID
  inner join [dbo].[AS_Academy_C_ClassRoom] c on a.ClassRoomID=c.ID inner join [dbo].[AS_Academy_Subject] d on c.SubjectID=d.ID
  where a.ID={0}", ID);
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataSet getStatisticsSendEmailLecturer()
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"select * from [dbo].[AS_Academy_Lecturer] where 
  ID in (  SELECT distinct([LecturerID])
  FROM [dbo].[AS_Edu_QA_Question]
  where Status>1 and IsLecturer=1)
  and isNull(email,'')!=''
  and ID not in (select [Partner] from [dbo].[AS_Edu_QA_Log_Mail] where Type=1 and  DATEADD(DAY,5,createdDate)>getDate());

  SELECT LecturerID,count([LecturerID]) as Count
  FROM [dbo].[AS_Edu_QA_Question]
  where Status>1 and IsLecturer=1
  Group by [LecturerID];

  SELECT LecturerID,count([LecturerID]) as Count
  FROM [dbo].[AS_Edu_QA_Question]
  where Status>1 
  Group by [LecturerID];

");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql);
                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataSet getStatisticsSendEmailStudent()
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@" select * from [dbo].[AS_Academy_Student] where 
  ID in (  SELECT distinct(StudentID)
  FROM [dbo].[AS_Edu_QA_Question]
  where Status=4 and IsStudent=1)
  and status=3
  and ID not in (select [Partner] from [dbo].[AS_Edu_QA_Log_Mail] where Type=2 and  DATEADD(DAY,5,createdDate)>getDate());


  SELECT StudentID,count(StudentID) as Count
  FROM [dbo].[AS_Edu_QA_Question]
  where Status=4 and IsStudent=1
  Group by StudentID;


  SELECT StudentID,count(StudentID) as Count
  FROM [dbo].[AS_Edu_QA_Question]
  where Status=4 
  Group by StudentID;

");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql);
                }
                catch
                {
                    return null;
                }
            }
        }
        public static void UpdateQA_Log_Mail(int ID, int Type,string Message)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = "";
                        strSql = string.Format(@"INSERT INTO [dbo].[AS_Edu_QA_Log_Mail]
           ([Partner]
           ,[Type]
           ,[Message]
           ,[CreatedDate]
           ,[Status])
     VALUES
           ({0}
           ,{1}
           ,'{2}'
           ,{3}
           ,{4})", ID, Type,Message,DateTime.Now,1);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, CommandType.Text, strSql);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }
        #endregion
        #region 
        public static void insertWithIDAS_Edu_Survey_DapAn(int ID,int CauHoiID, int SubQuestionId, string Content, bool IsCheck, string Matching,
           float Mark, float MarkFail, int Order, int Status)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, "AS_Edu_Survey_DapAn_insertWithID", ID, CauHoiID, SubQuestionId, Content, IsCheck, Matching
                , Mark, MarkFail, Order, Status);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }
        public static void insertWithID_AS_Edu_Survey_CauHoi(int ID,int BoCauHoiID, int DoKhoID, string Ma, string NoiDung, string Image, string Media_Url, string Media_Setting
    , float Mark, float MarkFail, bool IsOption, int PartId, string Explain, int ParentQuestionId, int NoAnswareOnRow,
    DateTime DateExpired, int TimeVisibleAnswer, int InsertedUser, int UpdatedUser, DateTime InsertedDate, DateTime UpdatedDate
    , int Order, int Level, string Type, int Status)
        {
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, "AS_Edu_Survey_CauHoi_insertWithID", ID, BoCauHoiID, DoKhoID, Ma, NoiDung, Image, Media_Url,
                Media_Setting, Mark, MarkFail, IsOption, PartId, Explain, ParentQuestionId, NoAnswareOnRow,
                DateExpired, TimeVisibleAnswer, InsertedUser, UpdatedUser, InsertedDate, UpdatedDate,
                Order, Level, Type, Status);
                }
                catch(Exception ex)
                {
                    return;
                }
            }
        }
        #endregion
        #endregion
        #region User
        public static DataTable Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;
            password = password.MD5Hash();
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"SELECT * from GS_User where Username=@Param0 and Password=@Param1", username, password);
                    SqlParameter[] sqlParams = new SqlParameter[2];
                    sqlParams[0] = new SqlParameter("@Param0", username);
                    sqlParams[1] = new SqlParameter("@Param1", password);
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.Text, strSql, sqlParams).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public static void U_SetPassword(int ID, string password)
        {
            if (string.IsNullOrEmpty(password))
                return ;
            password = password.MD5Hash();
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return ;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"Update [dbo].[GS_User] set [Password]='{1}' where ID={0}", ID, password);
                     Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, CommandType.Text, strSql);
                }
                catch
                {
                    return ;
                }
            }
        }
        public static void U_ChangePassword(int ID, string passwordOld,string password)
        {
            if (string.IsNullOrEmpty(password)&& string.IsNullOrEmpty(passwordOld))
                return;
            password = password.MD5Hash();
            passwordOld = passwordOld.MD5Hash();
            using (SqlConnection objConnection = GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"Update [dbo].[GS_User] set [Password]='{1}' where ID={0} and Password='{2}'", ID, password,passwordOld);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, CommandType.Text, strSql);
                }
                catch
                {
                    return;
                }
            }
        }

        #endregion
    }
}
