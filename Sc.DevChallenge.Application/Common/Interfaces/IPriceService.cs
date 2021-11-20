using System;
using System.Threading.Tasks;
using Sc.DevChallenge.Application.Models;
using Sc.DevChallenge.Application.Models.RequestModels;
using Sc.DevChallenge.Application.Models.ResultModels;

namespace Sc.DevChallenge.Application.Common.Interfaces
{
    public interface IPriceService
    {
        Task<AveragePriceResultModel> CalculateAveragePrice(AveragePriceRequestModel model);
        Task<BenchmarkResultModel> CalculateBenchmarkAveragePrice(BenchmarkRequestModel model);
    }
}