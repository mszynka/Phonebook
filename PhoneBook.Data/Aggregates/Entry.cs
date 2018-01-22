namespace PhoneBook.Data.Aggregates
{
    public sealed class Entry
    {
        public Entry(int id)
        {
            Id = id;
            Name = string.Empty;
            Surname = string.Empty;
            Number = string.Empty;
        }

        public Entry(int id, string name, string surname, string number)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Number = number;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public string Surname { get; private set; }

        public string Number { get; private set; }

        public override string ToString()
        {
            return $"{Name} {Surname}";
        }

        public void Modify(string name, string surname, string number)
        {
            Name = name;
            Surname = surname;
            Number = number;
        }
    }
}