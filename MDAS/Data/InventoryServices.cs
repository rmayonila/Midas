using Microsoft.EntityFrameworkCore;
using MDAS.Data;

namespace MDAS.Data
{
    public class InventoryServices
    {
        private readonly AppDbContext _db;

        public InventoryServices(AppDbContext db)
        {
            _db = db;
        }

        // 1. GET ALL
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _db.Products.ToListAsync();
        }

        // 2. ADD
        public async Task AddProductAsync(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
        }

        // 3. UPDATE (New: Needed for Stock Adjustment)
        public async Task UpdateProductAsync(Product product)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
        }

        // 4. DELETE
        public async Task DeleteProductAsync(Product product)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }

        // 5. GENERATE SAMPLE DATA
        public async Task GenerateSampleDataAsync()
        {
            if (!await _db.Products.AnyAsync())
            {
                _db.Products.Add(new Product { Name = "Oxford Shirt", SKU = "MD-001", Category = "Men", Price = 1200, Stock = 45, Variants = "S, M, L, XL" });
                _db.Products.Add(new Product { Name = "Summer Dress", SKU = "MD-003", Category = "Women", Price = 2100, Stock = 5, Variants = "Red, Blue" });
                _db.Products.Add(new Product { Name = "Leather Belt", SKU = "ACC-01", Category = "Accessories", Price = 850, Stock = 100, Variants = "One Size" });
                await _db.SaveChangesAsync();
            }
        }
    }
}