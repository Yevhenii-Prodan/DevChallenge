using System.Collections.Generic;
using Sc.DevChallenge.Domain.Entities;

namespace Sc.DevChallenge.Application.Services.Abstractions
{
    public interface IPriceCalculator
    {
        decimal CalculateAveragePrice(IList<PriceEntity> prices);
    }
}