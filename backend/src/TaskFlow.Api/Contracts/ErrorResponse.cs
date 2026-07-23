namespace TaskFlow.Api.Contracts;

public sealed record ErrorResponse(
    string Error,
    IReadOnlyList<string>? Details = null);
