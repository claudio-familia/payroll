using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollApp.Models.Dto
{
    public class PayrollFilter
    {
        public IEnumerable<Employee> Employees { get; set; }

        public DateTime PayrollDate { get; set; }        
    }
}
