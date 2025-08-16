using System.Net;

namespace Swim_Feedback.Models
{
    public class ResponseMessage<T>
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ResponseMessage(HttpStatusCode status, string message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        public bool IsEqual(ResponseMessage<T> other)
        {
            return Status == other.Status &&
                Message == other.Message;
        }
    }
}
