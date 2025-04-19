using Bigtoria.Context;
using Bigtoria.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Bigtoria.Controllers
{
    public class AccesoController : Controller
    {
        private readonly AppDbContext _context;

        public AccesoController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var log = await _context.LoginEmpleyoee.Where(le => le.Email == model.Email && le.Password == model.Password)
                                        .Include(e => e.Employee)
                                        .Include(e => e.Employee.EmployeeType).FirstOrDefaultAsync();

                    if (log == null)
                    {
                        TempData["Message"] = "Usuario o contraseña incorrecta";
                        return View(model);
                    }

                    if (!log.Employee.Status)
                    {
                        TempData["Message"] = "Usuario inactivo";
                        return View(model);
                    }

                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Role, log.Employee.EmployeeType.Name),
                        new Claim("EmployeeId", log.EmployeeId.ToString())
                    };

                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);

                    return RedirectToAction("Index", "Home");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return View(model);
            }
        }
    }
}
