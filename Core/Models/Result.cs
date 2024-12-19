namespace Core.Models;

public class Result<T> : Result
{
    public T Data { get; set; }

    public Result(bool success, T data, string[] errors) : base(success, errors)
    {
        Success = success;
        Data = data;
    }

    public static Result<T> Ok(T data) => new(true, data, []);
    public new static Result<T> Fail(string[] errors) => new(false, default!, errors);
}


public class Result(bool success, string[] errors)
{
    public bool Success { get; set; } = success;
    public string[] Errors { get; set; } = errors;

    public static Result Ok() => new(true, []);
    public static Result Fail(string[] errors) => new(false, errors);
}

