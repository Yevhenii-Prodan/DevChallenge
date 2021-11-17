using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SC.DevChallenge.Api.Controllers.RequestModels;
using SC.DevChallenge.Api.Exceptions.ApiException;
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
            // var parsed = DateTime.TryParseExact(model.DateTime, "dd/MM/yyyy HH:mm:ss",CultureInfo.InvariantCulture,DateTimeStyles.None,  out var dateTime);
            //
            // if (!parsed)
            //     throw new BadRequestException("Wrong datetime format");

            var result = await _priceService.CalculateAveragePrice(model, model.DateTimePoint);
            return Ok(result);
        }
    }
}
