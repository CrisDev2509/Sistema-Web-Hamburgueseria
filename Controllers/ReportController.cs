using Bigtoria.Context;
using Bigtoria.utils;
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
            States.MenuSelect = Menu.REPORT;

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
            States.MenuSelect = Menu.REPORT;

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

        [HttpGet]
        public async Task<IActionResult> Invoice(int? id, bool isSale = false)
        {
            if(id is not null && id != 0)
            {
                var sales = await _context.Sales.Where(s => s.Id == id)
                            .Include(s => s.Employee)
                            .Include(s => s.Client).FirstOrDefaultAsync();

                var salesDet = await _context.SalesDetail.Where(s => s.SaleId == id)
                            .Include(sd => sd.Product)
                            .ToListAsync();

                var invoince = new InvoiceViewModel()
                {
                    CustomerName = sales.Client.Name + " " + sales.Client.Lastname,
                    EmpleyeeName = sales.Employee.Name + " " + sales.Employee.Lastname,
                    Date = sales.Date,
                    Id = sales.Id,
                    Status = sales.State == 1 ? "VENDIDO" : "CANCELADO",
                    Total = sales.Total,
                    Products = salesDet,
                    IsSale = isSale
                };

                States.MenuSelect = isSale ? Menu.SALE : Menu.REPORT;

                return View(invoince);
            }

            return View();
        }
    }
}
