using System;
using System.Globalization;


namespace TelegramKey.Text.Helpers
{
    public static class CommandArgumentHelper
    {
        public static bool IsEnum<T>(this ICommandArgument argument) where T : struct
        {
            return Enum.TryParse(argument.Value, true, out T _);
        }

        public static T CastEnum<T>(this ICommandArgument argument) where T : struct
        {
            return (T) Enum.Parse(typeof(T), argument.Value, true);
        }

        public static bool Is<T>(this ICommandArgument argument) where T : struct
        {
            switch (typeof(T).FullName)
            {
                case "System.Int16":
                {
                    return Int16.TryParse(argument.Value, out _);
                }
                case "System.Int32":
                {
                    return Int32.TryParse(argument.Value, out _);
                }
                case "System.Int64":
                {
                    return Int64.TryParse(argument.Value, out _);
                }
                case "System.Byte":
                {
                    return Byte.TryParse(argument.Value, out _);
                }

                case "System.Boolean":
                {
                    return bool.TryParse(argument.Value, out _);
                }

                case "System.Char":
                {
                    return Char.TryParse(argument.Value, out _);
                }

                case "System.Single":
                {
                    return Single.TryParse(argument.Value, out _);
                }
                case "System.Double":
                {
                    return Double.TryParse(argument.Value, out _);
                }
                case "System.Decimal":
                {
                    return Decimal.TryParse(argument.Value, out _);
                }
                case "System.DateTime":
                {
                    return DateTime.TryParseExact(
                               argument.Value,
                               "dd.MM.yyyy",
                               CultureInfo.InvariantCulture,
                               DateTimeStyles.None,
                               out _) ||
                           DateTime.TryParseExact(
                               argument.Value,
                               "HH:mm",
                               CultureInfo.InvariantCulture,
                               DateTimeStyles.None,
                               out _);
                }

                default:
                    throw new NotSupportedException($"{typeof(T)} is not supported!");
            }
        }


        public static T Cast<T>(this ICommandArgument argument) where T : struct
        {
            switch (typeof(T).FullName)
            {
                case "System.Int16":
                    return (T) (object) Int16.Parse(argument.Value);
                case "System.Int32":
                    return (T) (object) Int32.Parse(argument.Value);
                case "System.Int64":
                    return (T) (object) Int64.Parse(argument.Value);
                case "System.Byte":
                    return (T) (object) byte.Parse(argument.Value);

                case "System.Boolean":
                    return (T) (object) bool.Parse(argument.Value);

                case "System.Char":
                    return (T) (object) char.Parse(argument.Value);
                case "System.String":
                    return (T) (object) argument.Value;

                case "System.Single":
                    return (T) (object) Single.Parse(argument.Value);
                case "System.Double":
                    return (T) (object) Double.Parse(argument.Value);
                case "System.Decimal":
                    return (T) (object) Decimal.Parse(argument.Value);

                case "System.DateTime":
                {
                    if (DateTime.TryParseExact(
                        argument.Value,
                        "dd.MM.yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var result1))
                    {
                        return (T) (object) result1;
                    }


                    if (DateTime.TryParseExact(
                        argument.Value,
                        "HH:mm",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var result2))
                    {
                        return (T) (object) result2;
                    }

                    throw new InvalidOperationException();
                }

                default:
                    throw new NotSupportedException($"{typeof(T)} is not supported!");
            }
        }
    }
}