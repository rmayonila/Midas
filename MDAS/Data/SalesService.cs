using Microsoft.EntityFrameworkCore;
using MDAS.Data;

namespace MDAS.Data
{
    public class SalesService
    {
        private readonly AppDbContext _db;

        public SalesService(AppDbContext db)
        {
            _db = db;
        }

        // 1. GET ALL TRANSACTIONS
        public async Task<List<SalesTransaction>> GetSalesHistoryAsync()
        {
            var transactions = await _db.SalesTransactions
                                        .OrderByDescending(t => t.Date)
                                        .ToListAsync();

            // Populate the "ItemsSummary" helper for the UI
            foreach (var t in transactions)
            {
                var itemCount = await _db.SalesTransactionItems.CountAsync(i => i.SalesTransactionId == t.Id);
                t.ItemsSummary = $"{itemCount} items";
            }

            return transactions;
        }

        // 2. CREATE A NEW SALE (Use this later in your POS Screen)
        public async Task CreateSaleAsync(SalesTransaction sale, List<SalesTransactionItem> items)
        {
            _db.SalesTransactions.Add(sale);
            await _db.SaveChangesAsync();

            foreach (var item in items)
            {
                item.SalesTransactionId = sale.Id;
                _db.SalesTransactionItems.Add(item);

                // DEDUCT INVENTORY
                var product = await _db.Products.FirstOrDefaultAsync(p => p.Name == item.ProductName);
                if (product != null)
                {
                    product.Stock -= item.Qty;
                }
            }
            await _db.SaveChangesAsync();
        }

        // 3. VOID / REFUND TRANSACTION
        public async Task ProcessRefundAsync(int transactionId, string type)
        {
            var txn = await _db.SalesTransactions.FindAsync(transactionId);
            if (txn != null)
            {
                txn.Status = type == "Void" ? "Voided" : "Refunded";

                // RESTOCK INVENTORY
                var items = await _db.SalesTransactionItems.Where(i => i.SalesTransactionId == transactionId).ToListAsync();
                foreach (var item in items)
                {
                    var product = await _db.Products.FirstOrDefaultAsync(p => p.Name == item.ProductName);
                    if (product != null)
                    {
                        product.Stock += item.Qty; // Return stock
                    }
                }

                await _db.SaveChangesAsync();
            }
        }
    }
}