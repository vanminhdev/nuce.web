using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nuce.web.manager.Attributes.ValidationAttributes;
using nuce.web.manager.Common;
using nuce.web.manager.Pages.Base;

namespace nuce.web.manager.Pages.Admin.Core.UserManager
{
    public class DetailModel : PageModelBase<DetailModel>
    {
        public DetailModel(ILogger<DetailModel> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        [BindProperty]
        public UserUpdate UserUpdateBind { get; set; }

        public class UserUpdate
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "id không được để trống")]
            public string id { get; set; }

            //chỉ để xem
            public string username { get; set; }

            [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
            public string phoneNumber { get; set; }

            [EmailAddress(ErrorMessage = "Email không hợp lệ")]
            public string email { get; set; }

            //[Required]
            //[EnumDataType(typeof(UserStatus))]
            //chỉ để xem
            public int status { get; set; }

            [Required(ErrorMessage = "Vai trò không được để trống")]
            [Roles(ErrorMessage = "Vai trò không hợp lệ")]
            public List<string> roles { get; set; }
        }

        public async Task<IActionResult> OnGetAsync([Required(AllowEmptyStrings = false)] string userId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/User/GetUserById?id={HttpUtility.UrlEncode(userId)}");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Redirect("/admin/login");
            }
            else if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                UserUpdateBind = JsonSerializer.Deserialize<UserUpdate>(jsonString);
                return Page();
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return Redirect($"/notfound?message={HttpUtility.UrlEncode("Không tìm thấy tài khoản")}");
            }
            UserUpdateBind = new UserUpdate();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateUser()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            var stringContent = new StringContent(JsonSerializer.Serialize<UserUpdate>(UserUpdateBind), Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/User/UpdateUser?id={UserUpdateBind.id}", stringContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Redirect("/admin/login");
            }
            else if (response.IsSuccessStatusCode)
            {
                ViewData["UpdateSuccessMessage"] = "Cập nhật thành công";
                return Page();
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return Redirect($"/notfound?message={HttpUtility.UrlEncode("Không tìm thấy tài khoản")}");
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                ViewData["UpdateErrorMessage"] = "Cập nhật không thành công";
                var jsonString = await response.Content.ReadAsStringAsync();
                var resMess = JsonSerializer.Deserialize<ResponseMessage>(jsonString);
                ViewData["UpdateErrorMessageDetail"] = resMess.message;
                return Page();
            }
            UserUpdateBind = new UserUpdate();
            return Page();
        }

        public Password ResetPassword { get; set; }

        public class Password
        {
            public string newPassword { get; set; }
            public string confirmPassword { get; set; }
        }

        public async Task<IActionResult> OnPostResetPassword()
        {
            ModelState.AddModelError("ResetPassword.newPassword", "abcdef");
            return Page();
        }
    }
}
