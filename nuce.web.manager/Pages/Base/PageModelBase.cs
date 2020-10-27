using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nuce.web.manager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace nuce.web.manager.Pages.Base
{
    public class PageModelBase<T> : PageModel
    {
        protected readonly ILogger<T> _logger;
        protected readonly HttpClient _client;
        protected readonly CookieContainer _cookieContainer;
        protected readonly IConfiguration _configuration;
        protected readonly string API_URL;
        private readonly Uri _apiUri;

        public PageModelBase(ILogger<T> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            API_URL = _configuration.GetValue<string>("API_URL");
            _apiUri = new Uri(API_URL);

            _cookieContainer = new CookieContainer();
            HttpClientHandler _handler = new HttpClientHandler()
            {
                CookieContainer = _cookieContainer
            };
            _client = new HttpClient(_handler) {
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
            _cookieContainer.Add(_apiUri, new Cookie(UserParameters.JwtAccessToken, Request.Cookies[UserParameters.JwtAccessToken]));
            var response = await SendRequestAsync(method, requestUri, content);
            
            if(response.StatusCode == HttpStatusCode.Unauthorized && Request.Cookies[UserParameters.JwtRefreshToken] != null)
            {
                //lấy token mới
                _cookieContainer.Add(_apiUri, new Cookie(UserParameters.JwtRefreshToken, Request.Cookies[UserParameters.JwtRefreshToken]));
                var resRefreshToken = await _client.PostAsync("/api/user/refreshToken", new StringContent(""));
                if (resRefreshToken.IsSuccessStatusCode)
                {
                    //đưa token mới vào cookie ở trình duyệt
                    IEnumerable<Cookie> responseCookies = GetAllCookies();
                    var accessToken = responseCookies.FirstOrDefault(c => c.Name == UserParameters.JwtAccessToken);
                    var cookieOptions = new CookieOptions() { HttpOnly = true };
                    if (accessToken != null)
                        Response.Cookies.Append(UserParameters.JwtAccessToken, accessToken.Value, cookieOptions);

                    //send lại request với token mới
                    _cookieContainer.Add(_apiUri, new Cookie(UserParameters.JwtAccessToken, accessToken.Value));
                    response = await SendRequestAsync(method, requestUri, content);
                }
            }

            return response;
        }
    }
}
