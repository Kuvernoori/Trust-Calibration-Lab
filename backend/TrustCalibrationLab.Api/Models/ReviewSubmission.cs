namespace TrustCalibrationLab.Api.Models;

public enum SubmissionStatus { Draft, Submitted }

public sealed class ReviewSubmission
{
    public int Id { get; set; }
    public int CaseId { get; set; }
    public required string SessionId { get; set; }
    public SubmissionStatus Status { get; set; } = SubmissionStatus.Draft;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public List<ReviewAnswer> Answers { get; set; } = [];
}
