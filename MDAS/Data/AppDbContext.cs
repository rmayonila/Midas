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
        public DbSet<SalesTransaction> SalesTransactions { get; set; }
        public DbSet<SalesTransactionItem> SalesTransactionItems { get; set; }
        public DbSet<ExpenseRecord> ExpenseRecords { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<PayrollRecord> PayrollRecords { get; set; }
        
        public DbSet<Employee> Employees { get; set; }
        public DbSet<PayrollSetting> PayrollSettings { get; set; }
        public DbSet<AttendanceLog> AttendanceLogs { get; set; }

        public DbSet<EmployeePerformance> EmployeePerformance { get; set; }
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

    // --- 5. SALES TRANSACTION HEADER ---
    public class SalesTransaction
    {
        public int Id { get; set; }
        public string ReceiptNo { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.Now;
        public string CashierName { get; set; } = "";
        public string PaymentMethod { get; set; } = "Cash"; // Cash, Card, GCash
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Completed"; // Completed, Voided, Refunded

        // Helper property for UI (Not mapped to DB)
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string ItemsSummary { get; set; } = "";
    }

    // --- 6. SALES ITEMS (Details) ---
    public class SalesTransactionItem
    {
        public int Id { get; set; }
        public int SalesTransactionId { get; set; } // Link to Header
        public string ProductName { get; set; } = "";
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
    }

    // --- 7. EXPENSE RECORD ---
    public class ExpenseRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Category { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Amount { get; set; }
    }

    // --- 8. EXPENSE CATEGORY ---
    public class ExpenseCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Type { get; set; } = "Variable"; // Fixed or Variable
    }

    // --- 9. PAYROLL RECORD ---
    public class PayrollRecord
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = "";
        public DateTime PayDate { get; set; } = DateTime.Now;
        public DateTime PeriodStart { get; set; } = DateTime.Now.AddDays(-15);
        public DateTime PeriodEnd { get; set; } = DateTime.Now;

        // Earnings
        public decimal BasicSalary { get; set; }
        public decimal Allowances { get; set; }

        // Deductions
        public decimal DeductionTax { get; set; }
        public decimal DeductionBenefits { get; set; } // SSS, PhilHealth
        public decimal DeductionLoans { get; set; }

        // Net Pay is calculated, but we store it for easy reporting
        public decimal NetPay { get; set; }
    }
    // --- MODELS ---
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Role { get; set; } = "Staff";
        public decimal HourlyRate { get; set; }
        public string Status { get; set; } = "Active";
        public DateTime JoinedDate { get; set; } = DateTime.Now;
    }

    public class PayrollSetting
    {
        public int Id { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal SSSAmount { get; set; }
        public decimal PhilHealthAmount { get; set; }
        public decimal PagIbigAmount { get; set; }
    }

    public class AttendanceLog
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public decimal HoursWorked { get; set; }
    }

    public class EmployeePerformance
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int Rating { get; set; } // 1-10
        public string Notes { get; set; } = "";
    }
}

