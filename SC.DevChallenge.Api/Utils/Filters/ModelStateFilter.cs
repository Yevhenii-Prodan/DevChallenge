using System.Linq;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using SC.DevChallenge.Api.Exceptions.ApiException;

namespace SC.DevChallenge.Api.Utils.Filters
{
    public class ModelStateFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var failures = context.ModelState.Keys
                    .SelectMany(key => context.ModelState[key].Errors.Select(x => new ValidationFailure(key, x.ErrorMessage)))
                    .ToList();
                
                throw new BadRequestException(failures);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}