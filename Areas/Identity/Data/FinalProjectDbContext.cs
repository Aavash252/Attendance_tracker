using FinalProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Identity.Data;

public class FinalProjectDbContext : IdentityDbContext<FinalProjectUser>
{
    public FinalProjectDbContext(DbContextOptions<FinalProjectDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
    }

    public DbSet<TimeTable> TimeModel { get; set; }
}
