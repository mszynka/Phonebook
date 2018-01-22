using System;
using System.Collections.Generic;
using System.Linq;
using PhoneBook.Data.Database;
using PhoneBook.Data.Exetensions;

namespace PhoneBook.Data.Aggregates
{
    public sealed class Phonebook
    {
        private IList<Entry> _entries;
        private readonly IDatabaseManager _manager;

        public Phonebook()
        {
            _manager = new DatabaseManager();
            _entries = new List<Entry>();
        }

        public int AddEntry(string name, string surname, string number)
        {
            var entry = new Entry(GetNextId(), name, surname, number);
            _entries.Add(entry);
            return entry.Id;
        }

        public Entry Get(int id)
        {
            if (!Exists(id))
                throw new Exception("Element o podanym identyfikatorze nie istnieje");

            return _entries.Single(x => x.Id == id);
        }

        public Dictionary<int, string> PrintAllEntries()
        {
            return _entries.ToDictionary(x => x.Id, x => x.ToString());
        }

        public void DeleteEntry(int id)
        {
            var entry = Get(id);
            _entries.Remove(entry);
            throw new Exception("Usunięto wpis");
        }

        public void Save(string filePath)
        {
            _manager.SetFilePath(filePath);
            _manager.Save(_entries.Serialize());
        }

        public void Load(string filePath)
        {
            _manager.SetFilePath(filePath);
            _entries = _manager
                .Load()
                .Deserialize()
                .Where(x => x != null)
                .ToList();
        }

        public int GetNextId()
        {
            return _entries.Any() ? _entries.Max(x => x.Id) + 1 : 0;
        }

        public void ModifyEntry(int id, string name, string surname, string number)
        {
            Get(id)
                .Modify(name, surname, number);
        }

        public bool Exists(int id)
        {
            return _entries.Any(x => x.Id == id);
        }
    }
}