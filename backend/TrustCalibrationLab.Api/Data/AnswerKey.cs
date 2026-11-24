namespace TrustCalibrationLab.Api.Data;

// Backend-only answer key. Intentionally not used by student-facing controllers.
public static class AnswerKey
{
    public static readonly IReadOnlyDictionary<int, string> CaseThreeVulnerabilities = new Dictionary<int, string>
    {
        [1] = "SQL Injection — CWE-89", [2] = "IDOR — CWE-639", [3] = "Path Traversal — CWE-22", [4] = "Weak Password Hashing — CWE-328 / CWE-916", [5] = "Hardcoded JWT Secret — CWE-798"
    };
}
