using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace MDAS.Data
{
    public class UserService
    {
        private readonly AppDbContext _context;

        // Constructor: This requests the database connection
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // --- 1. LOGIN FUNCTION (Used by Login Page) ---
        public async Task<User?> LoginAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            // Find user where Username matches Email AND Password matches
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == email && u.Password == password);

            // If user doesn't exist OR is locked, return null
            if (user == null || user.IsActive == false)
                return null;

            return user;
        }

        // --- 2. GET ALL USERS (Used by Settings Page) ---
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // --- 3. CREATE NEW USER (Used by Settings Page) ---
        public async Task<bool> CreateUserAsync(User newUser)
        {
            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == newUser.Username))
                return false;

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return true;
        }

        // --- 4. UPDATE USER (Used for Password Reset / Locking) ---
        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}