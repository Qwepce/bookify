namespace Bookify.Api.Middleware;

internal record ExceptionDetails(
    int Status,
    string Type,
    string Title,
    string Details,
    IEnumerable<object?> Errors );