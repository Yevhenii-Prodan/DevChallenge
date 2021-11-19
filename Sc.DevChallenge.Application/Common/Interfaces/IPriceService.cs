using System;
using System.Threading.Tasks;
using Sc.DevChallenge.Application.Models;
using Sc.DevChallenge.Application.Models.RequestModels;

namespace Sc.DevChallenge.Application.Common.Interfaces
{
    public interface IPriceService
    {
        Task<AveragePriceResultModel> CalculateAveragePrice(AveragePriceRequestModel model);
    }
}