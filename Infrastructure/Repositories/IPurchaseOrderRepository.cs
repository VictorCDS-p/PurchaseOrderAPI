using PurchaseOrderAPI.Domain.Entities;
using System.Collections.Generic;

namespace PurchaseOrderAPI.Infrastructure.Repositories
{
    public interface IPurchaseOrderRepository
    {
        List<PurchaseOrder> GetAll();
        PurchaseOrder? GetById(int id);
        void Add(PurchaseOrder order);
        void Update(PurchaseOrder order);
    }
}