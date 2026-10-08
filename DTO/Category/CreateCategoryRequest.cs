using System.ComponentModel.DataAnnotations;

public class CreateCategoryRequest
{
    [Required(ErrorMessage = "The Name field is required.")]
    [StringLength(50, ErrorMessage = "The Name field must be between 2 and 50 characters long.", MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}