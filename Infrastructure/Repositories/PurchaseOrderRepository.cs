using Microsoft.EntityFrameworkCore;
using PurchaseOrderAPI.Domain.Entities;
using PurchaseOrderAPI.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;

namespace PurchaseOrderAPI.Infrastructure.Repositories
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly AppDbContext _context;

        public PurchaseOrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<PurchaseOrder> GetAll()
        {
            return _context.PurchaseOrders
                .Include(o => o.Items)
                .Include(o => o.User)

                // 🔹 Aprovações + usuário que aprovou
                .Include(o => o.Approvals)
                    .ThenInclude(a => a.User)

                // 🔹 Histórico + usuário que executou ação
                .Include(o => o.History)
                    .ThenInclude(h => h.User)

                .ToList();
        }

        public PurchaseOrder? GetById(int id)
        {
            return _context.PurchaseOrders
                .Include(o => o.Items)
                .Include(o => o.User)

                // 🔹 Aprovações + usuário
                .Include(o => o.Approvals)
                    .ThenInclude(a => a.User)

                // 🔹 Histórico + usuário
                .Include(o => o.History)
                    .ThenInclude(h => h.User)

                .FirstOrDefault(o => o.Id == id);
        }

        public void Add(PurchaseOrder order)
        {
            _context.PurchaseOrders.Add(order);
            _context.SaveChanges();
        }

        public void Update(PurchaseOrder order)
        {
            _context.PurchaseOrders.Update(order);
            _context.SaveChanges();
        }
    }
}