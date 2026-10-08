using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/books")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;
    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<List<BookResponse>>> GetAllBooks([FromQuery] bool? isRead, 
    [FromQuery] int? categoryId, [FromQuery] string? title, [FromQuery] string? sortBy, 
    [FromQuery] bool? descending, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var books = await _bookService.GetAllBooks(isRead, categoryId, 
        title, sortBy, descending, page, pageSize);

        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookResponse>> GetBookById(int id)
    {
        var book = await _bookService.GetBookById(id);

        if(book is null)
            return NotFound();
        
        return Ok(book);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<BookResponse>> CreateBook(CreateBookRequest request)
    {
        var book = await _bookService.CreateBook(request);

        if(book is null)
            return BadRequest("Category not found.");
        
        return Created($"/api/books/{book.Id}", book);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<ActionResult<BookResponse>> UpdateBook(int id, UpdateBookRequest request)
    {
        (string, BookResponse?) book = await _bookService.UpdateBook(id, request);

        if(book.Item2 is null)
        {
            if(book.Item1 == "Book not found")
                return NotFound(book.Item1);
            
            if(book.Item1 == "Category not found")
                return BadRequest(book.Item1);
            
            return BadRequest(book.Item1);
        }
            

        return Ok(book.Item2);
    }

    [HttpDelete("{id:int}")]
    [Authorize( Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var deleted = await _bookService.DeleteBook(id);

        if(deleted is false)
            return NotFound();
        
        return NoContent();
    }
}