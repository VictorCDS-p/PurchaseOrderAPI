using PurchaseOrderAPI.Domain.Enums;

namespace PurchaseOrderAPI.Domain.Entities
{
    public class Approval
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }

        public UserRole Role { get; set; }
        public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;
    }
}