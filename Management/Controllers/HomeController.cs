using Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;    // Claims會用到
using Microsoft.AspNetCore.Authorization;
using prjDelicious.Models;
using Management.ViewModel;

namespace Management.Controllers
{
    
    public class HomeController : Controller
    {

        private readonly DeliciousContext _deliciousContext;
        //private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DeliciousContext deliciousContext)
        {
            _logger = logger;
            _deliciousContext = deliciousContext;
            // _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Logout()
        {
            // 自己宣告 Microsoft.AspNetCore.Authentication.Cookies; 命名空間
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);



            return RedirectToAction("Login", "Home");  // 回 首頁。 return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string AccountName, string Password)
        {

            if (ModelState.IsValid)
            {
                if (AccountName != null && Password != null)
                {
                    bool authentication = false;
                    string user_name = "";
                    var q = _deliciousContext.Tadmins.Where(n => n.AccountName == AccountName).Select(n => n);

                    for (int i = 0; i < 100; i++)
                    {
                        Password = Csha256.ConvertToSha256(Password);
                    }
                    foreach (var item in q)
                    {
                        if (item.AccountName == AccountName && item.Password == Password)
                        {
                            authentication = true;
                            user_name = item.AdminName;
                            break;
                        }
                    }


                    if (!authentication)
                    {
                        ViewData["ErrorMessage"] = "帳號與密碼有錯";
                        return View();
                    }

                    #region ***** 不使用ASP.NET Core Identity的 cookie 驗證 *****
                    var claims = new List<Claim>   // 搭配 System.Security.Claims; 命名空間
                {
                    new Claim(ClaimTypes.Name, user_name)
                    // new Claim(ClaimTypes.Role, "Administrator"),
                    // 如果要有「群組、角色、權限」，可以加入這一段
                };

                    // 底下的 ** 登入 Login ** 需要下面兩個參數 (1) claimsIdentity  (2) authProperties
                    var claimsIdentity = new ClaimsIdentity(
                                               claims, CookieAuthenticationDefaults.AuthenticationScheme);



                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.

                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        // The time at which the authentication ticket expires. A
                        // value set here overrides the ExpireTimeSpan option of
                        // CookieAuthenticationOptions set with AddCookie.

                        IsPersistent = true,
                        // Whether the authentication session is persisted across
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http
                        // redirect response value.
                    };


                    // *** 登入 Login *********
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                            new ClaimsPrincipal(claimsIdentity),
                                                            authProperties);
                    #endregion


                    return RedirectToAction("Index", "Home");
                }
            }

            // Something failed. Redisplay the form.
            return View();
        }
        [Authorize]
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public string Signup(CAdminViewModel Model)
        {
            string accountname = Model.AccountName;
            var count = _deliciousContext.Tadmins.Where(n => n.AccountName == accountname).Count();
            if (count > 0)
            {
                return "帳號名稱重複";
            }
            for (int i = 0; i < 100; i++)
            {
                Model.Password = Csha256.ConvertToSha256(Model.Password);
            }


            Tadmin NewTable = new Tadmin()
            {
                AdminName = Model.AdminName,
                AccountName = Model.AccountName,
                Password = Model.Password

            };
            _deliciousContext.Tadmins.Add(NewTable);
            _deliciousContext.SaveChanges();
            return "新增帳號成功";
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
