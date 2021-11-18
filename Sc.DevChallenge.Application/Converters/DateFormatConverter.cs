using Newtonsoft.Json.Converters;

namespace Sc.DevChallenge.Application.Converters
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}