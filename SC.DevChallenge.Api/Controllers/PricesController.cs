using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SC.DevChallenge.Api.Controllers.RequestModels;
using SC.DevChallenge.Api.Models;
using SC.DevChallenge.Api.Services.Abstractions;

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

            var validationResult = await new AveragePriceRequestModelValidator().ValidateAsync(model);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);


            // TODO: replace with middleware
            try
            {
                var result = await _priceService.CalculateAveragePrice(model);
                return Ok(result);
            }
            catch
            {
                return NotFound();
            }


        }
    }
}
