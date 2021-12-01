using System.Net;
using Domain.types;

namespace Domain
{
    public class HttpResponse<TData>: IHttpResponse<TData>
    {
        public bool Ok { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public TData Data { get; set; }
        public string Message { get; set; }
    }
}