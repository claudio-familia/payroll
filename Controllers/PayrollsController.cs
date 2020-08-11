using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using PayrollApp.Models;
using PayrollApp.Models.Dto;
using PayrollApp.Repository.Contracts;
using PayrollApp.Services.Contracts;

namespace PayrollApp.Controllers
{
    public class PayrollsController : Controller
    {
        private readonly IBaseRepository<Employee> _employeeRepository;
        private readonly IBaseRepository<Payroll> _payrollRepository;
        private readonly IPayrollService _payrollService;

        public PayrollsController(
            IBaseRepository<Employee> repository,
            IBaseRepository<Payroll> payrollRepository,
            IPayrollService payrollService
            )
        {
            _employeeRepository = repository;
            _payrollRepository = payrollRepository;
            _payrollService = payrollService;
        }

        public async Task<IActionResult> Index()
        {
            List<PayrollResponseDto> response = await _payrollService.GetAllPayrollsAsync();

            return View(response);
        }

        public IActionResult Generate()
        {            
            return View();
        }

        [HttpGet]
        [Route("validategenerate/{date}")]
        public async Task<IActionResult> ValidateGenerateAsync(DateTime? date)
        {
            var validate = await _payrollRepository.FindAsync(payroll => payroll.PayrollDate == date && payroll.Status == true);

            if (validate.Any())
            {
                return Json(new { message = "Ya existe una nomina para la fecha ingresada puede intentar deshabilitarla" });
            }

            return Json(new { message = "Ok" });
        }

        [HttpPost]
        public async Task GenerateAsync(PayrollRequestDto payrollRequestDto)
        {
            await _payrollService.GeneratePayrollAsync(payrollRequestDto);
        }

        public async Task<JsonResult> GetEmployeesAsync()
        {
            return Json(await _employeeRepository.GetAllAsync());
        }
        
        public async Task<IActionResult> Inactive(string id)
        {
            var validate = await _payrollRepository.FindAsync(payroll => payroll.PayrollDate.Value.ToString("m") == id && payroll.Status == true);

            if (validate.Any())
            {
                return BadRequest("Ya existe una nomina activa en la fecha especificada");
            }

            IEnumerable<Payroll> payrolls = await _payrollRepository.GetAllAsync();

            foreach(var item in payrolls.Where(payroll => payroll.PayrollDate.Value.ToString("m") == id).ToList())
            {
                item.Status = !item.Status;
                await _payrollRepository.Update(item);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(string id)
        {
            var response = await _payrollService.GetPayrollDetails(id);
            return View(response);
        }

        public IActionResult Report()
        {
            var response = new PayrollReportResponseDto
            {
                Filters = new ReportFilterDto(),
                Results = new List<PayrollResults>()
            };
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReportAsync(ReportFilterDto payrollFilterDto)
        {
            PayrollReportResponseDto response = await _payrollService.GetPayrollReportAsync(payrollFilterDto);           

            return View(response);
        }
    }
}
