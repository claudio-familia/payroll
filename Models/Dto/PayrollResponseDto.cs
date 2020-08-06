using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollApp.Models.Dto
{
    public class PayrollResponseDto
    {
        public int Id { get; set; }

        public int EmployeeQuantity { get; set; }

        public string PayrollDate { get; set; }
        
        public string RawTotal { get; set; }

        public string TaxTotal { get; set; }

        public string NetTotal { get; set; }

        public string Status { get; set; }
    }
}
