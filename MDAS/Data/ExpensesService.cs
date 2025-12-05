using Microsoft.EntityFrameworkCore;
using MDAS.Data;

namespace MDAS.Data
{
    public class ExpenseService
    {
        private readonly AppDbContext _db;

        public ExpenseService(AppDbContext db)
        {
            _db = db;
        }

        // --- EXPENSES ---
        public async Task<List<ExpenseRecord>> GetExpensesAsync()
        {
            return await _db.ExpenseRecords.OrderByDescending(e => e.Date).ToListAsync();
        }

        public async Task AddExpenseAsync(ExpenseRecord expense)
        {
            _db.ExpenseRecords.Add(expense);
            await _db.SaveChangesAsync();
        }

        // --- CATEGORIES ---
        public async Task<List<ExpenseCategory>> GetCategoriesAsync()
        {
            return await _db.ExpenseCategories.ToListAsync();
        }

        public async Task AddCategoryAsync(ExpenseCategory category)
        {
            _db.ExpenseCategories.Add(category);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(ExpenseCategory category)
        {
            _db.ExpenseCategories.Remove(category);
            await _db.SaveChangesAsync();
        }

        // --- SEEDING (Run once) ---
        public async Task SeedDefaultsAsync()
        {
            if (!await _db.ExpenseCategories.AnyAsync())
            {
                _db.ExpenseCategories.AddRange(
                    new ExpenseCategory { Name = "Rent", Type = "Fixed" },
                    new ExpenseCategory { Name = "Utilities", Type = "Variable" },
                    new ExpenseCategory { Name = "Salaries", Type = "Fixed" }
                );
                await _db.SaveChangesAsync();
            }
        }
    }
}