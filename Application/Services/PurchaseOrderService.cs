using PurchaseOrderAPI.Domain.Entities;
using PurchaseOrderAPI.Domain.Enums;
using PurchaseOrderAPI.Infrastructure.Repositories;

namespace PurchaseOrderAPI.Application.Services
{
    public class PurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _repository;
        private readonly UserService _userService;

        public PurchaseOrderService(IPurchaseOrderRepository repository, UserService userService)
        {
            _repository = repository;
            _userService = userService;
        }

        public (PurchaseOrder order, string message) Create(PurchaseOrder order, int userId)
        {
            var (user, _) = _userService.GetById(userId);

            if (order.Items == null || !order.Items.Any())
                throw new Exception("Pedido inválido: deve conter pelo menos 1 item.");

            order.UserId = user.Id;
            order.User = user;

            order.TotalAmount = order.Items.Sum(i => i.Quantity * i.UnitPrice);
            order.Approvals = GenerateApprovals(order.TotalAmount);
            order.Status = OrderStatus.PendingApproval;

            order.History.Add(new OrderHistory
            {
                Action = $"Pedido criado pelo usuário {user.Name}",
                UserId = user.Id
            });

            _repository.Add(order);

            return (order, $"Pedido criado com sucesso. Total: {order.TotalAmount:C2}");
        }

        public (List<PurchaseOrder> orders, string message) GetAll()
        {
            var orders = _repository.GetAll();
            return (orders, orders.Any() ? $"Encontrados {orders.Count} pedidos." : "Nenhum pedido encontrado.");
        }

        public (PurchaseOrder order, string message) Approve(int orderId, int userId)
        {
            var order = _repository.GetById(orderId)
                        ?? throw new Exception($"Pedido {orderId} não encontrado.");

            if (order.Status == OrderStatus.Cancelled)
                throw new Exception("Pedido cancelado não pode ser aprovado.");

            if (order.Status == OrderStatus.Completed)
                throw new Exception("Pedido já finalizado.");

            var (user, _) = _userService.GetById(userId);

            var nextApproval = order.Approvals
                .FirstOrDefault(a => a.Status == ApprovalStatus.Pending)
                ?? throw new Exception("Não há aprovações pendentes.");

            if (user.Role != nextApproval.Role)
                throw new Exception($"Usuário {user.Name} não tem permissão ({nextApproval.Role}).");

            nextApproval.Status = ApprovalStatus.Approved;
            nextApproval.UserId = user.Id;

            order.History.Add(new OrderHistory
            {
                Action = $"Pedido aprovado por {user.Name}",
                UserId = user.Id
            });

            if (order.Approvals.All(a => a.Status == ApprovalStatus.Approved))
            {
                order.Status = OrderStatus.Completed;

                order.History.Add(new OrderHistory
                {
                    Action = "Pedido finalizado após todas as aprovações",
                    UserId = user.Id
                });
            }

            _repository.Update(order);
            return (order, $"Aprovado por {user.Name}");
        }

        public (PurchaseOrder order, string message) RequestReview(int orderId, int userId)
        {
            var order = _repository.GetById(orderId)
                        ?? throw new Exception($"Pedido {orderId} não encontrado.");

            if (order.Status == OrderStatus.Cancelled)
                throw new Exception("Pedido cancelado não pode ser alterado.");

            var (user, _) = _userService.GetById(userId);

            order.Status = OrderStatus.PendingApproval;

            foreach (var approval in order.Approvals)
                approval.Status = ApprovalStatus.Pending;

            order.History.Add(new OrderHistory
            {
                Action = $"Pedido retornou para revisão por {user.Name}",
                UserId = user.Id
            });

            _repository.Update(order);
            return (order, "Pedido enviado para revisão.");
        }

        public (PurchaseOrder order, string message) Complete(int orderId, int userId)
        {
            var order = _repository.GetById(orderId)
                        ?? throw new Exception($"Pedido {orderId} não encontrado.");

            if (order.Status == OrderStatus.Cancelled)
                throw new Exception("Pedido cancelado não pode ser concluído.");

            var (user, _) = _userService.GetById(userId);

            order.Status = OrderStatus.Completed;

            order.History.Add(new OrderHistory
            {
                Action = $"Pedido concluído manualmente por {user.Name}",
                UserId = user.Id
            });

            _repository.Update(order);
            return (order, "Pedido concluído.");
        }

        public (PurchaseOrder order, string message) Cancel(int orderId, int userId)
        {
            var order = _repository.GetById(orderId)
                        ?? throw new Exception($"Pedido {orderId} não encontrado.");

            if (order.Status == OrderStatus.Completed)
                throw new Exception("Pedido finalizado não pode ser cancelado.");

            if (order.Status == OrderStatus.Cancelled)
                throw new Exception("Pedido já cancelado.");

            var (user, _) = _userService.GetById(userId);

            order.Status = OrderStatus.Cancelled;

            order.History.Add(new OrderHistory
            {
                Action = $"Pedido cancelado por {user.Name}",
                UserId = user.Id
            });

            _repository.Update(order);
            return (order, $"Cancelado por {user.Name}");
        }

        public (List<OrderHistory> history, string message) GetHistory(int orderId)
        {
            var order = _repository.GetById(orderId)
                        ?? throw new Exception($"Pedido {orderId} não encontrado.");

            var history = order.History
                .OrderBy(h => h.CreatedAt)
                .ToList();

            return (history, $"Histórico contém {history.Count} registros.");
        }

        private List<Approval> GenerateApprovals(decimal total)
        {
            var approvals = new List<Approval>
            {
                new Approval { Role = UserRole.Supply }
            };

            if (total > 100)
                approvals.Add(new Approval { Role = UserRole.Manager });

            if (total > 1000)
                approvals.Add(new Approval { Role = UserRole.Director });

            return approvals;
        }
    }
}