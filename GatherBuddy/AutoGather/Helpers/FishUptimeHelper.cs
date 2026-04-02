using GatherBuddy.Classes;
using GatherBuddy.Time;

namespace GatherBuddy.AutoGather
{
    /// <summary>
    /// Provides uptime percentage calculation for fish, shared between
    /// the Fish Tab UI and the AutoGather priority system.
    /// </summary>
    public static class FishUptimeHelper
    {
        /// <summary>
        /// Returns the uptime percentage for a fish on a 0–10000 scale
        /// (i.e. 10000 = 100.00%, 100 = 1.00%).
        /// Takes the best (highest) uptime across all fishing spots.
        /// </summary>
        public static ushort GetUptimePercent(Fish fish)
        {
            var uptime = 10000L;
            if (!fish.Interval.AlwaysUp())
                uptime = uptime * fish.Interval.OnTime / EorzeaTimeStampExtensions.MillisecondsPerEorzeaHour / RealTime.HoursPerDay;

            ushort bestUptime = 0;
            foreach (var spot in fish.FishingSpots)
            {
                var tmp = uptime
                    * spot.Territory.WeatherRates.ChanceForWeather(fish.PreviousWeather)
                    * spot.Territory.WeatherRates.ChanceForWeather(fish.CurrentWeather)
                    / 10000;
                if (tmp > bestUptime)
                    bestUptime = (ushort)tmp;
            }

            return bestUptime;
        }
    }
}
