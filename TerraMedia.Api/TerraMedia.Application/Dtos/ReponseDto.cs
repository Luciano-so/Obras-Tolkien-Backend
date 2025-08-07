using System.Net;

namespace TerraMedia.Application.Dtos;

public class ReponseDto<T> where T : class
{
    public HttpStatusCode StatusCode { get; private set; }
    public string Message { get; private set; }
    public T? Data { get; private set; }

    public ReponseDto(HttpStatusCode statusCode, string message, T? data = null)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
    }

    public static ReponseDto<TResult> Create<TResult>(HttpStatusCode statusCode, string message, TResult data) where TResult : class
    {
        return new ReponseDto<TResult>(statusCode, message, data);
    }
}

public class ReponseDto : ReponseDto<object>
{
    public ReponseDto(HttpStatusCode statusCode, string message) : base(statusCode, message, null) { }

    public static ReponseDto Create(HttpStatusCode statusCode, string message)
    {
        return new ReponseDto(statusCode, message);
    }
}
