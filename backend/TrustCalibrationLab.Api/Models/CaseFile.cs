namespace TrustCalibrationLab.Api.Models;

public sealed class CaseFile
{
    public int Id { get; set; }
    public int CaseId { get; set; }
    public required string FilePath { get; set; }
    public required string Content { get; set; }
}
