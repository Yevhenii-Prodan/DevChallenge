using ValueOf;
using System;

namespace Sc.DevChallenge.Domain.ValueObjects
{
    public class PriceTimeSlot : ValueOf<(DateTime startPoint, DateTime endPoint), PriceTimeSlot>
    {
        public PriceTimeSlot Next()
        {
            return PriceTimeSlot.From((Value.startPoint.AddSeconds(10000), Value.endPoint.AddSeconds(10000)));
        }
    }
}