using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement_AIAPI.Models
{
    public class TagSuggestionRequest
    {
        [Required]
        [MaxLength(50000)]
        public string Content { get; set; } = string.Empty;
    }
}
