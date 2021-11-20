using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sc.DevChallenge.Application.Common.Exceptions.ApiException;
using Sc.DevChallenge.Application.Common.Interfaces;
using Sc.DevChallenge.Application.Models.RequestModels;
using Sc.DevChallenge.Application.Models.ResultModels;

namespace Sc.DevChallenge.Application.Services
{
    public class PriceService : IPriceService
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IPriceCalculator _priceCalculator;
        
        
        public PriceService(IApplicationDbContext dbContext, IPriceCalculator priceCalculator)
        {
            _dbContext = dbContext;
            _priceCalculator = priceCalculator;
        }

        /// <summary>
        /// Calculates average price
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<AveragePriceResultModel> CalculateAveragePrice(AveragePriceRequestModel model)
        {
            var timeSlot = _priceCalculator.CalculatePriceTimeSlot(model.DateTimePoint);

            var query = _dbContext.Prices.Where(x => x.DateTime >= timeSlot.Value.startPoint && x.DateTime <= timeSlot.Value.endPoint);


            if (!string.IsNullOrWhiteSpace(model.Instrument))
                query = query.Where(x => x.Instrument! == model.Instrument.ToLower());
            
            if (!string.IsNullOrWhiteSpace(model.Owner))
                query = query.Where(x => x.Owner! == model.Owner.ToLower());

            if (!string.IsNullOrWhiteSpace(model.Portfolio))
                query = query.Where(x => x.Portfolio! == model.Portfolio.ToLower());
            

            var prices = await query.ToListAsync();
            if (prices.Count == 0)
                throw new NotFoundException("No records with provided parameters were found");
            
            
 
            return new AveragePriceResultModel
            {
                Date = timeSlot.Value.startPoint,
                Price = _priceCalculator.CalculateAveragePrice(prices)
            };
            
        }

        /// <summary>
        /// Calculates benchmark average price
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<BenchmarkResultModel> CalculateBenchmarkAveragePrice(BenchmarkRequestModel model)
        {
            var timeSlot = _priceCalculator.CalculatePriceTimeSlot(model.DateTimePoint);

            var prices = await _dbContext.Prices.Where(x =>
                x.DateTime >= timeSlot.Value.startPoint &&
                x.DateTime <= timeSlot.Value.endPoint &&
                x.Portfolio == model.Portfolio!.ToLower()).ToListAsync();
            
            if (prices.Count == 0)
                throw new NotFoundException("No records with provided parameters were found");

            return new BenchmarkResultModel
            {
                Date = timeSlot.Value.startPoint,
                Price = _priceCalculator.CalculateBenchmarkPrice(prices)
            };

        }
        
        
        /// <summary>
        /// Calculates aggregated benchmark average price
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AggregatedBenchmarkResultModel>> CalculateAggregatedBenchmarkAveragePrice(AggregatedBenchmarkRequestModel model)
        {
            var firstTimeSlot = _priceCalculator.CalculatePriceTimeSlot(model.StartDatePoint);
            var lastTimeSlot = _priceCalculator.CalculatePriceTimeSlot(model.EndDatePoint);
            
            var intervalCount = _priceCalculator.CalculateIntervalCount(firstTimeSlot, lastTimeSlot);

            var priceSlotCount = intervalCount / model.Intervals;
            var rest = intervalCount % model.Intervals;


            var result = new List<AggregatedBenchmarkResultModel>();
            
            
            var startSlot = firstTimeSlot; 

            for (int i = 0; i < model.Intervals; i++)
            {
                var slotCountForCurrentInterval = priceSlotCount;
                if (i < rest)
                    slotCountForCurrentInterval++;


                var endSlot = _priceCalculator.CalculateEndSlot(startSlot, slotCountForCurrentInterval -1);

                var startDate = startSlot.Value.startPoint;
                var endDate = endSlot.Value.endPoint;
                
                
                var prices = await _dbContext.Prices.Where(x =>
                    x.DateTime >= startDate &&
                    x.DateTime <= endDate &&
                    x.Portfolio == model.Portfolio!.ToLower()).ToListAsync();

                var intervalBenchmarks = new List<decimal>();

                var timePoint = startSlot;
                for (int j = 0; j < slotCountForCurrentInterval; j++)
                {
                    var pricesForInterval = prices.Where(x =>
                        x.DateTime > timePoint.Value.startPoint &&
                        x.DateTime < timePoint.Value.endPoint)
                        .ToList();
                    intervalBenchmarks.Add(_priceCalculator.CalculateBenchmarkPrice(pricesForInterval));

                    timePoint = timePoint.Next();
                }
                
                result.Add(new()
                {
                    Date = endSlot.Value.startPoint,
                    Price = Math.Round(intervalBenchmarks.Where(x => x > 0).Average(), 2)
                });
                startSlot = endSlot.Next();
            }

            return result;
        }
    }
}