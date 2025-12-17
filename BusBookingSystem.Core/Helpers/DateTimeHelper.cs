namespace BusBookingSystem.Core.Helpers
{
    public static class DateTimeHelper
    {
        private static readonly TimeZoneInfo TurkeyTimeZone;

        static DateTimeHelper()
        {
            // Türkiye saat dilimini bul, bulunamazsa manuel oluştur
            try
            {
                // Windows için
                TurkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            }
            catch
            {
                try
                {
                    // Linux/Mac için
                    TurkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul");
                }
                catch
                {
                    // Fallback: UTC+3 olarak manuel oluştur
                    TurkeyTimeZone = TimeZoneInfo.CreateCustomTimeZone(
                        "Turkey Time",
                        TimeSpan.FromHours(3),
                        "Turkey Time",
                        "Turkey Time");
                }
            }
        }

        /// <summary>
        /// Türkiye saatine göre şu anki zamanı döndürür
        /// </summary>
        public static DateTime GetTurkeyTimeNow()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TurkeyTimeZone);
        }

        /// <summary>
        /// UTC zamanını Türkiye saatine çevirir
        /// </summary>
        public static DateTime ToTurkeyTime(DateTime utcDateTime)
        {
            if (utcDateTime.Kind == DateTimeKind.Utc)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TurkeyTimeZone);
            }
            return utcDateTime;
        }

        /// <summary>
        /// Türkiye saatini UTC'ye çevirir
        /// </summary>
        public static DateTime ToUtc(DateTime turkeyDateTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(turkeyDateTime, TurkeyTimeZone);
        }
    }
}

