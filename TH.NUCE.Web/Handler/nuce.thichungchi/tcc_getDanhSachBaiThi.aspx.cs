using Newtonsoft.Json;
using nuce.web.data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace nuce.web.sinhvien
{
    public partial class tcc_getDanhSachBaiThi : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string strData = "-1";
            try
            {
                Dictionary<int, model.KiThiLopHocSinhVien> KiThiLopHocSinhViens = new Dictionary<int, model.KiThiLopHocSinhVien>();
                KiThiLopHocSinhViens = (Dictionary< int, model.KiThiLopHocSinhVien >) Session[Utils.session_kithi_lophoc_sinhvien];
                strData=JsonConvert.SerializeObject(KiThiLopHocSinhViens, Formatting.Indented);
            }
            catch (Exception ex)
            {
                strData = ex.Message;
            }
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Write(strData);
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}