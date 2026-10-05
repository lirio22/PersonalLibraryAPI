public class UpdateCategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public List<Book> Books { get; set; } = new List<Book>();
}