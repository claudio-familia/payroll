using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollApp.Models.Dto
{
    public class PayrollReportResponseDto
    {       
        public ReportFilterDto Filters { get; set; }
        public List<PayrollResults> Results { get; set; }
    }
}
