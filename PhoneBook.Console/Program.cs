using System;
using System.Linq;
using PhoneBook.Data.Aggregates;
using C = System.Console;

namespace PhoneBook.Console
{
    internal static class Program
    {
        private static void Main()
        {
            var phonebook = new Phonebook();

            const string helpMenu = "Choose your action:\n" +
                                    "\th - presents current menu\n" +
                                    "\tshow - prints entry details\n" +
                                    "\tadd - starts new entry creator\n" +
                                    "\trm - removes entry with specified ID\n" +
                                    "\tall - prints all entries list\n" +
                                    "\texit - closes application";

            C.WriteLine("Welcome in your phonebook.\n" + helpMenu);

            string input;
            C.Write("phonebook:>");
            while (!string.Equals(input = C.ReadLine()?.ToLower().Trim(), "exit"))
            {
                switch (input)
                {
                    case "h":
                        C.WriteLine(helpMenu);
                        break;

                    case "show":
                        ShowEntry(phonebook);
                        break;

                    case "add":
                        AddEntry(phonebook);
                        break;

                    case "rm":
                        RemoveEntry(phonebook);
                        break;

                    case "all":
                        ShowAllEntries(phonebook);
                        break;

                    default:
                        C.WriteLine($"Command \"{input}\" not understood. For help type \"help\".");
                        break;
                }

                C.Write("phonebook:>");
            }
        }

        private static void RemoveEntry(Phonebook phonebook)
        {
            if (GetId(phonebook, out var idInt)) return;

            try
            {
                phonebook.DeleteEntry(idInt);
            }
            catch(Exception e)
            {
                C.WriteLine(e.Message);
            }
        }

        private static bool GetId(Phonebook phonebook, out int idInt)
        {
            C.Write("\nType entry ID: ");
            var id = C.ReadLine();

            if (!int.TryParse(id, out idInt))
            {
                C.WriteLine($"Entry ID {id} is not a number!");
                return true;
            }

            if (!phonebook.Exists(idInt))
            {
                C.WriteLine($"Entry with ID {id} was not found!");
                return true;
            }

            return false;
        }

        private static void ShowEntry(Phonebook phonebook)
        {
            if (GetId(phonebook, out var idInt)) return;

            var entry = phonebook.Get(idInt);
            C.WriteLine($"{entry.Name} {entry.Surname} ({entry.Number})");
        }

        private static void AddEntry(Phonebook phonebook)
        {
            C.Write("\nType name: ");
            var name = C.ReadLine();

            C.Write("\nType surname: ");
            var surname = C.ReadLine();

            C.Write("\nType phone number: ");
            var phone = C.ReadLine();

            phonebook.AddEntry(name, surname, phone);
            C.WriteLine("Entry added!");
        }

        private static void ShowAllEntries(Phonebook phonebook)
        {
            var entries = phonebook.PrintAllEntries();
            if(!entries.Any())
            {
                C.WriteLine("No entries found!");
                return;
            }

            C.WriteLine("Current entries:");
            foreach (var entry in entries)
            {
                C.WriteLine($"{entry.Key}: {entry.Value}");
            }
        }
    }
}
