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
    public class EditQuestionModel : PageModelBase<EditQuestionModel>
    {
        public EditQuestionModel(ILogger<EditQuestionModel> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        [BindProperty]
        public Question QuestionEdit { get; set; }

        public class Question
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Id không được để trống")]
            public string id { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Mã không được để trống")]
            public string ma { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Nội dung không được để trống")]
            public string content { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Loại câu hỏi không được để trống")]
            public string type { get; set; }

            [Required(ErrorMessage = "Số thứ tự không được để trống")]
            public int? order { get; set; }
        }

        public async Task OnGetAsync(string id)
        {
            var response = await base.MakeRequestAuthorizedAsync("GET", $"/api/question/getbyid?id={id}");
            if(response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                QuestionEdit = JsonSerializer.Deserialize<Question>(jsonString);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            var content = new StringContent(JsonSerializer.Serialize(QuestionEdit), Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await base.MakeRequestAuthorizedAsync("PUT", $"/api/question/update?id={QuestionEdit.id}", content);
            if (response.IsSuccessStatusCode)
            {
                return Redirect("/admin/survey/question");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                ViewData["ErrorMessage"] = "Không tìm thấy câu hỏi cần cập nhật";
                return Page();
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewData["ErrorMessage"] = "Điền thiếu thông tin";
                return Page();
            }
            else
            {
                ViewData["ErrorMessage"] = "Không cập nhật được câu hỏi";
                return Page();
            }
        }
    }
}
