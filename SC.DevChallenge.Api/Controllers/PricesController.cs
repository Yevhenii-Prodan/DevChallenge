using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sc.DevChallenge.Application.Common.Interfaces;
using Sc.DevChallenge.Application.Models;
using Sc.DevChallenge.Application.Models.RequestModels;

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
        public async Task<IActionResult> Average([FromQuery]AveragePriceRequestModel model)
        {
            var result = await _priceService.CalculateAveragePrice(model, model.DateTimePoint);
            return Ok(result);
        }
    }
}
