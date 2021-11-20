using System;
using System.Globalization;
using FluentValidation;
using Newtonsoft.Json;

namespace Sc.DevChallenge.Application.Models.RequestModels
{
    public class BenchmarkRequestModel
    {
        public string? Portfolio { get; set; }
        public string Date { get; init; } = null!;

        [JsonIgnore]
        internal DateTime DateTimePoint
        {
            get
            {
                var parsed = System.DateTime.TryParseExact(Date, "dd/MM/yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime);

                return parsed ? dateTime : default;
            }
        }
    }
    
    
    public class BenchmarkRequestModelModelValidator : AbstractValidator<BenchmarkRequestModel>
    {
        public BenchmarkRequestModelModelValidator()
        {

            RuleFor(x => x.Portfolio).NotEmpty();

            RuleFor(x => x.Date).NotEmpty()
                .Must(x => DateTime.TryParseExact(x, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                .WithMessage("provided datetime does not match 'dd/MM/yyyy HH:mm:ss' format");
        }
    }
}