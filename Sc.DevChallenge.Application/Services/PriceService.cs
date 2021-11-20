using System;
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

        public async Task<BenchmarkResultModel> CalculateBenchmarkAveragePrice(BenchmarkRequestModel model)
        {
            var timeSlot = _priceCalculator.CalculatePriceTimeSlot(model.DateTimePoint);

            var prices = await _dbContext.Prices.Where(x =>
                x.DateTime >= timeSlot.Value.startPoint &&
                x.DateTime <= timeSlot.Value.endPoint &&
                x.Portfolio == model.Portfolio!.ToLower()).ToListAsync();

            prices = prices.OrderBy(x => x.Price).ToList();

            if (prices.Count == 0)
                throw new NotFoundException("No records with provided parameters were found");

            var quartile1 = _priceCalculator.CalculateQuartile(prices.Count, 1);
            var quartile3 = _priceCalculator.CalculateQuartile(prices.Count, 3);

            var interQuartileInterval = quartile3 - quartile1;

            var bottomInterval = prices[decimal.ToInt32(quartile1)].Price - new decimal(1.5) * interQuartileInterval;
            var topInterval = prices[decimal.ToInt32(quartile3)].Price + new decimal(1.5) * interQuartileInterval;

            var filteredPrices = prices.Where(x => x.Price >= bottomInterval && x.Price <= topInterval).ToList();

            return new BenchmarkResultModel
            {
                Date = timeSlot.Value.startPoint,
                Price = _priceCalculator.CalculateAveragePrice(filteredPrices)
            };

        }
    }
}