using Microsoft.EntityFrameworkCore;
using ShopEase.Domain.Features.Auth.Entities;
using ShopEase.Domain.Repositories;
using ShopEase.Infrastructure.Data;

namespace ShopEase.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ShopEaseDbContext _db;

    public UserRepository(ShopEaseDbContext db) => _db = db;

    public Task<List<User>> GetAllAsync() =>
        _db.Users.Include(u => u.Addresses).AsNoTracking().ToListAsync();

    public Task<User?> GetByIdAsync(int id) =>
        _db.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Id == id);

    public Task<User?> GetByEmailAsync(string email) =>
        _db.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

    public async Task<User> AddAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
    }
}
