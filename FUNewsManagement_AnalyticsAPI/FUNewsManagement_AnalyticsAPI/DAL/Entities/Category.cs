using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement_AnalyticsAPI.DAL.Entities;

public partial class Category
{
    [Key]
    public short CategoryId { get; set; }

    [Required(ErrorMessage = "Cateogory Name can not be blank or null"), StringLength(100, ErrorMessage = "Category Name max length is 100")]
    public string CategoryName { get; set; } = null!;

    [Required(ErrorMessage = "Cateogory Description can not be blank or null"), StringLength(250, ErrorMessage = "Category Name max length is 250")]
    public string CategoryDesciption { get; set; } = null!;

    public short? ParentCategoryId { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Category> InverseParentCategory { get; set; } = new List<Category>();

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();

    public virtual Category? ParentCategory { get; set; }
}