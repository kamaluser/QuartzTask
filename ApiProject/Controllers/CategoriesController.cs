using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos;
using Service.Dtos.CategoryDtos;
using Service.Services.Interfaces;

namespace ApiProject.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpPost("")]
        public ActionResult Create(CategoryCreateDto createDto)
        {
            var id = _categoryService.Create(createDto);
            return StatusCode(201, new { id });
        }

        [HttpGet("")]
        public ActionResult<PaginatedList<CategoryGetDto>> GetAll(string? search = null, int page = 1, int size = 3)
        {
            return StatusCode(200, _categoryService.GetAllByPage(search, page, size));
        }

        [HttpGet("all")]
        public ActionResult<List<CategoryGetDto>> GetAllGroups(string? search = null)
        {
            return StatusCode(200, _categoryService.GetAll(search));
        }

        [HttpGet("{id}")]
        public ActionResult<CategoryGetDto> GetById(int id)
        {
            var result = _categoryService.GetById(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public ActionResult Edit(int id, CategoryEditDto editDto)
        {
            _categoryService.Edit(id, editDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _categoryService.Delete(id);
            return NoContent();
        }
    }
}
