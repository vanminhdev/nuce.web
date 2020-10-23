using System.Data;
using System.Data.SqlClient;

namespace nuce.web.data
{
    public static class dnn_Nuce_KS_SinhVienRaTruong_SinhVien1
    {
        public static DataTable get(int ID)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienRaTruong_SinhVien_get");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, ID).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataSet getTongHop()
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienRaTruong_TongHop_2017");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql);

                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getByLopQDAndDotTotNghiep(string lop,int dotkhaosat)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienRaTruong_getByLopQDAndDotTotNghiep");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, lop, dotkhaosat).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getByDotKhaoSat(int dotkhaosat, string masv)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienRaTruong_SinhVien_getByDotKhaoSat");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, dotkhaosat, masv).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable checkLogin(string masv, string psw)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienRaTruong_SinhVien_checkLogin");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, masv, psw).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        public static int update(string masv, string email, string email1, string email2, string mobile, string mobile1, string mobile2, string addinfo1, string addinfo2)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return -1;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienRaTruong_SinhVien_update_email");
                    return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, masv, email, email1, email2, mobile, mobile1, mobile2, addinfo1, addinfo2);

                }
                catch
                {
                    return -1;
                }
            }
        }
        public static int update1(string masv, string email, string email1, string email2, string mobile, string mobile1, string mobile2, string addinfo1, string addinfo2)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return -1;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienRaTruong_SinhVien_update_email1");
                    return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, masv, email, email1, email2, mobile, mobile1, mobile2, addinfo1, addinfo2);

                }
                catch
                {
                    return -1;
                }
            }
        }
        public static int updateByChecksum(string checksum, string email, string email1, string email2, string mobile, string mobile1, string mobile2, string addinfo1, string addinfo2)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return -1;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienRaTruong_SinhVien_update_emailbychecksum");
                    return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, checksum, email, email1, email2, mobile, mobile1, mobile2, addinfo1, addinfo2);

                }
                catch
                {
                    return -1;
                }
            }
        }
        #region insert
        public static int insert(
string @dottotnghiep,
string @sovaoso,
string @masv,
string @noisiti,
string @tbcht,
string @xeploai,
string @soqdtn,
string @sohieuba,
string @tinh,
string @truong,
string @gioitinh,
string @ngaysinh,
string @tkhau,
string @lop12,
string @namtn,
string @sobaodanh,
string @tcong,
string @ghichu_thi,
string @lopqd,
string @k,
string @dtoc,
string @quoctich,
string @bangclc,
string @manganh,
string @tenchnga,
string @tennganh,
string @hedaotao,
string @khoahoc,
string @tensinhvien,
string @email,
string @email1,
string @email2,
string @mobile,
string @mobile1,
string @mobile2,
string @thongtinthem,
string @thongtinthem1,
int @dotkhoasat_id,
string @psw,
string @checksum, 
string @ex_masv,
int @type,
int @status
)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return -1;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienRaTruong_SinhVien_insert");
                    return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql,
                @dottotnghiep,
@sovaoso,
@masv,
@noisiti,
@tbcht,
@xeploai,
@soqdtn,
@sohieuba,
@tinh,
@truong,
@gioitinh,
@ngaysinh,
@tkhau,
@lop12,
@namtn,
@sobaodanh,
@tcong,
@ghichu_thi,
@lopqd,
@k,
@dtoc,
@quoctich,
@bangclc,
@manganh,
@tenchnga,
@tennganh,
@hedaotao,
@khoahoc,
@tensinhvien,
@email,
@email1,
@email2,
@mobile,
@mobile1,
@mobile2,
@thongtinthem,
@thongtinthem1,
@dotkhoasat_id, @psw, @checksum, @ex_masv,
@type, @status
                );

                }
                catch
                {
                    return -1;
                }
            }
        }
        #endregion
    }
}
