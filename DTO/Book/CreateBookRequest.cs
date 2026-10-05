using System.ComponentModel.DataAnnotations;

public class CreateBookRequest
{
    [Required(ErrorMessage = "The Title field is required.")]
    [StringLength(150, ErrorMessage = "The Title field must be between 2 and 150characters long.", MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Author field is required.")]
    [StringLength(100, ErrorMessage = "The Author field must be between 2 and 100 characters long.", MinimumLength = 2)]
    public string Author { get; set;} = string.Empty;
    public bool IsRead { get; set; }

    [Range(1, 5, ErrorMessage = "The Rating field must be between 1 and 5.")]
    public int? Rating { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}