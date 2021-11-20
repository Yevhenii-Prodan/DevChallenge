using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Sc.DevChallenge.Application.Common;
using Sc.DevChallenge.Application.Common.Exceptions.ApiException;
using Sc.DevChallenge.Application.Common.Interfaces;
using Sc.DevChallenge.Domain.Entities;
using Sc.DevChallenge.Domain.ValueObjects;

namespace Sc.DevChallenge.Application.Services
{
    public class PriceCalculator : IPriceCalculator
    {
        
        private readonly DateTime _startPointGeneral;
        private readonly int _timeIntervalInSec;

        public PriceCalculator(ApplicationSettings settings)
        {
            _startPointGeneral = settings.GeneralStartPoint;
            _timeIntervalInSec = settings.TimeIntervalInSec;
        }
        
        
        /// <inheritdoc/>
        public decimal CalculateAveragePrice(IList<PriceEntity> prices) => Math.Round(prices.Average(x => x.Price), 2);
        
        /// <inheritdoc/>
        public PriceTimeSlot CalculatePriceTimeSlot(DateTime dateTimePoint)
        {
            if (dateTimePoint < _startPointGeneral)
            {
                throw new BadRequestException("Passed datetime is less than the start point");
            }
            
            var generalStartPointTicks = _startPointGeneral.Ticks;
            var datePointTicks = dateTimePoint.Ticks;
            

            var ticksDifference = datePointTicks - generalStartPointTicks;

            var timeIntervalCount = ticksDifference / ((_timeIntervalInSec) * TimeSpan.TicksPerSecond);

            var startPointTicks = generalStartPointTicks + timeIntervalCount * (_timeIntervalInSec * TimeSpan.TicksPerSecond);
            
            var startPoint = new DateTime(startPointTicks);
            var endPoint = startPoint.AddSeconds(_timeIntervalInSec - 1);
        
            return PriceTimeSlot.From((startPoint, endPoint));
        }
        
        /// <inheritdoc/>
        public int CalculateQuartile(int n, int quartile)
        {
            if (quartile is < 1 or > 4)
                throw new InvalidEnumArgumentException("Quartile value must be between 1 and 4");

            return decimal.ToInt32(Math.Ceiling((quartile * n - quartile) / new decimal(4)));
        }

        /// <inheritdoc/>
        public decimal CalculateBenchmarkPrice(List<PriceEntity> prices)
        {
            if (prices.Count == 0)
                return 0;

            if (prices.Count <= 3)
            {
                return CalculateAveragePrice(prices);
            }
            
            prices = prices.OrderBy(x => x.Price).ToList();
            var quartile1 = CalculateQuartile(prices.Count, 1);
            var quartile3 = CalculateQuartile(prices.Count, 3);

            var interQuartileInterval = prices[quartile3].Price - prices[quartile1].Price;

            var lowerBound = prices[quartile1].Price - new decimal(1.5) * interQuartileInterval;
            var upperBound = prices[quartile3].Price + new decimal(1.5) * interQuartileInterval;
            

            var filteredPrices =
                prices.Where(x => x.Price > lowerBound && x.Price < upperBound).ToList();

            return CalculateAveragePrice(filteredPrices);
        }
        
        /// <inheritdoc/>
        public int CalculateIntervalCount(PriceTimeSlot firstTimeSlot, PriceTimeSlot lastTimeSlot)
        {
            var intervalCount = 0;

            var interval = firstTimeSlot.Value.startPoint;

            while (true)
            {
                if (interval > lastTimeSlot.Value.startPoint)
                    break;

                intervalCount++;
                interval = interval.AddSeconds(10000);
            }

            return intervalCount;
        }
        
        /// <inheritdoc/>
        public PriceTimeSlot CalculateEndSlot(PriceTimeSlot startSlot, int offset)
        {
            return PriceTimeSlot.From(
                (startSlot.Value.startPoint.AddSeconds(offset * _timeIntervalInSec),
                startSlot.Value.endPoint.AddSeconds(offset * _timeIntervalInSec)));
        }
    }
}