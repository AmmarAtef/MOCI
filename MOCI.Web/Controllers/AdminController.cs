using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MOCI.Core.DTOs;
using MOCI.Services.Interfaces;
using MOCI.Services;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MOCI.DAL.Repositories;
using MTRS.Web.Utilities;
using Microsoft.Extensions.Options;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MOCI.Core.Enums;

namespace MOCI.Web.Controllers
{
    
    public class AdminController : BaseController
    {
       
        private readonly ILogger<HomeController> _logger;
     
      
        private readonly IUserService _userService;
        
        private readonly INotyfService _notyf;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly EmailService _emailService;
        private readonly EmailSettings _emailSettings;
        private readonly IConfiguration _configuration;
    

        public AdminController(ILogger<HomeController> logger, 
          
            INotyfService notyf, IUserService userService,
           IOptions<EmailSettings> emailSettings,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            _hostingEnvironment = environment;
         
          
            _userService = userService;
            _emailSettings = emailSettings.Value;
            _emailService = new EmailService(_emailSettings, _userService);
           
            _logger = logger;
            _notyf = notyf;
          
            _configuration = configuration;
           
        }

         

         

        #region Employees
        [Authorize(Roles = "Admin")]
        public IActionResult Employees()
        {
            ViewData["PageTitle"] = "Employees";
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetEmployees()
        {
            var employees = _userService.GetAll();
            return Json(employees);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditEmployee(long id)
        {
            ViewData["PageTitle"] = "Employees";

           
            var users = _userService.GetAll().ToList().OrderBy(e => e.FirstName);
            foreach (var user in users)
            {
                user.FirstName = user.FirstName + " " + user.LastName;
            }
            ViewBag.Managers = users.OrderBy(x => x.FirstName).ToList().OrderBy(e => e.FirstName); 

            var employee = _userService.GetById(id);
            return View(employee);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditEmployee(UserDto model)
        {
            ViewData["PageTitle"] = "Employees";

           
            var users = _userService.GetAll().ToList().OrderBy(e => e.FirstName); 
            foreach (var user in users)
            {
                user.FirstName = user.FirstName + " " + user.LastName;
            }
            ViewBag.Managers = users.OrderBy(x => x.FirstName).ToList().OrderBy(e => e.FirstName);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _userService.Update(model);
                _notyf.Success("Employee updated successfully");
                return RedirectToAction("Employees", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }
      


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddEmployee()
        {
            ViewData["PageTitle"] = "Add Employee";


           

            var employee = new UserDto();
            return View(employee);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddEmployee(UserDto model)
        {
            ViewData["PageTitle"] = "Add Employee";


           
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _userService.Add(model);
                _notyf.Success("Employee Added successfully");
                return RedirectToAction("Employees", "Admin", null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View(model);
            }
        }


        #endregion








    }
}
