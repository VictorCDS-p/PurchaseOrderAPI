public class PurchaseOrderResponseDto
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;

    public List<PurchaseOrderItemDto> Items { get; set; } = new();
    public List<ApprovalDto> Approvals { get; set; } = new();
}