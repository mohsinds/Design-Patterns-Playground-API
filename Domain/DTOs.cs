namespace DesignPatterns.Playground.Api.Domain;

/// <summary>
/// Response for pattern demo endpoints.
/// </summary>
public record PatternDemoResponse(
    string Pattern,
    string Description,
    object? Result,
    Dictionary<string, object>? Metadata = null
);

/// <summary>
/// Response for pattern test endpoints.
/// </summary>
public record PatternTestResponse(
    string Pattern,
    string Status, // "PASS" or "FAIL"
    List<TestCheck> Checks
);

/// <summary>
/// Individual test check result.
/// </summary>
public record TestCheck(
    string Name,
    bool Pass,
    string Details
);
