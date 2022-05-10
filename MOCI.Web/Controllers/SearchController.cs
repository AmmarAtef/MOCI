using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MOCI.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using AspNetCoreHero.ToastNotification.Abstractions;

using System.Linq;
using System.Collections.Generic;
using MOCI.Core.DTOs;
using MOCI.Core.Entities;
using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Authorization;

namespace MOCI.Web.Controllers
{
    [Authorize]
    public class SearchController : BaseController
    {

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notyf;
        private readonly IUserService _userService;
        private readonly IFINHUB_REVENUE_HEADERService _IFINHUB_REVENUE_DETAILService;
        private readonly IConfiguration _configuration;
        private readonly IImportsService _importsService;

        public SearchController(ILogger<HomeController> logger,
              INotyfService notyf,
            IUserService userService, IImportsService importsService, IWebHostEnvironment environment, IFINHUB_REVENUE_HEADERService ifINHUB_REVENUE_DETAILService, IConfiguration configuration)
        {
            _configuration = configuration;
            _hostingEnvironment = environment;
            _userService = userService;
            _notyf = notyf;
            _IFINHUB_REVENUE_DETAILService = ifINHUB_REVENUE_DETAILService;
            _logger = logger;
            _importsService = importsService;
        }

        public IActionResult Index()
        {
            ViewData["PageTitle"] = "Search";
            return View();
        }


        [HttpGet]
        public IActionResult Search(SearchParams searchParams)
        {
            try
            {

                var result = _importsService.GetImportedBySearch(searchParams);
                var maxRow = result.Max(e => e.Trxn_DateValue);
                var Posting_Date = result.Max(e => e.Posting_DateValue);
                ViewBag.Posting_Date = Posting_Date.Value.ToString("dd-MM-yyyy");
                var minRow = result.Min(e => e.Trxn_DateValue);
                ViewBag.MaxDate = maxRow.Value.ToString("dd-MM-yyyy");
                ViewBag.MinDate = minRow.Value.ToString("dd-MM-yyyy");
                var connection = _configuration.GetConnectionString("MOCIDataConnection");
                _IFINHUB_REVENUE_DETAILService.Connection = connection;
                var mociData = _IFINHUB_REVENUE_DETAILService.GetAllbyDate(minRow.Value, maxRow.Value);





                ViewBag.Services = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.SERVICE_NAME);
                ViewBag.Ledgers = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.LEDGER_ACCOUNT);
                ViewBag.Departments = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.DEPARTMENT);


                var mociDataDetails = _IFINHUB_REVENUE_DETAILService.GetAllDetails(minRow.Value, maxRow.Value);
                List<CombineItem> results = new List<CombineItem>();

                ViewBag.MatchedCount = 0;
                ViewBag.UnmtachedCount = 0;
                ViewBag.MatchedAmount = 0;
                ViewBag.UnmtachedAmount = 0;
                ViewBag.TotalCommision = 0;
                decimal commision = 0;
                decimal dif = 0;
                foreach (var excleSheetRow in result)
                {
                    var mociItem = mociData.Where(e => e.ImportedRowId == excleSheetRow.Id).FirstOrDefault();
                    if (mociItem != null && mociItem != default(FINHUB_REVENUE_HEADER))
                    {
                        mociItem.Details = mociDataDetails.Where(e => e.SERIAL_NUMBER == mociItem.SERIAL_NUMBER).ToList();
                        ViewBag.MatchedCount++;
                        ViewBag.MatchedAmount += mociItem.TRANSACTION_AMOUNT;
                        commision += excleSheetRow.Commission;
                        decimal val = mociItem.TRANSACTION_AMOUNT - mociItem.Details.Sum(e => e.FEES_AMOUNT);
                        dif = val + dif;
                    }
                    else
                    {
                        ViewBag.UnmtachedCount++;
                        if (excleSheetRow.Amount != null)
                            ViewBag.UnmtachedAmount += excleSheetRow.Amount;
                    }
                    CombineItem c = new CombineItem()
                    {
                        ExcleRow = excleSheetRow,
                        MOCI = mociItem
                        //excleSheetRow =, mociItem
                    };

                    results.Add(c);
                }
                ViewBag.TotalCommision = commision;
                ViewBag.Data = results;
                ViewBag.TotalRecords = ViewBag.MatchedCount + ViewBag.UnmtachedCount;

                var matchedData = results.Where(d => d.MOCI != null).ToList();
                ViewBag.MatchedData = matchedData;
                ViewBag.UnmatchedData = results.Where(d => d.MOCI == null).ToList();
                ViewBag.Diff = dif;
                var matchedDetails = matchedData.SelectMany(e => e.MOCI.Details).ToList();

                var report = matchedDetails.
               GroupBy(m => m.LEDGER_ACCOUNT).
               Select(c =>
                   new ReportRespons
                   {
                       Key = c.Key,
                       Value = c.Sum(p => p.FEES_AMOUNT)
                   }).ToList();

                ViewBag.report = report;



