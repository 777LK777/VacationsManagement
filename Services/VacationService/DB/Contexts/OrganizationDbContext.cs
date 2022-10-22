using System.Reflection;
using Microsoft.EntityFrameworkCore;

using VacationService.DB.Models.Organization;

namespace VacationService.DB.Contexts;

public class OrganizationDbContext : DbContext
{
    public DbSet<DbEmployee> Employees { get; set; }
    public DbSet<DbTeam> Teams { get; set; }

    public OrganizationDbContext(DbContextOptions options)
        : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}