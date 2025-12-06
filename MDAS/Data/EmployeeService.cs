using Microsoft.EntityFrameworkCore;
using MDAS.Data;

namespace MDAS.Data
{
    public class EmployeeService
    {
        private readonly AppDbContext _db;

        public EmployeeService(AppDbContext db)
        {
            _db = db;
        }

        // 1. GET ALL EMPLOYEES
        public async Task<List<Employee>> GetEmployeesAsync()
        { 
        try
    {
        return await _db.Employees.ToListAsync();
    }
    catch (Exception ex)
    {
        // 🚨 THIS IS WHERE THE ERROR MESSAGE WILL APPEAR 🚨
        Console.WriteLine($"Database Load Error: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
        }

// Re-throw the exception so it stops at your breakpoint/log.
throw; 
    }
    }

// 2. ADD / UPDATE EMPLOYEE
public async Task SaveEmployeeAsync(Employee emp)
        {
            if (emp.Id == 0)
                _db.Employees.Add(emp);
            else
                _db.Employees.Update(emp);

            await _db.SaveChangesAsync();
        }

        // 3. GET PERFORMANCE HISTORY
        public async Task<List<EmployeePerformance>> GetPerformanceAsync(int employeeId)
        {
            return await _db.EmployeePerformance
                            .Where(p => p.EmployeeId == employeeId)
                            .OrderByDescending(p => p.Date)
                            .ToListAsync();
        }

        // 4. LOG PERFORMANCE (Daily)
        public async Task AddPerformanceLogAsync(EmployeePerformance log)
        {
            _db.EmployeePerformance.Add(log);
            await _db.SaveChangesAsync();
        }

        // 5. GET AVERAGE RATING (For Dashboard)
        public async Task<double> GetAverageRatingAsync(int employeeId)
        {
            var logs = await _db.EmployeePerformance.Where(p => p.EmployeeId == employeeId).ToListAsync();
            if (logs.Count == 0) return 0;
            return logs.Average(p => p.Rating);
        }

    }
}