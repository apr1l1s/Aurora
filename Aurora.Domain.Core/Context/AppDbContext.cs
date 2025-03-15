using Aurora.Domain.Core.User;
using Microsoft.EntityFrameworkCore;

namespace Aurora.Domain.Core.Context;

public class AppDbContext
    : DbContext
{
    public DbSet<TelegramUser> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            options.UseNpgsql("Host=postgres;Database=mydatabase;Username=user;Password=password");
        }
    }
}