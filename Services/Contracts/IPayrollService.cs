using PayrollApp.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollApp.Services.Contracts
{
    public interface IPayrollService
    {    
        public Task<List<PayrollResponseDto>> GetAllPayrollsAsync();
        public Task<List<PayrollDetailDto>> GetPayrollDetails(string id);
        Task<PayrollReportResponseDto> GetPayrollReportAsync(ReportFilterDto payrollFilterDto);
        Task GeneratePayrollAsync(PayrollRequestDto payrollRequestDto);
    }
}
