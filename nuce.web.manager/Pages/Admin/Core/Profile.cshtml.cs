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

namespace nuce.web.manager.Pages.Admin.Core
{
    public class ProfileModel : PageModelBase<ProfileModel>
    {
        public ProfileModel(ILogger<ProfileModel> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        [BindProperty]
        public UserUpdate UserUpdateBind { get; set; }

        public class UserUpdate
        {
            [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
            public string phoneNumber { get; set; }

            [EmailAddress(ErrorMessage = "Email không hợp lệ")]
            [EmailRegex(ErrorMessage = "Email không hợp lệ")]
            public string email { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/User/GetUserProfile");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Redirect("/admin/login");
            }
            else if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                this.UserUpdateBind = JsonSerializer.Deserialize<UserUpdate>(jsonString);
                return Page();
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return Redirect($"/notfound?message={HttpUtility.UrlEncode("Không tìm thấy tài khoản")}");
            }
            this.UserUpdateBind = new UserUpdate();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateUser()
        {
            base.RemoveValidMessagePartialModel<Password>(this.ChangePassword, "ChangePassword");
            if (!base.IsValidPartialModel<UserUpdate>(this.UserUpdateBind, "UserUpdateBind"))
            {
                return Page();
            }
            var stringContent = new StringContent(JsonSerializer.Serialize<UserUpdate>(UserUpdateBind), Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/User/UpdateUserProfile", stringContent);
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
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewData["UpdateErrorMessage"] = "Cập nhật không thành công";
                ViewData["UpdateErrorMessageDetail"] = "Dữ liệu đã nhập không hợp lệ";
                return Page();
            }
            UserUpdateBind = new UserUpdate();
            return Page();
        }

        [BindProperty]
        public Password ChangePassword { get; set; }

        public class Password
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu cũ không được để trống")]
            public string password { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu mới không được để trống")]
            [MinLength(6, ErrorMessage = "Mật khẩu mới dài tối thiểu 6 kí tự")]
            [MaxLength(30, ErrorMessage = "Mật khẩu mới dài tối đa 30 kí tự")]
            public string newPassword { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Xác nhận mật khẩu mới không được để trống")]
            [MinLength(6, ErrorMessage = "Mật khẩu mới dài tối thiểu 6 kí tự")]
            [MaxLength(30, ErrorMessage = "Mật khẩu mới dài tối đa 30 kí tự")]
            [Compare("newPassword", ErrorMessage = "Xác nhận mật khẩu không khớp")]
            public string confirmPassword { get; set; }
        }

        public async Task<IActionResult> OnPostChangePassword()
        {
            base.RemoveValidMessagePartialModel<UserUpdate>(this.UserUpdateBind, "UserUpdateBind");
            if (!base.IsValidPartialModel<Password>(this.ChangePassword, "ChangePassword"))
            {
                return Page();
            }
            var stringContent = new StringContent(JsonSerializer.Serialize<Password>(ChangePassword), Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/User/ChangePassword", stringContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Redirect("/admin/login");
            }
            else if (response.IsSuccessStatusCode)
            {
                ViewData["ChangePasswordSuccessMessage"] = "Đổi mật khẩu thành công";
                return Page();
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return Redirect($"/notfound?message={HttpUtility.UrlEncode("Không tìm thấy tài khoản")}");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewData["ChangePasswordErrorMessage"] = "đổi mật khẩu không thành công";
                ViewData["ChangePasswordErrorMessageDetail"] = "Dữ liệu đã nhập không hợp lệ";
                return Page();
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                ViewData["ChangePasswordErrorMessage"] = "Đổi mật khẩu không thành công";
                var jsonString = await response.Content.ReadAsStringAsync();
                var resMess = JsonSerializer.Deserialize<ResponseMessage>(jsonString);
                ViewData["ChangePasswordErrorMessageDetail"] = resMess.message;
                return Page();
            }
            return Page();
        }
    }
}
