using Bigtoria.Context;
using Bigtoria.Models;
using Bigtoria.utils;
using Bigtoria.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bigtoria.Controllers
{
    [Authorize]
    public class SaleController : Controller
    {
        private readonly AppDbContext _context;

        public SaleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id = 0)
        {
            ViewData["Select"] = "SALES";

            if(id == 0)
            {
                ViewData["filter"] = "All";
                var sales = await getAll();

                return View(sales);
            }
            else
            {
                var sales = await getByCategory(id);

                ViewData["filter"] = await _context.Categories.Where(c => c.Id == id)
                                            .Select(c => c.Name).FirstOrDefaultAsync();

                return View(sales);
            }
        }

        //Post methods
        [HttpGet]
        public async Task<IActionResult> addCard(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound(new {Message = $"No encontrado por id: {id}"});

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

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> add(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            var pList = SalesProduct.products.Where(p => p.Id == id).FirstOrDefault();

            if (product == null || pList == null) return NotFound();

            if (product.Stock > 0)
            {
                product.Stock--;
                await _context.SaveChangesAsync();

                pList.Quantity++;
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> subs(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            var pList = SalesProduct.products.Where(p => p.Id == id).FirstOrDefault();

            if (product == null || pList == null) return NotFound();

            if (pList.Quantity > 1)
            {
                product.Stock++;
                await _context.SaveChangesAsync();

                pList.Quantity--;
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> deleteAll()
        {
            if(SalesProduct.products.Count <= 0) return NotFound();

            foreach (var item in SalesProduct.products)
            {
                var p = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.Id);
                if (p == null) continue;

                p.Stock += item.Quantity;
                await _context.SaveChangesAsync();
            }

            SalesProduct.products.Clear();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> deleteById(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            var pList = SalesProduct.products.Where(p => p.Id == id).FirstOrDefault();

            if (product == null || pList == null) return NotFound();

            product.Stock += pList.Quantity;
            await _context.SaveChangesAsync();

            SalesProduct.products.Remove(pList);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Index(VoucherViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(SalesProduct.products.Count > 0)
                {
                    int? idClient = null;
                    if (model.Type != "Tienda")
                    {
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
                    }

                    string? id = User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")?.Value;
                    int idEmployee = int.Parse(id);
                    Sale s = new Sale()
                    {
                        Date = DateTime.Now,
                        SaleType = model.Type == "Tienda" ? (byte)1 : (byte)0,
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

                    if(model.Type != "Tienda")
                    {
                        Delivery d = new Delivery()
                        {
                            Address = model.Address,
                            Reference = model.Reference,
                            State = 0,
                            SaleId = sale.Entity.Id
                        };

                        await _context.Delivery.AddAsync(d);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    ViewData["Message"] = "Agregue productos al carrito";
                    return RedirectToAction("Index", "Sale");
                }

                SalesProduct.products.Clear();

                ViewData["Message"] = "Venta exitosa";

                return RedirectToAction("Index", "Sale");   
            }

            ViewData["Message"] = "Error al completar venta";

            return RedirectToAction("Index", "Sale");
        }

        //Methods
        /// <summary>
        /// Metodo para obtner todo la lista de productos que se encuentran en el 
        /// inventario con la propiedad mostrar en tienda
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Metodo para obtener el catalogo de productos por categoria
        /// </summary>
        /// <param name="id">Numero entero deiferente de 0</param>
        /// <returns></returns>
        private async Task<SalesViewModel> getByCategory(int id)
        {
            return new SalesViewModel()
            {
                Products = await _context.Products.Where(p => p.ShowStore && p.CategoryId == id)
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
