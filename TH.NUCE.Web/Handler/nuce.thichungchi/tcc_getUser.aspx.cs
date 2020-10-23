﻿using nuce.web.data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace nuce.web.sinhvien
{
    public partial class tcc_getUser : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string strData = "-1";
            try
            {
                model.SinhVien SinhVien = new model.SinhVien();
                SinhVien = (model.SinhVien)Session[Utils.session_sinhvien];
                strData  = new JavaScriptSerializer().Serialize(SinhVien);
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