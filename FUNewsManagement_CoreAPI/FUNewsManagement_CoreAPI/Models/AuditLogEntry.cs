using System.Text.Json;

namespace FUNewsManagement_CoreAPI.Models
{
    public class AuditLogEntry
    {
        public string Timestamp { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Entity { get; set; } = string.Empty;
        public JsonElement? Before { get; set; }
        public JsonElement? After { get; set; }
        public string? IP { get; set; }
    }
}
