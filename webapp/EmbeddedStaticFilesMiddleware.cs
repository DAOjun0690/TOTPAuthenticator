using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace TOTPAuthenticatorWeb;

public class EmbeddedStaticFilesMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private readonly ManifestEmbeddedFileProvider _fileProvider;
    private readonly Dictionary<string, string> _contentTypes = new()
    {
        { ".html", "text/html; charset=utf-8" },
        { ".css", "text/css; charset=utf-8" },
        { ".js", "application/javascript; charset=utf-8" },
        { ".json", "application/json" },
        { ".png", "image/png" },
        { ".jpg", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".gif", "image/gif" },
        { ".svg", "image/svg+xml" },
        { ".ico", "image/x-icon" }
    };

    public EmbeddedStaticFilesMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
        _fileProvider = new ManifestEmbeddedFileProvider(Assembly.GetExecutingAssembly(), "wwwroot");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? "/";

        // 如果是根路徑，重定向到 index.html
        if (path == "/")
        {
            path = "/index.html";
        }

        // 移除開頭的斜線
        var filePath = path.TrimStart('/');

        // 嘗試從嵌入式資源中獲取檔案
        var fileInfo = _fileProvider.GetFileInfo(filePath);

        if (fileInfo.Exists && !fileInfo.IsDirectory)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            var contentType = _contentTypes.ContainsKey(extension)
                ? _contentTypes[extension]
                : "application/octet-stream";

            context.Response.ContentType = contentType;
            context.Response.ContentLength = fileInfo.Length;

            using var stream = fileInfo.CreateReadStream();
            await stream.CopyToAsync(context.Response.Body);
            return;
        }

        // 如果找不到檔案，繼續到下一個中間件
        await _next(context);
    }
}

public static class EmbeddedStaticFilesMiddlewareExtensions
{
    public static IApplicationBuilder UseEmbeddedStaticFiles(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<EmbeddedStaticFilesMiddleware>();
    }
}
