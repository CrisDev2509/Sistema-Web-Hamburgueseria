using Bigtoria.Context;
using Bigtoria.Models;
using Bigtoria.utils;
using Bigtoria.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bigtoria.Controllers
{
    public class UserCardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly string directorioDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes");

        public UserCardController(AppDbContext context)
        {
            if (!Directory.Exists(directorioDestino))
                Directory.CreateDirectory(directorioDestino);

            _context = context;
        }

        //GET
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sales = await getAll();
            return View(sales);
        }

        [HttpGet]
        public IActionResult Shop()
        {
            States.MenuSelect = Menu.SALE;
            return View();
        }

        //Post methods
        [HttpPost]
        public async Task<IActionResult> addCard(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            if (SalesProduct.products.Where(p => p.Id == product.Id).FirstOrDefault() != null)
            {
                if (product.Stock > 0)
                {
                    SaleProductViewModel? spvm = SalesProduct.products.Where(p => p.Id == product.Id).FirstOrDefault();

                    if (spvm != null)
                        spvm.Quantity++;
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (product.Stock > 0)
                {
                    SaleProductViewModel spvm = new SaleProductViewModel()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Discount = product.Discount,
                        Price = product.Price,
                        Quantity = 1
                    };

                    SalesProduct.products.Add(spvm);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            product.Stock--;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Shop(VoucherViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int idDel = 0;
                    if (SalesProduct.products.Count > 0)
                    {
                        int? idClient = null;
                        Client c = new Client()
                        {
                            Name = model.Name,
                            Lastname = model.Lastname,
                            Email = model.Lastname,
                            Phone = model.Phone
                        };

                        var client = await _context.Clients.AddAsync(c);
                        await _context.SaveChangesAsync();

                        idClient = client.Entity.Id;

                        string? id = User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")?.Value;
                        int idEmployee = int.Parse(id);
                        Sale s = new Sale()
                        {
                            Date = DateTime.Now,
                            SaleType = (byte)0,
                            Payment = model.Payment,
                            PercentageIGV = 10,
                            IGV = SalesProduct.getIgv(),
                            SubTotal = SalesProduct.getSubTotal(),
                            Total = SalesProduct.getAmmount(),
                            State = 1,
                            ClientId = idClient,
                            EmployeeId = idEmployee
                        };

                        var sale = await _context.Sales.AddAsync(s);
                        await _context.SaveChangesAsync();

                        foreach (var item in SalesProduct.products)
                        {
                            SaleDetail sd = new SaleDetail()
                            {
                                ProductId = item.Id,
                                Quantity = item.Quantity,
                                UnitPrice = item.Price,
                                SaleId = sale.Entity.Id
                            };

                            await _context.SalesDetail.AddAsync(sd);
                            await _context.SaveChangesAsync();
                        }

                        Delivery d = new Delivery()
                        {
                            Address = model.Address,
                            Reference = model.Reference,
                            State = 0,
                            SaleId = sale.Entity.Id
                        };

                        var del = await _context.Delivery.AddAsync(d);
                        await _context.SaveChangesAsync();
                        idDel = del.Entity.Id;

                        SalesProduct.products.Clear();

                        TempData["Message"] = $"Venta exitosa, orden N° {idDel}";

                        return RedirectToAction("Invoice", "Report", new {id = sale.Entity.Id, isSale = true });
                    }
                    else
                    {
                        TempData["Message"] = "Agregue productos al carrito";
                        return RedirectToAction("Index", "UserCard");
                    }
                }

                TempData["Message"] = "Error al completar venta";

                return RedirectToAction("Shop", "UserCard");
            }
            catch (Exception e)
            {
                TempData["Message"] = e.Message;

                return RedirectToAction("Shop", "UserCard");
            }
        }

        //Private methods
        private async Task<SalesViewModel> getAll()
        {
            return new SalesViewModel()
            {
                Products = await _context.Products.Where(p => p.ShowStore)
                                .Select(p => new ProductViewModel
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Description = p.Description,
                                    Stock = p.Stock,
                                    Price = p.Price,
                                    Discount = p.Discount,
                                    ImagePath = p.ImagePath,
                                    CategoryName = p.Category.Name,
                                }).ToListAsync(),
                Categories = await _context.Categories.Where(c => c.ShowFilter)
                                        .Select(c => new CategoryViewModel
                                        {
                                            Id = c.Id,
                                            Name = c.Name,
                                            State = c.Status
                                        })
                                        .ToListAsync()
            };
        }
    }
}
