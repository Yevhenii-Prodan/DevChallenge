using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sc.DevChallenge.Application.Common.Interfaces;
using Sc.DevChallenge.Application.Models;
using Sc.DevChallenge.Application.Models.RequestModels;
using Sc.DevChallenge.Application.Models.ResultModels;

namespace SC.DevChallenge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PricesController : ControllerBase
    {

        private readonly IPriceService _priceService;
        private readonly IApplicationDbContext _dbContext;
        private readonly IPriceCalculator _priceCalculator;

        public PricesController(IPriceService priceService, IApplicationDbContext dbContext, IPriceCalculator priceCalculator)
        {
            _priceService = priceService;
            _dbContext = dbContext;
            _priceCalculator = priceCalculator;
        }


        [HttpGet("average")]
        [ProducesResponseType(typeof(AveragePriceResultModel),200)]
        [ProducesResponseType(typeof(BaseErrorModel),400)]
        [ProducesResponseType(typeof(BaseErrorModel),404)]
        [ProducesResponseType(typeof(BaseErrorModel),500)]
        public async Task<IActionResult> Average([FromQuery]AveragePriceRequestModel model)
        {
            var result = await _priceService.CalculateAveragePrice(model);
            return Ok(result);
        }


        [HttpGet("benchmark")]
        [ProducesResponseType(typeof(BenchmarkResultModel),200)]
        [ProducesResponseType(typeof(BaseErrorModel),400)]
        [ProducesResponseType(typeof(BaseErrorModel),404)]
        [ProducesResponseType(typeof(BaseErrorModel),500)]
        public async Task<IActionResult> Benchmark([FromQuery] BenchmarkRequestModel model)
        {
            var result = await _priceService.CalculateBenchmarkAveragePrice(model);
            return Ok(result);
        }

        [HttpGet("aggregate")]
        [ProducesResponseType(typeof(AggregatedBenchmarkResultModel),200)]
        [ProducesResponseType(typeof(BaseErrorModel),400)]
        [ProducesResponseType(typeof(BaseErrorModel),404)]
        [ProducesResponseType(typeof(BaseErrorModel),500)]
        public async Task<IActionResult> AggregatedBenchmark([FromQuery] AggregatedBenchmarkRequestModel model)
        {
            var result = await _priceService.CalculateAggregatedBenchmarkAveragePrice(model);
            return Ok(result);
        }
    }
}
