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

        public static Dictionary<int, T> ToDictionary<T>(this IEnumerable<T> array)
        {
            return array
                .Select((v, i) => new { Key = i, Value = v })
                .ToDictionary(o => o.Key, o => o.Value);
        }
    }
}
