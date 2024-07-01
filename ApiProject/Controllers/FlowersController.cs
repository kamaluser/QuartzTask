using Microsoft.AspNetCore.Mvc;
using Service.Dtos.FlowerDtos;
using Service.Dtos;
using Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ApiProject.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class FlowersController : ControllerBase
    {
        private readonly IFlowerService _flowerService;

        public FlowersController(IFlowerService flowerService)
        {
            _flowerService = flowerService;
        }

        [HttpPost("")]
        public ActionResult<int> Create(FlowerCreateDto createDto)
        {
            var id = _flowerService.Create(createDto);
            return StatusCode(201, new { id });
        }

        [HttpGet("")]
        public ActionResult<PaginatedList<FlowerGetDto>> GetAll(string? search = null, int page = 1, int size = 3)
        {
            return StatusCode(200, _flowerService.GetAllByPage(search, page, size));
        }

        [HttpGet("{id}")]
        public ActionResult<FlowerGetDto> GetById(int id)
        {
            var result = _flowerService.GetById(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public ActionResult Edit(int id, FlowerEditDto editDto)
        {
            _flowerService.Edit(id, editDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _flowerService.Delete(id);
            return NoContent();
        }
    }
}
