using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

public class BookService : IBookService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public BookService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<List<BookResponse>> GetAllBooks(bool? isRead, 
    int? categoryId, string? title, string? sortBy, 
    bool? descending, int page, int pageSize)
    {
        IQueryable<Book> query = _context.Books;

        if(isRead.HasValue)
            query = query.Where(x => x.IsRead == isRead.Value);

        if(categoryId.HasValue)
            query = query.Where(x => x.CategoryId == categoryId.Value);
        
        if(!string.IsNullOrWhiteSpace(title))
            query = query.Where(x => x.Title.Contains(title));

        if(!string.IsNullOrWhiteSpace(sortBy))
        {
            switch(sortBy)
            {
                case "title":
                if(descending.HasValue && descending.Value)
                    query = query.OrderByDescending(x => x.Title);
                else    
                    query = query.OrderBy(x => x.Title);
                break;

                case "author":
                if(descending.HasValue && descending.Value)
                    query = query.OrderByDescending(x => x.Author);
                else
                    query = query.OrderBy(x => x.Author);
                break;

                case "id":
                if(descending.HasValue && descending.Value)
                    query = query.OrderByDescending(x => x.Id);
                else
                    query = query.OrderBy(x => x.Id);
                break;

                default:
                break;
            }
        }                    

        var currentPage = page;
        if(currentPage < 1)
                currentPage = 1;

            int currentPageSize = pageSize;
            int maxPageSize = _configuration.GetValue<int>("LibrarySettings:MaxPageSize");
            if(currentPageSize > maxPageSize)
                currentPageSize = maxPageSize;
            query = query.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize);
       

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

    public async Task<(string, BookResponse?)> UpdateBook(int id, UpdateBookRequest request)
    {
        var book = await _context.Books.Where(x => x.Id == id).FirstOrDefaultAsync();

        if(book is null)
            return ("Book not found", null);

        Category? category = await _context.Categories.FindAsync(request.CategoryId);

        if(category is null)
            return ("Category not found", null);

        book.Title = request.Title;
        book.Author = request.Author;
        book.IsRead = request.IsRead;
        book.Rating = request.Rating;
        book.CategoryId = request.CategoryId;
        book.Category = category;

        await _context.SaveChangesAsync();

        return ("", new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            IsRead = book.IsRead,
            Rating = book.Rating,
            CategoryId = book.CategoryId,
            CategoryName = book.Category.Name
        });
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