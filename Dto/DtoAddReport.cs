namespace Invoices.Dto
{
    public class DtoAddReport
    {
        public int InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

        public DtoAddClient Client { get; set; }
        public DtoAddUser User { get; set; }
        public List<DtoAddPayment> Payments { get; set; }
        public List<DtoAddItem> Items { get; set; }
    }
}
