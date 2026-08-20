using Microsoft.EntityFrameworkCore;
using ShopEase.Domain.Features.Coupons.Entities;
using ShopEase.Domain.Repositories;
using ShopEase.Infrastructure.Data;

namespace ShopEase.Infrastructure.Repositories;

public class CouponRepository : ICouponRepository
{
    private readonly ShopEaseDbContext _db;

    public CouponRepository(ShopEaseDbContext db) => _db = db;

    public Task<List<Coupon>> GetAllAsync() => _db.Coupons.AsNoTracking().ToListAsync();

    public Task<Coupon?> GetByCodeAsync(string code) =>
        _db.Coupons.FirstOrDefaultAsync(c => c.Code.ToUpper() == code.ToUpper());

    public Task<AppliedCoupon?> GetAppliedAsync(int userId) =>
        _db.AppliedCoupons.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == userId);

    public async Task SetAppliedAsync(int userId, string code)
    {
        var existing = await _db.AppliedCoupons.FirstOrDefaultAsync(a => a.UserId == userId);
        if (existing == null)
        {
            _db.AppliedCoupons.Add(new AppliedCoupon { UserId = userId, Code = code });
        }
        else
        {
            existing.Code = code;
        }

        await _db.SaveChangesAsync();
    }

    public async Task RemoveAppliedAsync(int userId)
    {
        var existing = await _db.AppliedCoupons.FirstOrDefaultAsync(a => a.UserId == userId);
        if (existing == null) return;
        _db.AppliedCoupons.Remove(existing);
        await _db.SaveChangesAsync();
    }
}
