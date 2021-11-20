using System;
using System.Globalization;
using FluentValidation;
using Newtonsoft.Json;

namespace Sc.DevChallenge.Application.Models.RequestModels
{
    public class AggregatedBenchmarkRequestModel
    {
        public string? Portfolio { get; init; }
        public int Intervals { get; set; }
        
        public string StartDate { get; init; } = null!;

        [JsonIgnore]
        internal DateTime StartDatePoint
        {
            get
            {
                var parsed = DateTime.TryParseExact(StartDate, "dd/MM/yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime);

                return parsed ? dateTime : default;
            }
        }
        
        public string EndDate { get; init; } = null!;

        [JsonIgnore]
        internal DateTime EndDatePoint
        {
            get
            {
                var parsed = DateTime.TryParseExact(EndDate, "dd/MM/yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime);

                return parsed ? dateTime : default;
            }
        }
        
    }
    
    
    public class AggregatedBenchmarkRequestModelValidator : AbstractValidator<AggregatedBenchmarkRequestModel>
    {
        public AggregatedBenchmarkRequestModelValidator()
        {

            RuleFor(x => x.Portfolio).NotEmpty();
            RuleFor(x => x.Intervals).NotEmpty().GreaterThan(0);

            RuleFor(x => x.StartDate).NotEmpty()
                .Must(x => DateTime.TryParseExact(x, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                .WithMessage("Provided datetime does not match 'dd/MM/yyyy HH:mm:ss' format");

            RuleFor(x => x.EndDate).NotEmpty()
                .Must(x => DateTime.TryParseExact(x, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                .WithMessage("Provided datetime does not match 'dd/MM/yyyy HH:mm:ss' format");
        }
    }
}