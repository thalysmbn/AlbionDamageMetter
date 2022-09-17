using System.Collections;

namespace AlbionDamageMetter.Extensions
{
    public static class AlbionExtensionMethod
    {
        public static Guid? ObjectToGuid(this object value)
        {
            try
            {
                if (value is IEnumerable valueEnumerable)
                {
                    var myBytes = valueEnumerable.OfType<byte>().ToArray();
                    return new Guid(myBytes);
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        public static ulong? ObjectToUlong(this object value)
        {
            return value as byte? ?? value as ushort? ?? value as uint? ?? value as ulong?;
        }

        public static long? ObjectToLong(this object value)
        {
            return value as byte? ?? value as short? ?? value as int? ?? value as long?;
        }

        public static int ObjectToInt(this object value)
        {
            return value as byte? ?? value as short? ?? value as int? ?? 0;
        }

        public static short ObjectToShort(this object value)
        {
            return value as byte? ?? value as short? ?? 0;
        }

        public static byte ObjectToByte(this object value)
        {
            return value as byte? ?? 0;
        }

        public static bool ObjectToBool(this object value)
        {
            return value as bool? ?? false;
        }

        public static double ObjectToDouble(this object value)
        {
            return value as float? ?? value as double? ?? 0;
        }

        public static Dictionary<int, T> ToDictionary<T>(this IEnumerable<T> array)
        {
            return array
                .Select((v, i) => new { Key = i, Value = v })
                .ToDictionary(o => o.Key, o => o.Value);
        }
    }
}
