using System;
using FluentValidation;

namespace SC.DevChallenge.Api.Controllers.RequestModels
{
    public class AveragePriceRequestModel
    {
        public string Portfolio { get; init; }
        public string Owner { get; init; }
        public string Instrument { get; init; }
        public string DateTime { get; init; }
    }

    public class AveragePriceRequestModelValidator : AbstractValidator<AveragePriceRequestModel>
    {
        public AveragePriceRequestModelValidator()
        {

            RuleFor(x => x).Must(x =>
                !(string.IsNullOrWhiteSpace(x.Instrument) &&
                  string.IsNullOrWhiteSpace(x.Portfolio) &&
                  string.IsNullOrWhiteSpace(x.Owner)))
                .WithMessage("At least one of Instrument, Portfolio, Owner should be provided");
            
        }
    }

}