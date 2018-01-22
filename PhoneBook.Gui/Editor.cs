using System.Windows.Forms;
using PhoneBook.Data.Aggregates;

namespace PhoneBook.Gui
{
    public sealed partial class Editor : Form
    {
        private readonly List _parent;
        private readonly Entry _entry;
        private bool _saveMode;

        public Editor(EditorMode mode, List parent, Entry selected = null)
        {
            _parent = parent;
            _entry = selected ?? new Entry(_parent.Phonebook.GetNextId());
            InitializeComponent();

            idTextBox.Text = _entry.Id.ToString();
            nameTextBox.Text = _entry.Name;
            surnameTextBox.Text = _entry.Surname;
            numberTextBox.Text = _entry.Number;

            Text = mode == EditorMode.Add ? "Add" : "Edit";
        }

        private void OnFormClosingHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_saveMode && IsChanged() && MessageBox.Show(@"You have unsaved actions! Do you really wanna close?", @"Editor", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                _parent.ReloadList();
                _parent.Show();
            }
        }

        private bool IsChanged()
        {
            return nameTextBox.Text != _entry.Name
                   || surnameTextBox.Text != _entry.Surname
                   || numberTextBox.Text != _entry.Number;
        }

        private void saveButton_Click(object sender, System.EventArgs e)
        {
            if (_parent.Phonebook.Exists(_entry.Id))
                _parent.Phonebook.ModifyEntry(_entry.Id, nameTextBox.Text, surnameTextBox.Text, numberTextBox.Text);
            else
                _parent.Phonebook.AddEntry(nameTextBox.Text, surnameTextBox.Text, numberTextBox.Text);

            _saveMode = true;
            Close();
        }

        private void cancelButton_Click(object sender, System.EventArgs e)
        {
            _saveMode = false;
            Close();
        }
    }
}
