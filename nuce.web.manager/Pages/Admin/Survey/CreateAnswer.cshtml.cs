using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nuce.web.manager.Pages.Base;

namespace nuce.web.manager.Pages.Admin.Survey
{
    public class CreateAnswerModel : PageModelBase<CreateAnswerModel>
    {
        public CreateAnswerModel(ILogger<CreateAnswerModel> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        [BindProperty]
        public Answer AnswerCreate { get; set; }

        [BindProperty]
        public string QuestionContent { get; set; }

        [BindProperty]
        public string QuestionId { get; set; }

        public class Answer
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Mã không được để trống")]
            public int? dapAnId { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Nội dung không được để trống")]
            public string content { get; set; }

            [Required(ErrorMessage = "Số thứ tự không được để trống")]
            public int? order { get; set; }

            [Required(AllowEmptyStrings = false)]
            public string cauHoiGId { get; set; }

            [Required]
            public int? cauHoiId { get; set; }
        }

        public async Task OnGetAsync([Required(AllowEmptyStrings = false)] string questionId)
        {
            var resQues = await base.MakeRequestAuthorizedAsync("Get", $"/api/question/GetById?id={questionId}");
            if (resQues.IsSuccessStatusCode)
            {
                var jsonString = await resQues.Content.ReadAsStringAsync();
                var question = (JsonSerializer.Deserialize<QuestionModel.Question>(jsonString));
                QuestionContent = question.content;
                QuestionId = questionId;
                AnswerCreate = new Answer()
                {
                    cauHoiGId = questionId,
                    cauHoiId = int.Parse(question.ma)
                };
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var content = new StringContent(JsonSerializer.Serialize(AnswerCreate), Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/answer/create", content);
            if (response.IsSuccessStatusCode)
            {
                return Redirect($"/admin/survey/answer?questionId={QuestionId}");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewData["ErrorMessage"] = "Điền thiếu thông tin";
                return Page();
            }
            else
            {
                ViewData["ErrorMessage"] = "Không tạo được câu trả lời";
                return Page();
            }
        }
    }
}
