using Microsoft.AspNetCore.Mvc;
using Service.Dtos.FlowerDtos;
using Service.Dtos;
using Service.Services.Interfaces;
using Service.Dtos.SliderDtos;
using Microsoft.AspNetCore.Authorization;

namespace ApiProject.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class SlidersController : Controller
    {
        private readonly ISliderService _sliderService;

        public SlidersController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        [HttpPost("")]
        public ActionResult<int> Create(SliderCreateDto createDto)
        {
            var id = _sliderService.Create(createDto);
            return StatusCode(201, new { id });
        }

        [HttpGet("")]
        public ActionResult<PaginatedList<SliderGetDto>> GetAll(string? search = null, int page = 1, int size = 3)
        {
            return StatusCode(200, _sliderService.GetAllByPage(search, page, size));
        }

        [HttpGet("{id}")]
        public ActionResult<SliderGetDto> GetById(int id)
        {
            var result = _sliderService.GetById(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public ActionResult Edit(int id, SliderEditDto editDto)
        {
            _sliderService.Edit(id, editDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _sliderService.Delete(id);
            return NoContent();
        }
    }
}
