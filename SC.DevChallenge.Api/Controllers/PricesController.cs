using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public PricesController(IPriceService priceService)
        {
            _priceService = priceService;
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
        public async Task<IActionResult> Benchmark([FromQuery] BenchmarkRequestModel model)
        {
            var result = await _priceService.CalculateBenchmarkAveragePrice(model);
            return Ok(result);
        }
        
    }
}
