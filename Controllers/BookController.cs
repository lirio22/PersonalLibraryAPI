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
    [FromQuery] bool? descending, [FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var books = await _bookService.GetAllBooks(isRead, categoryId, 
        title, sortBy, descending, page, pageSize);

        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookResponse>> GetBookById(int id)
    {
        var book = await _bookService.GetBookById(id);

        if(book is null)
            return NotFound();
        
        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<BookResponse>> CreateBook(CreateBookRequest request)
    {
        var book = await _bookService.CreateBook(request);

        if(book is null)
            return BadRequest("Category not found.");
        
        return Created($"/api/books/{book.Id}", book);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BookResponse>> UpdateBook(int id, UpdateBookRequest request)
    {
        var book = await _bookService.UpdateBook(id, request);

        if(book is null)
            return NotFound();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var deleted = await _bookService.DeleteBook(id);

        if(deleted is false)
            return NotFound();
        
        return NoContent();
    }
}