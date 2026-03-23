namespace PurchaseOrderAPI.Domain.Entities
{
    public class OrderHistory
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }

        public string Action { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}