using System;
using System.Collections.Generic;
using System.Linq;
using Sc.DevChallenge.Application.Common.Interfaces;
using Sc.DevChallenge.Domain.Entities;
using Sc.DevChallenge.Domain.ValueObjects;

namespace Sc.DevChallenge.Application.Services
{
    public class PriceCalculator : IPriceCalculator
    {
        
        private readonly DateTime _startPointGeneral = DateTime.Parse("2018-01-01 00:00:00");
        private const int TimeIntervalInSec = 10000;
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        public decimal CalculateAveragePrice(IList<PriceEntity> prices) => Math.Round(prices.Average(x => x.Price), 2);
        public PriceDateTimeInterval CalculatePriceTimeInterval(DateTime dateTimePoint)
        {
            var generalStartPointTicks = _startPointGeneral.Ticks;
            var datePointTicks = dateTimePoint.Ticks;
            

            var ticksDifference = datePointTicks - generalStartPointTicks;

            var timeIntervalCount = ticksDifference / (TimeIntervalInSec * TimeSpan.TicksPerSecond);

            var startPointTicks = generalStartPointTicks + timeIntervalCount * (TimeIntervalInSec * TimeSpan.TicksPerSecond);
            
            var startPoint = new DateTime(startPointTicks);
            var endPoint = startPoint.AddSeconds(TimeIntervalInSec);

            return PriceDateTimeInterval.From((startPoint, endPoint));
        }
    }
}