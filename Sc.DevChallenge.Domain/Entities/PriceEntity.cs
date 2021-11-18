using System;

namespace Sc.DevChallenge.Domain.Entities
{
    public class PriceEntity
    {
        public int Id { get; set; }
        public string? Portfolio { get; set; }
        public string? Owner { get; set; }
        public string? Instrument { get; set; }
        public decimal Price { get; set; }
        public DateTime DateTime { get; set; }
    }
}