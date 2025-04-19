using Bigtoria.Context;
using Bigtoria.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bigtoria.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Select"] = "REPORTS";

            var rvm = await _context.Sales.Select(s => new ReportViewModel
            {
                Id = s.Id,
                Employee = s.Employee.Name + " " + s.Employee.Lastname,
                Type = s.SaleType,
                Date = s.Date,
                Ammout = s.Total,
                State = s.State

            }).ToListAsync();

            return View(rvm);
        }

        public async Task<IActionResult> EmployeeReport()
        {
            ViewData["Select"] = "REPORTS";

            var rvm = new ReportViewModel()
            {
                Employees = await _context.Employees.Select(e => new EmployeeReportViewModel
                {
                    Id = e.Id,
                    EmployeeName = e.Name + " " + e.Lastname,
                    RangeDate = e.Sales.Min(s => s.Date).ToString("dd/MM/yyyy") + " - " + e.Sales.Max(s => s.Date).ToString("dd/MM/yyyy"),
                    Orders = e.Sales.Count(),
                    Ammount = e.Sales.Sum(s => s.Total)
                }).ToListAsync(),
            };

            return View(rvm);
        }
    }
}
