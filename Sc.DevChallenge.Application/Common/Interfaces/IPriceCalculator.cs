using System;
using System.Collections.Generic;
using Sc.DevChallenge.Domain.Entities;
using Sc.DevChallenge.Domain.ValueObjects;

namespace Sc.DevChallenge.Application.Common.Interfaces
{
    public interface IPriceCalculator
    {
        /// <summary>
        /// Calculates an average price
        /// </summary>
        /// <param name="prices">List of price entities</param>
        /// <returns>An average price in decimal with 2 numbers after comma</returns>
        decimal CalculateAveragePrice(IList<PriceEntity> prices);
        
        /// <summary>
        /// Calculates a time interval for price records 
        /// </summary>
        /// <param name="dateTimePoint">DateTime point</param>
        /// <returns>A time interval</returns>
        PriceTimeSlot CalculatePriceTimeSlot(DateTime dateTimePoint);

        /// <summary>
        /// Calculates a quartile
        /// </summary>
        /// <param name="n">Count of elements</param>
        /// <param name="quartile">Number of quartile must be 1-4</param>
        /// <returns>A quartile value</returns>
        decimal CalculateQuartile(int n, int quartile);
    }
}