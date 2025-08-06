using System;
using System.Windows.Forms;

namespace TOTPAuthenticator
{
    public partial class EditAccountForm : Form
    {
        public string AccountName { get; private set; }
        public string CustomString { get; private set; }

        public EditAccountForm(string currentAccountName, string currentCustomString)
        {
            InitializeComponent();
            AccountName = currentAccountName;
            CustomString = currentCustomString;
            nameTextBox.Text = currentAccountName;
            customStringTextBox.Text = currentCustomString;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show("Account Name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AccountName = nameTextBox.Text;
            CustomString = customStringTextBox.Text;
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
