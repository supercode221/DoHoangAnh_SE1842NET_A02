namespace FUNewsManagement_AIAPI.Models
{
    public class TagSuggestionResponse
    {
        public List<string> Tags { get; set; } = new();
        public int Count => Tags.Count;
        public bool Cached { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
