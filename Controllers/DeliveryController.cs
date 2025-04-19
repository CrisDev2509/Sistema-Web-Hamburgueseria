using Bigtoria.Context;
using Bigtoria.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bigtoria.Controllers
{
    [Authorize]
    public class DeliveryController : Controller
    {
        private readonly AppDbContext _context;

        public DeliveryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int filter = -2)
        {
            ViewData["Select"] = "DELIVERY";

            if (filter == -2)
            {
                ViewData["filter"] = "-2";
                var dvm = await GetAll();

                return View(dvm);
            }
            else
            {
                ViewData["filter"] = filter.ToString();
                var dvm = await GetByState(filter);

                return View(dvm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            ViewData["Select"] = "DELIVERY";
            var delivery = await _context.Delivery.Where(d => d.Id == id)
                                            .Select(d => new DeliveryViewModel
                                            {
                                                Id = d.Id,
                                                ClientName = d.Sale.Client.Name + " " + d.Sale.Client.Lastname,
                                                Date = d.Sale.Date,
                                                Products = _context.SalesDetail.Where(p => p.SaleId == d.SaleId)
                                                                            .Select(p => new DetailViewModel
                                                                            {
                                                                                Name = p.Product.Name,
                                                                                Quantity = p.Quantity,
                                                                                
                                                                            }).ToList(),
                                                Address = d.Address,
                                                Payment = d.Sale.Payment,
                                                Reference = d.Reference

                                            }).FirstOrDefaultAsync();

            if (delivery == null) return NotFound();
            return View(delivery);
        }

        //Metodos POST
        [HttpPost]
        public async Task<IActionResult> Attended(int id)
        {
            var delivery = await _context.Delivery.FirstOrDefaultAsync(d => d.Id == id);

            if (delivery == null) return NotFound();

            delivery.State = 1;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Canceled(int id)
        {
            var delivery = await _context.Delivery.FirstOrDefaultAsync(d => d.Id == id);

            if (delivery == null) return NotFound();

            delivery.State = -1;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        //Private methods
        private async Task<List<DeliveryViewModel>> GetAll()
        {
            return await _context.Delivery.Select(d => new DeliveryViewModel
            {
                Id = d.Id,
                ClientName = d.Sale.Client.Name + " " + d.Sale.Client.Lastname,
                Date = d.Sale.Date,
                Detail = string.Join(',', _context.SalesDetail
                                        .Where(sd => sd.SaleId == d.SaleId)
                                        .Select(sd => sd.Product.Name + "(" + sd.Quantity + ")")
                                        .ToArray()),
                Status = d.State,
                SaleId = d.SaleId
            }).ToListAsync();
        }

        private async Task<List<DeliveryViewModel>> GetByState(int filter)
        {
            return await _context.Delivery.Where(d => d.State == filter)
                .Select(d => new DeliveryViewModel
                        {
                            Id = d.Id,
                            ClientName = d.Sale.Client.Name + " " + d.Sale.Client.Lastname,
                            Date = d.Sale.Date,
                            Detail = string.Join(',', _context.SalesDetail
                                                    .Where(sd => sd.SaleId == d.SaleId)
                                                    .Select(sd => sd.Product.Name + "(" + sd.Quantity + ")")
                                                    .ToArray()),
                            Status = d.State,
                            SaleId = d.SaleId
                        }).ToListAsync();
        }
    }
}
