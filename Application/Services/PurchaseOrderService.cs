using PurchaseOrderAPI.Domain.Entities;
using PurchaseOrderAPI.Domain.Enums;
using PurchaseOrderAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PurchaseOrderAPI.Application.Services
{
    public class PurchaseOrderService
    {
        private readonly AppDbContext _context;

        public PurchaseOrderService(AppDbContext context)
        {
            _context = context;
        }

        public PurchaseOrder Create(PurchaseOrder order)
        {
            if (order.Items == null || !order.Items.Any())
                throw new Exception("Pedido deve ter pelo menos 1 item");

            order.TotalAmount = order.Items.Sum(i => i.Quantity * i.UnitPrice);
            order.Approvals = GenerateApprovals(order.TotalAmount);
            order.Status = OrderStatus.PendingApproval;
            order.History.Add(new OrderHistory { Action = "Pedido criado" });

            _context.PurchaseOrders.Add(order);
            _context.SaveChanges();

            return order;
        }

        public List<PurchaseOrder> GetAll()
        {
            return _context.PurchaseOrders
                .Include(o => o.Items)
                .Include(o => o.Approvals)
                .Include(o => o.History)
                .ToList();
        }

        public PurchaseOrder Approve(int orderId)
        {
            var order = _context.PurchaseOrders
                .Include(o => o.Approvals)
                .Include(o => o.History)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Pedido não encontrado");

            var nextApproval = order.Approvals.FirstOrDefault(a => a.Status == ApprovalStatus.Pending);
            if (nextApproval == null)
                throw new Exception("Pedido já aprovado");

            nextApproval.Status = ApprovalStatus.Approved;
            order.History.Add(new OrderHistory { Action = $"Aprovado por {nextApproval.Role}" });

            if (order.Approvals.All(a => a.Status == ApprovalStatus.Approved))
            {
                order.Status = OrderStatus.Completed;
                order.History.Add(new OrderHistory { Action = "Pedido finalizado" });
            }

            _context.SaveChanges();
            return order;
        }

        public PurchaseOrder RequestReview(int orderId)
        {
            var order = _context.PurchaseOrders
                .Include(o => o.Approvals)
                .Include(o => o.History)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Pedido não encontrado");

            order.Status = OrderStatus.PendingApproval;
            foreach (var approval in order.Approvals)
                approval.Status = ApprovalStatus.Pending;

            order.History.Add(new OrderHistory { Action = "Solicitação de revisão" });

            _context.SaveChanges();
            return order;
        }

        public PurchaseOrder Complete(int orderId)
        {
            var order = _context.PurchaseOrders
                .Include(o => o.Approvals)
                .Include(o => o.History)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Pedido não encontrado");

            order.Status = OrderStatus.Completed;
            order.History.Add(new OrderHistory { Action = "Pedido concluído" });

            _context.SaveChanges();
            return order;
        }

        public List<OrderHistory> GetHistory(int orderId)
        {
            var order = _context.PurchaseOrders
                .Include(o => o.History)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Pedido não encontrado");

            return order.History.OrderBy(h => h.CreatedAt).ToList();
        }

        private List<Approval> GenerateApprovals(decimal total)
        {
            var approvals = new List<Approval> { new Approval { Role = UserRole.Supply } };

            if (total > 100)
                approvals.Add(new Approval { Role = UserRole.Manager });

            if (total > 1000)
                approvals.Add(new Approval { Role = UserRole.Director });

            return approvals;
        }
    }
}