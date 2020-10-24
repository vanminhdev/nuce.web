using System;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_Nuce_KS_SinhVienRaTruong_SinhVien
    {
        public static DataTable get(int ID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("Nuce_KS_SinhVienRaTruong_SinhVien_get", ID).Tables[0];
        }
        public static DataTable getByDotKhaoSat(int dotkhaosat,string masv)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("Nuce_KS_SinhVienRaTruong_SinhVien_getByDotKhaoSat", dotkhaosat,masv).Tables[0];
        }
        public static DataTable checkLogin(string masv,string psw)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("Nuce_KS_SinhVienRaTruong_SinhVien_checkLogin", masv, psw).Tables[0];
        }
        /*
           @masv varchar(50),
           @email varchar(100),
           @email1 varchar(100),
           @email2 varchar(100),
           @mobile varchar(20),
           @mobile1 varchar(20),
           @mobile2 varchar(20),
           @thongtinthem nvarchar(1000),
           @thongtinthem1 nvarchar(1000)
        */
        public static int update(string masv, string email, string email1, string email2, string mobile,string mobile1,string mobile2,string addinfo1,string addinfo2)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("Nuce_KS_SinhVienRaTruong_SinhVien_update_email", masv, email, email1, email2, mobile, mobile1, mobile2, addinfo1, addinfo2);
        }
        public static int updatelopQD(string masv, string lopQd)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("Nuce_KS_SinhVienRaTruong_SinhVien_update_lopQD", masv, lopQd);
        }
        /*          @khoahoc varchar(30),
		  @xeploai nvarchar(50),
		  @namtotnghiep varchar(20),
		  @soqdtn varchar(100),
		  @sohieuba varchar(20),
		  @hedaotao nvarchar(50)*/
        public static int update_addInfo(string masv, string khoahoc,string xeploai,string namtotnghiep
            ,string soqdtn,string sohieuba,string hedaotao)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("Nuce_KS_SinhVienRaTruong_SinhVien_update_addInfo", masv, khoahoc
                , xeploai, namtotnghiep, soqdtn, sohieuba, hedaotao);
        }
        public static int updateByChecksum(string checksum, string email, string email1, string email2, string mobile, string mobile1, string mobile2, string addinfo1, string addinfo2)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("Nuce_KS_SinhVienRaTruong_SinhVien_update_emailbychecksum", checksum, email, email1, email2, mobile, mobile1, mobile2, addinfo1, addinfo2);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("Nuce_KS_SinhVienRaTruong_update_status", iID, status);
        }
        public static int insert(string @dottotnghiep,
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
string @checksum,string @ex_masv,
int @type,
int @status
)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("Nuce_KS_SinhVienRaTruong_SinhVien_insert",
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
@type,@status
                );
        }
    }
}
