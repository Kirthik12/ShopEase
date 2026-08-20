using Microsoft.EntityFrameworkCore;
using ShopEase.Domain.Features.Catalog.Entities;
using ShopEase.Domain.Repositories;
using ShopEase.Infrastructure.Data;

namespace ShopEase.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ShopEaseDbContext _db;

    public ProductRepository(ShopEaseDbContext db) => _db = db;

    public Task<List<Product>> GetAllAsync() => _db.Products.AsNoTracking().ToListAsync();

    // AsNoTracking, matching GetAllAsync: UpdateAsync always explicitly reattaches via .Update(),
    // so nothing here relies on change-tracking — and this avoids identity-map conflicts if a
    // caller fetches this product both individually and via GetAllAsync within the same DbContext.
    public Task<Product?> GetByIdAsync(int id) => _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    public Task<bool> ExistsWithSkuAsync(string sku, int? excludeId = null) =>
        _db.Products.AnyAsync(p => p.Sku.ToUpper() == sku.ToUpper() && (excludeId == null || p.Id != excludeId));

    public async Task<Product> AddAsync(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        _db.Products.Update(product);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return false;
        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return true;
    }

    public Task<int> CountByCategoryAsync(int categoryId) =>
        _db.Products.CountAsync(p => p.CategoryId == categoryId);

    public Task<List<string>> GetDistinctBrandsAsync() =>
        _db.Products.Where(p => p.IsActive).Select(p => p.Brand).Distinct().OrderBy(b => b).ToListAsync();
}
