namespace TrustCalibrationLab.Api.Models;

public sealed class ReviewComment
{
    public int Id { get; set; }
    public int CaseId { get; set; }
    public required string FilePath { get; set; }
    public int LineNumber { get; set; }
    public required string Text { get; set; }
}
