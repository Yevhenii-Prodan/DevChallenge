﻿using Newtonsoft.Json.Converters;

namespace SC.DevChallenge.Api.Utils.Converters
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}