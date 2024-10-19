using Invoices.Entity;

namespace Invoices.Dto
{
    public class DtoAddInvoice
    {
        public int InvoiceId { get; set; }
        public int UserId { get; set; } // Foreign Key to User
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // Ödendi, Beklemede, İptal

        // Navigation Properties
        public User User { get; set; } // Faturayı kesen kullanıcı
    }
}
