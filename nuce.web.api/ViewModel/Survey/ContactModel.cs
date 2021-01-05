using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey
{
    /// <summary>
    /// Model gửi email cho phòng ktdb
    /// </summary>
    public class SendEmailKtdbModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
    }

    /// <summary>
    /// Model chung gửi email bằng service anh Quý
    /// </summary>
    public class SendEmailByNuceModel
    {
        public object Data { get; set; }
        public string EmailSender { get; set; }
        public string EmailReceiver { get; set; }
        public int TemplateId { get; set; }
        public string Subject { get; set; }
    }
}
