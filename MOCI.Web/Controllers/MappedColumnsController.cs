using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MOCI.Core.DTOs;
using MOCI.Core.Entities;
using MOCI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

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


        public async Task<IActionResult> Index()
        {
            List<string> names = await _mappedColumnsService.GetColumnsNames();
            ViewBag.Columns = names;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Mapping([FromBody] List<MappedColumns> mappedColumns)
        {
            bool added = await _mappedColumnsService.AddColumns(mappedColumns);

            return View("/Home/Upload");
        }

        


    }
}
