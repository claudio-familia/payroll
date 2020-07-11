using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollApp.Models
{
    public class Payroll
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public double RawSalary { get; set; }

        public double AfpRetention { get; set; }

        public double ArsRetention { get; set; }

        public double TaxableSalary { get; set; }

        public double IsrRetention { get; set; }

        public double NetSalary { get; set; }

        public DateTime? PayrollDate { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool Status { get; set; }

    }
}
