using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nuce.web.manager.Pages.Base;

namespace nuce.web.manager.Pages.Admin.Survey
{
    public class AnswerModel : PageModelBase<AnswerModel>
    {
        public AnswerModel(ILogger<AnswerModel> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        [BindProperty]
        public List<Answer> Answers { get; set; }

        [BindProperty]
        public string QuestionContent { get; set; }

        [BindProperty]
        public string QuestionId { get; set; }

        public class Answer
        {
            public string id { get; set; }

            public int? dapAnId { get; set; }

            public string content { get; set; }

            public int? order { get; set; }

            public string cauHoiGId { get; set; }

            public int? cauHoiId { get; set; }
        }

        public async Task OnGetAsync(string questionId)
        {
            var resQues = await base.MakeRequestAuthorizedAsync("Get", $"/api/question/GetById?id={questionId}");
            if (resQues.IsSuccessStatusCode)
            {
                var jsonString = await resQues.Content.ReadAsStringAsync();
                QuestionContent = (JsonSerializer.Deserialize<QuestionModel.Question>(jsonString)).content;
                QuestionId = questionId;
            }

            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/answer/GetByQuestionId?questionId={questionId}");
            if(response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Answers = JsonSerializer.Deserialize<List<AnswerModel.Answer>>(jsonString);
            }
        }

        public void OnPost()
        {

        }
    }
}
