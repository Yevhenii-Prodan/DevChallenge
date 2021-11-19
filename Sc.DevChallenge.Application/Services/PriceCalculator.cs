using System;
using System.Collections.Generic;
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
        
            var timeIntervalCount = ticksDifference / ((_timeIntervalInSec+1) * TimeSpan.TicksPerSecond);
        
            var startPointTicks = generalStartPointTicks + timeIntervalCount * (_timeIntervalInSec * TimeSpan.TicksPerSecond) + (
                (timeIntervalCount) * TimeSpan.TicksPerSecond);
            
            var startPoint = new DateTime(startPointTicks);
            var endPoint = startPoint.AddSeconds(_timeIntervalInSec);
        
            return PriceTimeSlot.From((startPoint, endPoint));
        }
        
    }
}