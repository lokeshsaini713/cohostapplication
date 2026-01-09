using System.ComponentModel.DataAnnotations;

public class Article
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string ShortDescription { get; set; }

    public string Category { get; set; } // Tech / Press

    public string ImagePath { get; set; }

    public DateTime PublishedDate { get; set; } = DateTime.Now;

    public bool IsActive { get; set; } = true;
}



