using System;
using Newtonsoft.Json;
using Sc.DevChallenge.Application.Converters;

namespace Sc.DevChallenge.Application.Models.ResultModels
{
    public class BenchmarkResultModel
    {
        [JsonConverter(typeof(DateFormatConverter),"dd'/'MM'/'yyyy' 'HH:mm:ss")]
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}