using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.Services;
using nuce.web.tienich;
using nuce.web.data;
using System.IO;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Microsoft.ApplicationBlocks.Data;

namespace nuce.web.commons
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ad_ctsv_qldv_xacnhan_changestatus_selected : CoreHandlerCommonAdminCTSV
    {
        public override void WriteData(HttpContext context)
        {
            string body = new StreamReader(context.Request.InputStream).ReadToEnd();
            string response = "";
            string type = context.Request["type"] ?? "";

            if (string.IsNullOrEmpty(type.Trim()))
            {
                type = "0";
            }
            try
            {
                updateStatus(body, type);
            }
            catch (Exception ex)
            {
                response = ex.Message;
                throw;
            }

            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(response);
        }
        private void updateStatus(string jsonData, string type)
        {
            var data = JsonConvert.DeserializeObject<List<JsonData>>(jsonData);
            int daXuLyCoLichHen = 4;
            foreach (var item in data)
            {
                if (item.Status < daXuLyCoLichHen)
                {
                    updateRequestStatus(item.ID.ToString(), daXuLyCoLichHen.ToString(), type);
                }
            }
        }
        private void updateRequestStatus(string id, string status, string loaiDichVu)
        {
            string phanHoi = "";
            string ngayBatDau = "01/01/1990 00:00";
            string ngayKetThuc = "01/01/1990 00:00";
            nuce.web.data.Nuce_CTSV.UpdateRequestStatus(id, status, phanHoi, ngayBatDau, ngayKetThuc, loaiDichVu);
        }
        private class JsonData
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
}
