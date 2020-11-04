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
using static nuce.web.api.Common.Ctsv;

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
                return new ResponseBody { Message = "Template không tồn tại", StatusCode = System.Net.HttpStatusCode.NotFound };
            }
            string templateContent = await File.ReadAllTextAsync(dir);
            string tinNhanContent = templateContent.Replace("[ho_ten]", model.StudentName)
                                                .Replace("[ten_dich_vu]", model.TenDichVu)
                                                .Replace("[ngay_tao]", now.ToString("dd/MM/yyyy HH:mm"));
            await SaveTinNhanAsync(model, tinNhanContent, now);
            return null;
        }

        public async Task<ResponseBody> SendEmailUpdateStatusRequest(TinNhanModel model)
        {
            var now = DateTime.Now;
            var status = model.YeuCauStatus;

            bool henGap = (TrangThaiYeuCau)status == TrangThaiYeuCau.DaXuLyVaCoLichHen;

            var dir = _pathProvider.MapPath("Templates/Ctsv/template_mail_cap_nhat_trang_thai.txt");
            if (!File.Exists(dir))
            {
                return new ResponseBody { Message = "Template không tồn tại", StatusCode = System.Net.HttpStatusCode.NotFound };
            }
            string templateContent = await File.ReadAllTextAsync(dir);
            string trangThaiYeuCau = TrangThaiYeuCauDictionary[status];
            string tinNhanContent = templateContent.Replace("[ten_sinh_vien]", model.StudentName)
                                                .Replace("[trang_thai_yeu_cau]", trangThaiYeuCau)
                                                .Replace("[ten_dich_vu]", model.TenDichVu)
                                                .Replace("[ngay_tao_gio_tao]", model.NgayTao?.ToString("dd/MM/yyyy HH:mm"));
            string henGapContent = "";
            if (henGap)
            {
                dir = _pathProvider.MapPath("Templates/Ctsv/template_content_cap_nhat_trang_thai_da_co_hen.txt");
                if (!File.Exists(dir))
                {
                    return new ResponseBody { Message = "Template có hẹn không tồn tại", StatusCode = System.Net.HttpStatusCode.NotFound };
                }
                templateContent = await File.ReadAllTextAsync(dir);
                henGapContent = templateContent.Replace("[gio_hen]", model.NgayHen?.Hour.ToString())
                                                .Replace("[phut_hen]", model.NgayHen?.Minute.ToString())
                                                .Replace("[ngay_hen]", model.NgayHen?.ToString("dd/MM/yyyy"));

            }
            tinNhanContent = tinNhanContent.Replace("[HEN_GAP]", henGapContent);

            await SaveTinNhanAsync(model, tinNhanContent, now);
            return null;
        }

        private async Task SaveTinNhanAsync(TinNhanModel model, string tinNhanContent, DateTime now)
        {
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
        }
    }
}
