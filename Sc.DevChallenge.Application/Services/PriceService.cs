using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sc.DevChallenge.Application.Common.Exceptions.ApiException;
using Sc.DevChallenge.Application.Common.Interfaces;
using Sc.DevChallenge.Application.Models;
using Sc.DevChallenge.Application.Models.RequestModels;
using Sc.DevChallenge.Application.Services.Abstractions;

namespace Sc.DevChallenge.Application.Services
{
    public class PriceService : IPriceService
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IPriceCalculator _priceCalculator;
        private readonly DateTime _startPointGeneral = DateTime.Parse("2018-01-01 00:00:00");
        private const int TimeIntervalInSec = 10000;
        


        public PriceService(IApplicationDbContext dbContext, IPriceCalculator priceCalculator)
        {
            _dbContext = dbContext;
            _priceCalculator = priceCalculator;
        }

        public async Task<AveragePriceResultModel> CalculateAveragePrice(AveragePriceRequestModel model, DateTime dateTime)
        {
            if (dateTime < _startPointGeneral)
            {
                throw new BadRequestException("Passed datetime is less than the start point");
            }
            
            var (startTimeInterval, endTimeInterval) = GetTimeInterval(dateTime);
            var (startTimeIntervalV2, endTimeIntervalV2) = GetTimeIntervalV2(dateTime);

            var query = _dbContext.Prices.Where(x => x.DateTime > startTimeInterval && x.DateTime < endTimeInterval);


            if (!string.IsNullOrWhiteSpace(model.Instrument))
                query = query.Where(x => x.Instrument!.ToLower() == model.Instrument.ToLower());
            
            if (!string.IsNullOrWhiteSpace(model.Owner))
                query = query.Where(x => x.Owner!.ToLower() == model.Owner.ToLower());

            if (!string.IsNullOrWhiteSpace(model.Portfolio))
                query = query.Where(x => x.Portfolio!.ToLower() == model.Portfolio.ToLower());
            

            var prices = await query.ToListAsync();
            if (prices.Count == 0)
                throw new NotFoundException("No records with provided parameters were found");
            
            
 
            return new AveragePriceResultModel
            {
                Date = startTimeInterval,
                Price = _priceCalculator.CalculateAveragePrice(prices)
            };
            
        }


        private (DateTime, DateTime) GetTimeInterval(DateTime datePoint)
        {
            var startPoint = _startPointGeneral;
            var endPoint = startPoint.AddSeconds(10000);
            
            while (true)
            {
                if (datePoint >= startPoint && datePoint < endPoint)
                    return (startPoint, endPoint);

                startPoint = endPoint;
                endPoint = startPoint.AddSeconds(10000);
            }
        }


        private (DateTime, DateTime) GetTimeIntervalV2(DateTime datePoint)
        {
            var generalStartPointTicks = _startPointGeneral.Ticks;
            var datePointTicks = datePoint.Ticks;
            

            var ticksDifference = datePointTicks - generalStartPointTicks;

            var timeIntervalCount = ticksDifference / (TimeIntervalInSec * TimeSpan.TicksPerSecond);

            var startPointTicks = generalStartPointTicks + timeIntervalCount * (TimeIntervalInSec * TimeSpan.TicksPerSecond);
            
            var startPoint = new DateTime(startPointTicks);
            var endPoint = startPoint.AddSeconds(TimeIntervalInSec);

            return (startPoint, endPoint);
        }
    }
}