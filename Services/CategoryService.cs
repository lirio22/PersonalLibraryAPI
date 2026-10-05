using Microsoft.EntityFrameworkCore;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        
        _context = context;
    }

    public async Task<List<CategoryResponse>> GetAllCategoriesAsync()
    {        
        return  await _context.Categories.Select( category => new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Books = category.Books
        }).AsNoTracking().ToListAsync();
    }

    public async Task<CategoryResponse?> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories.Where(category => category.Id == id)
        .Select(category => new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Books = category.Books
        }).AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request)
    {
        Category category = new Category
        {
            Name = request.Name,
            Books = request.Books
        };

        _context.Categories.Add(category);

        await _context.SaveChangesAsync();

        return new CategoryResponse
        {
          Id = category.Id,
          Name = category.Name,
          Books = category.Books  
        };
    }

    public async Task<CategoryResponse?> UpdateCategoryAsync(int id, UpdateCategoryRequest request)
    {
        Category? category = await _context.Categories.FindAsync(id);

        if(category is null)
            return null;

        category.Name = request.Name;
        category.Books = request.Books;

        await _context.SaveChangesAsync();

        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Books = category.Books,
        };
    }

    public async Task<bool?> DeleteCategoryAsync(int id)
    {
        Category? category = await _context.Categories.FindAsync(id);

        if(category is null)
            return null;

        //Avoid deleting categories that still have books in them   
        bool hasBooks = await _context.Books.AnyAsync(book => book.CategoryId == id);

        if(hasBooks)
            return false;

        _context.Categories.Remove(category);

        await _context.SaveChangesAsync();

        return true;
    }
}