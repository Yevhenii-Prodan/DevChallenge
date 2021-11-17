using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentValidation.Results;

namespace SC.DevChallenge.Api.Exceptions.ApiException
{
    /// <summary>
    /// Indicates that one or more validation failures have occurred
    /// </summary>
    public class BadRequestException : BaseApiException
    {
        private static HttpStatusCode _statusCode = HttpStatusCode.BadRequest;
        private static string _status = "Bad request";
        private static string _message = "One or more validation failures have occurred";

        public BadRequestException() : base(_statusCode, _status, _message)
        {
            Failures = new Dictionary<string, string[]>();
        }
        

        public BadRequestException(string message) : base(_statusCode, _status, message)
        {
            _message = message;
        }
        
        public BadRequestException(List<ValidationFailure> failures)
            : this()
        {
            var propertyNames = failures
                .Select(e => e.PropertyName)
                .Distinct();

            foreach (var propertyName in propertyNames)
            {
                var propertyFailures = failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                Failures.Add(propertyName, propertyFailures);
            }
        }

        public IDictionary<string, string[]> Failures { get; }
    }
}