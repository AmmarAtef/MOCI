using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MOCI.Core.DTOs;
using MOCI.DAL.DbContexts;
using MOCI.Services.Interfaces;

namespace MOCI.Web.Controllers
{
    public class TerminalController : BaseController
    {
        private readonly MTRSDBContext _context;
        private readonly INotyfService _notyf;
        private readonly IFINHUB_REVENUE_HEADERService _IFINHUB_REVENUE_DETAILService;
        private readonly IConfiguration _configuration;
        public TerminalController(MTRSDBContext context, IConfiguration configuration, IFINHUB_REVENUE_HEADERService ifINHUB_REVENUE_DETAILService, INotyfService notyf)
        {
            _IFINHUB_REVENUE_DETAILService = ifINHUB_REVENUE_DETAILService;
            _configuration = configuration;
            _context = context;
            _notyf = notyf;
        }

        // GET: Terminal
        public async Task<IActionResult> Index()
        {
            return View(await _context.Terminals.ToListAsync());
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(_context.Terminals.ToList());
        }
        // GET: Terminal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terminalDto = await _context.Terminals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (terminalDto == null)
            {
                return NotFound();
            }

            return View(terminalDto);
        }

        // GET: Terminal/Create
        public IActionResult Create()
        {
            // ViewBag.Services = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.SERVICE_NAME);
            var connection = _configuration.GetConnectionString("MOCIDataConnection");
            _IFINHUB_REVENUE_DETAILService.Connection = connection;
            ViewBag.Ledgers = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.LEDGER_ACCOUNT);
            ViewBag.Departments = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.DEPARTMENT);

            return View();
        }

        // POST: Terminal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TerminalId,MerchantId,Location,Department,Account")] TerminalDto terminalDto)
        {
            var terminal = _context.Terminals.Where(r => r.TerminalId == terminalDto.TerminalId).FirstOrDefault();

            if (ModelState.IsValid && (terminal == null || terminal.TerminalId == "Online"))
            {
                _context.Add(terminalDto);
                await _context.SaveChangesAsync();
                _notyf.Success("The terminals is added");
                return RedirectToAction(nameof(Index));
            }
            _notyf.Error("This terminalID already exisit before");
            var connection = _configuration.GetConnectionString("MOCIDataConnection");
            _IFINHUB_REVENUE_DETAILService.Connection = connection;
            ViewBag.Ledgers = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.LEDGER_ACCOUNT);
            ViewBag.Departments = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.DEPARTMENT);

            return View(terminalDto);
        }

        // GET: Terminal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var connection = _configuration.GetConnectionString("MOCIDataConnection");
            _IFINHUB_REVENUE_DETAILService.Connection = connection;
            ViewBag.Ledgers = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.LEDGER_ACCOUNT);
            ViewBag.Departments = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.DEPARTMENT);

            if (id == null)
            {
                return NotFound();
            }

            var terminalDto = await _context.Terminals.FindAsync(id);
            if (terminalDto == null)
            {
                return NotFound();
            }
            return View(terminalDto);
        }

        // POST: Terminal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TerminalId,MerchantId,Location,Department,Account")] TerminalDto terminalDto)
        {
            if (id != terminalDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(terminalDto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerminalDtoExists(terminalDto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var connection = _configuration.GetConnectionString("MOCIDataConnection");
            _IFINHUB_REVENUE_DETAILService.Connection = connection;
            ViewBag.Ledgers = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.LEDGER_ACCOUNT);
            ViewBag.Departments = _IFINHUB_REVENUE_DETAILService.GetAllUnique(DAL.Repositories.Cols.DEPARTMENT);

            return View(terminalDto);
        }

        // GET: Terminal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terminalDto = await _context.Terminals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (terminalDto == null)
            {
                return NotFound();
            }

            return View(terminalDto);
        }

        // POST: Terminal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var terminalDto = await _context.Terminals.FindAsync(id);
            _context.Terminals.Remove(terminalDto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TerminalDtoExists(int id)
        {
            return _context.Terminals.Any(e => e.Id == id);
        }
    }
}
