using Microsoft.EntityFrameworkCore;
using MDAS.Data;

namespace MDAS.Data
{
    public class PurchaseService
    {
        private readonly AppDbContext _db;

        public PurchaseService(AppDbContext db)
        {
            _db = db;
        }

        // --- SUPPLIER LOGIC ---
        public async Task<List<Supplier>> GetSuppliersAsync()
        {
            return await _db.Suppliers.ToListAsync();
        }

        public async Task AddSupplierAsync(Supplier supplier)
        {
            _db.Suppliers.Add(supplier);
            await _db.SaveChangesAsync();
        }

        // --- ORDER LOGIC ---
        public async Task<List<PurchaseOrder>> GetOrdersAsync()
        {
            // Get newest orders first
            return await _db.PurchaseOrders.OrderByDescending(o => o.OrderDate).ToListAsync();
        }

        public async Task CreateOrderAsync(PurchaseOrder order, List<PurchaseOrderItem> items)
        {
            // 1. Save the Main Order
            _db.PurchaseOrders.Add(order);
            await _db.SaveChangesAsync(); // This generates the Order ID

            // 2. Save the Items (Link them to the Order ID)
            foreach (var item in items)
            {
                item.PurchaseOrderId = order.Id;
                _db.PurchaseOrderItems.Add(item);
            }
            await _db.SaveChangesAsync();
        }

        public async Task ReceiveOrderAsync(PurchaseOrder order)
        {
            // Update Status
            var dbOrder = await _db.PurchaseOrders.FindAsync(order.Id);
            if (dbOrder != null)
            {
                dbOrder.Status = "Received";

                // HERE: You would also add logic to increase Product Stock
                // But for now, we just mark it received.

                await _db.SaveChangesAsync();
            }
        }
    }
}