using System;
using System.Linq;


namespace TelegramKey.Bot.Helpers
{
    public static class EnumHelper
    {
        public static bool IsOneOf<T>(this T obj, params T[] values) where T : struct, IConvertible
        {
            return values.Any(value => Equals(obj, value));
        }

        public static bool IsNotOneOf<T>(this T obj, params T[] values) where T : struct, IConvertible
        {
            return !obj.IsOneOf(values);
        }
    }
}