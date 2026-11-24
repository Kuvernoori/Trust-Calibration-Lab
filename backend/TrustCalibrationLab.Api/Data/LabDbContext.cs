using Microsoft.EntityFrameworkCore;
using TrustCalibrationLab.Api.Models;

namespace TrustCalibrationLab.Api.Data;

public sealed class LabDbContext(DbContextOptions<LabDbContext> options) : DbContext(options)
{
    public DbSet<Case> Cases => Set<Case>();
    public DbSet<CaseFile> CaseFiles => Set<CaseFile>();
    public DbSet<ReviewComment> ReviewComments => Set<ReviewComment>();
    public DbSet<ReviewSubmission> ReviewSubmissions => Set<ReviewSubmission>();
    public DbSet<ReviewAnswer> ReviewAnswers => Set<ReviewAnswer>();
    public DbSet<VulnerabilityFinding> VulnerabilityFindings => Set<VulnerabilityFinding>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Case>().Property(item => item.HintLevel).HasConversion<string>();
        modelBuilder.Entity<Case>().HasMany(item => item.Files).WithOne().HasForeignKey(item => item.CaseId);
        modelBuilder.Entity<Case>().HasMany(item => item.Comments).WithOne().HasForeignKey(item => item.CaseId);
        modelBuilder.Entity<ReviewComment>().Property(item => item.Text).IsRequired();
        modelBuilder.Entity<ReviewSubmission>().Property(item => item.Status).HasConversion<string>();
        modelBuilder.Entity<ReviewSubmission>().HasMany(item => item.Answers).WithOne(item => item.Submission).HasForeignKey(item => item.ReviewSubmissionId);
        modelBuilder.Entity<ReviewAnswer>().HasIndex(item => new { item.ReviewSubmissionId, item.ReviewCommentId }).IsUnique();
    }
}
