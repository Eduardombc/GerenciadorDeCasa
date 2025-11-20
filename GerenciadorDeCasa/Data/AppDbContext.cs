using GerenciadorDeCasa.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeCasa.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    DbSet<HouseTask> HouseTasks { get; set; }
}
