using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nuce.web.manager.Attributes.ValidationAttributes;
using nuce.web.manager.Pages.Base;

namespace nuce.web.manager.Pages.Admin.Core.UserManager
{
    public class CreateModel : PageModelBase<CreateModel>
    {
        public CreateModel(ILogger<CreateModel> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        [BindProperty]
        public UserCreate UserCreateBind { get; set; }

        public class UserCreate
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Tên đăng nhập không được để trống")]
            [MinLength(1, ErrorMessage = "Tên đăng nhập tối thiểu 1 ký tự")]
            [MaxLength(30, ErrorMessage = "Tên đăng nhập tối đa 30 kí tự")]
            public string username { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Tên đăng nhập không được để trống")]
            [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 kí tự")]
            [MaxLength(30, ErrorMessage = "Mật khẩu tối đa 30 kí tự")]
            public string password { get; set; }

            [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
            public string phoneNumber { get; set; }

            [EmailRegex(ErrorMessage = "Email không hợp lệ")]
            [EmailAddress]
            public string email { get; set; }

            [Required(ErrorMessage = "Vai trò không được để trống")]
            [Roles(ErrorMessage = "Vai trò không hợp lệ")]
            public List<string> roles { get; set; }
        }

        public void OnGet()
        {
        }

        class UserId
        {
            public string id { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!base.IsValidPartialModel<UserCreate>(this.UserCreateBind, "UserCreateBind"))
            {
                return Page();
            }
            var stringContent = new StringContent(JsonSerializer.Serialize<UserCreate>(UserCreateBind), Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/User/CreateUser", stringContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Redirect("/admin/login");
            }
            else if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var resMess = JsonSerializer.Deserialize<UserId>(jsonString);
                return Redirect($"/admin/usermanager/detail?userId={resMess.id}");
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                ViewData["UpdateErrorMessage"] = "Tạo không thành công";
                var jsonString = await response.Content.ReadAsStringAsync();
                var resMess = JsonSerializer.Deserialize<ResponseMessage>(jsonString);
                ViewData["UpdateErrorMessageDetail"] = resMess.message;
                return Page();
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewData["CreateErrorMessage"] = "Tạo không thành công";
                ViewData["CreateErrorMessageDetail"] = "Dữ liệu đã nhập không hợp lệ";
                return Page();
            }
            return Page();
        }
    }
}
