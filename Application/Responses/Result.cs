
using System.Text.Json.Serialization;

namespace Application.Responses;

public class Result<T>
{
    public bool Success { get; private set; }
    public T? Data { get; private set; }
    public string? Error { get; private set; }
    public int StatusCode { get; private set; }

    [JsonConstructor]
    private Result(bool success, T? data, string? error, int statusCode)
    {
        Success = success;
        Data = data;
        Error = error;
        StatusCode = statusCode;
    }

    public static Result<T> Ok(T data, int statusCode = 200)
        => new(true, data, null, statusCode);

    public static Result<T> Create(T data, int statusCode = 201)
        => new(true, data, null, statusCode);

    public static Result<T> Fail(string error, int statusCode = 400)
        => new(false, default, error, statusCode);    

    public static Result<T> InternalError(string error = "Internal Server Error")
        => new(false, default, error, 500);
}