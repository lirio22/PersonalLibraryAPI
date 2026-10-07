using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

public class BookService : IBookService
{
    private readonly AppDbContext _context;
    public BookService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookResponse>> GetAllBooks(bool? isRead, 
    int? categoryId, string? title, string? sortBy, 
    bool? descending, int? page, int? pageSize)
    {
        IQueryable<Book> query = _context.Books;

        if(isRead.HasValue)
            query = query.Where(x => x.IsRead == isRead.Value);

        if(categoryId.HasValue)
            query = query.Where(x => x.CategoryId == categoryId.Value);
        
        if(!string.IsNullOrWhiteSpace(title))
            query = query.Where(x => x.Title == title);

        if(!string.IsNullOrWhiteSpace(sortBy))
        {
            switch(sortBy)
            {
                case "title":
                query = query.OrderBy(x => x.Title);
                break;

                case "author":
                query = query.OrderBy(x => x.Author);
                break;

                case "id":
                query = query.OrderBy(x => x.Id);
                break;

                default:
                break;
            }
        }

        if(descending.HasValue)
            query = query.Reverse();

        if(page.HasValue && pageSize.HasValue)
            query = query.Skip(page.Value * pageSize.Value).Take(pageSize.Value);

        return await query.AsNoTracking()
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
        var books = await _context.Books.AsNoTracking()
        .Where(book => book.Id == id)
        .Include(x => x.Category)
        .FirstOrDefaultAsync();

        if(books is null)
            return null;

        return new BookResponse
        {
            Id = books.Id,
            Title = books.Title,
            Author = books.Author,
            IsRead = books.IsRead,
            Rating = books.Rating,
            CategoryId = books.CategoryId,
            CategoryName = books.Category.Name
        };
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