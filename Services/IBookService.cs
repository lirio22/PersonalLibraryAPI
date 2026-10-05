public interface IBookService
{
    Task<List<BookResponse>> GetAllBooks();
    Task<BookResponse?> GetBookById(int id);
    Task<BookResponse?> CreateBook(CreateBookRequest request);
    Task<BookResponse?> UpdateBook(int id, UpdateBookRequest request);
    Task<bool> DeleteBook(int id);
}