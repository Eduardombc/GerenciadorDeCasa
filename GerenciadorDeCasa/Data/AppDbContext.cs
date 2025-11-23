using GerenciadorDeCasa.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeCasa.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    public DbSet<HouseTask> HouseTasks { get; set; }
}
