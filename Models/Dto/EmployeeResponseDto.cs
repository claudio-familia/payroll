using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollApp.Models.Dto
{
    public class EmployeeResponseDto
    {
        public IEnumerable<EmployeeDto> Results { get; set; }
        public ReportFilterDto Filters { get; set; }
        public string TotalSalary { get; set; }
    }
}
