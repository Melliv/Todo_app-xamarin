using System.Net;

namespace Domain.types
{
    public interface IHttpResponse<TData>
    {
        bool Ok { get; set; }
        HttpStatusCode StatusCode { get; set; }
        TData Data { get; set; }
        string Message { get; set; }
    }
}