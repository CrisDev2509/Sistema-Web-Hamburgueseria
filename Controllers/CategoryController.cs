using Bigtoria.Context;
using Bigtoria.Models;
using Bigtoria.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bigtoria.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                return Ok(new { success=true, categories=categories });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {success=false, message="Error en el servidor.", error=ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            if(id <= 0)
            {
                return BadRequest(new { success = false, message = "Id de categoria invalido." });
            }

            try
            {
                var category = await _context.Categories.SingleOrDefaultAsync(c => c.Id == id);

                if (category == null) return NotFound();

                return Ok(new { success=true, category=category });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error en el servidor.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Category c = new Category()
                    {
                        Name = model.Name,
                        Status = model.State,
                        ShowFilter = model.ShowFilter
                    };

                    await _context.Categories.AddAsync(c);
                    await _context.SaveChangesAsync();

                    return Ok(new { success=true, message="Guardado con exito." });
                }

                return BadRequest(new { success=false, message="Campos requeridos invalidos." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error en el servidor.", error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, CategoryViewModel model)
        {
            if (id <= 0)
            {
                return BadRequest(new { success = false, message = "Id de categoria invalido." });
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var category = await _context.Categories.SingleOrDefaultAsync(c => c.Id == id);

                    if (category == null) return NotFound();

                    category.Name = model.Name;
                    category.Status = model.State;
                    category.ShowFilter = model.ShowFilter;

                    await _context.SaveChangesAsync();

                    return Ok(new { success = true, message = "Actualizado con exito." });
                }

                return BadRequest(new { success = false, message = "Campos requeridos invalidos." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error en el servidor.", error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { success = false, message = "Id de categoria invalido." });
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var category = await _context.Categories.SingleOrDefaultAsync(c => c.Id == id);

                    if (category == null) return NotFound();

                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();

                    return Ok(new { success = true, message = "Actualizado con exito." });
                }

                return BadRequest(new { success = false, message = "Campos requeridos invalidos." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error en el servidor.", error = ex.Message });
            }
        }
    }
}
