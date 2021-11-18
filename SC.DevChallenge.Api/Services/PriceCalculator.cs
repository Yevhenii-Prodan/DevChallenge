using System;
using System.Collections.Generic;
using System.Linq;
using SC.DevChallenge.Api.Database.Entities;
using SC.DevChallenge.Api.Services.Abstractions;

namespace SC.DevChallenge.Api.Services
{
    public class PriceCalculator : IPriceCalculator
    {
        public decimal CalculateAveragePrice(IList<PriceEntity> prices) => Math.Round(prices.Average(x => x.Price), 2);
    }
}