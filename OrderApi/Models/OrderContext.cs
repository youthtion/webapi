using Microsoft.EntityFrameworkCore;

namespace OrderApi.Models;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options)
        : base(options)
    {
    }

    public DbSet<OrderData> Orders { get; set; } = null!;
}