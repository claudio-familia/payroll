namespace PayrollApp.Models.Dto
{
    public class PayrollDetailDto
    {
        public int Id { get; set; }

        public string Employee { get; set; }

        public string PayrollDate { get; set; }

        public string RawTotal { get; set; }

        public string TaxTotal { get; set; }

        public string NetTotal { get; set; }

        public string Status { get; set; }
    }
}