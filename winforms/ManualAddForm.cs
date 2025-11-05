using System;
using System.Windows.Forms;

namespace TOTPAuthenticator
{
    public partial class ManualAddForm : Form
    {
        public Account? Account { get; private set; }

        public ManualAddForm()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) || string.IsNullOrWhiteSpace(secretTextBox.Text))
            {
                MessageBox.Show("Name and Secret cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Account = new Account { Name = nameTextBox.Text, Secret = secretTextBox.Text, CustomString = customStringTextBox.Text };
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}