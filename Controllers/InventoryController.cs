using Bigtoria.Context;
using Bigtoria.Models;
using Bigtoria.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Bigtoria.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly string directorioDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes");

        public InventoryController(AppDbContext context)
        {
            if (!Directory.Exists(directorioDestino))
                Directory.CreateDirectory(directorioDestino);

            _context = context;
        }

        public async Task<IActionResult> Index(bool all = true)
        {
            ViewData["Select"] = "INVENTORY";

            if (all)
            {
                ViewData["filter"] = true;
                var inv = await _context.Products.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    Discount = p.Discount,
                    ImagePath = p.ImagePath
                }).ToListAsync();

                return View(inv);
            }
            else
            {
                ViewData["filter"] = false;
                var inv = await _context.Products.Where(p => p.Stock == 0)
                                    .Select(p => new ProductViewModel
                                    {
                                        Id = p.Id,
                                        Name = p.Name,
                                        Price = p.Price,
                                        Stock = p.Stock,
                                        Discount = p.Discount,
                                        ImagePath = p.ImagePath
                                    }).ToListAsync();

                return View(inv);
            }
        }

        [Authorize(Roles = "GERENTE, ADMINISTRADOR")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Select"] = "INVENTORY";
            string? id = User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")?.Value;
            ViewData["User"] = await _context.Employees.Where(e => e.Id == int.Parse(id)).Select(e => e.EmployeeType.Name).FirstOrDefaultAsync();

            var create = new CreateProductViewModel()
            {
                Categories = await _context.Categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    State = c.Status
                }).ToListAsync()
            };
            return View(create);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            ViewData["Select"] = "INVENTORY";

            var product = await _context.Products.Where(p => p.Id == id)
                                    .Select(p => new ProductViewModel
                                    {
                                        Id= p.Id,
                                        Name= p.Name,
                                        Description = p.Description,
                                        Price = p.Price,
                                        Stock = p.Stock,
                                        Discount = p.Discount,
                                        ImagePath = p.ImagePath,
                                        CategoryName = p.Category.Name,
                                        ShowStore = p.ShowStore
                                    }).FirstOrDefaultAsync();

            if (product == null) return NotFound();

            return View(product);
        }

        [Authorize(Roles = "GERENTE, ADMINISTRADOR")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Select"] = "INVENTORY";

            var edit = new CreateProductViewModel()
            {
                Categories = await _context.Categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    State = c.Status
                }).ToListAsync(),
                Product = await _context.Products.Where(p => p.Id==id)
                                    .Select(p => new ProductViewModel
                                    {
                                        Id = p.Id,
                                        Name= p.Name,
                                        Description = p.Description,
                                        Price = p.Price,
                                        Stock = p.Stock,
                                        Discount = p.Discount,
                                        ImagePath = p.ImagePath,
                                        CategoryName = p.Category.Name,
                                        ShowStore = p.ShowStore
                                    }).FirstOrDefaultAsync()
            };

            if (edit == null || edit.Product == null) return NotFound();

            return View(edit);
        }

        //Post
        [Authorize(Roles = "GERENTE, ADMINISTRADOR")]
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                int idCategory = 0;
                if (isDigit(model.CategoryName))
                    idCategory = int.Parse(model.CategoryName);
                else
                {
                    var cat = await _context.Categories.Where(c => c.Name.ToUpper() == model.CategoryName.ToUpper())
                                            .FirstOrDefaultAsync();

                    if (cat == null)
                    {
                        Category cartegory = new Category()
                        {
                            Name = model.CategoryName,
                            Status = "ACTIVO"
                        };

                        await _context.Categories.AddAsync(cartegory);
                        await _context.SaveChangesAsync();

                        cat = await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToUpper() == cartegory.Name.ToUpper());
                    }
                    idCategory = cat.Id;
                }

                if (model.Image != null && model.Image.Length > 0)
                {
                    var nombreArchivo = Path.GetFileName(model.Image.FileName);
                    var rutaArchivo = Path.Combine(directorioDestino, nombreArchivo);
                    model.ImagePath = "/imagenes/" + nombreArchivo;

                    using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }
                }

                Product product = new Product()
                {
                    Name = model.Name,
                    Description = model.Description,
                    ShowStore = model.ShowStore,
                    Price = model.Price,
                    Stock = model.Stock,
                    Discount = model.Discount,
                    ImagePath = model.ImagePath,
                    CategoryId = idCategory,
                };

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                ViewData["Message"] = "Producto guardado con exito";

                return RedirectToAction("Index", "Inventory");
            }

            var create = new CreateProductViewModel()
            {
                Categories = await _context.Categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    State = c.Status
                }).ToListAsync(),
                Product = model
            };

            ViewData["Message"] = "Error al guardar el producto";

            return View(create);
        }

        [Authorize(Roles = "GERENTE, ADMINISTRADOR")]
        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                int idCategory = 0;
                if (isDigit(model.CategoryName))
                    idCategory = int.Parse(model.CategoryName);
                else
                {
                    var cat = await _context.Categories.Where(c => c.Name.ToUpper() == model.CategoryName.ToUpper())
                                            .FirstOrDefaultAsync();

                    if (cat == null)
                    {
                        Category cartegory = new Category()
                        {
                            Name = model.CategoryName,
                            Status = "ACTIVO"
                        };

                        await _context.Categories.AddAsync(cartegory);
                        await _context.SaveChangesAsync();

                        cat = await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToUpper() == cartegory.Name.ToUpper());
                    }

                    idCategory = cat.Id;
                }

                var prod = await _context.Products.FirstOrDefaultAsync(p => p.Id == model.Id);
                
                if (prod == null) return NotFound();

                if (model.Image != null && model.Image.Length > 0)
                {
                    var nombreArchivo = Path.GetFileName(model.Image.FileName);
                    var rutaArchivo = Path.Combine(directorioDestino, nombreArchivo);
                    model.ImagePath = "/imagenes/" + nombreArchivo;

                    using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }
                }

                prod.Name = model.Name;
                prod.Description = model.Description;
                prod.Stock = model.Stock;
                prod.Price = model.Price;
                prod.Discount = model.Discount;
                prod.ImagePath = model.ImagePath;
                prod.CategoryId = idCategory;
                prod.ShowStore = model.ShowStore;
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Inventory");
            }

            var edit = new CreateProductViewModel()
            {
                Categories = await _context.Categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    State = c.Status
                }).ToListAsync(),
                Product = model
            };

            return View(edit);
        }

        [Authorize(Roles = "GERENTE, ADMINISTRADOR")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool isDigit(string cad)
        {
            foreach (var c in cad)
                if(!char.IsDigit(c))
                    return false;

            return true;
        }
    }
}
