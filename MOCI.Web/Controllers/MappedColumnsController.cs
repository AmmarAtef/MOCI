using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MOCI.Services.Interfaces;
using System.Collections.Generic;

namespace MOCI.Web.Controllers
{
    public class MappedColumnsController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notyf;
        private readonly IFINHUB_REVENUE_HEADERService _IFINHUB_REVENUE_DETAILService;
        private readonly IConfiguration _configuration;
        private readonly IMappedColumnsService _mappedColumnsService;

        public MappedColumnsController(ILogger<MappedColumnsController> logger,
             INotyfService notyf,
             IMappedColumnsService mappedColumnsService,
             IFINHUB_REVENUE_HEADERService ifINHUB_REVENUE_DETAILService
             )
        {
           
            _mappedColumnsService = mappedColumnsService;
        }


        public IActionResult Index()
        {
          List<string>names =  _mappedColumnsService.GetColumnsNames();
            return View();
        }
    }
}
