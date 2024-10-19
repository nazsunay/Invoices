namespace Invoices.Entity
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int InvoiceId { get; set; } // Foreign Key to Invoice
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } // Başarılı, Başarısız


        public Invoice Invoice { get; set; }
    }

}
