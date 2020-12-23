using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nuce.web.quanly.Models
{
    public class ClientParameterModel
    {
        public int id { get; set; }
        public string code { get; set; }
        public string type { get; set; }
        public int status { get; set; }
        public string value { get; set; }
        public string entryUsername { get; set; }
        public DateTime? entryDatetime { get; set; }
        public string updateUsername { get; set; }
        public DateTime? updateDatetime { get; set; }
        public string role { get; set; }
    }

    public class UpdateClientParameterModel
    {
        public int id { get; set; }
        public string value { get; set; }
        public int status { get; set; }
    }

}