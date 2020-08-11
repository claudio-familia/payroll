using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollApp.Models.Dto
{
    public class ReportFilterDto
    {
        public string Date { get; set; }
        public string Filter { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public string OrderBy { get; set; }
    }
}
