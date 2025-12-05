using Microsoft.EntityFrameworkCore;
using MDAS.Data;

namespace MDAS.Data
{
    public class PayrollService
    {
        private readonly AppDbContext _db;

        public PayrollService(AppDbContext db)
        {
            _db = db;
        }

        // --- 1. SETTINGS ---
        public async Task<PayrollSetting> GetSettingsAsync()
        {
            var settings = await _db.PayrollSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                // Create default if missing
                settings = new PayrollSetting { TaxPercentage = 5, SSSAmount = 500, PhilHealthAmount = 300, PagIbigAmount = 100 };
                _db.PayrollSettings.Add(settings);
                await _db.SaveChangesAsync();
            }
            return settings;
        }

        public async Task SaveSettingsAsync(PayrollSetting settings)
        {
            _db.PayrollSettings.Update(settings);
            await _db.SaveChangesAsync();
        }

        // --- 2. EMPLOYEES ---
        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _db.Employees.ToListAsync();
        }

        public async Task AddEmployeeAsync(Employee emp)
        {
            _db.Employees.Add(emp);
            await _db.SaveChangesAsync();
        }

        // --- 3. ATTENDANCE (Simulation for Calculation) ---
        public async Task<decimal> GetTotalHoursAsync(int employeeId, DateTime start, DateTime end)
        {
            // In a real app, this queries AttendanceLogs. 
            // For now, we simulate that logs exist or return 0 if none.
            // Let's verify if actual logs exist:
            var logs = await _db.AttendanceLogs
                .Where(a => a.EmployeeId == employeeId && a.Date >= start && a.Date <= end)
                .SumAsync(a => a.HoursWorked);

            // If no logs found (since dashboard is coming soon), 
            // we return 0 so the user can manually override in the UI.
            return logs;
        }

        // --- 4. GENERATE PAYROLL ---
        public async Task<List<PayrollRecord>> GetHistoryAsync()
        {
            return await _db.PayrollRecords.OrderByDescending(p => p.PayDate).ToListAsync();
        }

        public async Task CreatePayslipAsync(PayrollRecord record)
        {
            _db.PayrollRecords.Add(record);
            await _db.SaveChangesAsync();
        }
    }
}