namespace TaskFlow.Application.Common;

public sealed class ApplicationResult<T>
{
    private ApplicationResult(
        bool succeeded,
        T? value,
        ApplicationErrorType? errorType,
        IReadOnlyList<ApplicationError> errors)
    {
        Succeeded = succeeded;
        Value = value;
        ErrorType = errorType;
        Errors = errors;
    }

    public bool Succeeded { get; }

    public T? Value { get; }

    public ApplicationErrorType? ErrorType { get; }

    public IReadOnlyList<ApplicationError> Errors { get; }

    public static ApplicationResult<T> Success(T value)
    {
        return new ApplicationResult<T>(
            succeeded: true,
            value,
            errorType: null,
            errors: Array.Empty<ApplicationError>());
    }

    public static ApplicationResult<T> ValidationFailure(params ApplicationError[] errors)
    {
        return Failure(ApplicationErrorType.Validation, errors);
    }

    public static ApplicationResult<T> NotFound(string code, string message)
    {
        return Failure(
            ApplicationErrorType.NotFound,
            new[] { new ApplicationError(code, message) });
    }

    private static ApplicationResult<T> Failure(
        ApplicationErrorType errorType,
        IReadOnlyList<ApplicationError> errors)
    {
        if (errors.Count == 0)
        {
            throw new ArgumentException("At least one error is required.", nameof(errors));
        }

        return new ApplicationResult<T>(
            succeeded: false,
            value: default,
            errorType,
            errors);
    }
}
