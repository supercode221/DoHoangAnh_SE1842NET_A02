using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement_AnalyticsAPI.DAL.Entities;

public partial class SystemAccount
{
    [Key]
    public short AccountId { get; set; }

    [Required(ErrorMessage = "Account Name can not be blank or null")]
    [StringLength(100, ErrorMessage = "Account Name max length is 100")]
    public string? AccountName { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email format (abc@xyz.com)"), StringLength(70, ErrorMessage = "Email max length is 70 characters")]
    public string? AccountEmail { get; set; }

    [Required(ErrorMessage = "Role can not be null")]
    [Range(1, 2, ErrorMessage = "Role identifier must in range 1 (Staff), 2 (Lecturer)")]
    public int? AccountRole { get; set; }

    [Required(ErrorMessage = "Password cannot be blank or null")]
    [StringLength(70, ErrorMessage = "Password max length is 70 characters")]
    public string? AccountPassword { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}