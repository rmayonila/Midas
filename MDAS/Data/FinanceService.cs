using Microsoft.EntityFrameworkCore;
using MDAS.Data;

namespace MDAS.Data
{
    public class FinanceService
    {
        private readonly AppDbContext _db;

        public FinanceService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<FinancialReport> GetQuarterlyReportAsync(int year, int quarter)
        {
            // 1. Determine Date Range
            DateTime start = new DateTime(year, (quarter - 1) * 3 + 1, 1);
            DateTime end = start.AddMonths(3).AddDays(-1);

            // 2. FETCH REVENUE (Sales)
            var revenue = await _db.SalesTransactions
                .Where(s => s.Date >= start && s.Date <= end && s.Status == "Completed")
                .SumAsync(s => s.TotalAmount);

            // 3. FETCH EXPENSES
            // A. Operational Expenses (Rent, Utilities)
            var opex = await _db.ExpenseRecords
                .Where(e => e.Date >= start && e.Date <= end)
                .SumAsync(e => e.Amount);

            // B. COGS (Purchases - Raw Materials)
            var cogs = await _db.PurchaseOrders
                .Where(p => p.OrderDate >= start && p.OrderDate <= end && p.Status == "Received")
                .SumAsync(p => p.TotalCost);

            // C. Payroll (Salaries)
            var payroll = await _db.PayrollRecords
                .Where(p => p.PayDate >= start && p.PayDate <= end)
                .SumAsync(p => p.NetPay); // Ideally Gross, but using Net for now

            // 4. COMPILE REPORT
            return new FinancialReport
            {
                Year = year,
                Quarter = quarter,
                TotalRevenue = revenue,
                CostOfGoodsSold = cogs,
                OperationalExpenses = opex,
                PayrollExpenses = payroll
            };
        }
    }

    public class FinancialReport
    {
        public int Year { get; set; }
        public int Quarter { get; set; }
        public decimal TotalRevenue { get; set; }

        // Expenses
        public decimal CostOfGoodsSold { get; set; }
        public decimal OperationalExpenses { get; set; }
        public decimal PayrollExpenses { get; set; }

        // Computed
        public decimal TotalExpenses => CostOfGoodsSold + OperationalExpenses + PayrollExpenses;
        public decimal NetProfit => TotalRevenue - TotalExpenses;
        public decimal ProfitMargin => TotalRevenue > 0 ? (NetProfit / TotalRevenue) * 100 : 0;
    }
}