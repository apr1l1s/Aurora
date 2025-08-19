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

        try
        {
            if (Database.CanConnect())
            {
                Console.WriteLine("✅ Успешное подключение к базе данных.");
            }
            else
            {
                Console.WriteLine("⚠️ Подключение не удалось. Пытаемся создать базу...");
                Database.EnsureCreated();
                Console.WriteLine("✅ База данных успешно создана.");
                Database.Migrate();
                Console.WriteLine("✅ Миграции применены и база создана при необходимости.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка: {ex.Message}");
        }
    }
}