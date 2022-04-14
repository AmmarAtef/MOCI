using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
 
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using MTRS.Web.Utilities;
using MOCI.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
namespace MOCI.Web.Controllers
{
    public class BaseController : Controller
    {
        private string _userName;
        private string _employeeNumber;
        private string _position;
        private List<string> _roles;
      

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _userName = filterContext.HttpContext.User?.FindFirst(ClaimTypes.Name)?.Value;
            _employeeNumber = filterContext.HttpContext.User?.FindFirst(ClaimTypes.PostalCode)?.Value;
            _position = filterContext.HttpContext.User?.FindFirst(ClaimTypes.Actor)?.Value;
            _roles = filterContext.HttpContext.User?.FindAll(ClaimTypes.Role)?.Select(x => x.Value).ToList();

            ViewBag.UserName = _userName;
            ViewBag.EmployeeNumber = _employeeNumber;
            ViewBag.Photo = _employeeNumber+".png";
            ViewBag.Roles = _roles;
            ViewBag.Position = _position;
        }
        

    }
}
