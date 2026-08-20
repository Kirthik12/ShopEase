using Microsoft.EntityFrameworkCore;
using ShopEase.Domain.Features.Payments.Entities;
using ShopEase.Domain.Repositories;
using ShopEase.Infrastructure.Data;

namespace ShopEase.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly ShopEaseDbContext _db;

    public PaymentRepository(ShopEaseDbContext db) => _db = db;

    public Task<List<Payment>> GetAllAsync() =>
        _db.Payments.AsNoTracking().OrderByDescending(p => p.CreatedAt).ToListAsync();

    public Task<List<Payment>> GetByUserIdAsync(int userId) =>
        _db.Payments.AsNoTracking().Where(p => p.UserId == userId).OrderByDescending(p => p.CreatedAt).ToListAsync();

    public Task<Payment?> GetByOrderIdAsync(int orderId) =>
        _db.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.OrderId == orderId);

    public async Task<Payment> AddAsync(Payment payment)
    {
        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();
        return payment;
    }

    public Task<List<PaymentMethod>> GetMethodsAsync() => _db.PaymentMethods.AsNoTracking().ToListAsync();

    public Task<List<string>> GetStatusNamesAsync() =>
        _db.PaymentStatuses.AsNoTracking().Select(s => s.Name).ToListAsync();
}
