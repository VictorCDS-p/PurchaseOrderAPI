using PurchaseOrderAPI.Domain.Enums;

namespace PurchaseOrderAPI.Domain.Entities
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Draft;

        public List<PurchaseOrderItem> Items { get; set; } = new();
        public List<Approval> Approvals { get; set; } = new();
        public List<OrderHistory> History { get; set; } = new();
    }
}