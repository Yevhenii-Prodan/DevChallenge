using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sc.DevChallenge.Application.Common.Exceptions.ApiException;
using Sc.DevChallenge.Application.Common.Interfaces;
using Sc.DevChallenge.Application.Models;
using Sc.DevChallenge.Application.Models.RequestModels;

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

        public async Task<AveragePriceResultModel> CalculateAveragePrice(AveragePriceRequestModel model, DateTime dateTime)
        {
            var timeInterval = _priceCalculator.CalculatePriceTimeInterval(dateTime);

            var query = _dbContext.Prices.Where(x => x.DateTime > timeInterval.Value.startPoint && x.DateTime < timeInterval.Value.endPoint);


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
                Date = timeInterval.Value.startPoint,
                Price = _priceCalculator.CalculateAveragePrice(prices)
            };
            
        }


        private (DateTime, DateTime) GetTimeInterval(DateTime datePoint)
        {
            return default;
            // var startPoint = _startPointGeneral;
            // var endPoint = startPoint.AddSeconds(10000);
            //
            // while (true)
            // {
            //     if (datePoint >= startPoint && datePoint < endPoint)
            //         return (startPoint, endPoint);
            //
            //     startPoint = endPoint;
            //     endPoint = startPoint.AddSeconds(10000);
            // }
        }


    }
}