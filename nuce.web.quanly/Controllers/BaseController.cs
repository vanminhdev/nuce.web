using Antlr.Runtime.Misc;
using Newtonsoft.Json;
using nuce.web.quanly.Common;
using nuce.web.quanly.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.quanly.Controllers
{
    public class BaseController : Controller
    {
        protected readonly HttpClient _client;
        protected readonly CookieContainer _cookieContainer;
        protected readonly string API_URL;
        private readonly Uri _apiUri;
        public BaseController()
        {
            API_URL = ConfigurationManager.AppSettings.Get("API_URL");
            _apiUri = new Uri(API_URL);

            _cookieContainer = new CookieContainer();
            HttpClientHandler _handler = new HttpClientHandler()
            {
                CookieContainer = _cookieContainer
            };
            _client = new HttpClient(_handler)
            {
                BaseAddress = _apiUri
            };
        }

        protected IEnumerable<Cookie> GetAllCookies()
        {
            return _cookieContainer.GetCookies(_apiUri).Cast<Cookie>();
        }

        private async Task<HttpResponseMessage> SendRequestAsync(string method, string requestUri, HttpContent content = null)
        {
            HttpResponseMessage response = null;
            switch (method)
            {
                case "get":
                case "Get":
                case "GET":
                    response = await _client.GetAsync(requestUri);
                    break;
                case "post":
                case "Post":
                case "POST":
                    response = await _client.PostAsync(requestUri, content);
                    break;
                case "put":
                case "Put":
                case "PUT":
                    response = await _client.PutAsync(requestUri, content);
                    break;
                case "delete":
                case "Delete":
                case "DELETE":
                    response = await _client.DeleteAsync(requestUri);
                    break;
            }
            return response;
        }

        /// <summary>
        /// Tạo 1 request với jwt token
        /// </summary>
        /// <param name="method">get, post, put, delete</param>
        /// <param name="requestUri">request uri</param>
        /// <param name="content">Dùng khi sử dụng method post</param>
        protected async Task<HttpResponseMessage> MakeRequestAuthorizedAsync(string method, string requestUri, HttpContent content = null)
        {
            var accessTokenInCookie = Request.Cookies[UserParameters.JwtAccessToken];
            if(accessTokenInCookie == null)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Unauthorized
                };
            }
            _cookieContainer.Add(_apiUri, new Cookie(UserParameters.JwtAccessToken, accessTokenInCookie.Value));
            var response = await SendRequestAsync(method, requestUri, content);

            if (response.StatusCode == HttpStatusCode.Unauthorized && Request.Cookies[UserParameters.JwtRefreshToken] != null)
            {
                //lấy token mới
                _cookieContainer.Add(_apiUri, new Cookie(UserParameters.JwtRefreshToken, Request.Cookies[UserParameters.JwtRefreshToken].Value));
                var resRefreshToken = await _client.PostAsync("/api/user/refreshToken", new StringContent(""));
                if (resRefreshToken.IsSuccessStatusCode)
                {
                    //đưa token mới vào cookie ở trình duyệt
                    IEnumerable<Cookie> responseCookies = _cookieContainer.GetCookies(_apiUri).Cast<Cookie>();
                    var accessToken = responseCookies.FirstOrDefault(c => c.Name == UserParameters.JwtAccessToken);
                    if (accessToken != null)
                    {
                        Response.Cookies[UserParameters.JwtAccessToken].Value = accessToken.Value;
                        Response.Cookies[UserParameters.JwtAccessToken].HttpOnly = true;
                        Response.Cookies[UserParameters.JwtAccessToken].Expires = accessToken.Expires;
                    }
                    //send lại request với token mới
                    _cookieContainer.Add(_apiUri, new Cookie(UserParameters.JwtAccessToken, accessToken.Value));
                    response = await SendRequestAsync(method, requestUri, content);
                }
            }

            return response;
        }

        protected async Task<ActionResult> HandleResponseAsync(
            HttpResponseMessage response,
            System.Func<HttpResponseMessage, Task<ActionResult>> action200Async = null,
            System.Func<HttpResponseMessage, ActionResult> action200 = null,
            System.Func<HttpResponseMessage, Task<ActionResult>> action500Async = null,
            System.Func<HttpResponseMessage, ActionResult> action500 = null,
            System.Func<HttpResponseMessage, Task<ActionResult>> action400Async = null,
            System.Func<HttpResponseMessage, ActionResult> action400 = null,
            System.Func<HttpResponseMessage, Task<ActionResult>> actionDefaultAsync = null,
            System.Func<HttpResponseMessage, ActionResult> actionDefault = null)
        {
            if (response.IsSuccessStatusCode)
            {
                if (action200Async != null)
                    return await action200Async(response);
                if (action200 != null)
                    return action200(response);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Redirect("/admin/account");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return Redirect($"/error?message={HttpUtility.UrlEncode("Không có quyền truy cập")}&code={(int)HttpStatusCode.Forbidden}");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return Redirect($"/error?message={HttpUtility.UrlEncode("Không tìm thấy tài nguyên")}&code={(int)HttpStatusCode.NotFound}");
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                if(action500Async != null)
                    return await action500Async(response);
                if (action500 != null)
                    return action500(response);
                return Redirect($"/error?message={HttpUtility.UrlEncode("Có lỗi xảy ra")}&code={(int)HttpStatusCode.InternalServerError}");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                if (action400Async != null)
                    return await action400Async(response);
                if (action400 != null)
                    return action400(response);
                return Redirect($"/error?message={HttpUtility.UrlEncode("Dữ liệu truyền vào không hợp lệ")}&code={(int)HttpStatusCode.BadRequest}");
            }
            if (actionDefaultAsync != null)
                return await actionDefaultAsync(response);
            if (actionDefault != null)
                return actionDefault(response);
            return View();
        }

        protected bool IsValidPartialModel<PartialModelType>(object partialModel, string partialModelName)
        {
            var tagetModel = (PartialModelType)partialModel;
            foreach (var prop in tagetModel.GetType().GetProperties())
            {
                if (ModelState[$"{partialModelName}.{prop.Name}"].Errors.Count > 0)
                {
                    return false;
                }
            }
            return true;
        }

        protected void RemoveValidMessagePartialModel<PartialModelType>(object partialModel, string partialModelName)
        {
            var tagetModel = (PartialModelType)partialModel;
            foreach (var prop in tagetModel.GetType().GetProperties())
            {
                ModelState.Remove($"{partialModelName}.{prop.Name}");
            }
        }
    }
}