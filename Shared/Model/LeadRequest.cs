
using System.ComponentModel.DataAnnotations;

public class LeadRequest
{
    [Required]
    public string FullName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Phone { get; set; }

    public string Company { get; set; }
    public string Message { get; set; }
    public bool NDA { get; set; }

    public string Source { get; set; }
    public string PageUrl { get; set; }
}





