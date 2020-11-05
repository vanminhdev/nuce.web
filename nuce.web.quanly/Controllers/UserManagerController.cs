using Newtonsoft.Json;
using nuce.web.quanly.Controllers;
using nuce.web.quanly.Models;
using nuce.web.quanly.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.quanly.Controllers
{
    public class UserManagerController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string userId)
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", $"/api/User/GetUserById?id={userId}");
            return await base.HandleResponseAsync(response,
                action200Async: async (res) =>
                {
                    var jsonString = await res.Content.ReadAsStringAsync();
                    var userUpdate = JsonConvert.DeserializeObject<UserUpdate>(jsonString);
                    var userDetail = new UserDetail()
                    {
                        UserUpdateBind = userUpdate
                    };
                    return View(userDetail);
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> GetAllUser(DataTableRequest request)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/User/GetAllUser", stringContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Redirect("");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return Redirect($"/notfound?message={HttpUtility.UrlEncode("Không có quyền truy cập")}");
            }
            else if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<DataTableResponse<UserModel>>(jsonString);
                return Json(new
                {
                    draw = res.Draw,
                    recordsTotal = res.RecordsTotal,
                    recordsFiltered = res.RecordsFiltered,
                    data = res.Data
                });
            }
            return Json(new
            {
                draw = ++request.Draw,
                recordsTotal = 0,
                recordsFiltered = 0,
                data = new List<object>()
            });
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUser(UserDetail userDetail)
        {
            base.RemoveValidMessagePartialModel<Password>(userDetail.ResetPassword, "ResetPassword");
            if (!base.IsValidPartialModel<UserUpdate>(userDetail.UserUpdateBind, "UserUpdateBind"))
            {
                return View("Detail", userDetail);
            }
            var stringContent = new StringContent(JsonConvert.SerializeObject(userDetail.UserUpdateBind), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/User/UpdateUser?id={userDetail.UserUpdateBind.id}", stringContent);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    ViewData["UpdateSuccessMessage"] = "Cập nhật thành công";
                    return View("Detail", userDetail);
                },
                action500Async: async res =>
                {
                    ViewData["UpdateErrorMessage"] = "Cập nhật không thành công";
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    ViewData["UpdateErrorMessageDetail"] = resMess.message;
                    return View("Detail", userDetail);
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(UserDetail userDetail)
        {
            base.RemoveValidMessagePartialModel<UserUpdate>(userDetail.UserUpdateBind, "UserUpdateBind");
            if (!base.IsValidPartialModel<Password>(userDetail.ResetPassword, "ResetPassword"))
            {
                return View("Detail", userDetail);
            }
            var stringContent = new StringContent(JsonConvert.SerializeObject(userDetail.ResetPassword), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/User/ResetPassword?id={userDetail.UserUpdateBind.id}", stringContent);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    ViewData["ResetPasswordSuccessMessage"] = "Khôi phục mật khẩu thành công";
                    return View("Detail", userDetail);
                },
                action500Async: async res =>
                {
                    ViewData["ResetPasswordErrorMessage"] = "Khôi phục mật khẩu không thành công";
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    ViewData["ResetPasswordErrorMessageDetail"] = resMess.message;
                    return View("Detail", userDetail);
                },
                action400: res => {
                    ViewData["ResetPasswordErrorMessage"] = "Khôi phục mật khẩu không thành công";
                    ViewData["ResetPasswordErrorMessageDetail"] = "Dữ liệu đã nhập không hợp lệ";
                    return View("Detail", userDetail);
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> ChangeStatus(UserDetail userDetail)
        {
            var action = "ActiveUser";
            if (userDetail.ChangeStatus == "unlock-user")
            {
                action = "ActiveUser";
            }
            else if (userDetail.ChangeStatus == "lock-user")
            {
                action = "DeactiveUser";
            }
            else if (userDetail.ChangeStatus == "delete-user")
            {
                action = "DeleteUser";
            }

            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/User/{action}?id={userDetail.UserUpdateBind.id}");
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    if(action == "DeleteUser")
                        return Redirect($"/usermanager/index");
                    return Redirect($"/usermanager/detail?userId={userDetail.UserUpdateBind.id}");
                },
                action500: res =>
                {
                    return Redirect($"/usermanager/detail?userId={userDetail.UserUpdateBind.id}");
                },
                action400: res =>
                {
                    return Redirect($"/usermanager/detail?userId={userDetail.UserUpdateBind.id}");
                },
                actionDefault: res =>
                {
                    return Redirect($"/usermanager/detail?userId={userDetail.UserUpdateBind.id}");
                }
            );
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new UserCreate());
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(UserCreate user)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", user);
            }
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await base.MakeRequestAuthorizedAsync("Post", $"/api/User/CreateUser", stringContent);
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    ViewData["UpdateSuccessMessage"] = "Tạo tài khoản thành công";
                    return View("Create", user);
                },
                action500Async: async res =>
                {
                    ViewData["UpdateErrorMessage"] = "Tạo tài khoản không thành công";
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    ViewData["UpdateErrorMessageDetail"] = resMess.message;
                    return View("Create", user);
                }
            );
        }
    }
}