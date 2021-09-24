using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nuce.web.api.HandleException;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Implements
{
    public class SendEmailService : ISendEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SendEmailService> _logger;
        public SendEmailService(IConfiguration _configuration, ILogger<SendEmailService> _logger)
        {
            this._configuration = _configuration;
            this._logger = _logger;
        }
        public async Task SendEmailByNuceAsync(SendEmailByNuceModel model)
        {
            HttpClient client = new HttpClient();
            var strContent = JsonSerializer.Serialize(new
            {
                emails = new[] {
                    new {
                        email = model.EmailReceiver,
                        data = model.Data
                    }
                },
                template = model.TemplateId,
                subject = model.Subject,
                email_identifier = "emails",
                datetime = DateTime.Now.ToString("dd-MM-yyyy hh:mm"),
                send_later_email = 0,
                timezone = 7
            });
            _logger.LogInformation(strContent);
            var content = new StringContent(strContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync(_configuration.GetValue<string>("ApiSendEmail"), content);
            if (!response.IsSuccessStatusCode)
            {
                throw new SendEmailException(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
