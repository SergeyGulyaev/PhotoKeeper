using Microsoft.EntityFrameworkCore;

namespace PhotoKeeper;

/// <summary>
/// Контекст БД
/// </summary>
public class KeeperDbContext : DbContext
{
    /// <inheritdoc cref="PhotoKeeper.PhotoDescriptor"/>
    public DbSet<PhotoDescriptor> PhotoDescriptor { get; set; }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbConstants.DbName}.db");
}
