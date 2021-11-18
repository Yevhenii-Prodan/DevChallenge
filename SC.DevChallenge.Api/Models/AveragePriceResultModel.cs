using System;
using Newtonsoft.Json;
using SC.DevChallenge.Api.Utils.Converters;

namespace SC.DevChallenge.Api.Models
{
    public class AveragePriceResultModel
    {
        [JsonConverter(typeof(DateFormatConverter),"dd'/'MM'/'yyyy' 'HH:mm:ss")]
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}