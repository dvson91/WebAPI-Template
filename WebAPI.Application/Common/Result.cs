namespace WebAPI.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T? Data { get; private set; }
    public string Message { get; private set; }
    public List<string> Errors { get; private set; }

    public Result()
    {
        Message = string.Empty;
        Errors = new List<string>();
    }

    public static Result<T> Success(T data, string message = MessageConstants.OperationSuccessful)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message,
            Errors = new List<string>()
        };
    }

    public static Result<T> Failure(string message, List<string>? errors = null)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Data = default,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }

    public static Result<T> Failure(string message, string error)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Data = default,
            Message = message,
            Errors = new List<string> { error }
        };
    }
}

public class Result
{
    public bool IsSuccess { get; private set; }
    public string Message { get; private set; }
    public List<string> Errors { get; private set; }

    public Result()
    {
        Message = string.Empty;
        Errors = new List<string>();
    }

    public static Result Success(string message = MessageConstants.OperationSuccessful)
    {
        return new Result
        {
            IsSuccess = true,
            Message = message,
            Errors = new List<string>()
        };
    }

    public static Result Failure(string message, List<string>? errors = null)
    {
        return new Result
        {
            IsSuccess = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }

    public static Result Failure(string message, string error)
    {
        return new Result
        {
            IsSuccess = false,
            Message = message,
            Errors = new List<string> { error }
        };
    }
}