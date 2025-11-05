using TOTPAuthenticatorWeb;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// 設定預設監聽端口為 5044 (生產環境)
// 開發模式會使用 launchSettings.json 的設定
if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls("http://localhost:5044", "http://0.0.0.0:5044");
}

// Add services to the container
builder.Services.AddSingleton<AccountService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();

// 檢查 wwwroot 資料夾是否存在（開發模式）或使用嵌入式資源（發布模式）
var wwwrootPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot");
if (Directory.Exists(wwwrootPath))
{
    // 開發模式：使用檔案系統
    app.UseDefaultFiles();
    app.UseStaticFiles();
}
else
{
    // 發布模式：使用嵌入式資源
    app.UseEmbeddedStaticFiles();
}

var accountService = app.Services.GetRequiredService<AccountService>();

// API Endpoints
app.MapGet("/api/accounts", () =>
{
    return Results.Ok(accountService.GetAllAccounts());
});

app.MapGet("/api/accounts/{name}", (string name) =>
{
    var account = accountService.GetAccount(name);
    return account != null ? Results.Ok(account) : Results.NotFound();
});

app.MapPost("/api/accounts", ([FromBody] Account account) =>
{
    try
    {
        accountService.AddAccount(account);
        return Results.Created($"/api/accounts/{account.Name}", account);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { error = ex.Message });
    }
});

app.MapPut("/api/accounts/{name}", (string name, [FromBody] Account account) =>
{
    try
    {
        accountService.UpdateAccount(name, account);
        return Results.Ok(account);
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
});

app.MapDelete("/api/accounts/{name}", (string name) =>
{
    try
    {
        accountService.DeleteAccount(name);
        return Results.NoContent();
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
});

app.MapGet("/api/totp/{name}", (string name) =>
{
    var account = accountService.GetAccount(name);
    if (account == null)
    {
        return Results.NotFound();
    }

    try
    {
        var totp = accountService.GenerateTotp(account.Secret);
        var remainingSeconds = accountService.GetRemainingSeconds();
        return Results.Ok(new { totp, remainingSeconds });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapGet("/api/time", () =>
{
    return Results.Ok(new { remainingSeconds = accountService.GetRemainingSeconds() });
});

app.MapPost("/api/import/google-authenticator", async (HttpRequest request) =>
{
    try
    {
        if (!request.HasFormContentType || request.Form.Files.Count == 0)
        {
            return Results.BadRequest(new { error = "請上傳圖片檔案" });
        }

        var file = request.Form.Files[0];

        // 驗證檔案類型
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
        {
            return Results.BadRequest(new { error = "不支援的檔案格式，請上傳圖片檔案" });
        }

        // 保存上傳的檔案到暫存目錄
        var tempPath = Path.Combine(Path.GetTempPath(), $"google_auth_{Guid.NewGuid()}{extension}");

        using (var stream = new FileStream(tempPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // 呼叫 AccountService 處理匯入
        int addedCount = accountService.ImportFromGoogleAuthenticator(tempPath);

        if (addedCount > 0)
        {
            return Results.Ok(new { message = $"成功匯入 {addedCount} 個帳戶", addedCount });
        }
        else
        {
            return Results.Ok(new { message = "圖片中未找到任何 OTP 帳戶資料", addedCount = 0 });
        }
    }
    catch (FileNotFoundException ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.Run();
