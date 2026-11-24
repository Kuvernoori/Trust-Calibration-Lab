namespace TrustCalibrationLab.Api.Models;

public enum HintLevel { None, CategoryOnly, HintsAndExampleVerdict }

public sealed class Case
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Difficulty { get; set; }
    public int VulnerabilityCount { get; set; }
    public int CommentCount { get; set; }
    public HintLevel HintLevel { get; set; }
    public List<CaseFile> Files { get; set; } = [];
    public List<ReviewComment> Comments { get; set; } = [];
}
