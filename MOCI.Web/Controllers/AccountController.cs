 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MOCI.Core.Entities;
using MOCI.Web.Auth;
using MOCI.Web.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;

namespace MOCI.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly  IConfiguration _configuration;
        public AccountController(UserManager userManager, ILogger<AccountController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View(new LoginModel());
        }
        private   bool AuthenticateUser(string domainName, string userName, string password)
        {
            bool ret = false;

            try
            {
                DirectoryEntry de = new DirectoryEntry("LDAP://" + domainName, userName, password);
                DirectorySearcher dsearch = new DirectorySearcher(de);
                SearchResult results = null;

                results = dsearch.FindOne();

                ret = true;
            }
            catch
            {
                ret = false;
            }

            return ret;
        }
       
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel form)
        {
            if (!ModelState.IsValid)
                return View(form);

            try
            {
                //TODO authunticate user agenist AD

                var domain = _configuration.GetSection("Domain").Value.ToString();
                var correctUser = AuthenticateUser(domain, form.UserName, form.Password);
                //authenticate

                if (!correctUser)
                {
                    ModelState.AddModelError("summery", "Your email or password is incorrect");
                    return View(form);
                }
                    var user = new User()
                {
                    UserName = form.UserName
                };
                var result = await _userManager.SignIn(this.HttpContext, user);

                if (result)
                    return RedirectToAction("Index", "Home", null);
                else
                {
                    ModelState.AddModelError("summery", "You are not in MTRS");
                    return View(form);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                ModelState.AddModelError("summery", "Your email or password is incorrect");
                return View(form);
            }
        }

        public IActionResult LogOut()
        {
            _userManager.SignOut(this.HttpContext);
            return RedirectToAction("Login", "Account", null);
        }
    }
}
