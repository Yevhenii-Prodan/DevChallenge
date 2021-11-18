using System;
using System.Collections.Generic;
using System.Linq;
using Sc.DevChallenge.Application.Services.Abstractions;
using Sc.DevChallenge.Domain.Entities;

namespace Sc.DevChallenge.Application.Services
{
    public class PriceCalculator : IPriceCalculator
    {
        public decimal CalculateAveragePrice(IList<PriceEntity> prices) => Math.Round(prices.Average(x => x.Price), 2);
    }
}