namespace TrustCalibrationLab.Api.Models;

public sealed class ReviewAnswer
{
    public int Id { get; set; }
    public int ReviewSubmissionId { get; set; }
    public int ReviewCommentId { get; set; }
    public required string Verdict { get; set; }
    public required string Confidence { get; set; }
    public required string EvidenceType { get; set; }
    public required string EvidenceText { get; set; }
    public ReviewSubmission? Submission { get; set; }
}
