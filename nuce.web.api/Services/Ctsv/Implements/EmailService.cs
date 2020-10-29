using Microsoft.AspNetCore.Hosting;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.Services.Ctsv.Interfaces;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Ctsv.Implements
{
    public class EmailService : IEmailService
    {
        private readonly ITinNhanRepository _tinNhanRepository;
        private readonly IPathProvider _pathProvider;
        public EmailService(ITinNhanRepository _tinNhanRepository, IPathProvider _pathProvider)
        {
            this._tinNhanRepository = _tinNhanRepository;
            this._pathProvider = _pathProvider;
        }
        public async Task<ResponseBody> SendEmailNewServiceRequest(TinNhanModel model)
        {
            var now = DateTime.Now;

            var dir = _pathProvider.MapPath("Templates/Ctsv/template_mail_tao_yeu_cau_dich_vu.txt");
            if (!File.Exists(dir))
            {
                return new ResponseBody { Message = "Template không tồn tại" };
            }
            string templateContent = File.ReadAllText(dir);
            string tinNhanContent = templateContent.Replace("[ho_ten]", model.StudentName)
                                                .Replace("[ten_dich_vu]", model.TenDichVu)
                                                .Replace("[ngay_tao]", now.ToString("dd/MM/yyyy HH:mm"));
            var tinNhan = new AsAcademyStudentTinNhan
            {
                Content = tinNhanContent,
                SenderId = -1,
                ReceiverEmail = model.StudentEmail,
                Receiver = model.StudentCode,
                Code = model.TinNhanCode,
                StudentCode = model.StudentCode,
                StudentName = model.StudentName,
                CreatedBy = model.StudentID,
                LastModifiedBy = model.StudentID,
                DeletedBy = model.StudentID,
                CreatedTime = now,
                LastModifiedTime = now,
                DeletedTime = now,
                Title = model.TinNhanTitle,
                Status = 1,
                Type = 1,
                Deleted = false,
            };
            await _tinNhanRepository.addTinNhanAsync(tinNhan);
            return null;
        }
    }
}
