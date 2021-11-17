using System;
using System.Net;
using System.Runtime.Serialization;

namespace SC.DevChallenge.Api.Exceptions.ApiException
{
    public abstract class BaseApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Status { get; set; }

        protected BaseApiException(HttpStatusCode statusCode, string status)
        {
            StatusCode = statusCode;
            Status = status;
        }

        protected BaseApiException(HttpStatusCode statusCode, string status, string message) : base(message)
        {
            StatusCode = statusCode;
            Status = status;
        }

        protected BaseApiException(HttpStatusCode statusCode, string status, string message, Exception innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
            Status = status;
        }

        protected BaseApiException(HttpStatusCode statusCode, string status, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            StatusCode = statusCode;
            Status = status;
        }
    }
}