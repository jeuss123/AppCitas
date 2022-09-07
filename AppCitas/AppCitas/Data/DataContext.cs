using AppCitas.Ententies;
using Microsoft.EntityFrameworkCore;

namespace AppCitas.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<AppUser> Users { get; set; }
}
