using System;
using System.Windows.Forms;

namespace TOTPAuthenticator
{
    public partial class EditAccountForm : Form
    {
        public string AccountName { get; private set; }
        public string CustomString { get; private set; }
        public string? Issuer { get; private set; }

        public EditAccountForm(string currentAccountName, string currentCustomString, string? currentIssuer)
        {
            InitializeComponent();
            AccountName = currentAccountName;
            CustomString = currentCustomString;
            Issuer = currentIssuer;
            nameTextBox.Text = currentAccountName;
            customStringTextBox.Text = currentCustomString;
            issuerTextBox.Text = currentIssuer;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show("名稱不能為空!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AccountName = nameTextBox.Text;
            CustomString = customStringTextBox.Text;
            Issuer = issuerTextBox.Text;
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
