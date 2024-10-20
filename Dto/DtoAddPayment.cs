namespace Invoices.Dto
{
    public class DtoAddPayment
    {
        public int PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }
}
