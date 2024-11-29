

using System.Net;

namespace Core.CrossCuttingConcerns.Responses;

public class ReturnModel<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public List<string>? ErrorMessage { get; set; }

    public static ReturnModel<T> Success(T data, string message, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new ReturnModel<T>()
        {
            Data = data,
            Message = message,
            StatusCode = statusCode
        };
    }
    public static ReturnModel<T> Fail(List<string> errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ReturnModel<T>()
        {
            ErrorMessage = errorMessage,
            StatusCode = statusCode
        };
    }

    public static ReturnModel<T> Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ReturnModel<T>()
        {
            ErrorMessage = [errorMessage],
            StatusCode = statusCode

        };
    }
}

public class ReturnModel
{
    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
    public List<string>? ErrorMessage { get; set; }


    public static ReturnModel Success(string message, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new ReturnModel()
        {
            Message = message,
            StatusCode = statusCode
        };
    }

    public static ReturnModel Fail(List<string> errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ReturnModel()
        {
            ErrorMessage = errorMessage,
            StatusCode = statusCode
        };
    }

    public static ReturnModel Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ReturnModel()
        {
            ErrorMessage = [errorMessage],
            StatusCode = statusCode
        };
    }
}
