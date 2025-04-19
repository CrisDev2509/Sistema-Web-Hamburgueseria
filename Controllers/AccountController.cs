using Bigtoria.Context;
using Bigtoria.Models;
using Bigtoria.utils;
using Bigtoria.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bigtoria.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        //Metdos GET
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Select"] = "ACCOUNT";
            string? id = User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")?.Value;

            if (id == null) return NotFound();

            var model = await _context.LoginEmpleyoee.Where(le => le.EmployeeId == int.Parse(id))
                                                        .Select(le => new AccountViewModel
                                                        {
                                                            Id = le.EmployeeId,
                                                            Email = le.Email,
                                                            Password = le.Password,
                                                            userId = le.Employee.Id,
                                                            Name = le.Employee.Name,
                                                            Lastname = le.Employee.Lastname,
                                                            Birthdate = le.Employee.Birthdate,
                                                            PersonalEmail = le.Employee.Email,
                                                            PhoneNumber = le.Employee.Phone
                                                        }).FirstOrDefaultAsync();

            if (model == null) return NotFound();

            return View(model);
        }

        [Authorize(Roles = "GERENTE, ADMINISTRADOR")]
        [HttpGet]
        public async Task<IActionResult> Accounts()
        {
            ViewData["Select"] = "ACCOUNT";
            States.IsChangePassword = false;
            var accounts = await _context.LoginEmpleyoee
                                .Select(le => new AccountViewModel
                                {
                                    Id = le.EmployeeId,
                                    Email = le.Email,
                                    Password = le.Password,
                                    userId = le.Employee.Id,
                                    Name = le.Employee.Name,
                                    Lastname = le.Employee.Lastname,
                                    Birthdate = le.Employee.Birthdate,
                                    PersonalEmail = le.Employee.Email,
                                    PhoneNumber = le.Employee.Phone,
                                    Type = le.Employee.EmployeeType.Name
                                })
                                .ToListAsync();

            return View(accounts);
        }

        [Authorize(Roles = "GERENTE, ADMINISTRADOR")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Select"] = "ACCOUNT";

            var employees = new CreateAccountViewModel()
            {
               Employee  = await _context.Employees.Where(e => e.LoginEmployee == null)
                                            .Select(e => new EmployeeViewModel
                                            {
                                                Id = e.Id,
                                                Name= e.Name,
                                                Lastname = e.Lastname,
                                                Email = e.Email,
                                            }).ToListAsync()
                                            
            };

            return View(employees);
        }

        [Authorize(Roles = "GERENTE, ADMINISTRADOR")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Select"] = "ACCOUNT";
            var employees = await _context.LoginEmpleyoee.Where(le => le.EmployeeId == id)
                                                    .Select(e => new CreateAccountViewModel
                                                    {
                                                        Account = new AccountViewModel
                                                        {
                                                            Id = e.Id,
                                                            Email = e.Email,
                                                            Password = e.Password,
                                                            userId = e.Employee.Id,
                                                            Name = e.Employee.Name,
                                                            Lastname = e.Employee.Lastname
                                                        }
                                                    })
                                                    .FirstOrDefaultAsync();

            return View(employees);
        }

        [Authorize(Roles = "GERENTE, ADMINISTRADOR")]
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            ViewData["Select"] = "ACCOUNT";
            var employees = await _context.LoginEmpleyoee.Where(le => le.EmployeeId == id)
                                                    .Select(e => new AccountViewModel
                                                    {
                                                        Id = e.Id,
                                                        Email = e.Email,
                                                        Password = e.Password,
                                                        userId = e.Employee.Id,
                                                        Name = e.Employee.Name,
                                                        Lastname = e.Employee.Lastname
                                                    })
                                                    .FirstOrDefaultAsync();

            return View(employees);
        }

        [HttpGet]
        public IActionResult AccessDenied(string ReturnUrl)
        {
            ViewData["Select"] = "ACCOUNT";
            return View("AccessDenied", ReturnUrl);
        }

        //Metodos POST
        [HttpPost]
        public async Task<IActionResult> ChangePassword(AccountViewModel model)
        {
            States.IsChangePassword = true;

            if (ModelState.IsValid)
            {
                var log = await _context.LoginEmpleyoee.FirstOrDefaultAsync(c => c.Id == model.Id);

                if (log == null) return NotFound();

                if(model.Password != model.RepeatPassword)
                {
                    TempData["Message"] = "Las contraseñas no son iguales";
                    return RedirectToAction("Index");
                }

                log.Password = model.Password;
                await _context.SaveChangesAsync();

                States.IsChangePassword = false;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "GERENTE, ADMINISTRADOR")]
        [HttpPost]
        public async Task<IActionResult> Create(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {

                var email = await _context.LoginEmpleyoee.Where(e => e.Email.ToUpper() == model.Email.ToUpper())
                                                        .Select(e => e.Email)
                                                        .FirstOrDefaultAsync();

                if (email != null)
                {
                    TempData["Message"] = "Ya existe este correo";
                    goto ret;
                }


                if (model.Password != model.RepeatPassword)
                {
                    TempData["Message"] = "Contraseñas no coinciden";
                    goto ret;
                }


                LoginEmployee le = new LoginEmployee()
                {
                    Email = model.Email,
                    Password = model.Password,
                    EmployeeId = model.userId
                };

                await _context.LoginEmpleyoee.AddAsync(le);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            TempData["Message"] = "Error al crear la cuenta";

            ret:
            var employees = new CreateAccountViewModel()
            {
                Employee = await _context.Employees.Where(e => e.EmployeeType == null)
                                            .Select(e => new EmployeeViewModel
                                            {
                                                Id = e.Id,
                                                Name = e.Name,
                                                Lastname = e.Lastname,
                                                Email = e.Email,
                                            }).ToListAsync(),
                Account = model

            };

            return View(employees);
        }

        [Authorize(Roles = "GERENTE, ADMINISTRADOR")]
        [HttpPost]
        public async Task<IActionResult> Edit(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = await _context.LoginEmpleyoee.Where(e => e.Email.ToUpper() == model.Account.Email.ToUpper())
                                                        .Select(e => e.Email)
                                                        .FirstOrDefaultAsync();

                if (email != null)
                {
                    TempData["Message"] = "Ya existe este correo";
                    goto ret;
                }

                if (model.Account.Password != model.Account.RepeatPassword)
                {
                    TempData["Message"] = "Las contraseñas no coinciden";
                    goto ret;
                }

                var le = await _context.LoginEmpleyoee.FirstOrDefaultAsync(le => le.Id == model.Account.Id);
                if (le == null) return NotFound();

                le.Email = model.Account.Email;
                le.Password = model.Account.Password;
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            TempData["Message"] = "Error al actualizar la cuenta";

            ret:
            return View(model);
        }

        [Authorize(Roles = "GERENTE, ADMINISTRADOR")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var le = await _context.LoginEmpleyoee.FirstOrDefaultAsync(le => le.Id == id);
            if (le == null) return NotFound();

            _context.LoginEmpleyoee.Remove(le);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
