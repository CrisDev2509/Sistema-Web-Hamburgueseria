using Bigtoria.Context;
using Bigtoria.Models;
using Bigtoria.utils;
using Bigtoria.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bigtoria.Controllers
{
    [Authorize(Roles = "GERENTE, ADMINISTRADOR")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            States.MenuSelect = Menu.USERS;
            var list = await _context.Employees.Select(e => new EmployeeViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Lastname = e.Lastname,
                Email = e.Email,
                Birthdate = DateTime.Parse(e.Birthdate),
                ContractDate = e.ContractDate,
                ImagePath = e.ImagePath,
                Phone = e.Phone,
                Status = e.Status,
                CategoryType = new CategoryTypeViewModel
                {
                    IdCategoryType = e.EmployeeType.Id,
                    Name = e.EmployeeType.Name
                }
            }).ToListAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            States.MenuSelect = Menu.USERS;
            var model = new CreateEmployeeViewModel
            {
                Categories = await _context.EmployeesType.Where(et => et.Id != 1).Select(et => new CategoryTypeViewModel
                {
                    IdCategoryType = et.Id,
                    Name = et.Name,
                    Status = et.Status
                }).ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                int idType = 0;
                if (model.CategoryType.IdCategoryType == 0)
                {
                    var check = await _context.EmployeesType.Where(et => et.Name.ToUpper() == model.CategoryType.Name.ToUpper())
                                                        .FirstOrDefaultAsync();

                    if (check == null)
                    {
                        EmployeeType et = new EmployeeType()
                        {
                            Name = model.CategoryType.Name,
                            Status = true
                        };

                        var save = await _context.EmployeesType.AddAsync(et);
                        await _context.SaveChangesAsync();

                        idType = save.Entity.Id;
                    }
                    else
                    {
                        idType = check.Id;
                    }
                }
                else
                {
                    idType = model.CategoryType.IdCategoryType;
                }

                Employee employee = new Employee()
                {
                    Name = model.Name,
                    Lastname = model.Lastname,
                    Birthdate = model.Birthdate.ToString("dd-MM-yyyy"),
                    Email = model.Email,
                    Phone = model.Phone,
                    Salary = model.Salary,
                    ContractDate = model.ContractDate,
                    Status = true,
                    EmployeeTypeId = idType,
                };

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            var viewModel = new CreateEmployeeViewModel
            {
                Categories = await _context.EmployeesType.Where(et => et.Id != 1).Select(et => new CategoryTypeViewModel
                {
                    IdCategoryType = et.Id,
                    Name = et.Name,
                    Status = et.Status
                }).ToListAsync(),
                Employee = model
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            States.MenuSelect = Menu.USERS;
            var model = await _context.Employees.Where(e => e.Id == id)
                                .Select(e => new CreateEmployeeViewModel
                                {
                                    Employee = new EmployeeViewModel
                                    {
                                        Id = e.Id,
                                        Name = e.Name,
                                        Lastname = e.Lastname,
                                        Birthdate = DateTime.Parse(e.Birthdate),
                                        Email = e.Email,
                                        Phone = e.Phone,
                                        Salary = e.Salary,
                                        ContractDate = e.ContractDate,
                                        ImagePath = e.ImagePath,
                                        Status = e.Status,
                                        CategoryType = new CategoryTypeViewModel
                                        {
                                            IdCategoryType = e.EmployeeType.Id,
                                            Name = e.EmployeeType.Name
                                        }
                                    },
                                    Categories = _context.EmployeesType.Where(et => et.Id != 1)
                                                            .Select(et => new CategoryTypeViewModel
                                                            {
                                                                IdCategoryType = et.Id,
                                                                Name = et.Name
                                                            })
                                                            .ToList()
                                })
                                .FirstOrDefaultAsync();

            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {

                var employee = await _context.Employees.SingleOrDefaultAsync(e => e.Id == model.Employee.Id);

                if (employee == null) return NotFound();

                int idType = 0;
                if (model.Employee.CategoryType.IdCategoryType == 0)
                {
                    var check = await _context.EmployeesType.Where(et => et.Name.ToUpper() == model.Employee.CategoryType.Name.ToUpper())
                                                        .FirstOrDefaultAsync();

                    if (check == null)
                    {
                        EmployeeType et = new EmployeeType()
                        {
                            Name = model.Employee.CategoryType.Name,
                            Status = true
                        };

                        var save = await _context.EmployeesType.AddAsync(et);
                        await _context.SaveChangesAsync();

                        idType = save.Entity.Id;
                    }
                    else
                    {
                        idType = check.Id;
                    }
                }
                else
                {
                    idType = model.Employee.CategoryType.IdCategoryType;
                }

                employee.Name = model.Employee.Name;
                employee.Lastname = model.Employee.Lastname;
                employee.Birthdate = model.Employee.Birthdate.ToString("dd-MM-yyyy");
                employee.Email = model.Employee.Email;
                employee.Phone = model.Employee.Phone;
                employee.Salary = model.Employee.Salary;
                employee.ContractDate = model.Employee.ContractDate;
                employee.Status = model.Employee.Status;
                employee.EmployeeTypeId = idType;
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var model = await _context.Employees.Where(_e => _e.Id == id)
                                    .Select(e => new EmployeeViewModel
                                    {
                                        Id = e.Id,
                                        Name = e.Name,
                                        Lastname = e.Lastname,
                                        Birthdate = DateTime.Parse(e.Birthdate),
                                        Email = e.Email,
                                        Phone = e.Phone,
                                        Salary = e.Salary,
                                        ContractDate = e.ContractDate,
                                        ImagePath = e.ImagePath,
                                        Status = e.Status,
                                        CategoryType = new CategoryTypeViewModel
                                        {
                                            IdCategoryType = e.EmployeeType.Id,
                                            Name = e.EmployeeType.Name
                                        }
                                    }).FirstOrDefaultAsync();
            if (model == null) return NotFound();

            return View(model);
        }

        //Method DELETE
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null) return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
