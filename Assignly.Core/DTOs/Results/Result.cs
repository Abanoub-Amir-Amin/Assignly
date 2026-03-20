namespace Assignly.Core.DTOs.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }
    public int StatusCode { get; set; }

    protected Result(bool isSuccess, string error, int statusCode)
    {
        if (isSuccess && error != null)
            throw new InvalidOperationException();

        if (!isSuccess && error == null)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
        StatusCode = statusCode;
    }

    public static Result Success()
    {
        return new Result(true, null, 200);
    }

    public static Result Failure(string error, int statusCode)
    {
        return new Result(false, error, statusCode);
    }
}
