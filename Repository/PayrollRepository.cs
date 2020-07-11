using PayrollApp.Models;

namespace PayrollApp.Repository
{
    public class PayrollRepository : BaseRepository<Payroll>
    {
        public PayrollRepository(PayrollContext context) : base(context)
        {
        }
    }
}
