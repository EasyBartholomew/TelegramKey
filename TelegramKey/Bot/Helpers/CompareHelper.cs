namespace TelegramKey.Bot.Helpers
{
    internal static class CompareHelper
    {
        public static bool NullOrEquals<T1, TPropertyType>(
            this T1       obj,
            TPropertyType objProperty,
            TPropertyType otherProperty)
            where T1 : class
        {
            return obj == null || objProperty.Equals(otherProperty);
        }
    }
}