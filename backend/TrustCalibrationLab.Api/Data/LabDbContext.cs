using Microsoft.EntityFrameworkCore;
using TrustCalibrationLab.Api.Models;

namespace TrustCalibrationLab.Api.Data;

public sealed class LabDbContext(DbContextOptions<LabDbContext> options) : DbContext(options)
{
    public DbSet<Case> Cases => Set<Case>();
    public DbSet<CaseFile> CaseFiles => Set<CaseFile>();
    public DbSet<ReviewComment> ReviewComments => Set<ReviewComment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Case>().Property(item => item.HintLevel).HasConversion<string>();
        modelBuilder.Entity<Case>().HasMany(item => item.Files).WithOne().HasForeignKey(item => item.CaseId);
        modelBuilder.Entity<Case>().HasMany(item => item.Comments).WithOne().HasForeignKey(item => item.CaseId);
        modelBuilder.Entity<ReviewComment>().Property(item => item.Text).IsRequired();
    }
}
