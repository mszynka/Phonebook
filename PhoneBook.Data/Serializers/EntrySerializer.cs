using PhoneBook.Data.Aggregates;

namespace PhoneBook.Data.Serializers
{
    public interface IEntrySerializer
    {
        string Serialize(Entry entry);

        Entry Deserialize(string data);
    }

    public sealed class EntrySerializer : IEntrySerializer
    {
        public string Serialize(Entry entry)
        {
            return $"{entry.Id};{entry.Name};{entry.Surname};{entry.Number}";
        }

        public Entry Deserialize(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return null;

            var split = data.Split(';');
            return new Entry(int.Parse(split[0]), split[1], split[2], split[3]);
        }
    }
}