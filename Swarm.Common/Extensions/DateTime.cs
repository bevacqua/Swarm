using System;

namespace Swarm.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToLongDateString(this DateTime date)
        {
            return date.ToString(Resources.User.LongDateFormat);
        }

        public static string ToLongDateTimeString(this DateTime date)
        {
            return date.ToString(Resources.User.LongDateTimeFormat);
        }

        public static string ToTimeAgoString(this DateTime since)
        {
            return since.ToTimeAgoString(DateTime.Now);
        }

        public static string ToUtcTimeAgoString(this DateTime since)
        {
            return since.ToTimeAgoString(DateTime.UtcNow);
        }

        public static string ToTimeAgoString(this DateTime since, DateTime start)
        {
            var ts = new TimeSpan(start.Ticks - since.Ticks);
            return ts.ToTimeAgoString();
        }

        public static string ToDurationString(this DateTime since, DateTime start)
        {
            var ts = new TimeSpan(start.Ticks - since.Ticks);
            return ts.ToDurationString();
        }

        /// <summary>
        /// Converts a DateTime object into an ISO-8601 compliant date string.
        /// </summary>
        public static string ToIso8601(this DateTime date)
        {
            return date.ToString("o");
        }

        public static long ToUnixTime(this DateTime date)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalMilliseconds);
        }
    }
}
