using System;
using System.Globalization;
using FluentValidation;
using Newtonsoft.Json;

namespace Sc.DevChallenge.Application.Models.RequestModels
{
    public class AveragePriceRequestModel
    {
        public string? Portfolio { get; init; }
        public string? Owner { get; init; }
        public string? Instrument { get; init; }
        
        public string DateTime { get; init; } = null!;

        [JsonIgnore]
        public DateTime DateTimePoint
        {
            get
            {
                var parsed = System.DateTime.TryParseExact(DateTime, "dd/MM/yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime);

                return parsed ? dateTime : default;
            }
        }
    }

    public class AveragePriceRequestModelValidator : AbstractValidator<AveragePriceRequestModel>
    {
        public AveragePriceRequestModelValidator()
        {

            RuleFor(x => x).Must(x =>
                !(string.IsNullOrWhiteSpace(x.Instrument) &&
                  string.IsNullOrWhiteSpace(x.Portfolio) &&
                  string.IsNullOrWhiteSpace(x.Owner)))
                .WithName("Search criteria")
                .WithMessage("At least one of Instrument, Portfolio, Owner should be provided");

            RuleFor(x => x.DateTime).NotEmpty()
                .Must(x => DateTime.TryParseExact(x, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                .WithMessage("provided datetime does not match 'dd/MM/yyyy HH:mm:ss' format");
        }
    }
}