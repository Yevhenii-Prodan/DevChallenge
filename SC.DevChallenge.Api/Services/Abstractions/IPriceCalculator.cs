using System.Collections.Generic;
using SC.DevChallenge.Api.Database.Entities;

namespace SC.DevChallenge.Api.Services.Abstractions
{
    public interface IPriceCalculator
    {
        decimal CalculateAveragePrice(IList<PriceEntity> prices);
    }
}