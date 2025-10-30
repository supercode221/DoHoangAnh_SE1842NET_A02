using System.Text.Json;
using FUNewsManagement_CoreAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagement_CoreAPI.Controllers
{
    [Route("api/auditlog")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class AuditLogController : ControllerBase
    {
        private readonly string _auditLogFile;

        public AuditLogController()
        {
            var baseDir = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName
                          ?? AppContext.BaseDirectory;
            _auditLogFile = Path.Combine(baseDir, "Logs", "audit-log.txt");
        }

        [HttpGet]
        public async Task<IActionResult> GetAuditLogs(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string? action,
            [FromQuery] string? entity)
        {
            try
            {
                if (!System.IO.File.Exists(_auditLogFile))
                {
                    return Ok(new { logs = new List<object>() });
                }

                var lines = await System.IO.File.ReadAllLinesAsync(_auditLogFile);
                var logs = new List<AuditLogEntry>();

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    try
                    {
                        var log = JsonSerializer.Deserialize<AuditLogEntry>(line);
                        if (log != null)
                        {
                            // Apply filters
                            if (fromDate.HasValue && DateTime.Parse(log.Timestamp) < fromDate.Value)
                                continue;

                            if (toDate.HasValue && DateTime.Parse(log.Timestamp) > toDate.Value.AddDays(1))
                                continue;

                            if (!string.IsNullOrEmpty(action) && !log.Action.Equals(action, StringComparison.OrdinalIgnoreCase))
                                continue;

                            if (!string.IsNullOrEmpty(entity) && !log.Entity.Equals(entity, StringComparison.OrdinalIgnoreCase))
                                continue;

                            logs.Add(log);
                        }
                    }
                    catch { }
                }

                // Sort by timestamp descending (newest first)
                logs = logs.OrderByDescending(x => x.Timestamp).ToList();

                return Ok(new { logs });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error reading audit logs", error = ex.Message });
            }
        }
    }
}
