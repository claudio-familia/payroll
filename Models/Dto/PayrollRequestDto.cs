using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollApp.Models.Dto
{
    public class PayrollRequestDto
    {
        public List<Employee> Employees { get; set; }
        public DateTime Date { get; set; }
        public string Afp { get; set; }
        public string ARS { get; set; }
    }
}
