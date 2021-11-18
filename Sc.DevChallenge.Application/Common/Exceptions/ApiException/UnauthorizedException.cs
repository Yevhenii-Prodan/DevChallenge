using System;
using System.Net;

namespace Sc.DevChallenge.Application.Common.Exceptions.ApiException
{
    /// <summary>
    /// Indicates that user is not authorized
    /// </summary>
    public class UnauthorizedException : BaseApiException
    {
        private static HttpStatusCode _statusCode = HttpStatusCode.Unauthorized;
        private static string _status = "Unauthorized";
        private static string _message = "User is not authorized";

        public UnauthorizedException() : base(_statusCode, _status, _message)
        {
        }

        public UnauthorizedException(string message) : base(_statusCode, _status, message)
        {
        }

        public UnauthorizedException(string message, Exception innerException) : base(_statusCode, _status, message,
            innerException)
        {
        }
    }
}