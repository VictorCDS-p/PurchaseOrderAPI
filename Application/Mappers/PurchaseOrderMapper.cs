using PurchaseOrderAPI.Domain.Entities;

public static class PurchaseOrderMapper
{
    public static PurchaseOrderResponseDto ToDto(PurchaseOrder order)
    {
        return new PurchaseOrderResponseDto
        {
            Id = order.Id,
            TotalAmount = order.TotalAmount,
            Status = order.Status.ToString(),

            Items = order.Items.Select(i => new PurchaseOrderItemDto
            {
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList(),

            Approvals = order.Approvals.Select(a => new ApprovalDto
            {
                Role = a.Role.ToString(),
                Status = a.Status.ToString(),
                ApprovedBy = a.User?.Name
            }).ToList()
        };
    }
}