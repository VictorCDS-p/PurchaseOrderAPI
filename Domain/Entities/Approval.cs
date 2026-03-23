using PurchaseOrderAPI.Domain.Enums;

namespace PurchaseOrderAPI.Domain.Entities
{
    public class Approval
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }

        public UserRole Role { get; set; }
        public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;

        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}