public class UpdateBookRequest
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set;} = string.Empty;
    public bool IsRead { get; set; }
    public int? Rating { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}