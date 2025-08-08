using System.Net;

namespace TerraMedia.Application.Dtos;

public class ResponseDto<T> where T : class
{
    public HttpStatusCode StatusCode { get; private set; }
    public string Message { get; private set; }
    public T? Data { get; private set; }

    public ResponseDto(HttpStatusCode statusCode, string message, T? data = null)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
    }

    public static ResponseDto<TResult> Create<TResult>(HttpStatusCode statusCode, string message, TResult data) where TResult : class
    {
        return new ResponseDto<TResult>(statusCode, message, data);
    }
}

public class ResponseDto : ResponseDto<object>
{
    public ResponseDto(HttpStatusCode statusCode, string message) : base(statusCode, message, null) { }

    public static ResponseDto Create(HttpStatusCode statusCode, string message)
    {
        return new ResponseDto(statusCode, message);
    }
}
