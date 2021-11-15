using System;
using System.Threading.Tasks;
using SC.DevChallenge.Api.Controllers.RequestModels;
using SC.DevChallenge.Api.Models;

namespace SC.DevChallenge.Api.Services.Abstractions
{
    public interface IPriceService
    {
        Task<AveragePriceResultModel> CalculateAveragePrice(AveragePriceRequestModel model, DateTime dateTime);
    }
}