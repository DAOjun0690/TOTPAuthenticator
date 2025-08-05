using System;
using System.Windows.Forms;

namespace TOTPAuthenticator
{
    public partial class EditCustomStringForm : Form
    {
        public string CustomString { get; private set; }

        public EditCustomStringForm(string currentCustomString)
        {
            InitializeComponent();
            CustomString = currentCustomString;
            customStringTextBox.Text = currentCustomString;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
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
