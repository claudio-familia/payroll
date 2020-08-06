using PayrollApp.Models;
using PayrollApp.Models.Dto;
using PayrollApp.Repository.Contracts;
using PayrollApp.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollApp.Services
{
    public class PayrollService : IPayrollService
    {
        private readonly IBaseRepository<Payroll> _payrollRepository;
        private readonly IBaseRepository<Employee> _employeeRepository;
        public PayrollService(IBaseRepository<Payroll> payrollRepository, IBaseRepository<Employee> employeeRepository)
        {
            _payrollRepository = payrollRepository;
            _employeeRepository = employeeRepository;
        }

        public double GetAfpRetention(string afpRetentition, double salary)
        {
            return (double.Parse(afpRetentition, CultureInfo.InvariantCulture) / 100) * salary;
        }

        public async Task<List<PayrollResponseDto>> GetAllPayrollsAsync()
        {
            IEnumerable<Payroll> payrolls = await _payrollRepository.GetAllAsync();

            List<PayrollResponseDto> response = new List<PayrollResponseDto>();

            CultureInfo ci = new CultureInfo("es-Do");

            foreach (var payrollGroup in payrolls.GroupBy(payroll => payroll.PayrollDate.Value.ToString("m")))
            {
                response.Add(new PayrollResponseDto()
                {
                    EmployeeQuantity = payrollGroup.Count(),
                    PayrollDate = payrollGroup.Key,
                    NetTotal = payrollGroup.Sum(payroll => payroll.NetSalary).ToString("C", ci),
                    RawTotal = payrollGroup.Sum(payroll => payroll.RawSalary).ToString("C", ci),
                    TaxTotal = (payrollGroup.Sum(payroll => payroll.RawSalary) - payrollGroup.Sum(payroll => payroll.NetSalary)).ToString("C", ci),
                    Status = payrollGroup.First().Status ? "Activo" : "Inactivo"
                });
            }

            return response;
        }

        public double GetArsRetention(string arsRetentition, double salary)
        {
            return (double.Parse(arsRetentition, CultureInfo.InvariantCulture) / 100) * salary;
        }

        public double GetIsrRetention(double salary)
        {
            double result = 0;

            double salaryByYear = salary * 12;

            if (salaryByYear > 416220.01 && salaryByYear <= 624329)
            {
                result = (salaryByYear - 416220.01) * 0.15;
            }
            else if (salaryByYear > 624329.01 && salaryByYear <= 867123)
            {
                result = 2601.33 + ((salaryByYear - 624329.01) * 0.2);                
            }
            else if (salaryByYear > 867123.01)
            {
                result = 6648 + ((salaryByYear - 867123.01) * 0.25);                
            }

            result = result / 12;

            return result;
        }

        public async Task<List<PayrollDetailDto>> GetPayrollDetails(string id)
        {
            IEnumerable<Payroll> payrolls = await _payrollRepository.GetAllAsync();

            CultureInfo ci = new CultureInfo("es-Do");

            List<PayrollDetailDto> response = payrolls.Where(payroll => payroll.PayrollDate.Value.ToString("m") == id)
                                                      .Select(payroll => new PayrollDetailDto() { 
                                                        Employee = payroll.EmployeeId.ToString(),
                                                        NetTotal = payroll.NetSalary.ToString("C", ci),
                                                        RawTotal = payroll.RawSalary.ToString("C", ci),
                                                        TaxTotal = payroll.TaxableSalary.ToString("C", ci),
                                                        Status = payroll.Status ? "Activo" : "Inactivo",
                                                        PayrollDate = payroll.PayrollDate.Value.ToString("m"),
                                                        Id = payroll.Id                                                                                                              
                                                      }).ToList();

            foreach(var item in response)
            {
                Employee employee = await _employeeRepository.GetByIdAsync(int.Parse(item.Employee));
                item.Employee = $"{employee.Name} {employee.LastName}";
            }

            return response;
        }
    }
}
