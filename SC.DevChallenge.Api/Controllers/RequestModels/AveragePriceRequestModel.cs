using System;
using FluentValidation;

namespace SC.DevChallenge.Api.Controllers.RequestModels
{
    public class AveragePriceRequestModel
    {
        public string Portfolio { get; init; }
        public string Owner { get; init; }
        public string Instrument { get; init; }
        public string DateTime { get; set; }
    }

    public class AveragePriceRequestModelValidator : AbstractValidator<AveragePriceRequestModel>
    {
        public AveragePriceRequestModelValidator()
        {
            RuleFor(x => x.Portfolio).NotEmpty();
            RuleFor(x => x.Owner).NotEmpty();
            RuleFor(x => x.Instrument).NotEmpty();
            RuleFor(x => x.DateTime).NotEmpty();
        }
    }

}