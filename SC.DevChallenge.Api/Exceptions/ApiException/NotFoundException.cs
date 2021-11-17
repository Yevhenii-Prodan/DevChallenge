using System;
using System.Net;

namespace SC.DevChallenge.Api.Exceptions.ApiException
{
    /// <summary>
    ///     Indicates that requested resource was not found
    /// </summary>
    public class NotFoundException : BaseApiException
    {
        private static HttpStatusCode _statusCode = HttpStatusCode.NotFound;
        private static string _status = "Not found";
        private static string _message = "Requested resource was not found";

        public NotFoundException() : base(_statusCode, _status, _message)
        {
        }

        public NotFoundException(string message) : base(_statusCode, _status, message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(_statusCode, _status, message,
            innerException)
        {
        }
    }
}