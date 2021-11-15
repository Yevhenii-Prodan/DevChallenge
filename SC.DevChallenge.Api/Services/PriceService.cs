using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SC.DevChallenge.Api.Controllers.RequestModels;
using SC.DevChallenge.Api.Database;
using SC.DevChallenge.Api.Database.Entities;
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

        public async Task<AveragePriceResultModel> CalculateAveragePrice(AveragePriceRequestModel model)
        {
            if (model.DateTime < startPointGeneral)
            {
                throw new Exception();
            }
            
            var (startTimeInterval, endTimeInterval) = GetTimeInterval(model.DateTime);

            var prices = await _dbContext.Prices.Where(x =>
                x.Instrument.ToLower() == model.Instrument.ToLower() &&
                x.Owner.ToLower() == model.Owner.ToLower() &&
                x.Portfolio.ToLower() == model.Portfolio.ToLower() &&
                x.DateTime > startTimeInterval &&
                x.DateTime < endTimeInterval
            ).ToListAsync();

            if (prices.Count == 0)
                throw new Exception();
            
            
 
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