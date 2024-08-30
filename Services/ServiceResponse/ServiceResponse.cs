using System.Net;

namespace Todo_List_API.Services
{
    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public HttpStatusCode HttpResponse { get; set; }
        public object? Content { get; set; }

        public static ServiceResponse Factory(bool success, string message, HttpStatusCode httpResponse, object? content)
        {
            return new ServiceResponse
            {
                Success = success,
                Message = message,
                HttpResponse = httpResponse,
                Content = content
            };
        }
    }
}
