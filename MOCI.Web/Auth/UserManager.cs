using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using MOCI.Core.DTOs;
using MOCI.Core.Entities;
using MOCI.DAL.DbContexts;
using MOCI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MOCI.Web.Auth
{
    public class UserManager
    {
        private readonly IUserService _userService;

        public UserManager(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> SignIn(HttpContext httpContext, User userData)
        {
            Expression<Func<User, bool>> UserExpr = (User user) => user.UserName == userData.UserName;
            var user = _userService.GetEmployee(userData.UserName);

            if (user != null && user.Enabled)
            {
                ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(user), CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return true;
            }
            return false;
        }
        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }

        private IEnumerable<Claim> GetUserClaims(UserDto userData)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, userData.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, userData.FirstName + " " + userData.LastName));
            claims.Add(new Claim(ClaimTypes.Email, userData.Email));
            claims.Add(new Claim(ClaimTypes.GivenName, userData.UserName));


            claims.AddRange(this.GetUserRoleClaims(userData));
            return claims;
        }

        private IEnumerable<Claim> GetUserRoleClaims(UserDto userData)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, userData.Id.ToString()));

            if (userData.IsAdmin)
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));


            return claims;
        }
    }
}
