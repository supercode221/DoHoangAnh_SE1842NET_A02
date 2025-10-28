using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement_AnalyticsAPI.DAL.Entities;

public partial class NewsArticle
{
    [Key]
    public string NewsArticleId { get; set; } = null!;

    [Required(ErrorMessage = "News Title can not be blank or null"), StringLength(400, ErrorMessage = "News Title max length is 400 characters")]
    public string? NewsTitle { get; set; }

    [Required(ErrorMessage = "News Head Line can not be blank or null"), StringLength(150, ErrorMessage = "News Head Line max length is 150 characters")]
    public string Headline { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    [Required(ErrorMessage = "News Content can not be blank or null"), StringLength(4000, ErrorMessage = "News Content max length is 4000 characters")]
    public string? NewsContent { get; set; }

    [StringLength(400, ErrorMessage = "News Source max length is 400 characters")]
    public string? NewsSource { get; set; }

    public short? CategoryId { get; set; }

    [Required]
    [AllowedValues([true, false], ErrorMessage = "News status just can be true or false")]
    public bool? NewsStatus { get; set; }

    public short? CreatedById { get; set; }

    public short? UpdatedById { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Category? Category { get; set; }

    public virtual SystemAccount? CreatedBy { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
