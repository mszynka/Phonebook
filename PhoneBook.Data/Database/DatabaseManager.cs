using System;
using System.Collections.Generic;
using System.IO;

namespace PhoneBook.Data.Database
{
    public interface IDatabaseManager
    {
        void Save(IEnumerable<string> serialize);
        IEnumerable<string> Load();
        void SetFilePath(string filePath);
    }

    public sealed class DatabaseManager : IDatabaseManager
    {
        public string FilePath { get; private set; }

        public void Save(IEnumerable<string> lines)
        {
            if(string.IsNullOrWhiteSpace(FilePath))
                throw new ArgumentException("Podaj ścieżkę docelową.");

            using (var outputFile = new StreamWriter(FilePath, true))
            {
                foreach (var line in lines)
                    outputFile.WriteLine(line);
            }
        }

        public IEnumerable<string> Load()
        {
            if (string.IsNullOrWhiteSpace(FilePath))
                throw new ArgumentException("Podaj ścieżkę docelową.");

            using(var inputFile = new StreamReader(FilePath))
            {
                string line;

                while ((line = inputFile.ReadLine()) != null)
                    yield return line;
            }
        }

        public void SetFilePath(string filePath)
        {
            FilePath = filePath;
        }
    }
}