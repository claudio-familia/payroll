using PayrollApp.Models.Dto;
using System.Threading.Tasks;

namespace PayrollApp.Repository.Contracts
{
    public interface IPayrollRepository
    {
        public Task GeneratePayroll(PayrollFilter payrollFilter);
    }
}
