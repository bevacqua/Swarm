using System;

namespace Swarm.Common.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToShortDurationString(this TimeSpan ts)
        {
            return Resources.TimeSpan.ShortDurationString.FormatWith((int)ts.TotalHours, ts.Minutes, ts.Seconds);
        }

        public static string ToTimeAgoString(this TimeSpan ts)
        {
            return GetDurationString(ts, Resources.TimeSpan.TimeAgoTense);
        }

        public static string ToDurationString(this TimeSpan ts)
        {
            return GetDurationString(ts, Resources.TimeSpan.DurationTense);
        }

        public static string ToAbsoluteTimeString(this TimeSpan ts)
        {
            return GetDurationString(ts, string.Empty);
        }

        private static string GetDurationString(TimeSpan ts, string tense)
        {
            bool isTimeAgo = tense == Resources.TimeSpan.TimeAgoTense;
            double seconds = Math.Abs(ts.TotalSeconds);

            const int SECOND = 1;
            const int SECONDS_PER_MINUTE = 60;
            const int MINUTE = SECONDS_PER_MINUTE * SECOND;
            const int MINUTES_PER_HOUR = 60;
            const int HOUR = MINUTES_PER_HOUR * MINUTE;
            const int HOURS_PER_DAY = 24;
            const int DAY = HOUR * HOURS_PER_DAY;
            const int DAYS_PER_WEEK = 7;
            const int DAYS_PER_MONTH = 30;
            const int DAYS_PER_YEAR = 365;
            const int WEEK = DAY * DAYS_PER_WEEK;
            const int WEEKS_PER_MONTH = 4;
            const int MONTH = DAY * DAYS_PER_MONTH;
            const int MONTHS_PER_YEAR = 12;

            string result;

            if (seconds < MINUTE)
            {
                result = (ts.Seconds < 2 ? Resources.TimeSpan.OneSecond : Resources.TimeSpan.FewSeconds);
            }
            else if (seconds < 2 * MINUTE)
            {
                result = Resources.TimeSpan.OneMinute;
            }
            else if (seconds < HOUR)
            {
                result = Resources.TimeSpan.FixedMinutes.FormatWith(ts.Minutes);
            }
            else if (seconds < 1.8 * HOUR)
            {
                result = Resources.TimeSpan.OneHour;
            }
            else if (seconds < HOURS_PER_DAY * HOUR)
            {
                result = Resources.TimeSpan.FixedHours.FormatWith(ts.Hours);
            }
            else if (seconds < 1.8 * HOURS_PER_DAY * HOUR)
            {
                if (isTimeAgo)
                {
                    return Resources.TimeSpan.Yesterday;
                }
                else
                {
                    result = Resources.TimeSpan.OneDay;
                }
            }
            else if (seconds < WEEK)
            {
                result = Resources.TimeSpan.FixedDays.FormatWith(ts.Days);
            }
            else if (seconds < WEEKS_PER_MONTH * WEEK)
            {
                int weeks = Convert.ToInt32(Math.Floor((double)ts.Days / DAYS_PER_WEEK));
                result = (weeks < 2 ? Resources.TimeSpan.OneWeek : Resources.TimeSpan.FixedWeeks.FormatWith(weeks));
            }
            else if (seconds < MONTHS_PER_YEAR * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / DAYS_PER_MONTH));
                result = (months < 2 ? Resources.TimeSpan.OneMonth : Resources.TimeSpan.FixedMonths.FormatWith(months));
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / DAYS_PER_YEAR));
                result = (years < 2 ? Resources.TimeSpan.OneYear : Resources.TimeSpan.FixedYears.FormatWith(years));
            }

            return tense.FormatWith(result);
        }
    }
}
