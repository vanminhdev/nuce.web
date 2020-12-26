using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Survey;

namespace nuce.web.api.Controllers.Survey
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ILogger<ContactController> _logger;
        private readonly ISendEmailService _sendEmailService;
        private readonly IConfiguration _configuration;
        public ContactController(ILogger<ContactController> _logger, ISendEmailService _sendEmailService, IConfiguration _configuration)
        {
            this._logger = _logger;
            this._sendEmailService = _sendEmailService;
            this._configuration = _configuration;
        }
        [Route("email/ktdb")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SendEmailKtdb([FromBody] SendEmailKtdbModel model)
        {
            try
            {
                var TemplateId = _configuration.GetValue<int>("EmailInfo:Ktdb:TemplateId");
                var EmailReceiver = _configuration.GetValue<string>("EmailInfo:Ktdb:Email");
                var sendEmailModel = new SendEmailByNuceModel
                {
                    EmailReceiver = EmailReceiver,
                    Subject = "Hỗ trợ người dùng",
                    Data = new {
                        name = model.Name,
                        email = model.Email,
                        content = model.Content
                    },
                    TemplateId = TemplateId,
                };
                await _sendEmailService.SendEmailByNuceAsync(sendEmailModel);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(new ResponseBody {
                    Data = ex,
                    Message = ex.Message,
                });
            }
        }
    }
}