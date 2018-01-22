using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PhoneBook.Data.Aggregates;

namespace PhoneBook.Gui
{
    public partial class List : Form
    {
        private Editor _childEditor;
        private int? _currentEntryId;
        private string _currentFileName;

        public readonly Phonebook Phonebook;

        public List()
        {
            Phonebook = new Phonebook();
            InitializeComponent();
        }

        public void ReloadList()
        {
            var entries = Phonebook.PrintAllEntries();

            if (entries.Any())
            {
                contactList.DataSource = new BindingSource(entries, null);
                contactList.DisplayMember = "Value";
                contactList.ValueMember = "Key";
            }
            else
                contactList.DataSource = null;

            if (!string.IsNullOrWhiteSpace(_currentFileName))
                toolStripStatusLabel1.Text = _currentFileName;

            if (contactList.SelectedItem == null)
                phoneNumberLabel.Text = string.Empty;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            _childEditor = new Editor(EditorMode.Add, this);
            _childEditor.Show();
            Hide();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentEntryId == null)
                    throw new Exception("Nie zaznaczono żadnego elementu");

                var selected = Phonebook.Get(_currentEntryId.Value);
                _childEditor = new Editor(EditorMode.Edit, this, selected);
                _childEditor.Show();
                Hide();
            }
            catch (Exception ex)
            {
                HandleChildMessage(ex);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (contactList.SelectedItem == null || _currentEntryId == null)
                    return;

                Phonebook.DeleteEntry(_currentEntryId.Value);
            }
            catch (Exception ex)
            {
                HandleChildMessage(ex);
            }
            finally
            {
                ReloadList();
            }
        }

        private void HandleChildMessage(Exception ex)
        {
            MessageBox.Show(ex.Message);
            _childEditor.Dispose();
        }

        private void contactList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (contactList.SelectedItem == null) 
                return;

            _currentEntryId = ((KeyValuePair<int, string>)contactList.SelectedItem).Key;

            phoneNumberLabel.Text = Phonebook.Get(_currentEntryId.Value).Number;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveFileDialog1 = new SaveFileDialog
            {
                Filter = @"Phonebook file|*.txt",
                Title = @"Save the phonebook"
            };
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                _currentFileName = saveFileDialog1.FileName.Split('/').Last();
                Phonebook.Save(saveFileDialog1.FileName);
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog
            {
                Filter = @"Phonebook file|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _currentFileName = openFileDialog1.FileName.Split('/').Last();
                    Phonebook.Load(openFileDialog1.FileName);
                    ReloadList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
    }
}
