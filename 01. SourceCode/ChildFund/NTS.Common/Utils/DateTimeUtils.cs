using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NTS.Common.Utils
{
    public static class DateTimeUtils
    {
        public static DateTime ConvertDateFromStr(string date)
        {
            if (date == null)
            {
                return new DateTime();
            }

            string[] items = date.Split('/');
            if (items.Length != 3)
            {
                return new DateTime();
            }
            else
            {
                return new DateTime(int.Parse(items[2]), int.Parse(items[1]), int.Parse(items[0]));
            }
        }

        public static DateTime ConvertDateToStr(string date)
        {
            if (date == null)
            {
                return new DateTime();
            }

            DateTime returnValue = DateTime.ParseExact(date + " 23:59:59", "dd/MM/yyyy HH:mm:ss", null);

            return returnValue;
        }

        public static DateTime ConvertDateFrom(DateTime? date)
        {
            if (date == null)
            {
                return new DateTime();
            }

            DateTime returnValue = DateTime.Parse(date.Value.ToShortDateString() + " 00:00:00");

            return returnValue;
        }

        public static DateTime ConvertDateTo(DateTime? date)
        {
            if (date == null)
            {
                return new DateTime();
            }

            DateTime returnValue = DateTime.Parse(date.Value.ToShortDateString() + " 23:59:59");

            return returnValue;
        }

        public static DateTime FirstDayOfWeek(DateTime dt)
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (diff < 0)
                diff += 7;
            return dt.AddDays(-diff).Date;
        }

        public static string ConvertDateText(DateTime? dt)
        {
            if (dt.HasValue)
            {
                return $"Ngày {dt.Value.ToString("dd")} tháng {dt.Value.ToString("MM")} năm {dt.Value.ToString("yyyy")}";
            }
            return "Ngày    tháng    năm    ";
        }

        public static string ConvertDateToDDMMYYYY(DateTime? dt)
        {
            if (dt.HasValue)
            {
                return dt.Value.ToString("dd/MM/yyyy");
            }
            return string.Empty;
        }
    }
}
