using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement_AnalyticsAPI.DAL.Entities;

public partial class Tag
{
    [Key]
    public int TagId { get; set; }

    [Required(ErrorMessage = "Tag Name can not be blank or null")]
    [StringLength(50, ErrorMessage = "Tag Name max length is 50 characters")]
    public string? TagName { get; set; }

    [StringLength(400, ErrorMessage = "Tag Note max length is 400 characters")]
    public string? Note { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
