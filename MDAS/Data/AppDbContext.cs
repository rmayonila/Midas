using Microsoft.EntityFrameworkCore;
using MDAS.Data;

namespace MDAS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // --- DATABASE TABLES ---
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
    }

    // --- 1. PRODUCT MODEL (Existing) ---
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string SKU { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Variants { get; set; } = "";
    }

    // --- 2. SUPPLIER MODEL (New) ---
    public class Supplier
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = "";
        public string ContactPerson { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Category { get; set; } = "";
    }

    // --- 3. PURCHASE ORDER HEAD (New) ---
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public string PONumber { get; set; } = "";
        public string SupplierName { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Received, Cancelled
        public int TotalItems { get; set; }
        public decimal TotalCost { get; set; }
    }

    // --- 4. ORDER ITEMS (New) ---
    public class PurchaseOrderItem
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; } // Links to the main order
        public string ProductName { get; set; } = "";
        public string Variant { get; set; } = "";
        public int Qty { get; set; }
        public decimal UnitCost { get; set; }
    }
}