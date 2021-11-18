using System;
using System.Net;

namespace Sc.DevChallenge.Application.Common.Exceptions.ApiException
{
    /// <summary>
    ///     Access denied
    /// </summary>
    public class ForbiddenException : BaseApiException
    {
        private static HttpStatusCode _statusCode = HttpStatusCode.Forbidden;
        private static string _status = "Forbidden";
        private static string _message = "Access denied";

        public ForbiddenException() : base(_statusCode, _status, _message)
        {
        }

        public ForbiddenException(string message) : base(_statusCode, _status, message)
        {
        }

        public ForbiddenException(string message, Exception innerException) : base(_statusCode, _status, message, innerException)
        {
        }
    }
}