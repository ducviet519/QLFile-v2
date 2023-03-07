using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTools.Context;
using WebTools.Models;
using WebTools.Models.Entity;
using WebTools.Services;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _services;
        public UserController(IUnitOfWork services)
        {
            _services = services;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Denied()
        {
            return View();
        }

        #region .

        private Users CheckLoginDomain(string domain, string username, string password)
        {
            try
            {
                var activeDirectoryHelper = new ActiveDirectoryHelper(domain);

                if (activeDirectoryHelper.IsAuthenticated(domain, username, password))
                {
                    var userprincipal = string.Format(@"{0}\{1}", domain, username);

                    using PrincipalContext context = new(ContextType.Domain, domain, userprincipal, password);

                    var userInfo = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userprincipal);

                    return new Users()
                    {
                        UserID = 0, //string.IsNullOrEmpty(userInfo.Description) ? 0 : int.Parse(userInfo.Description),
                        DisplayName = userInfo.DisplayName,
                        UserName = $@"{username.Trim().ToLower()}@{domain}",
                        Password = password,
                        Source = domain,
                        Email = userInfo.EmailAddress,
                        Status = (userInfo.Enabled ?? false)
                    };
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
            }
            return null;
        }

        #endregion

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        private Users GetUsers(LoginViewModel login)
        {
            string domain = "bvta.vn";
            var userprincipal = $@"{domain}\{login.UserName}";
            using PrincipalContext context = new PrincipalContext(ContextType.Domain, domain, userprincipal, login.Password);
            UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, login.UserName);
            if (user == null)
            {
                return null;
            }
            else
            {
                Users userAccount = new Users()
                {
                    UserID = 0, //string.IsNullOrEmpty(userInfo.Description) ? 0 : int.Parse(userInfo.Description),
                    DisplayName = user.DisplayName,
                    UserName = login.UserName.Trim().ToLower(),
                    Password = login.Password,
                    Source = domain,
                    Email = $@"{login.UserName.Trim().ToLower()}@{domain}",
                    Status = (user.Enabled ?? false)
                };
                return userAccount;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            string returnUrl = Request.Form["ReturnUrl"];
            try
            {
                #region Lấy thông tin người dùng từ Windows Account
                var UserInfo = GetUsers(login);
                if (UserInfo == null)
                {
                    TempData["Error"] = "Lỗi! Thông tin tài khoản hoặc mật khẩu không chính xác";
                    if (!String.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return View();
                }
                #endregion

                #region Kiểm tra thông tin User và thêm ClaimUser
                string domain = "bvta.vn";
                string adPath = $"LDAP://{domain}";
                var RoleInUser = (await _services.PhanQuyen.GetAllRoles(login.UserName)).Where(i => i.Status == true).ToList();
                var PermissionInUser = await _services.PhanQuyen.GetListPermissionKeys(login.UserName);
                var ad_authenticate = new ActiveDirectoryHelper(adPath);
                //if (isAuthorized)
                if (ad_authenticate.IsAuthenticated(domain, login.UserName, login.Password))
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, UserInfo.DisplayName),
                        new Claim(ClaimTypes.NameIdentifier, Convert.ToString(UserInfo.UserName)),
                        new Claim(ClaimTypes.GivenName, Convert.ToString(UserInfo.UserName)),
                        new Claim(ClaimTypes.GroupSid, string.IsNullOrEmpty(domain) ? "local" : domain),
                        new Claim(ClaimTypes.Email, $@"{login.UserName.Trim().ToLower()}@{domain}" ?? ""),
                    };
                    if (RoleInUser.Count > 0)
                    {
                        foreach (var role in RoleInUser)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.RoleName));

                        }
                        foreach (var permission in PermissionInUser)
                        {
                            claims.Add(new Claim("Permission", $"{permission.PermissionKey}"));
                        }
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "User"));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                        new AuthenticationProperties()
                        {
                            IsPersistent = login.RememberLogin
                        });
                    if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else
                {
                    TempData["Error"] = "Lỗi! Thông tin tài khoản hoặc mật khẩu không chính xác";
                    if (!String.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return View();
                }
                #endregion
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                TempData["Error"] = $"Lỗi! Thông tin tài khoản hoặc mật khẩu không chính xác";
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return View();
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
