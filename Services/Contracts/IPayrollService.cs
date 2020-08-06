using PayrollApp.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollApp.Services.Contracts
{
    public interface IPayrollService
    {
        public double GetAfpRetention(string afpRetentition, double salary);
        public double GetArsRetention(string arsRetentition, double salary);
        public double GetIsrRetention(double salary);
        public Task<List<PayrollResponseDto>> GetAllPayrollsAsync();
        public Task<List<PayrollDetailDto>> GetPayrollDetails(string id);
    }
}
