using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet()]
    public async Task<ActionResult<List<CategoryResponse>>> GetAllCategories()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponse>> GetCategoryById(int id)
    {
        var response = await _categoryService.GetCategoryByIdAsync(id);

        if(response is null)
            return NotFound();            

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> CreateCategory(CreateCategoryRequest request)
    {
        var createdCategory = await _categoryService.CreateCategoryAsync(request);

        return Created($"/api/categories/{createdCategory.Id}", createdCategory);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryResponse>> UpdateCategory(int id, UpdateCategoryRequest request)
    {
        var category = await _categoryService.UpdateCategoryAsync(id, request);

        if(category is null)
            return NotFound();

        return Ok(category);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        bool? deleted = await _categoryService.DeleteCategoryAsync(id);

        if(deleted is null)
            return NotFound();
        
        if(deleted is false)
            return BadRequest("Remove all books from this category before deleting it.");

        return NoContent();
    }
}