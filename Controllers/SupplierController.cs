using Bigtoria.Context;
using Bigtoria.utils;
using Bigtoria.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bigtoria.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        private readonly AppDbContext _context;

        public SupplierController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? ruc = null)
        {
            States.MenuSelect = Menu.SUPPLIER;

            if(ruc == null)
            {
                var supp = await _context.Suppliers.ToListAsync();
                return View(supp);
            }
            else
            {
                var supp = await _context.Suppliers.Where(s => s.RUC == ruc).ToListAsync();

                return View(supp);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            TempData["Message"] = null;
            States.MenuSelect = Menu.SUPPLIER;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SupplierViewModel model)
        {
            if (ModelState.IsValid)
            {
                var supp = await _context.Suppliers.FirstOrDefaultAsync(s => s.RUC.Equals(model.RUC));

                if (supp != null)
                {
                    TempData["Message"] = "Ya existe proveedor con este RUC";
                    return View(model);
                }

                await _context.Suppliers.AddAsync(new Models.Supplier
                {
                    Name = model.Name,
                    RUC = model.RUC,
                    Email = model.Email
                });

                await _context.SaveChangesAsync();

                TempData["Message"] = "Proveedor registrado";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Complete todos los campos";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            States.MenuSelect = Menu.SUPPLIER;
            TempData["Message"] = null;
            var supp = await _context.Suppliers.Where(s => s.Id == id).Select(s => new SupplierViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                RUC = s.RUC
            }).FirstOrDefaultAsync();

            if(supp == null)
            {
                return RedirectToAction("Index");
            }

            return View(supp);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SupplierViewModel model)
        {
            if (ModelState.IsValid)
            {
                var supp = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == model.Id);

                if (supp == null)
                {
                    return RedirectToAction("Index");
                }

                supp.Name = model.Name;
                supp.Email = model.Email;
                supp.RUC = model.RUC;

                await _context.SaveChangesAsync();

                TempData["Message"] = "Proveedor actualizado";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Complete todos los campos";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            States.MenuSelect = Menu.SUPPLIER;
            TempData["Message"] = null;
            var supp = await _context.Suppliers.Where(s => s.Id == id).Select(s => new SupplierViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                RUC = s.RUC
            }).FirstOrDefaultAsync();

            if (supp == null)
            {
                return RedirectToAction("Index");
            }

            return View(supp);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SupplierViewModel model)
        {
            if (ModelState.IsValid)
            {
                var supp = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == model.Id);

                if (supp == null)
                    return RedirectToAction("Index");

                _context.Suppliers.Remove(supp);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Proveedor eliminado";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Error al eliminar";
                return View(model);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            States.MenuSelect = Menu.SUPPLIER;
            TempData["Message"] = null;
            var supp = await _context.Suppliers.Where(s => s.Id == id).Select(s => new SupplierViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                RUC = s.RUC
            }).FirstOrDefaultAsync();

            if (supp == null)
            {
                return RedirectToAction("Index");
            }

            return View(supp);
        }
    }
}
