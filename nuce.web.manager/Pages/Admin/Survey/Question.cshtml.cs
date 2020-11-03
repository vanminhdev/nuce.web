using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class QuestionModel : PageModelBase<QuestionModel>
    {
        public QuestionModel(ILogger<QuestionModel> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        [BindProperty]
        public List<Question> Questions { get; set; }

        public class Question
        {
            public string id { get; set; }
            public string ma { get; set; }
            public string content { get; set; }
            public string type { get; set; }
            public int? order { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", "/api/question/getall");
            if(response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Questions = JsonSerializer.Deserialize<List<QuestionModel.Question>>(jsonString);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Redirect("/admin/core/login");
            }
            return Page();
        }

        public void OnPost()
        {

        }
    }
}
