﻿using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement_CoreAPI.DAL.Entities;

public partial class SystemAccount
{
    [Key]
    public short AccountId { get; set; }

    public string? AccountName { get; set; }

    public string? AccountEmail { get; set; }

    public int? AccountRole { get; set; }

    public string? AccountPassword { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
