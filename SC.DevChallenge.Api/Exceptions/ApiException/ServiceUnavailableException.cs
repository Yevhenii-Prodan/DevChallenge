using System;
using System.Net;

namespace SC.DevChallenge.Api.Exceptions.ApiException
{
    public class ServiceUnavailableException : BaseApiException

    {
        private new const HttpStatusCode StatusCode = HttpStatusCode.ServiceUnavailable;
        private const string Title = "Service is unavailable";

        public ServiceUnavailableException() : base(StatusCode, Title)
        {
        }

        public ServiceUnavailableException(string message) : base(StatusCode, Title, message)
        {
        }

        public ServiceUnavailableException(string message, Exception innerException)
            : base(StatusCode, Title, message, innerException)
        {
        }
    }
}