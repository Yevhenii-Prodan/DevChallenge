using System;
using System.Net;

namespace SC.DevChallenge.Api.Exceptions.ApiException
{
    /// <summary>
    ///     Indicates that the problem is on our side
    /// </summary>
    public class ServerErrorException : BaseApiException
    {
        private static HttpStatusCode _statusCode = HttpStatusCode.InternalServerError;
        private static string _status = "Internal error";
        private static string _message = "The problem is on our side";

        public ServerErrorException() : base(_statusCode, _status, _message)
        {
        }

        public ServerErrorException(string message) : base(_statusCode, _status, message)
        {
        }

        public ServerErrorException(string message, Exception innerException) : base(_statusCode, _status, message,
            innerException)
        {
        }
    }
}