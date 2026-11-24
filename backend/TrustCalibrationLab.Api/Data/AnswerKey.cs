namespace TrustCalibrationLab.Api.Data;

// Backend-only answer key. Intentionally not used by student-facing controllers.
public static class AnswerKey
{
    public static readonly IReadOnlyDictionary<int, (string Verdict, string Explanation)> CommentAnswers = new Dictionary<int, (string, string)>
    {
        [1001] = ("True", "The original file name is used in the storage path without a safe-name policy. File names should be generated server-side and validated."),
        [1002] = ("True", "Uploaded content should be type-checked and stored outside the executable or public content path where possible."),
        [1003] = ("False", "This response does not expose a stored path. The concern is not supported by the shown code."),
        [2001] = ("True", "Request paths can contain sensitive identifiers, so logging should use an appropriate privacy and redaction policy."),
        [2002] = ("False", "The request pipeline awaits the next middleware; the shown code does not establish a vulnerability by itself."),
        [2003] = ("Partially true", "A trace identifier can be useful for diagnostics, but response headers should be reviewed for information disclosure."),
        [2004] = ("False", "The snippet does not show that the trace identifier is predictable or security-sensitive."),
        [2005] = ("True", "Structured logging and controlled log levels are preferable when request data may contain sensitive values."),
        [2006] = ("False", "The middleware does not expose credentials or authorization data in the shown response header.")
    };
    public static readonly IReadOnlyDictionary<int, string> CaseThreeVulnerabilities = new Dictionary<int, string>
    {
        [1] = "SQL Injection — CWE-89", [2] = "IDOR — CWE-639", [3] = "Path Traversal — CWE-22", [4] = "Weak Password Hashing — CWE-328 / CWE-916", [5] = "Hardcoded JWT Secret — CWE-798"
    };

    public static readonly IReadOnlyDictionary<int, (string Verdict, string Explanation)> CaseThreeComments = new Dictionary<int, (string, string)>
    {
        [3001] = ("True", "Concatenating user input into raw SQL is unsafe. Parameterized queries should be used."),
        [3002] = ("True", "Secrets should not be stored directly in source code."),
        [3003] = ("Partially true", "The finding is correct, but SHA-256 is not an appropriate password hashing replacement. Use Argon2id, PBKDF2, or ASP.NET PasswordHasher."),
        [3004] = ("Partially true", "The vulnerability is real, but Replace(\"../\", \"\") is bypassable, including with the ....// pattern."),
        [3005] = ("True", "Missing HTTPS redirection and HSTS is a valid security hardening recommendation."),
        [3006] = ("True", "Login endpoints should have appropriate rate limiting."),
        [3007] = ("False", "The endpoint returns application/json and does not directly render the content as HTML."),
        [3008] = ("False", "In this scenario authentication uses a Bearer token in the Authorization header, not an automatically attached cookie."),
        [3009] = ("False", "EF Core parameterizes this LINQ query."),
        [3010] = ("False", "The requestId is not security-sensitive in this context.")
    };
    public static readonly IReadOnlyDictionary<string, string> CaseThreeFindings = new Dictionary<string, string>
    { ["CWE-89"] = "SQL Injection", ["CWE-639"] = "IDOR", ["CWE-22"] = "Path Traversal", ["CWE-328"] = "Weak Password Hashing", ["CWE-916"] = "Weak Password Hashing", ["CWE-798"] = "Hardcoded JWT Secret" };

    public static bool TryGetCommentAnswer(int commentId, out (string Verdict, string Explanation) answer) => CommentAnswers.TryGetValue(commentId, out answer) || CaseThreeComments.TryGetValue(commentId, out answer);
}
