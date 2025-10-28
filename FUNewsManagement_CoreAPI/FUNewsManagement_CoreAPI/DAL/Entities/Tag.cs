using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement_CoreAPI.DAL.Entities;

public partial class Tag
{
    [Key]
    public int TagId { get; set; }

    public string? TagName { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
