namespace Invoices.Entity
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } // Kredi Kartı, Banka Transferi vb.
        public int Quantity { get; set; }
        public decimal Price { get; set; } // Ürün fiyatı
        public decimal Total { get; set; } // Toplam Tutar
        public bool IsDeleted { get; set; } // Soft delete özelliği

        // Navigation Property for the Many-to-Many relation
        public List<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    }


}
