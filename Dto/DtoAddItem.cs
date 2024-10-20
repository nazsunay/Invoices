namespace Invoices.Dto
{
    public class DtoAddItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } // Kredi Kartı, Banka Transferi vb.
        public int Quantity { get; set; }
        public decimal Price { get; set; } // Ürün fiyatı
        public decimal Total { get; set; } // Toplam Tutar
    }
}
