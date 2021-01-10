using nuce.web.shared;
using nuce.web.survey.student.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.survey.student.Attributes.ActionFilter
{
    /// <summary>
    /// rolesCheck viết role cách nhau bởi dấu ,
    /// </summary>
    public class AuthorizeActionFilter : ActionFilterAttribute, IActionFilter
    {
        /// <summary>
        /// role kiểm tra
        /// </summary>
        private string _rolesCheck;

        public AuthorizeActionFilter(string rolesCheck = null)
        {
            _rolesCheck = rolesCheck;
        }

        public AuthorizeActionFilter(params string[] rolesCheck)
        {
            _rolesCheck = string.Join(",", rolesCheck);
        }

        private void CheckRole(ActionExecutingContext filterContext, string accessToken)
        {
            // rolesCheck khác null mới kiểm tra vai trò không thì thôi
            if(_rolesCheck == null)
            {
                return;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(accessToken);
            var roles = jwtSecurityToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).ToList();
            var rolesCheck = _rolesCheck.Split(',').ToList();
            foreach (var role in roles)
            {
                //có role đó
                if (rolesCheck.Any(r => r == role))
                {
                    return;
                }
            }
            filterContext.Result = new RedirectResult($"/error?message={HttpUtility.UrlEncode("Không có quyền truy cập")}&code={(int)HttpStatusCode.Forbidden}");
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var roleCheckSplited = _rolesCheck.Split(',');
            var roleCheckFirst = roleCheckSplited.FirstOrDefault();
            if (roleCheckSplited.FirstOrDefault(s => s == RoleNames.UndergraduateStudent) != null)
            {
                roleCheckFirst = RoleNames.UndergraduateStudent;
            }

            var accessTokenCookie = filterContext.RequestContext.HttpContext.Request.Cookies[UserParameters.JwtAccessToken];
            var refreshTokenCookie = filterContext.RequestContext.HttpContext.Request.Cookies[UserParameters.JwtRefreshToken];
            if(accessTokenCookie == null && refreshTokenCookie == null)
            {
                RedirecLoginPage(roleCheckFirst, filterContext);
            }
            else if(accessTokenCookie == null && refreshTokenCookie != null)
            {
                //lấy access token mới
                var _cookieContainer = new CookieContainer();
                var API_URL = ConfigurationManager.AppSettings.Get("API_URL");
                var _apiUri = new Uri(API_URL);
                var _handler = new HttpClientHandler()
                {
                    CookieContainer = _cookieContainer
                };
                var _client = new HttpClient(_handler)
                {
                    BaseAddress = _apiUri
                };
                _cookieContainer.Add(_apiUri, new Cookie(UserParameters.JwtRefreshToken, refreshTokenCookie.Value));
                var resRefreshToken = _client.PostAsync("/api/user/refreshToken", new StringContent("")).GetAwaiter().GetResult();
                if (resRefreshToken.IsSuccessStatusCode)
                {
                    //đưa token mới vào cookie ở trình duyệt
                    IEnumerable<Cookie> responseCookies = _cookieContainer.GetCookies(_apiUri).Cast<Cookie>();
                    var newAccessToken = responseCookies.FirstOrDefault(c => c.Name == UserParameters.JwtAccessToken);
                    if (newAccessToken != null)
                    {
                        filterContext.RequestContext.HttpContext.Response.Cookies[UserParameters.JwtAccessToken].Value = newAccessToken.Value;
                        filterContext.RequestContext.HttpContext.Response.Cookies[UserParameters.JwtAccessToken].HttpOnly = true;
                        filterContext.RequestContext.HttpContext.Response.Cookies[UserParameters.JwtAccessToken].Expires = newAccessToken.Expires;
                        //kiểm tra lại role trong access token
                        CheckRole(filterContext, newAccessToken.Value);
                    }
                    else // không lấy được access token mới cho đăng nhập lại
                    {
                        RedirecLoginPage(roleCheckFirst, filterContext);
                    }
                }
                else // không lấy được access token mới cho đăng nhập lại
                {
                    RedirecLoginPage(roleCheckFirst, filterContext);
                }
            }
            else //kiểm tra luôn
            {
                CheckRole(filterContext, accessTokenCookie.Value);
            }
        }

        private void RedirecLoginPage(string role, ActionExecutingContext filterContext)
        {
            if (role == RoleNames.Student)
            {
                filterContext.Result = new RedirectResult("/account/login");
            } else if (role == RoleNames.UndergraduateStudent)
            {
                filterContext.Result = new RedirectResult("/account/loginundergraduate");
            }
            else if (role == RoleNames.GraduateStudent)
            {
                filterContext.Result = new RedirectResult("/account/logingraduate");
            }
            else if (role == RoleNames.KhaoThi_Survey_KhoaBan)
            {
                filterContext.Result = new RedirectResult("/surveyresult/login?loginType=2");
            }
            else if (role == RoleNames.KhaoThi_Survey_Department)
            {
                filterContext.Result = new RedirectResult("/surveyresult/login?loginType=3");
            }
            else if (role == RoleNames.KhaoThi_Survey_GiangVien)
            {
                filterContext.Result = new RedirectResult("/surveyresult/login?loginType=4");
            }
            else
            {
                filterContext.Result = new RedirectResult("/account/login");
            }
        }

    }
}