using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrustCalibrationLab.Api.Data;
using TrustCalibrationLab.Api.Models;

namespace TrustCalibrationLab.Api.Controllers;

[ApiController]
[Route("api/submissions")]
public sealed class SubmissionsController(LabDbContext db) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateSubmissionRequest request)
    {
        if (!await db.Cases.AnyAsync(item => item.Id == request.CaseId)) return NotFound();
        var now = DateTime.UtcNow;
        var submission = new ReviewSubmission { CaseId = request.CaseId, SessionId = request.SessionId, CreatedAt = now, UpdatedAt = now };
        db.ReviewSubmissions.Add(submission); await db.SaveChangesAsync();
        return Ok(ToResponse(submission));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var submission = await db.ReviewSubmissions.AsNoTracking().Include(item => item.Answers).SingleOrDefaultAsync(item => item.Id == id);
        return submission is null ? NotFound() : Ok(ToResponse(submission));
    }

    [HttpPut("{id:int}/answers/{commentId:int}")]
    public async Task<IActionResult> SaveAnswer(int id, int commentId, SaveAnswerRequest request)
    {
        var submission = await db.ReviewSubmissions.Include(item => item.Answers).SingleOrDefaultAsync(item => item.Id == id);
        if (submission is null) return NotFound();
        if (submission.Status == SubmissionStatus.Submitted) return Conflict(new { message = "Submitted reviews cannot be edited." });
        if (!await db.ReviewComments.AnyAsync(item => item.Id == commentId && item.CaseId == submission.CaseId)) return NotFound();
        if (string.IsNullOrWhiteSpace(request.EvidenceText)) return BadRequest(new { message = "Evidence explanation is required." });
        var answer = submission.Answers.SingleOrDefault(item => item.ReviewCommentId == commentId);
        if (answer is null) submission.Answers.Add(new ReviewAnswer { ReviewCommentId = commentId, Verdict = request.Verdict, Confidence = request.Confidence, EvidenceType = request.EvidenceType, EvidenceText = request.EvidenceText });
        else { answer.Verdict = request.Verdict; answer.Confidence = request.Confidence; answer.EvidenceType = request.EvidenceType; answer.EvidenceText = request.EvidenceText; }
        submission.UpdatedAt = DateTime.UtcNow; await db.SaveChangesAsync();
        return Ok(ToResponse(submission));
    }

    [HttpPost("{id:int}/submit")]
    public async Task<IActionResult> Submit(int id)
    {
        var submission = await db.ReviewSubmissions.Include(item => item.Answers).SingleOrDefaultAsync(item => item.Id == id);
        if (submission is null) return NotFound();
        if (submission.Status == SubmissionStatus.Submitted) return Ok(ToResponse(submission));
        var required = await db.ReviewComments.CountAsync(item => item.CaseId == submission.CaseId);
        if (submission.Answers.Count != required) return BadRequest(new { message = $"Evaluate all {required} comments before submitting." });
        var now = DateTime.UtcNow; submission.Status = SubmissionStatus.Submitted; submission.SubmittedAt = now; submission.UpdatedAt = now; await db.SaveChangesAsync();
        return Ok(ToResponse(submission));
    }

    private static object ToResponse(ReviewSubmission submission) => new { submission.Id, submission.CaseId, Status = submission.Status.ToString(), submission.CreatedAt, submission.UpdatedAt, submission.SubmittedAt, Answers = submission.Answers.Select(item => new { item.Id, item.ReviewCommentId, item.Verdict, item.Confidence, item.EvidenceType, item.EvidenceText }) };
}

public sealed record CreateSubmissionRequest(int CaseId, string SessionId);
public sealed record SaveAnswerRequest(string Verdict, string Confidence, string EvidenceType, string EvidenceText);
