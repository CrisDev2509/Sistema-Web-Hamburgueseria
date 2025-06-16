using Bigtoria.Context;
using Bigtoria.Models;
using Bigtoria.utils;
using Bigtoria.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Bigtoria.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            States.MenuSelect = Menu.HOME;
            string? id = User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")?.Value;

            if (id == null)
                return NotFound();

            var user = await _context.Employees.FirstOrDefaultAsync(e => e.Id == int.Parse(id));
            var delivery = await _context.Delivery.Where(d => d.State == 0 && EF.Functions.DateDiffDay(d.Sale.Date, DateTime.Now) == 0).CountAsync();
            var inventory = await _context.Products.Where(p => p.Stock > 0 && p.ShowStore).Select(p => p.Stock).ToListAsync();
            var orders = await _context.Sales.Where(s => s.State == 1 && EF.Functions.DateDiffDay(s.Date, DateTime.Now) == 0).CountAsync();
            var totales = await _context.Sales.Where(s => EF.Functions.DateDiffDay(s.Date, DateTime.Now) == 0).Select(s => s.Total).ToListAsync();
            var employees = await _context.Employees
                                        .Include(e => e.Sales)
                                        .Select(e => new EmployeeRankViewModel
                                        {
                                            Fullname = e.Name + " " + e.Lastname,
                                            Total = e.Sales.Sum(s => s.Total)
                                        })
                                        .OrderByDescending(e => e.Total)
                                        .ToListAsync();



            decimal total = 0;
            decimal stock = 0;

            foreach (var item in inventory)
            {
                stock += item;
            }

            foreach (var order in totales)
            {
                total += order;
            }

            HomeViewModel home = new HomeViewModel()
            {
                Name = user.Name + " " + user.Lastname,
                Delivery = delivery,
                Inventory = stock,
                Orders = orders,
                TotalSales = total,
                Employee = employees
            };

            return View(home);
        }

        //Obtener datos para grafico
        [HttpGet]
        public async Task<IActionResult> getData()
        {
            var date = DateTime.Parse($"1/{DateTime.Now.Month}/{DateTime.Now.Year}");

            object[] data = new object[DateTime.Now.Day];
            for (int i = 0; i < DateTime.Now.Day; i++)
            {
                DateTime aux = date.AddDays(i);
                decimal value = 0;
                var sales = await _context.Sales.Where(s => s.Date.Date == aux.Date).ToListAsync();
                foreach (var item in sales)
                {
                    value += item.Total;
                }

                data[i] = new { Date = aux.ToString("yyyy/MM/dd"), Value = value };
            }

            return Json(data);
        }
    }
}
