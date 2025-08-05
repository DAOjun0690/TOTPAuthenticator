using OtpNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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

        private void accountsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTotp();
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
            accountsListBox.Items.Clear();
            foreach (var account in accounts)
            {
                accountsListBox.Items.Add(account.Name);
            }
        }

        private void UpdateTotp()
        {
            if (accountsListBox.SelectedIndex != -1)
            {
                var account = accounts[accountsListBox.SelectedIndex];
                var totp = new Totp(Base32Encoding.ToBytes(account.Secret));
                totpLabel.Text = totp.ComputeTotp();
                var remainingSeconds = 30 - (DateTime.UtcNow.Second % 30);
                countdownLabel.Text = remainingSeconds.ToString();

                if (remainingSeconds <= 5)
                {
                    countdownLabel.ForeColor = Color.Red;
                }
                else
                {
                    countdownLabel.ForeColor = SystemColors.ControlText;
                }
            }
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(totpLabel.Text);
        }
    }

    public class Account
    {
        public string Name { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public string? Issuer { get; set; }
    }
}