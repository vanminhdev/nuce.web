using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey.Graduate
{
    public class TransferDataUndergraduateModel
    {
        [Required]
        public DateTime? FromDate { get; set; }

        [Required]
        public DateTime? ToDate { get; set; }

        [Required]
        public List<string> HeTotNghieps { get; set; }
    }
}
