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
using MOCI.DAL.Interfaces;
using System.Linq.Expressions;
using MOCI.Services.Interfaces;
using MOCI.Core.DTOs;
using MOCI.Core.Entities;
using ExcelDataReader;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using MOCI.Web.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Newtonsoft.Json.Linq;
using MOCI.DAL.DbContexts;
using AutoMapper;

namespace MOCI.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notyf;
        private readonly IUserService _userService;
        private readonly IFINHUB_REVENUE_HEADERService _IFINHUB_REVENUE_DETAILService;
        private readonly IConfiguration _configuration;
        private readonly IImportsService _importsService;
        private readonly IMappedColumnsService _mappedColumnsService;
        private readonly MTRSDBContext _context;
        private readonly IMapper _mapper;
        public HomeController(ILogger<HomeController> logger,
               INotyfService notyf,
               IMapper mapper,
               IMappedColumnsService mappedColumnsService,
            IUserService userService, MTRSDBContext context, IImportsService importsService, IWebHostEnvironment environment, IFINHUB_REVENUE_HEADERService ifINHUB_REVENUE_DETAILService, IConfiguration configuration)
        {
            _configuration = configuration;
            _hostingEnvironment = environment;
            _userService = userService;
            _notyf = notyf;
            _IFINHUB_REVENUE_DETAILService = ifINHUB_REVENUE_DETAILService;
            _logger = logger;
            _importsService = importsService;
            _mappedColumnsService = mappedColumnsService;
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewData["PageTitle"] = "My Dashboard";
            return View();
        }

        [HttpGet]
        public IActionResult GetDashboard()
        {
            return null;
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Upload()
        {
            ViewData["PageTitle"] = "Upload";
            var connection = _configuration.GetConnectionString("MOCIDataConnection");
            _IFINHUB_REVENUE_DETAILService.Connection = connection;
            ViewBag.Accounts = _IFINHUB_REVENUE_DETAILService.GetAllAcounts();

            return View();
        }


        [HttpPost]
        public IActionResult ViewSummary([FromBody] FileDto fileDto)
        {
            try
            {
                //upload file 
                if (fileDto == null) return null;
                if (fileDto.File == null) return null;

                string[] data = fileDto.File.Split(new string[] { "base64," }, StringSplitOptions.None);
                Byte[] bytes = Convert.FromBase64String(data[1]);

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                DataTable dt = new DataTable();
                Stream stream = new MemoryStream(bytes);

                //loop on excel sheet
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    int count = 0;

                    while (reader.Read()) //Each row of the file
                    {
                        DataRow dr = dt.NewRow();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {


                            if (count == 0)
                            {
                                dt.Columns.Add(reader.GetValue(i).ToString().Replace(" ", "_"));
                            }
                            else
                            {
                                if (reader.GetValue(i) != null)
                                    dr[i] = reader.GetValue(i).ToString();
                            }

                        }
                        if (count > 0)
                        {
                            dt.Rows.Add(dr);
                        }

                        count++;
                    }

                }
                //remove last row of sheet if emplty
                DataRow lastRow = dt.Rows[dt.Rows.Count - 1];
                if (lastRow[0].ToString() == "" && lastRow[1].ToString() == "")
                {
                    dt.Rows.Remove(lastRow);
                }

                //remove empty rows
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    if (dt.Rows[i][1] == DBNull.Value)
                    {
                        dt.Rows[i].Delete();
                    }
                }
                dt.AcceptChanges();



                //convert dt to list of objects
                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(dt);
                List<ExcleRowDtp> excleSheetData = null;


                dynamic dynamic = JArray.Parse(JSONresult);
                dynamic dynamic_note = JObject.Parse(dynamic[0].ToString());
                var childs = ((Newtonsoft.Json.Linq.JObject)dynamic_note);
                var objects = ((Newtonsoft.Json.Linq.JObject)dynamic_note).Children().ToList();
                IEnumerable<MappedColumns> mappedColumns = _mappedColumnsService.GetAllMappedColumns();

                excleSheetData = JsonConvert.DeserializeObject<List<ExcleRowDtp>>(JSONresult);


                if ((int)excleSheetData[0].Amount == 0)
                {
                    excleSheetData = new List<ExcleRowDtp>();
                    for (int i = 0; i < dynamic.Count; i++)
                    {
                        ExcleRowDtp excleRowDtp = new ExcleRowDtp();
                        for (int len = 1; len < objects.Capacity - 1; len++)
                        {
                            string name = ((Newtonsoft.Json.Linq.JProperty)objects[len]).Name;
                            string mappedFrom = mappedColumns.FirstOrDefault(c => c.MappedTo.Replace(" ", "_") == name).MappedFrom;
                            PropertyInfo propertyInfo = excleRowDtp.GetType().GetProperty(mappedFrom);
                            propertyInfo.SetValue(excleRowDtp, Convert.ChangeType(dynamic[i][name], propertyInfo.PropertyType), null);

                        }
                        excleSheetData.Add(excleRowDtp);
                    }
                }









                //get max and min posting date
                var maxRow = excleSheetData.Max(e => e.Trxn_DateValue);
                var Posting_Date = excleSheetData.Max(e => e.Posting_DateValue);
                ViewBag.Posting_Date = Posting_Date.Value.ToString("dd-MM-yyyy");
                var minRow = excleSheetData.Min(e => e.Trxn_DateValue);
                ViewBag.MaxDate = maxRow.Value.ToString("dd-MM-yyyy");
                ViewBag.MinDate = minRow.Value.ToString("dd-MM-yyyy");


                //get all transaction from finhub from header
                var connection = _configuration.GetConnectionString("MOCIDataConnection");
                _IFINHUB_REVENUE_DETAILService.Connection = connection;
                var mociData = _IFINHUB_REVENUE_DETAILService.GetAllbyDate(minRow.Value, maxRow.Value);




                //get for ummatch form/manual match popup
                ViewBag.Services = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.SERVICE_NAME);
                ViewBag.Ledgers = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.LEDGER_ACCOUNT);
                ViewBag.Departments = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.DEPARTMENT);


                //get all transaction from finhub from details
                var mociDataDetails = _IFINHUB_REVENUE_DETAILService.GetAllDetails(minRow.Value, maxRow.Value);
                List<CombineItem> results = new List<CombineItem>();

                ViewBag.MatchedCount = 0;
                ViewBag.UnmtachedCount = 0;
                ViewBag.MatchedAmount = 0;
                ViewBag.UnmtachedAmount = 0;
                ViewBag.TotalCommision = 0;

                decimal dif = 0;

                decimal commision = 0;

                List<string> sources = new List<string>();
                sources.Add("BSS");
                sources.Add("INDUSTRY");
                sources.Add("SUPPLY");
                sources.Add("Recall");
                sources.Add("");

                //loop on every item on excel sheet
                foreach (var excleSheetRow in excleSheetData)
                {


                    //get matched iten from header
                    var mociItem = mociData.Where(e => excleSheetRow.Amount == e.TRANSACTION_AMOUNT && excleSheetRow.Amount > 0 && (sources.Contains(e.SOURCE) && e.APPROVED_CODE == excleSheetRow.Approved_Code && excleSheetRow.Terminal_Id.ToLower() != "online" && "000" + e.INVOICE_NO == excleSheetRow.Invoice_No)
                     ||
                     excleSheetRow.Amount == e.TRANSACTION_AMOUNT && excleSheetRow.Amount > 0 && (sources.Contains(e.SOURCE) && e.APPROVED_CODE == excleSheetRow.Approved_Code && e.INVOICE_NO == excleSheetRow.Invoice_No && excleSheetRow.Terminal_Id.ToLower() == "online")
                     ||
                     (excleSheetRow.Amount == e.TRANSACTION_AMOUNT && excleSheetRow.Amount > 0 && (e.SOURCE == "SW") && e.APPROVED_CODE == excleSheetRow.Approved_Code && e.CARD_NUMBER.Contains(excleSheetRow.Card_Number.Substring(12)))

                     ||
                     (excleSheetRow.Amount == e.TRANSACTION_AMOUNT && e.SOURCE == "" && e.INVOICE_NO == excleSheetRow.Invoice_No && e.APPROVED_CODE == excleSheetRow.Approved_Code && e.CARD_NUMBER.Contains(excleSheetRow.Card_Number.Substring(12)))


                      ).FirstOrDefault();

                    //get matched iten from details if header is found
                    if (mociItem != null && mociItem != default(FINHUB_REVENUE_HEADER))
                    {
                        mociItem.Details = mociDataDetails.Where(e => e.SERIAL_NUMBER == mociItem.SERIAL_NUMBER).ToList();
                        ViewBag.MatchedCount++;
                        ViewBag.MatchedAmount += excleSheetRow.Net_Amount;
                        commision += excleSheetRow.Commission;

                        decimal val = mociItem.TRANSACTION_AMOUNT - mociItem.Details.Sum(e => e.FEES_AMOUNT);
                        dif = val + dif;
                        //  SumOfDetails= mociItem.Details.Sum(e => e.FEES_AMOUNT);
                    }
                    else
                    {
                        ViewBag.UnmtachedCount++;
                        if (excleSheetRow.Amount != null)
                            ViewBag.UnmtachedAmount += excleSheetRow.Net_Amount;
                    }
                    CombineItem c = new CombineItem()
                    {
                        //   SumOfDetails = SumOfDetails,
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

                _notyf.Error("Succeeded");
                return PartialView("_ViewSummary");
            }
            catch (Exception ex)
            {
                _notyf.Error("Something wrong happen");
                _logger.LogError(ex.Message);
            }
            return BadRequest();
        }

        [HttpPost]
        public bool Save([FromBody] SaveEntity saveEntity)
        {
            try
            {

                //get data
                string json;
                json = Convert.ToString(saveEntity.Data);
                List<CombineItem> unmatchedData = JsonConvert.DeserializeObject<List<CombineItem>>(json);

                //prepare excle
                string[] data = saveEntity.File.File.Split(new string[] { "base64," }, StringSplitOptions.None);
                Byte[] bytes = Convert.FromBase64String(data[1]);
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                //read excle and convert to datatable
                DataTable dt = new DataTable();
                Stream stream = new MemoryStream(bytes);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    int count = 0;

                    while (reader.Read()) //Each row of the file
                    {
                        DataRow dr = dt.NewRow();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {


                            if (count == 0)
                            {
                                dt.Columns.Add(reader.GetValue(i).ToString().Replace(" ", "_"));
                            }
                            else
                            {
                                if (reader.GetValue(i) != null)
                                    dr[i] = reader.GetValue(i).ToString();
                            }

                        }
                        if (count > 0)
                        {
                            dt.Rows.Add(dr);
                        }

                        count++;
                    }

                }

                //remove empty row
                DataRow lastRow = dt.Rows[dt.Rows.Count - 1];
                if (lastRow[0].ToString() == "" && lastRow[1].ToString() == "")
                {
                    dt.Rows.Remove(lastRow);
                }
                //convert datattable to list of ExcleRow object
                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(dt);
                List<ExcleRowDtp> excleSheetData = JsonConvert.DeserializeObject<List<ExcleRowDtp>>(JSONresult);
                Guid g = Guid.NewGuid();

                //save all excle sheet rows
                foreach (ExcleRowDtp item in excleSheetData)
                {
                    var serializedParent = JsonConvert.SerializeObject(item);
                    ImportedData temp = JsonConvert.DeserializeObject<ImportedData>(serializedParent);
                    temp.ACCOUNT_NUMBER = saveEntity.Account;
                    temp.DateTime = DateTime.Now;
                    temp.FileName = saveEntity.File.FileName;
                    string id = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    temp.UserId = long.Parse(id);
                    temp.Guid = g.ToString();
                    _importsService.Add(temp);
                }

                //get max and min transaction date 
                var maxRow = excleSheetData.Max(e => e.Trxn_DateValue);
                var minRow = excleSheetData.Min(e => e.Trxn_DateValue);

                var connection = _configuration.GetConnectionString("MOCIDataConnection");
                _IFINHUB_REVENUE_DETAILService.Connection = connection;

                //save new unmatched data here
                foreach (var item in unmatchedData)
                {
                    item.MOCI.TRANSACTION_DATE = maxRow.Value;
                    if (item.MOCI.INVOICE_NO.StartsWith("00000"))
                    {
                        item.MOCI.INVOICE_NO = item.MOCI.INVOICE_NO.Remove(0, 3);
                    }
                    //string serial = Guid.NewGuid().ToString();
                    //item.MOCI.SERIAL_NUMBER = serial;
                    //item.MOCI.Details[0].SERIAL_NUMBER = serial;

                    _IFINHUB_REVENUE_DETAILService.Insert(item.MOCI);
                }


                //get datat from MOCI system based on dates header table

                var mociData = _IFINHUB_REVENUE_DETAILService.GetAllbyDate(minRow.Value, maxRow.Value);


                //get deatials 
                var mociDataDetails = _IFINHUB_REVENUE_DETAILService.GetAllDetails(minRow.Value, maxRow.Value);

                //get all file datat from our system DB DB
                List<ImportedData> importedData = _importsService.GetbyGuid(g.ToString());
                List<string> sources = new List<string>();
                sources.Add("BSS");
                sources.Add("INDUSTRY");
                sources.Add("SUPPLY");
                sources.Add("Recall");
                sources.Add("");
                foreach (var excleSheetRow in importedData)
                {

                    var mociItem = mociData.Where(e => excleSheetRow.Amount == e.TRANSACTION_AMOUNT && excleSheetRow.Amount > 0 && (sources.Contains(e.SOURCE) && e.APPROVED_CODE == excleSheetRow.Approved_Code && excleSheetRow.Terminal_Id.ToLower() != "online" && "000" + e.INVOICE_NO == excleSheetRow.Invoice_No)
                      ||
                      excleSheetRow.Amount == e.TRANSACTION_AMOUNT && excleSheetRow.Amount > 0 && (sources.Contains(e.SOURCE) && e.APPROVED_CODE == excleSheetRow.Approved_Code && e.INVOICE_NO == excleSheetRow.Invoice_No && excleSheetRow.Terminal_Id.ToLower() == "online")
                      ||
                      (excleSheetRow.Amount == e.TRANSACTION_AMOUNT && excleSheetRow.Amount > 0 && (e.SOURCE == "SW") && e.APPROVED_CODE == excleSheetRow.Approved_Code && e.CARD_NUMBER.Contains(excleSheetRow.Card_Number.Substring(12)))

                      ||
                      (excleSheetRow.Amount == e.TRANSACTION_AMOUNT && e.SOURCE == "" && e.INVOICE_NO == excleSheetRow.Invoice_No && e.APPROVED_CODE == excleSheetRow.Approved_Code && e.CARD_NUMBER.Contains(excleSheetRow.Card_Number.Substring(12)))


                       ).FirstOrDefault();
                    if (mociItem != null && mociItem != default(FINHUB_REVENUE_HEADER))
                    {

                        mociItem.ACCOUNT_NUMBER = saveEntity.Account;
                        mociItem.UserId = excleSheetRow.UserId;
                        mociItem.ImportedRowId = excleSheetRow.Id;
                        mociItem.MatchedTime = DateTime.Now;
                        mociItem.Ismatched = true;
                        mociItem.UserName = HttpContext.User?.FindFirst(ClaimTypes.Name)?.Value;
                        _IFINHUB_REVENUE_DETAILService.Update(mociItem);
                    }


                }




                _notyf.Success("Saved");
                return true;
            }
            catch (Exception ex)
            {
                _notyf.Error("Something wrong happen");
                _logger.LogError(ex.Message);
            }
            return false;
        }



        [HttpGet]
        public IActionResult CreateFinHub()
        {
            var connection = _configuration.GetConnectionString("MOCIDataConnection");
            _IFINHUB_REVENUE_DETAILService.Connection = connection;

            ViewBag.Terminals = _context.Terminals.ToList();
            ViewBag.Users = _userService.GetList();
            ViewBag.Services = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.SERVICE_NAME);
            ViewBag.Ledgers = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.LEDGER_ACCOUNT);
            ViewBag.Departments = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.DEPARTMENT);
            ViewBag.Accounts = _IFINHUB_REVENUE_DETAILService.GetAllAcounts();
            return View();
        }

        [HttpPost]
        public IActionResult CreateFinHub([FromBody] FINHUB_REVENUE_HEADERPostModel finHubModel)
        {
            var connection = _configuration.GetConnectionString("MOCIDataConnection");
            _IFINHUB_REVENUE_DETAILService.Connection = connection;
            FINHUB_REVENUE_HEADER finHub = _mapper.Map<FINHUB_REVENUE_HEADERPostModel, FINHUB_REVENUE_HEADER>(finHubModel);
            finHub.UserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            finHub.UserName = User.FindFirstValue(ClaimTypes.Name);
            Guid g = Guid.NewGuid();
            finHub.SERIAL_NUMBER = g.ToString();
            finHub.TRANSACTION_TIME = finHubModel.TRANSACTION_DATE.ToLocalTime();
            finHub.IsManual = 2;

            FINHUB_REVENUE_DETAIL fINHUB_REVENUE_DETAIL = new FINHUB_REVENUE_DETAIL
            {
                DEPARTMENT = finHubModel.DEPARTMENT,
                LEDGER_ACCOUNT = finHubModel.LEDGER_ACCOUNT,
                SERIAL_NUMBER = g.ToString(),
                SERVICE_NAME = finHubModel.SERVICE_NAME,
                ENTITY = "MOCI"
            };

            finHub.Details = new List<FINHUB_REVENUE_DETAIL>{
                fINHUB_REVENUE_DETAIL
            };

            _IFINHUB_REVENUE_DETAILService.InsertWithUser(finHub);
            return View("Index");
        }

    }
}
