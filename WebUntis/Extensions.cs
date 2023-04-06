using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntis
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime date, DayOfWeek firstDayOfWeek)
        {
            int diff = (7 + (date.DayOfWeek - firstDayOfWeek)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Converts Untis-Date-Format to standard DateTime-Object
        /// </summary>
        /// <param name="UntisDate">Untis-Date in the format yyyymmdd</param>
        /// <returns>Valid DateTime object</returns>
        public static DateTime FromUntisDate(long UntisDate)
        {
            int year = (int)(UntisDate / 10_000);
            int month = (int)((UntisDate - year * 10_000) / 100);
            int day = (int)(UntisDate - year * 10_000 - month * 100);

            return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
        }
        /// <summary>
        /// Converts Untis-Time-Format to standard TimeSpan-Object
        /// </summary>
        /// <param name="UntisTime">Untis-Time in the format (h)hmm</param>
        /// <returns>Valid TimeSpan object</returns>
        public static TimeSpan FromUntisTime(long UntisTime)
        {
            int hours = (int)(UntisTime / 100);
            int minutes = (int)(UntisTime - hours * 100);

            return new TimeSpan(hours, minutes, 0);
        }

        public static long ToUntisTime(this TimeSpan time)
        {
            return time.Hours * 100 + time.Minutes;
        }

        public static long ToUntisDate(this DateTime date)
        {
            return date.Year * 10_000 +
                date.Month * 100 +
                date.Day;
        }
    }
}
