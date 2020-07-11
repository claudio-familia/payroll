using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollApp.Models.Dto
{
    public class EmployeeResponseDto
    {
        public IEnumerable<Employee> Results { get; set; }
        public EmployeeFilterDto Filters { get; set; }
    }
}
