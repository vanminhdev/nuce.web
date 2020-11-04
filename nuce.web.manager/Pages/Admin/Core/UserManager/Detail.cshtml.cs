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
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
            [EmailRegex(ErrorMessage = "Email không hợp lệ")]
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
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return Redirect($"/notfound?message={HttpUtility.UrlEncode("Không có quyền truy cập")}");
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
            base.RemoveValidMessagePartialModel<Password>(this.ResetPassword, "ResetPassword");
            if (!base.IsValidPartialModel<UserUpdate>(this.UserUpdateBind, "UserUpdateBind"))
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
        public Password ResetPassword { get; set; }

        public class Password
        {
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

        public async Task<IActionResult> OnPostResetPassword()
        {
            base.RemoveValidMessagePartialModel<UserUpdate>(this.UserUpdateBind, "UserUpdateBind");
            if (!base.IsValidPartialModel<Password>(this.ResetPassword, "ResetPassword"))
            {
                return Page();
            }
            var stringContent = new StringContent(JsonSerializer.Serialize<Password>(ResetPassword), Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/User/ResetPassword?id={UserUpdateBind.id}", stringContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Redirect("/admin/login");
            }
            else if (response.IsSuccessStatusCode)
            {
                ViewData["ResetPasswordSuccessMessage"] = "Khôi phục mật khẩu thành công";
                return Page();
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return Redirect($"/notfound?message={HttpUtility.UrlEncode("Không tìm thấy tài khoản")}");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewData["ResetPasswordErrorMessage"] = "Khôi phục mật khẩu không thành công";
                ViewData["ResetPasswordErrorMessageDetail"] = "Dữ liệu đã nhập không hợp lệ";
                return Page();
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                ViewData["ResetPasswordErrorMessage"] = "Khôi phục mật khẩu không thành công";
                var jsonString = await response.Content.ReadAsStringAsync();
                var resMess = JsonSerializer.Deserialize<ResponseMessage>(jsonString);
                ViewData["ResetPasswordErrorMessageDetail"] = resMess.message;
                return Page();
            }
            return Page();
        }
    
        [BindProperty]
        public ChangeUserStatus ActionUserStatus { get; set; }

        public class ChangeUserStatus
        {
            public string id { get; set; }
            public string action { get; set; }
        }

        public async Task<IActionResult> OnPostChangeUserStatus()
        {
            HttpResponseMessage response = null;

            if(ActionUserStatus.action == "unlock-user")
            {
                response = await base.MakeRequestAuthorizedAsync("Put", $"/api/User/ActiveUser?id={ActionUserStatus.id}");
            }
            else if (ActionUserStatus.action == "lock-user")
            {
                response = await base.MakeRequestAuthorizedAsync("Put", $"/api/User/DeactiveUser?id={ActionUserStatus.id}");
            }
            else if (ActionUserStatus.action == "delete-user")
            {
                response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/User/DeleteUser?id={ActionUserStatus.id}");
            }

            if (response == null)
            {
                return Redirect($"/admin/usermanager/detail?userId={ActionUserStatus.id}");
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Redirect("/admin/login");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return Redirect($"/notfound?message={HttpUtility.UrlEncode("Không tìm thấy tài khoản")}");
            }
            else if (response.IsSuccessStatusCode && ActionUserStatus.action == "delete-user")
            {
                //xoá thì quay lại trang index
                return Redirect($"/admin/usermanager/index");
            }
            else //thành công hoặc lỗi đều quay lại trang cũ
            {
                return Redirect($"/admin/usermanager/detail?userId={ActionUserStatus.id}");
            }
        }
    }
}
