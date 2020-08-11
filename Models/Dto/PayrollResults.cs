using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollApp.Models.Dto
{
    public class PayrollResults
    {
        public string PayrollName { get; set; }
        public string RawTotal { get; set; }
        public string TaxTotal { get; set; }
        public string NetTotal { get; set; }
        public List<PayrollDetailDto> Results { get; set; }
    }
}
