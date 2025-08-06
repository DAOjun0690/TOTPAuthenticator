using OtpNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using ZXing;
using ZXing.Windows.Compatibility;

namespace TOTPAuthenticator
{
    public partial class Form1 : Form
    {
        private List<Account> accounts = new List<Account>();
        private const string ACCOUNTS_FILE = "accounts.json";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAccounts();
            UpdateAccountsList();
            timer1.Start();
        }

        private void addFromQRCodeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var barcodeReader = new BarcodeReader();
                    var result = barcodeReader.Decode(new Bitmap(openFileDialog.FileName));
                    if (result != null)
                    {
                        var uri = new Uri(result.Text);
                        var query = uri.Query;
                        var secret = GetValueFromQuery(query, "secret");
                        var issuer = GetValueFromQuery(query, "issuer");
                        var account = new Account { Name = uri.LocalPath.Trim('/'), Secret = secret ?? string.Empty, Issuer = issuer ?? string.Empty };
                        accounts.Add(account);
                        SaveAccounts();
                        UpdateAccountsList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading QR code: " + ex.Message);
                }
            }
        }

        private string? GetValueFromQuery(string query, string key)
        {
            var pairs = query.TrimStart('?').Split('&');
            foreach (var pair in pairs)
            {
                var parts = pair.Split('=');
                if (parts.Length == 2 && parts[0] == key)
                {
                    return Uri.UnescapeDataString(parts[1]);
                }
            }
            return null;
        }

        private void addManuallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new ManualAddForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (form.Account != null)
                    {
                        accounts.Add(form.Account);
                        SaveAccounts();
                        UpdateAccountsList();
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateTotp();
        }

        private void LoadAccounts()
        {
            if (File.Exists(ACCOUNTS_FILE))
            {
                var json = File.ReadAllText(ACCOUNTS_FILE);
                accounts = JsonSerializer.Deserialize<List<Account>>(json) ?? new List<Account>();
            }
        }

        private void SaveAccounts()
        {
            var json = JsonSerializer.Serialize(accounts);
            File.WriteAllText(ACCOUNTS_FILE, json);
        }

        private void UpdateAccountsList()
        {
            flowLayoutPanel1.Controls.Clear();
            var filteredAccounts = accounts.Where(a => string.IsNullOrEmpty(searchTextBox.Text) || a.Name.Contains(searchTextBox.Text) || (a.Issuer != null && a.Issuer.Contains(searchTextBox.Text)));
            foreach (var account in filteredAccounts)
            {
                var accountControl = new AccountControl { Account = account };
                accountControl.EditClicked += (s, ev) => EditAccount(account);
                accountControl.DeleteClicked += (s, ev) => DeleteAccount(account);
                flowLayoutPanel1.Controls.Add(accountControl);
            }
        }

        private void UpdateTotp()
        {
            foreach (AccountControl control in flowLayoutPanel1.Controls)
            {
                var totp = new Totp(Base32Encoding.ToBytes(control.Account.Secret));
                var remainingSeconds = 30 - (DateTime.UtcNow.Second % 30);
                control.UpdateTotp(totp.ComputeTotp(), remainingSeconds);
            }
        }

        private void EditAccount(Account account)
        {
            using (var form = new EditAccountForm(account.Name, account.CustomString ?? string.Empty))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    account.Name = form.AccountName;
                    account.CustomString = form.CustomString;
                    SaveAccounts();
                    UpdateAccountsList();
                }
            }
        }

        private void DeleteAccount(Account account)
        {
            if (MessageBox.Show($"Are you sure you want to delete {account.Name}?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                accounts.Remove(account);
                SaveAccounts();
                UpdateAccountsList();
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateAccountsList();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            addManuallyToolStripMenuItem_Click(sender, e);
        }
    }
}