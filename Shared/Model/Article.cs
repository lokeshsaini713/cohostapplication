public class Article
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Slug { get; set; }

    public string ShortDescription { get; set; }

    public string Content { get; set; }   // ✅ Rich text

    public string Category { get; set; }

    public string ImagePath { get; set; }

    public int SortOrder { get; set; }
    public DateTime PublishedDate { get; set; } = DateTime.Now;

    public bool IsActive { get; set; } = true;

    public string MetaTitle { get; set; }
    public string MetaDescription { get; set; }
    public string MetaKeywords { get; set; }
}





