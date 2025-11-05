using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using OtpNet;

namespace TOTPAuthenticatorWeb;

public class AccountService
{
    private readonly string _accountsFilePath;
    private List<Account> _accounts = new();

    public AccountService()
    {
#if DEBUG
        // DEBUG 模式：使用專案根目錄的 accounts.json
        string projectRoot = Path.GetFullPath(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\"));
        _accountsFilePath = Path.Combine(projectRoot, "accounts.json");
#else
            // RELEASE 模式：使用 AppData/Roaming 資料夾
            _accountsFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TOTPAuthenticator",
                "accounts.json");
            Directory.CreateDirectory(Path.GetDirectoryName(_accountsFilePath) ?? "");
#endif
        LoadAccounts();
    }

    public List<Account> GetAllAccounts() => _accounts;

    public Account? GetAccount(string name)
    {
        return _accounts.FirstOrDefault(a => a.Name == name);
    }

    public void AddAccount(Account account)
    {
        // 驗證 secret key
        try
        {
            var secretBytes = Base32Encoding.ToBytes(account.Secret);
        }
        catch
        {
            throw new ArgumentException("無效的 Secret Key");
        }

        if (_accounts.Any(a => a.Name == account.Name))
        {
            throw new InvalidOperationException("帳戶名稱已存在");
        }

        _accounts.Add(account);
        SaveAccounts();
    }

    public void UpdateAccount(string originalName, Account updatedAccount)
    {
        var account = _accounts.FirstOrDefault(a => a.Name == originalName);
        if (account == null)
        {
            throw new KeyNotFoundException("找不到該帳戶");
        }

        account.Name = updatedAccount.Name;
        account.Issuer = updatedAccount.Issuer;
        account.CustomString = updatedAccount.CustomString;
        SaveAccounts();
    }

    public void DeleteAccount(string name)
    {
        var account = _accounts.FirstOrDefault(a => a.Name == name);
        if (account == null)
        {
            throw new KeyNotFoundException("找不到該帳戶");
        }

        _accounts.Remove(account);
        SaveAccounts();
    }

    public string GenerateTotp(string secret)
    {
        try
        {
            var secretBytes = Base32Encoding.ToBytes(secret);
            var totp = new Totp(secretBytes);
            return totp.ComputeTotp();
        }
        catch
        {
            throw new ArgumentException("無效的 Secret Key");
        }
    }

    public int GetRemainingSeconds()
    {
        var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        return 30 - (int)(currentTime % 30);
    }

    private void LoadAccounts()
    {
        if (File.Exists(_accountsFilePath))
        {
            try
            {
                var json = File.ReadAllText(_accountsFilePath);
                _accounts = JsonSerializer.Deserialize<List<Account>>(json) ?? new List<Account>();
            }
            catch
            {
                _accounts = new List<Account>();
            }
        }
    }

    private void SaveAccounts()
    {
        var json = JsonSerializer.Serialize(_accounts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_accountsFilePath, json);
    }

    public int ImportFromGoogleAuthenticator(string imagePath)
    {
        // 取得 decodeGoogleOTP 工具路徑
        string decoderPath = ExtractEmbeddedDecoder();

        // 建立暫存輸出檔案路徑
        string outputCsvPath = Path.Combine(
            Path.GetTempPath(), 
            $"google_otp_output_{Guid.NewGuid()}.csv");

        try
        {
            // 執行解碼程式
            var processInfo = new ProcessStartInfo
            {
                FileName = decoderPath,
                Arguments = $"-i \"{imagePath}\" -c \"{outputCsvPath}\"",
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
            if (!File.Exists(outputCsvPath))
            {
                throw new Exception("解碼程式未產生輸出檔案");
            }

            var lines = File.ReadAllLines(outputCsvPath);

            if (lines.Length <= 1)
            {
                return 0; // 沒有找到任何帳戶
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
                        // 檢查帳戶名稱是否已存在，如果存在則跳過
                        if (!_accounts.Any(a => a.Name == name))
                        {
                            _accounts.Add(new Account
                            {
                                Name = name,
                                Secret = secret,
                                Issuer = string.IsNullOrEmpty(issuer) ? null : issuer
                            });
                            addedCount++;
                        }
                    }
                }
            }

            if (addedCount > 0)
            {
                SaveAccounts();
            }

            return addedCount;
        }
        finally
        {
            // 清理暫存檔案
            try
            {
                if (File.Exists(outputCsvPath))
                {
                    File.Delete(outputCsvPath);
                }
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }
            catch { }
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
