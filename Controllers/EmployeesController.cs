using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayrollApp.Models;
using PayrollApp.Models.Dto;
using PayrollApp.Repository.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollApp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IBaseRepository<Employee> _employeeRepository;

        public EmployeesController(IBaseRepository<Employee> repository)
        {
            _employeeRepository = repository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _employeeRepository.GetAllAsync());
        }

        public IActionResult Report()
        {
            var response = new EmployeeResponseDto();
            response.Filters = new EmployeeFilterDto();
            response.Results = new List<Employee>();
            return View(response);
        }


        [HttpPost]
        public async Task<IActionResult> ReportAsync(EmployeeFilterDto employeeFilterDto)
        {
            IEnumerable<Employee> response = await _employeeRepository.GetAllAsync();


            if (!string.IsNullOrEmpty(employeeFilterDto.Filter))
            {
                response = response.Where(item => item.Name.ToUpper().Contains(employeeFilterDto.Filter.ToUpper()) ||
                                          item.LastName.ToUpper().Contains(employeeFilterDto.Filter.ToUpper()) ||
                                          item.Salary.ToString().Contains(employeeFilterDto.Filter));
            }

            if (!string.IsNullOrEmpty(employeeFilterDto.Gender))
            {
                response = response.Where(item => item.Gender == employeeFilterDto.Gender);
            }

            if (!string.IsNullOrEmpty(employeeFilterDto.Status))
            {
                response = response.Where(item => item.Gender == employeeFilterDto.Status);
            }

            switch (employeeFilterDto.OrderBy)
            {
                case "NameUp":
                    response = response.OrderBy(item => item.Name);
                    break;

                case "NameDown":
                    response = response.OrderByDescending(item => item.Name);
                    break;

                case "LastNameUp":
                    response = response.OrderBy(item => item.LastName);
                    break;

                case "LastNameDown":
                    response = response.OrderByDescending(item => item.LastName);
                    break;

                case "SalaryUp":
                    response = response.OrderBy(item => item.Salary);
                    break;

                case "SalaryDown":
                    response = response.OrderByDescending(item => item.Salary);
                    break;

                default:
                    break;

            }

            return View(new EmployeeResponseDto() { Results = response, Filters = employeeFilterDto });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,LastName,Gender,Salary,Status")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _employeeRepository.AddAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LastName,Gender,Salary,Status")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeRepository.Update(employee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await EmployeeExistsAsync(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            await _employeeRepository.Remove(employee);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EmployeeExistsAsync(int id)
        {
            var response = await _employeeRepository.GetByIdAsync(id);
            return response != null;
        }
    }
}
