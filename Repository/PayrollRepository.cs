using PayrollApp.Models;
using PayrollApp.Models.Dto;
using PayrollApp.Repository.Contracts;
using System.Threading.Tasks;

namespace PayrollApp.Repository
{
    public class PayrollRepository : BaseRepository<Payroll>, IPayrollRepository
    {
        public PayrollRepository(PayrollContext context) : base(context)
        {
        }

        public Task GeneratePayroll(PayrollFilter payrollFilter)
        {
            throw new System.NotImplementedException();
        }
    }
}
