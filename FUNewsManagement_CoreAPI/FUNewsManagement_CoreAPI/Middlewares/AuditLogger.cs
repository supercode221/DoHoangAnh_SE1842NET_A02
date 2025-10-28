using System.Security.Claims;
using System.Text.Json;

namespace FUNewsManagement_CoreAPI.Middlewares
{
    public class AuditLogger
    {
        private static readonly string _auditLogFile;

        static AuditLogger()
        {
            // Get the absolute path to the project root (above /bin)
            var baseDir = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName
                          ?? AppContext.BaseDirectory;

            _auditLogFile = Path.Combine(baseDir, "Logs", "audit-log.txt");
        }

        public static async Task LogAsync(HttpContext context, string action, string entity, object? before = null, object? after = null)
        {
            try
            {
                var userEmail = context.User?.FindFirst(ClaimTypes.Email)?.Value ?? "Anonymous";
                var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

                var logEntry = new
                {
                    Timestamp = timestamp,
                    User = userEmail,
                    Action = action,
                    Entity = entity,
                    Before = before,
                    After = after,
                    IP = context.Connection.RemoteIpAddress?.ToString()
                };

                var logText = JsonSerializer.Serialize(logEntry) + Environment.NewLine;

                var directory = Path.GetDirectoryName(_auditLogFile);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory!);

                await File.AppendAllTextAsync(_auditLogFile, logText);
            }
            catch
            {
                // Never throw inside the audit logger
            }
        }
    }
}
