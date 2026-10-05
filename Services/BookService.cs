using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

public class BookService : IBookService
{
    private readonly AppDbContext _context;
    public BookService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookResponse>> GetAllBooks()
    {
        return await _context.Books.AsNoTracking()
        .Select(book => new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            IsRead = book.IsRead,
            Rating = book.Rating,
            CategoryId = book.CategoryId,
            CategoryName = book.Category.Name
        }).ToListAsync();
    }

    public async Task<BookResponse?> GetBookById(int id)
    {
        return await _context.Books.AsNoTracking()
        .Where(book => book.Id == id)
        .Select(book => new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            IsRead = book.IsRead,
            Rating = book.Rating,
            CategoryId = book.CategoryId,
            CategoryName = book.Category.Name
        })
        .FirstOrDefaultAsync();
    }

    public async Task<BookResponse?> CreateBook(CreateBookRequest request)
    {
        Category? category = await _context.Categories.FindAsync(request.CategoryId);

        if(category is null)
            return null;
        
        Book book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            IsRead = request.IsRead,
            Rating = request.Rating,
            CategoryId = request.CategoryId,
            Category = category
        };

        _context.Books.Add(book);

        await _context.SaveChangesAsync();

        return new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            IsRead = book.IsRead,
            Rating = book.Rating,
            CategoryId = book.CategoryId,
            CategoryName = book.Category.Name
        };
    }

    public async Task<BookResponse?> UpdateBook(int id, UpdateBookRequest request)
    {
        var book = await _context.Books.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();

        if(book is null)
            return null;

        book.Title = request.Title;
        book.Author = request.Author;
        book.IsRead = request.IsRead;
        book.Rating = request.Rating;
        book.CategoryId = request.CategoryId;
        book.Category = request.Category;

        await _context.SaveChangesAsync();

        return new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            IsRead = book.IsRead,
            Rating = book.Rating,
            CategoryId = book.CategoryId,
            CategoryName = book.Category.Name
        };
    }

    public async Task<bool> DeleteBook(int id)
    {
        var book = await _context.Books.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();

        if(book is null)
            return false;

        _context.Remove(book);

        await _context.SaveChangesAsync();

        return true;
    }
}