using Microsoft.EntityFrameworkCore;
using TrustCalibrationLab.Api.Models;

namespace TrustCalibrationLab.Api.Data;

public static class SeedData
{
    public static async Task InitializeAsync(LabDbContext db)
    {
        await db.Database.EnsureCreatedAsync();
        await db.Database.ExecuteSqlRawAsync("CREATE TABLE IF NOT EXISTS ReviewSubmissions (Id INTEGER PRIMARY KEY AUTOINCREMENT, CaseId INTEGER NOT NULL, SessionId TEXT NOT NULL, Status TEXT NOT NULL, CreatedAt TEXT NOT NULL, UpdatedAt TEXT NOT NULL, SubmittedAt TEXT NULL)");
        await db.Database.ExecuteSqlRawAsync("CREATE TABLE IF NOT EXISTS ReviewAnswers (Id INTEGER PRIMARY KEY AUTOINCREMENT, ReviewSubmissionId INTEGER NOT NULL, ReviewCommentId INTEGER NOT NULL, Verdict TEXT NOT NULL, Confidence TEXT NOT NULL, EvidenceType TEXT NOT NULL, EvidenceText TEXT NOT NULL, CONSTRAINT FK_ReviewAnswers_ReviewSubmissions_ReviewSubmissionId FOREIGN KEY (ReviewSubmissionId) REFERENCES ReviewSubmissions (Id), CONSTRAINT IX_ReviewAnswers_SubmissionComment UNIQUE (ReviewSubmissionId, ReviewCommentId))");
        if (await db.Cases.AnyAsync()) return;

        var cases = new List<Case>
        {
            new() { Id = 1, Title = "The suspicious upload", Description = "Triage a compact upload controller where the AI spots some, but not all, security concerns.", Difficulty = "Intro", VulnerabilityCount = 2, CommentCount = 3, HintLevel = HintLevel.HintsAndExampleVerdict },
            new() { Id = 2, Title = "A noisy middleware", Description = "Separate useful signals from plausible-sounding security advice in a request pipeline.", Difficulty = "Intermediate", VulnerabilityCount = 3, CommentCount = 6, HintLevel = HintLevel.CategoryOnly },
            new() { Id = 3, Title = "The legacy API", Description = "A realistic slice of a legacy API with five vulnerabilities and ten AI-generated review comments.", Difficulty = "Advanced", VulnerabilityCount = 5, CommentCount = 10, HintLevel = HintLevel.None }
        };

        cases[0].Files.Add(new CaseFile { Id = 101, CaseId = 1, FilePath = "Controllers/UploadController.cs", Content = "[HttpPost]\npublic IActionResult Upload(IFormFile file)\n{\n    var path = Path.Combine(\"uploads\", file.FileName);\n    using var stream = System.IO.File.Create(path);\n    file.CopyTo(stream);\n    return Ok();\n}" });
        cases[0].Comments.AddRange([new ReviewComment { Id = 1001, CaseId = 1, FilePath = "Controllers/UploadController.cs", LineNumber = 3, Text = "The upload path should be reviewed for unsafe file names." }, new ReviewComment { Id = 1002, CaseId = 1, FilePath = "Controllers/UploadController.cs", LineNumber = 4, Text = "The file is written directly to disk; validate its type before storage." }, new ReviewComment { Id = 1003, CaseId = 1, FilePath = "Controllers/UploadController.cs", LineNumber = 7, Text = "The response does not expose the stored file path to the caller." }]);
        cases[1].Files.Add(new CaseFile { Id = 201, CaseId = 2, FilePath = "Middleware/RequestPipeline.cs", Content = "public async Task InvokeAsync(HttpContext context)\n{\n    logger.LogInformation(context.Request.Path);\n    await next(context);\n    context.Response.Headers.Append(\"X-Trace\", context.TraceIdentifier);\n}" });
        cases[1].Comments.AddRange(new[] { 1, 2, 3, 4, 5, 6 }.Select((n, i) => new ReviewComment { Id = 2001 + i, CaseId = 2, FilePath = "Middleware/RequestPipeline.cs", LineNumber = i + 2, Text = $"Demonstration content: review signal {n} should be checked against the surrounding request context." }));

        cases[2].Files.AddRange([
            new CaseFile { Id = 301, CaseId = 3, FilePath = "Controllers/UsersController.cs", Content = "[HttpGet(\"{id}\")]\npublic IActionResult Get(int id)\n{\n    var sql = $\"SELECT * FROM Users WHERE Id = {id}\";\n    return Ok(db.Users.Where(user => user.Id == id));\n}" },
            new CaseFile { Id = 302, CaseId = 3, FilePath = "Security/TokenService.cs", Content = "private const string JwtSecret = \"dev-secret-change-me\";\npublic string Hash(string password)\n{\n    return MD5.HashData(Encoding.UTF8.GetBytes(password)).ToString();\n}" },
            new CaseFile { Id = 303, CaseId = 3, FilePath = "Controllers/FilesController.cs", Content = "[HttpGet]\npublic IActionResult Download(string name)\n{\n    var safeName = name.Replace(\"../\", \"\");\n    return PhysicalFile(Path.Combine(root, safeName), \"application/octet-stream\");\n}" },
            new CaseFile { Id = 304, CaseId = 3, FilePath = "Controllers/AuthController.cs", Content = "[HttpPost(\"login\")]\npublic IActionResult Login(LoginRequest request)\n{\n    var requestId = new Random().Next();\n    return Ok(tokenService.Create(request.UserName, requestId));\n}" }
        ]);
        cases[2].Comments.AddRange([
            new ReviewComment { Id = 3001, CaseId = 3, FilePath = "Controllers/UsersController.cs", LineNumber = 3, Text = "Build this SQL statement with parameterized queries." },
            new ReviewComment { Id = 3002, CaseId = 3, FilePath = "Security/TokenService.cs", LineNumber = 1, Text = "The JWT signing secret is embedded in source code." },
            new ReviewComment { Id = 3003, CaseId = 3, FilePath = "Security/TokenService.cs", LineNumber = 4, Text = "MD5 is not suitable for password hashing; consider a password-specific hashing algorithm." },
            new ReviewComment { Id = 3004, CaseId = 3, FilePath = "Controllers/FilesController.cs", LineNumber = 4, Text = "Replacing a relative path fragment is not a reliable way to constrain file access." },
            new ReviewComment { Id = 3005, CaseId = 3, FilePath = "Controllers/UsersController.cs", LineNumber = 1, Text = "The endpoint does not visibly enforce HTTPS or HSTS." },
            new ReviewComment { Id = 3006, CaseId = 3, FilePath = "Controllers/AuthController.cs", LineNumber = 1, Text = "Login attempts should be protected by rate limiting." },
            new ReviewComment { Id = 3007, CaseId = 3, FilePath = "Controllers/UsersController.cs", LineNumber = 5, Text = "Review the response encoding guarantees for data returned by this JSON endpoint." },
            new ReviewComment { Id = 3008, CaseId = 3, FilePath = "Controllers/AuthController.cs", LineNumber = 2, Text = "State-changing authentication flows should consider CSRF protection." },
            new ReviewComment { Id = 3009, CaseId = 3, FilePath = "Controllers/UsersController.cs", LineNumber = 5, Text = "Check whether this LINQ predicate is translated safely for the configured provider." },
            new ReviewComment { Id = 3010, CaseId = 3, FilePath = "Controllers/AuthController.cs", LineNumber = 4, Text = "A non-cryptographic random source may be unsuitable for security-sensitive request identifiers." }
        ]);
        await db.Cases.AddRangeAsync(cases);
        await db.SaveChangesAsync();
    }
}
