using Microsoft.EntityFrameworkCore;
using ShopEase.Domain.Features.Backup.Entities;
using ShopEase.Domain.Repositories;
using ShopEase.Infrastructure.Data;

namespace ShopEase.Infrastructure.Repositories;

public class BackupSnapshotRepository : IBackupSnapshotRepository
{
    private readonly ShopEaseDbContext _db;

    public BackupSnapshotRepository(ShopEaseDbContext db) => _db = db;

    public async Task<BackupSnapshot> AddAsync(BackupSnapshot snapshot)
    {
        _db.BackupSnapshots.Add(snapshot);
        await _db.SaveChangesAsync();
        return snapshot;
    }

    public Task<BackupSnapshot?> GetStagingAsync() =>
        _db.BackupSnapshots.AsNoTracking().Where(s => s.IsStaging).OrderByDescending(s => s.CreatedAt).FirstOrDefaultAsync();

    public async Task SetStagingAsync(BackupSnapshot snapshot)
    {
        await ClearStagingAsync();
        snapshot.IsStaging = true;
        _db.BackupSnapshots.Add(snapshot);
        await _db.SaveChangesAsync();
    }

    public async Task ClearStagingAsync()
    {
        var existing = await _db.BackupSnapshots.Where(s => s.IsStaging).ToListAsync();
        if (existing.Count == 0) return;
        _db.BackupSnapshots.RemoveRange(existing);
        await _db.SaveChangesAsync();
    }

    public async Task TrimAsync(string jobName, int keep)
    {
        var snapshots = await _db.BackupSnapshots
            .Where(s => s.JobName == jobName && !s.IsStaging)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        if (snapshots.Count <= keep) return;
        _db.BackupSnapshots.RemoveRange(snapshots.Skip(keep));
        await _db.SaveChangesAsync();
    }
}
