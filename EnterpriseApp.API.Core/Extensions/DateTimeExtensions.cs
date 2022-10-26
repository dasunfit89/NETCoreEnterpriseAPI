using System;
namespace EnterpriseApp.API.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ServerNow(this DateTime source)
        {
            return DateTime.Now;
        }
    }
}
