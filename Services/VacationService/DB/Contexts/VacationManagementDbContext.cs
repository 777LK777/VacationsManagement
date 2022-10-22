using System.Reflection;
using Microsoft.EntityFrameworkCore;

using VacationService.DB.Models;

namespace VacationService.DB.Contexts;

public class VacationManagementDbContext : DbContext
{
    public DbSet<DbVacation> Vacations { get; set; }
    public DbSet<DbVacationBalance> VacationsBalances { get; set; }

    public VacationManagementDbContext(DbContextOptions options) 
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}