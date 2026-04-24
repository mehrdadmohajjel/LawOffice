using System;
using System.Globalization;

namespace LawOffice.Utilities // در صورت نیاز فضای نام را مطابق پروژه خود تغییر دهید
{
    public static class PersianDateExtensions
    {
        /// <summary>
        /// تبدیل اعداد فارسی/عربی به اعداد انگلیسی
        /// </summary>
        public static string ConvertToEnglishNumber(this string number)
        {
            if (string.IsNullOrWhiteSpace(number)) return number;

            var arabic = new string[10] { "٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩" };
            var persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (var i = 0; i < 10; i++)
            {
                var iAsNum = i.ToString();
                number = number.Replace(persian[i], iAsNum).Replace(arabic[i], iAsNum);
            }

            return number;
        }

        /// <summary>
        /// تبدیل تاریخ شمسی (رشته) به میلادی (DateTime)
        /// </summary>
        public static DateTime ToLatinDate(this string shamsiDate, char splitter = '/')
        {
            if (string.IsNullOrWhiteSpace(shamsiDate))
                throw new ArgumentNullException(nameof(shamsiDate));

            // ابتدا اعداد فارسی احتمالی را به انگلیسی تبدیل می‌کنیم
            var englishShamsiDate = shamsiDate.ConvertToEnglishNumber();
            var shamsiList = englishShamsiDate.Split(splitter);

            if (shamsiList.Length < 3)
                throw new FormatException("فرمت تاریخ شمسی نامعتبر است.");

            var year = Convert.ToInt32(shamsiList[0]);
            var month = Convert.ToInt32(shamsiList[1]);
            var day = Convert.ToInt32(shamsiList[2]);

            var pc = new PersianCalendar();
            return new DateTime(year, month, day, pc);
        }

        /// <summary>
        /// تبدیل تاریخ شمسی و زمان به میلادی (DateTime)
        /// </summary>
        public static DateTime ToLatinDateTime(this string shamsiDate, string time)
        {
            var date = shamsiDate.ToLatinDate();

            if (string.IsNullOrWhiteSpace(time)) return date;

            var englishTime = time.ConvertToEnglishNumber();
            var splitTime = englishTime.Split(':');
            var hour = Convert.ToInt32(splitTime[0]);
            var minute = Convert.ToInt32(splitTime[1]);

            return date.Add(new TimeSpan(hour, minute, 0));
        }

        /// <summary>
        /// دریافت سال شمسی از یک تاریخ میلادی
        /// </summary>
        public static string GetShamsiYear(this DateTime date)
        {
            return new PersianCalendar().GetYear(date).ToString();
        }

        /// <summary>
        /// تبدیل تاریخ میلادی (DateTime) به رشته تاریخ شمسی
        /// </summary>
        public static string ToShamsiDateString(this DateTime value, string separator = "/")
        {
            var pc = new PersianCalendar();
            var year = pc.GetYear(value);
            var month = pc.GetMonth(value).ToString("00");
            var day = pc.GetDayOfMonth(value).ToString("00");

            return $"{year}{separator}{month}{separator}{day}";
        }
    }
}
