public interface IBookService
{
    Task<List<BookResponse>> GetAllBooks(bool? isRead, int? categoryId, 
    string? title, string? sortBy, bool? descending, int page, int pageSize);
    Task<BookResponse?> GetBookById(int id);
    Task<BookResponse?> CreateBook(CreateBookRequest request);
    Task<(string, BookResponse?)> UpdateBook(int id, UpdateBookRequest request);
    Task<bool> DeleteBook(int id);
}