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

        public async Task GeneratePayrollAsync(PayrollRequestDto payrollRequestDto)
        {
            foreach (var employee in payrollRequestDto.Employees)
            {
                double afpRetention = this.GetAfpRetention(payrollRequestDto.Afp, employee.Salary);
                double arsRetention = this.GetArsRetention(payrollRequestDto.ARS, employee.Salary);
                double isrRetention = this.GetIsrRetention(employee.Salary);

                double taxableSalary = employee.Salary - (afpRetention + arsRetention);

                await _payrollRepository.AddAsync(
                    new Payroll()
                    {
                        AfpRetention = afpRetention,
                        ArsRetention = arsRetention,
                        EmployeeId = employee.Id,
                        RawSalary = employee.Salary,
                        IsrRetention = isrRetention,
                        TaxableSalary = taxableSalary,
                        NetSalary = taxableSalary - isrRetention,
                        PayrollDate = payrollRequestDto.Date,
                        CreatedAt = DateTime.Now,
                        Status = true
                    }
                );
            }
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

        private double GetAfpRetention(string afpRetentition, double salary)
        {
            return (double.Parse(afpRetentition, CultureInfo.InvariantCulture) / 100) * salary;
        }

        private double GetArsRetention(string arsRetentition, double salary)
        {
            return (double.Parse(arsRetentition, CultureInfo.InvariantCulture) / 100) * salary;
        }

        private double GetIsrRetention(double salary)
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

        public async Task<PayrollReportResponseDto> GetPayrollReportAsync(ReportFilterDto payrollFilterDto)
        {
            string[] splitDate = payrollFilterDto.Date.Split("/");
            CultureInfo ci = new CultureInfo("es-Do");

            DateTime startedDate = new DateTime(int.Parse(splitDate[1]), int.Parse(splitDate[0]), 1);
            DateTime endDate = new DateTime(int.Parse(splitDate[1]), int.Parse(splitDate[0]) + 1, 1).AddDays(-1);            

            IEnumerable<Payroll> result = await _payrollRepository.FindAsync(payroll => payroll.PayrollDate >= startedDate && payroll.PayrollDate <= endDate);

            IEnumerable<PayrollDetailDto> response = result.OrderBy(payroll => payroll.PayrollDate).Select(payroll => new PayrollDetailDto()
            {
                Employee = payroll.EmployeeId.ToString(),
                NetTotal = payroll.NetSalary.ToString("C", ci),
                RawTotal = payroll.RawSalary.ToString("C", ci),
                Gender = "",
                TaxTotal = payroll.TaxableSalary.ToString("C", ci),
                Status = payroll.Status ? "Activo" : "Inactivo",
                PayrollDate = payroll.PayrollDate.Value.ToString("m"),
                Id = payroll.Id
            }).ToList();

            foreach (var item in response)
            {
                Employee employee = await _employeeRepository.GetByIdAsync(int.Parse(item.Employee));
                item.Employee = $"{employee.Name} {employee.LastName}";
                item.Gender = employee.Gender;
            }

            if (!string.IsNullOrEmpty(payrollFilterDto.Filter))
            {
                response = response.Where(item => item.Employee.ToUpper().Contains(payrollFilterDto.Filter.ToUpper()));
            }

            if (!string.IsNullOrEmpty(payrollFilterDto.Gender))
            {
                response = response.Where(item => item.Gender == payrollFilterDto.Gender);
            }

            if (!string.IsNullOrEmpty(payrollFilterDto.Status))
            {
                response = response.Where(item => item.Status == payrollFilterDto.Status);
            }

            switch (payrollFilterDto.OrderBy)
            {
                case "NameUp":
                    response = response.OrderBy(item => item.Employee);
                    break;

                case "NameDown":
                    response = response.OrderByDescending(item => item.Employee);
                    break;

                case "SalaryUp":
                    response = response.OrderBy(item => item.NetTotal);
                    break;

                case "SalaryDown":
                    response = response.OrderByDescending(item => item.NetTotal);
                    break;

                default:
                    break;
            }

            var groupList = response.GroupBy(item => item.PayrollDate);
            var responseList = new List<PayrollResults>();

            foreach(var item in groupList)
            {
                var itemToInsert = new PayrollResults();
                itemToInsert.PayrollName = $"Nomina del {item.Key} - {item.FirstOrDefault().Status}";
                
                itemToInsert.RawTotal = item.Sum(p => float.Parse(p.RawTotal.Replace(",", "").Replace("$",""), CultureInfo.InvariantCulture)).ToString("C", ci);
                itemToInsert.TaxTotal = item.Sum(p => float.Parse(p.TaxTotal.Replace(",", "").Replace("$", ""), CultureInfo.InvariantCulture)).ToString("C", ci);
                itemToInsert.NetTotal = item.Sum(p => float.Parse(p.NetTotal.Replace(",", "").Replace("$", ""), CultureInfo.InvariantCulture)).ToString("C", ci);
                
                itemToInsert.Results = new List<PayrollDetailDto>();
                foreach (var data in item)
                {
                    itemToInsert.Results.Add(data);
                }
                responseList.Add(itemToInsert);
            }
            

            return new PayrollReportResponseDto() { Results = responseList, Filters = payrollFilterDto };
        }
    }
}
