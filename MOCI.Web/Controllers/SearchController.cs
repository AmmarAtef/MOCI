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
using MOCI.DAL.Interfaces;
using AutoMapper;

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
        private readonly ICustomerDataService _customerDataService;
        private readonly IMapper _imapper;
        public SearchController(ILogger<HomeController> logger,
              INotyfService notyf,
              IMapper mapper,
              ICustomerDataService customerDataService,
            IUserService userService, IImportsService importsService, IWebHostEnvironment environment, IFINHUB_REVENUE_HEADERService ifINHUB_REVENUE_DETAILService, IConfiguration configuration)
        {
            _imapper = mapper;
            _configuration = configuration;
            _hostingEnvironment = environment;
            _userService = userService;
            _notyf = notyf;
            _IFINHUB_REVENUE_DETAILService = ifINHUB_REVENUE_DETAILService;
            _logger = logger;
            _importsService = importsService;
            _customerDataService = customerDataService;
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
                Search search = _imapper.Map<SearchParams, Search>(searchParams);

                var result = _importsService.GetImportedBySearch(search);

                var connection = _configuration.GetConnectionString("MOCIDataConnection");
                _IFINHUB_REVENUE_DETAILService.Connection = connection;
                //var mociData = _IFINHUB_REVENUE_DETAILService.GetAllbyDate(minRow.Value, maxRow.Value);
                var mociData = _IFINHUB_REVENUE_DETAILService.GetFinHubBySearchParams(search);


                var maxRow = mociData.Max(e => e.TRANSACTION_DATE);
                var Posting_Date = mociData.Max(e => e.TRANSACTION_DATE);
                ViewBag.Posting_Date = Posting_Date.ToString("dd-MM-yyyy");
                var minRow = mociData.Min(e => e.TRANSACTION_DATE);
                ViewBag.MaxDate = maxRow.ToString("dd-MM-yyyy");
                ViewBag.MinDate = minRow.ToString("dd-MM-yyyy");

                ViewBag.Services = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.SERVICE_NAME);
                ViewBag.Ledgers = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.LEDGER_ACCOUNT);
                ViewBag.Departments = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.DEPARTMENT);


                var mociDataDetails = _IFINHUB_REVENUE_DETAILService.GetAllDetails(minRow, maxRow);
                List<CombineItem> results = new List<CombineItem>();

                ViewBag.MatchedCount = 0;
                ViewBag.UnmtachedCount = 0;
                ViewBag.MatchedAmount = 0;
                ViewBag.UnmtachedAmount = 0;
                ViewBag.TotalCommision = 0;
                decimal commision = 0;
                decimal dif = 0;


                _customerDataService.Connection = connection;


                foreach (var mociItem in mociData)
                {
                    var excleSheetRow = result.Where(e => e.Id == mociItem.ImportedRowId).FirstOrDefault();

                    if (excleSheetRow == null)
                    {
                        ViewBag.UnmtachedCount++;
                        if (mociItem.TRANSACTION_AMOUNT != null)
                            ViewBag.UnmtachedAmount += mociItem.TRANSACTION_AMOUNT;
                    }
                    else
                    {
                        mociItem.Details = mociDataDetails.Where(e => e.SERIAL_NUMBER == mociItem.SERIAL_NUMBER).ToList();
                        ViewBag.MatchedCount++;
                        ViewBag.MatchedAmount += mociItem.TRANSACTION_AMOUNT;
                        commision += excleSheetRow.Commission;
                        decimal val = mociItem.TRANSACTION_AMOUNT - mociItem.Details.Sum(e => e.FEES_AMOUNT);
                        dif = val + dif;
                    }
                    CombineItem c = new CombineItem()
                    {
                        ExcleRow = excleSheetRow,
                        MOCI = mociItem,
                        CustomerData = _customerDataService.GetBySerialNumber(mociItem.SERIAL_NUMBER)
                        //excleSheetRow =, mociItem
                    };

                    results.Add(c);
                }

                if (!String.IsNullOrEmpty(searchParams.COMPANY_NAME))
                    results = results.Where(c => c.CustomerData != null).Where(c => c.CustomerData.COMPANY_NAME == searchParams.COMPANY_NAME).ToList();
                if (!String.IsNullOrEmpty(searchParams.APPLICANT_NAME))
                    results = results.Where(c => c.CustomerData != null).Where(c => c.CustomerData.APPLICANT_NAME == searchParams.APPLICANT_NAME).ToList();
                if (!String.IsNullOrEmpty(searchParams.COMMERCIAL_NO))
                    results = results.Where(c => c.CustomerData != null).Where(c => c.CustomerData.COMMERCIAL_NO == searchParams.COMMERCIAL_NO).ToList();


                var custData = results.Where(c => c.CustomerData != null);
                ViewBag.CustomerData = custData.Select(c => c.CustomerData).ToList();
                ViewBag.TotalCommision = commision;
                ViewBag.Data = results;
                ViewBag.TotalRecords = ViewBag.MatchedCount + ViewBag.UnmtachedCount;

                if (results.Count() < ViewBag.TotalRecords)
                {
                    ViewBag.MatchedCount = results.Where(d => d.ExcleRow != null).Count();
                    ViewBag.UnmtachedCount = results.Where(d => d.ExcleRow == null).Count();
                    ViewBag.TotalRecords = ViewBag.MatchedCount + ViewBag.UnmtachedCount;
                }


                var matchedData = results.Where(d => d.ExcleRow != null).ToList();
                ViewBag.MatchedData = matchedData;
                ViewBag.UnmatchedData = results.Where(d => d.ExcleRow == null).ToList();
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
                return PartialView("../Home/_ViewSummarySearch");
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
                reportParams.TransactionDateFrom = reportParams.TransactionDateFrom.Value.AddDays(-1 * (reportParams.TransactionDateFrom.Value.Day - 1));
                int days = System.DateTime.DaysInMonth(reportParams.TransactionDateTo.Value.Year,
                    reportParams.TransactionDateTo.Value.Month);
                reportParams.TransactionDateTo = reportParams.TransactionDateTo.Value.AddDays(days - 1);

                ViewBag.MaxDate = reportParams.TransactionDateTo;
                ViewBag.MinDate = reportParams.TransactionDateFrom;
                var connection = _configuration.GetConnectionString("MOCIDataConnection");
                _IFINHUB_REVENUE_DETAILService.Connection = connection;
                var mociData = _IFINHUB_REVENUE_DETAILService.GetFinHub(reportParams.TransactionDateFrom.Value,
                    reportParams.TransactionDateTo.Value);





                ViewBag.Services = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.SERVICE_NAME);
                ViewBag.Ledgers = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.LEDGER_ACCOUNT);
                ViewBag.Departments = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.DEPARTMENT);


                var mociDataDetails = _IFINHUB_REVENUE_DETAILService.GetFinHubDetails(reportParams.TransactionDateFrom.Value,
                    reportParams.TransactionDateTo.Value);
                //  List<CombineItem> results = new List<CombineItem>();


                mociData.ForEach(c => c.Details = mociDataDetails.Where(x => x.SERIAL_NUMBER == c.SERIAL_NUMBER).ToList());

                ViewBag.MatchedCount = 0;
                ViewBag.UnmtachedCount = 0;
                ViewBag.MatchedAmount = 0;
                ViewBag.UnmtachedAmount = 0;
                ViewBag.TotalCommision = 0;
                decimal commision = 0;
                decimal dif = 0;


                ViewBag.Data = mociData;


                var matchedData = mociData.Where(d => d.Ismatched != null).ToList();
                ViewBag.MatchedData = matchedData;
                ViewBag.UnmatchedData = mociData.Where(d => d.Ismatched == null).ToList();
                ViewBag.MatchedCount = mociData.Where(d => d.Ismatched != null).Count();
                ViewBag.UnmtachedCount = mociData.Where(d => d.Ismatched == null).Count();
                ViewBag.TotalRecords = ViewBag.MatchedCount + ViewBag.UnmtachedCount;

                var report = mociData
                            .SelectMany(e => e.Details)
                            .GroupBy(m => m.LEDGER_ACCOUNT)
                            .Select(c =>
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
                List<decimal> commisions = new List<decimal>();
                List<decimal> diffs = new List<decimal>();
                List<decimal> transactionAmounts = new List<decimal>();
                List<decimal> transactionFees = new List<decimal>();

                var groups = mociData.GroupBy(c => c.TRANSACTION_DATE.Month);
                List<int> keys = new List<int>();

                foreach (var group in groups)
                {
                    var reportByGroup = group
                        .SelectMany(e => e.Details).ToList()
                        .GroupBy(m => m.LEDGER_ACCOUNT).
                          Select(c =>
                          new ReportRespons
                          {
                              Key = c.Key,
                              Value = c.Sum(p => p.FEES_AMOUNT)
                          }).ToList();

                    //transactionAmount
                    //matched
                    decimal transactionAmount = group.Sum(c => c.TRANSACTION_AMOUNT);

                    transactionAmounts.Add(transactionAmount);
                    //Commission
                    decimal commision_month = 0;
                    var ImportedRowId = group.Where(d => d.Ismatched != null).Select(c => c.ImportedRowId).FirstOrDefault();
                    if (ImportedRowId > 0)
                        commision_month = _importsService.GetbyId((long)ImportedRowId).Commission;

                    //fees amount
                    decimal fees = group.SelectMany(c => c.Details).Sum(c => c.FEES_AMOUNT);
                    transactionFees.Add(fees);

                    //difference 
                    decimal diff_Month = transactionAmount - fees;

                    //total
                    decimal total_Month = reportByGroup.Sum(e => e.Value != null ? e.Value : 0);
                    decimal total_f = total_Month - commision_month + diff_Month;

                    //add total to list
                    totals.Add(total_f);

                    //add commision to list 
                    commisions.Add(commision_month);


                    //add diffs to list 
                    diffs.Add(diff_Month);

                    reports.Add(monthNames[group.Key - 1], reportByGroup);


                }

                ViewData["Reports"] = reports;

                ViewData["Diffs"] = diffs;
                ViewData["Total_f"] = totals;
                ViewData["Commisions"] = commisions;
                ViewBag.TotalCommision = commisions.Sum();
                ViewBag.Diff = transactionAmounts.Sum() - transactionFees.Sum();
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

        [HttpGet]
        public IActionResult Data()
        {
            return View();
        }

        [HttpGet]
        public ImportedData GetImportedData(long id)
        {
            return _importsService.GetbyId(id);
        }

        [HttpGet]
        public CustomerData GetCustomerData(string serialNumber)
        {
            var connection = _configuration.GetConnectionString("MOCIDataConnection");
            _customerDataService.Connection = connection;
            return _customerDataService.GetBySerialNumber(serialNumber);
        }

    }
}
