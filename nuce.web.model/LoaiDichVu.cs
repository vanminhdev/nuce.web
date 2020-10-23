using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Nuce.CTSV.Model
{
    public class LoaiDichVu
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
        public LoaiDichVu(int id,string code,string name,string des,string param1,string param2,string param3)
        {
            this.ID = id;
            this.Code = code;
            this.Name = name;
            this.Description = des;
            this.Param1 = param1;
            this.Param2 = param2;
            this.Param3 = param3;
        }

    }
    public class TrangThaiDichVu
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
        public TrangThaiDichVu(int id, string code, string name, string des, string param1, string param2, string param3)
        {
            this.ID = id;
            this.Code = code;
            this.Name = name;
            this.Description = des;
            this.Param1 = param1;
            this.Param2 = param2;
            this.Param3 = param3;
        }
    }
    public class FormDichVu
    {
        public HtmlGenericControl element { get; set; }
        public string sqlField { get; set; }
        public string error { get; set; }
        public int MyProperty { get; set; }
    }
}