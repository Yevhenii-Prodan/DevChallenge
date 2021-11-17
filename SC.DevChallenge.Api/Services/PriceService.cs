using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SC.DevChallenge.Api.Controllers.RequestModels;
using SC.DevChallenge.Api.Database;
using SC.DevChallenge.Api.Exceptions.ApiException;
using SC.DevChallenge.Api.Models;
using SC.DevChallenge.Api.Services.Abstractions;

namespace SC.DevChallenge.Api.Services
{
    public class PriceService : IPriceService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DateTime startPointGeneral = DateTime.Parse("2018-01-01 00:00:00");


        public PriceService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AveragePriceResultModel> CalculateAveragePrice(AveragePriceRequestModel model, DateTime dateTime)
        {
            if (dateTime < startPointGeneral)
            {
                throw new BadRequestException("Passed datetime is less than the start point");
            }

            if (string.IsNullOrWhiteSpace(model.Instrument) && string.IsNullOrWhiteSpace(model.Owner) &&
                string.IsNullOrWhiteSpace(model.Portfolio))
                throw new Exception();
            
            var (startTimeInterval, endTimeInterval) = GetTimeInterval(dateTime);

            var query = _dbContext.Prices.Where(x => x.DateTime > startTimeInterval && x.DateTime < endTimeInterval);


            if (!string.IsNullOrWhiteSpace(model.Instrument))
                query = query.Where(x => x.Instrument.ToLower() == model.Instrument.ToLower());
            
            if (!string.IsNullOrWhiteSpace(model.Owner))
                query = query.Where(x => x.Owner.ToLower() == model.Owner.ToLower());

            if (!string.IsNullOrWhiteSpace(model.Portfolio))
                query = query.Where(x => x.Portfolio.ToLower() == model.Portfolio.ToLower());
            

            var prices = await query.ToListAsync();

            if (prices.Count == 0)
                throw new NotFoundException("No records with provided parameters were found");
            
            
 
            return new AveragePriceResultModel
            {
                Date = startTimeInterval.ToString("dd/MM/yyyy HH:mm:ss"),
                Price = Math.Round(prices.Average(x => x.Price), 2)
            };
            
        }


        private (DateTime, DateTime) GetTimeInterval(DateTime datePoint)
        {
            var startPoint = startPointGeneral;
            var endPoint = startPoint.AddSeconds(10000);
            
            while (true)
            {
                if (datePoint >= startPoint && datePoint < endPoint)
                    return (startPoint, endPoint);

                startPoint = endPoint;
                endPoint = startPoint.AddSeconds(10000);
            }
            
            
        }
    }
}