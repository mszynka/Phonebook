using System.Collections.Generic;
using System.Linq;
using PhoneBook.Data.Aggregates;
using PhoneBook.Data.Serializers;

namespace PhoneBook.Data.Exetensions
{
    public static class SerializingExtensions
    {
        public static IEnumerable<string> Serialize(this IList<Entry> entries)
        {
            var serializer = new EntrySerializer();
            return entries.Select(x => serializer.Serialize(x));
        }

        public static IEnumerable<Entry> Deserialize(this IEnumerable<string> values)
        {
            var deserializer = new EntrySerializer();
            return values.Select(x => deserializer.Deserialize(x));
        }
    }
}