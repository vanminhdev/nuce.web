using ClosedXML.Excel;
using DotNetNuke.Entities.Modules;
using GemBox.Document;
using GemBox.Document.Drawing;
using GemBox.Document.Tables;
using Ionic.Zip;
using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using nuce.web.data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;

namespace nuce.ad.ctsv
{
    public partial class QuanLyDichVu_ThietLapThamSo : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

    }
}