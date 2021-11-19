using System;

namespace Sc.DevChallenge.Application.Common
{
    public class ApplicationSettings
    {
        public int TimeIntervalInSec { get; set; }
        public DateTime GeneralStartPoint { get; set; }
        public string SQLiteConnectionString { get; set; }
    }
}