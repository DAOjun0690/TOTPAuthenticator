using OtpNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private string _accountsFilePath; // Path for saving accounts

        public Form1()
        {
            InitializeComponent();
#if DEBUG
            // DEBUG 模式：使用專案根目錄的 accounts.json
            string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\"));
            _accountsFilePath = Path.Combine(projectRoot, ACCOUNTS_FILE);
#else
            // RELEASE 模式：使用 AppData/Roaming 資料夾
            _accountsFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TOTPAuthenticator",
                ACCOUNTS_FILE);
            Directory.CreateDirectory(Path.GetDirectoryName(_accountsFilePath));
#endif
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
                    var barcodeReader = new BarcodeReader
                    {
                        AutoRotate = true,
                        Options = new ZXing.Common.DecodingOptions
                        {
                            TryHarder = true,
                            PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE },
                            CharacterSet = "UTF-8"
                        }
                    };

                    using (var bitmap = (Bitmap)Image.FromFile(openFileDialog.FileName))
                    {
                        var result = barcodeReader.Decode(bitmap);

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
                if (form.ShowDialog() == DialogResult.OK && form.Account != null)
                {
                    accounts.Add(form.Account);
                    SaveAccounts();
                    UpdateAccountsList();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateTotp();
        }

        private void LoadAccounts()
        {
            if (File.Exists(_accountsFilePath))
            {
                var json = File.ReadAllText(_accountsFilePath);
                accounts = JsonSerializer.Deserialize<List<Account>>(json) ?? new List<Account>();
            }
        }

        private void SaveAccounts()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,  // 啟用縮排格式化
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping  // 允許中文字元不被編碼
            };
            var json = JsonSerializer.Serialize(accounts, options);
            File.WriteAllText(_accountsFilePath, json);
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
                var secret = control?.Account?.Secret;
                if (!string.IsNullOrEmpty(secret))
                {
                    var totp = new Totp(Base32Encoding.ToBytes(control?.Account?.Secret));
                    var remainingSeconds = 30 - (DateTime.UtcNow.Second % 30);
                    control.UpdateTotp(totp.ComputeTotp(), remainingSeconds);
                }
            }
        }

        private void EditAccount(Account account)
        {
            using (var form = new EditAccountForm(account.Name, account.CustomString ?? string.Empty, account.Issuer ?? string.Empty))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    account.Name = form.AccountName;
                    account.CustomString = form.CustomString;
                    account.Issuer = form.Issuer;
                    SaveAccounts();
                    UpdateAccountsList();
                }
            }
        }

        private void DeleteAccount(Account account)
        {
            if (MessageBox.Show($"是否要刪除 {account.Name} ?", "請確認刪除", MessageBoxButtons.YesNo) == DialogResult.Yes)
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

        private void openAccountsFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", Path.GetDirectoryName(_accountsFilePath));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"開啟資料夾時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addFromGoogleAuthenticatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 從內嵌資源解壓縮 decodeGoogleOTP.exe
                    string decoderPath = ExtractEmbeddedDecoder();

                    // 建立暫存輸出檔案路徑（decodeGoogleOTP 輸出 CSV 格式，但副檔名可以是 .json）
                    string outputCsvPath = Path.Combine(Path.GetTempPath(), $"google_otp_output_{Guid.NewGuid()}.csv");

                    // 執行解碼程式
                    var processInfo = new ProcessStartInfo
                    {
                        FileName = decoderPath,
                        Arguments = $"-i \"{openFileDialog.FileName}\" -c \"{outputCsvPath}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    using (var process = Process.Start(processInfo))
                    {
                        if (process == null)
                        {
                            throw new Exception("無法啟動解碼程式");
                        }

                        process.WaitForExit();

                        if (process.ExitCode != 0)
                        {
                            string error = process.StandardError.ReadToEnd();
                            throw new Exception($"解碼失敗：{error}");
                        }
                    }

                    // 讀取並解析 CSV
                    if (File.Exists(outputCsvPath))
                    {
                        var lines = File.ReadAllLines(outputCsvPath);

                        if (lines.Length <= 1)
                        {
                            MessageBox.Show("圖片中未找到任何 OTP 帳戶資料", "匯入失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            try { File.Delete(outputCsvPath); } catch { }
                            return;
                        }

                        int addedCount = 0;
                        // 跳過標題行（第一行），從第二行開始解析
                        for (int i = 1; i < lines.Length; i++)
                        {
                            string line = lines[i].Trim();
                            if (string.IsNullOrEmpty(line))
                                continue;

                            // CSV 格式：Issuer,Name,Secret,Type,Counter,URL
                            var parts = line.Split(',');
                            if (parts.Length >= 3)
                            {
                                string issuer = parts[0].Trim();
                                string name = parts[1].Trim();
                                string secret = parts[2].Trim();

                                if (!string.IsNullOrEmpty(secret))
                                {
                                    accounts.Add(new Account
                                    {
                                        Name = name,
                                        Secret = secret,
                                        Issuer = string.IsNullOrEmpty(issuer) ? null : issuer
                                    });
                                    addedCount++;
                                }
                            }
                        }

                        SaveAccounts();
                        UpdateAccountsList();

                        if (addedCount > 0)
                        {
                            MessageBox.Show($"成功匯入", "匯入成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("圖片中未找到任何有效的 OTP 帳戶資料", "匯入失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        // 刪除暫存 CSV 檔案
                        try { File.Delete(outputCsvPath); } catch { }
                    }
                    else
                    {
                        throw new Exception("解碼程式未產生輸出檔案");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"從 Google 驗證器圖片匯入時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string ExtractEmbeddedDecoder()
        {
            // 在暫存目錄中建立解碼器路徑
            string tempPath = Path.Combine(Path.GetTempPath(), "TOTPAuthenticator");
            Directory.CreateDirectory(tempPath);
            string decoderPath = Path.Combine(tempPath, "decodeGoogleOTP-windows-amd64.exe");

            // 如果檔案已存在，直接返回
            if (File.Exists(decoderPath))
            {
                return decoderPath;
            }

            // 從內嵌資源解壓縮
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = "TOTPAuthenticator.Resources.decodeGoogleOTP-windows-amd64.exe";

            using (Stream? resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new Exception($"找不到內嵌資源：{resourceName}");
                }

                using (FileStream fileStream = File.Create(decoderPath))
                {
                    resourceStream.CopyTo(fileStream);
                }
            }

            return decoderPath;
        }
    }
}