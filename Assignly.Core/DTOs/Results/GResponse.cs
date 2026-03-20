namespace Assignly.Core.DTOs.Results;

public class Result<T> : Result
{
    public T Data { get; }
    public int StatusCode { get; set; }

    protected Result(T data, bool isSuccess, string error, int statusCode)
        : base(isSuccess, error, statusCode)
    {
        Data = data;
        StatusCode = statusCode;
    }

    public static Result<T> Success(T data)
    {
        return new Result<T>(data, true, null, 200);
    }

    public static new Result<T> Failure(string error, int statusCode)
    {
        return new Result<T>(default, false, error, statusCode);
    }
}
