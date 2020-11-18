using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Core
{
    public partial class ManagerBackup
    {
        public Guid Id { get; set; }
        public string DatabaseName { get; set; }
        public int Type { get; set; }
        public DateTime Date { get; set; }
        public string Path { get; set; }
    }
}