                decimal total = report.Sum(e => e.Value != null ? e.Value : 0);
                ViewBag.Total = total - commision + dif;
                ViewBag.DiffTotal = total - commision + dif;
                _notyf.Success("Succeeded");
                return PartialView("../Home/_ViewSummary");
            }
            catch (Exception ex)
            {
                _notyf.Error("Something wrong happen");
                _logger.LogError(ex.Message);
            }
            return BadRequest();
        }


        public IActionResult Report()
        {
            ViewData["PageTitle"] = "Report";
            return View();
        }

        [HttpGet]
        public IActionResult ReportResult(Report reportParams)
        {
            try
            {


                //var result1 = _importsService.GetReport(reportParams)
                //    .GroupBy(c => c.Trxn_DateValue.Value.Month);


                // result1.ToList().FirstOrDefault();

                var result = _importsService.GetReport(reportParams);
                var maxRow = result.Max(e => e.Trxn_DateValue);
                var Posting_Date = result.Max(e => e.Posting_DateValue);
                ViewBag.Posting_Date = Posting_Date.Value.ToString("dd-MM-yyyy");
                var minRow = result.Min(e => e.Trxn_DateValue);
                ViewBag.MaxDate = maxRow.Value.ToString("dd-MM-yyyy");
                ViewBag.MinDate = minRow.Value.ToString("dd-MM-yyyy");
                var connection = _configuration.GetConnectionString("MOCIDataConnection");
                _IFINHUB_REVENUE_DETAILService.Connection = connection;
                var mociData = _IFINHUB_REVENUE_DETAILService.GetAllbyDate(minRow.Value, maxRow.Value);





                ViewBag.Services = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.SERVICE_NAME);
                ViewBag.Ledgers = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.LEDGER_ACCOUNT);
                ViewBag.Departments = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.DEPARTMENT);


                var mociDataDetails = _IFINHUB_REVENUE_DETAILService.GetAllDetails(minRow.Value, maxRow.Value);
                List<CombineItem> results = new List<CombineItem>();

                ViewBag.MatchedCount = 0;
                ViewBag.UnmtachedCount = 0;
                ViewBag.MatchedAmount = 0;
                ViewBag.UnmtachedAmount = 0;
                ViewBag.TotalCommision = 0;
                decimal commision = 0;
                decimal dif = 0;
                foreach (var excleSheetRow in result)
                {
                    var mociItem = mociData.Where(e => e.ImportedRowId == excleSheetRow.Id).FirstOrDefault();
                    if (mociItem != null && mociItem != default(FINHUB_REVENUE_HEADER))
                    {
                        mociItem.Details = mociDataDetails.Where(e => e.SERIAL_NUMBER == mociItem.SERIAL_NUMBER).ToList();
                        ViewBag.MatchedCount++;
                        ViewBag.MatchedAmount += mociItem.TRANSACTION_AMOUNT;
                        commision += excleSheetRow.Commission;
                        decimal val = mociItem.TRANSACTION_AMOUNT - mociItem.Details.Sum(e => e.FEES_AMOUNT);
                        dif = val + dif;
                    }
                    else
                    {
                        ViewBag.UnmtachedCount++;
                        if (excleSheetRow.Amount != null)
                            ViewBag.UnmtachedAmount += excleSheetRow.Amount;
                    }
                    CombineItem c = new CombineItem()
                    {
                        ExcleRow = excleSheetRow,
                        MOCI = mociItem
                        //excleSheetRow =, mociItem
                    };

                    results.Add(c);
                }
                ViewBag.TotalCommision = commision;
                ViewBag.Data = results;
                ViewBag.TotalRecords = ViewBag.MatchedCount + ViewBag.UnmtachedCount;

                var matchedData = results.Where(d => d.MOCI != null).ToList();
                ViewBag.MatchedData = matchedData;
                ViewBag.UnmatchedData = results.Where(d => d.MOCI == null).ToList();
                ViewBag.Diff = dif;
                var matchedDetails = matchedData.SelectMany(e => e.MOCI.Details).ToList();
              
                var report = results.SelectMany(e => e.MOCI.Details).
               GroupBy(m => m.LEDGER_ACCOUNT).
               Select(c =>
                   new ReportRespons
                   {
                       Key = c.Key,
                       Value = c.Sum(p => p.FEES_AMOUNT)
                   }).ToList();

                ViewBag.report = report;

                //report
                List<string> monthNames = new List<string>(){
                    "January", "February", "March", "April", "May", "June",
  "July", "August", "September", "October", "November", "December"
                };
                var reports = new Dictionary<string, List<ReportRespons>>();
                List<decimal> totals = new List<decimal>();
                var matchedDetail = results.GroupBy(c => c.MOCI.TRANSACTION_DATE.Month);
                List<int> keys = new List<int>();

                foreach (var group in matchedDetail)
                {
                    //keys.Add(monthNames[group.Key-1]);

                    var reportByGroup = group.SelectMany(e => e.MOCI.Details).ToList()
                         .GroupBy(m => m.LEDGER_ACCOUNT).
                             Select(c =>
                             new ReportRespons
                             {
                                 Key = c.Key,
                                 Value = c.Sum(p => p.FEES_AMOUNT)
                             }).ToList();


                    reports.Add(monthNames[group.Key - 1], reportByGroup);

                    decimal total1 = reportByGroup.Sum(e => e.Value != null ? e.Value : 0);
                    totals.Add(total1 - commision + dif);
                }

                ViewData["Reports"] = reports;

                decimal total = report.Sum(e => e.Value != null ? e.Value : 0);
                ViewBag.Total = total - commision + dif;
                ViewBag.DiffTotal = total - commision + dif;
                _notyf.Success("Succeeded");
                return PartialView("../Home/_ViewSummaryReports");
            }
            catch (Exception ex)
            {
                _notyf.Error("Something wrong happen");
                _logger.LogError(ex.Message);
            }
            return BadRequest();
        }

    }
}
