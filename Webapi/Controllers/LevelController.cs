using EnrollmentSystem.API.ApiResponses;
using EnrollmentSystem.Application.DTOs;
using EnrollmentSystem.Application.Services;
using EnrollmentSystem.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LevelController : ControllerBase
    {
        private readonly LevelService _levelService;

        public LevelController(LevelService levelService)
        {
            _levelService = levelService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllLevels()
        {
            var levels = await _levelService.GetAllLevelsAsync();
            return Ok(new ApiResponse(true, levels, null));

        }

        [HttpGet("{levelId}")]
        public async Task<ActionResult<ApiResponse>> GetLevelById(int levelId)
        {
            var levelDto = await _levelService.GetLevelByIdAsync(levelId);

            if (levelDto != null)
                return Ok(new ApiResponse(true, levelDto, null));
            return NotFound(new ApiResponse(false, null, ConstantsClass.NotFoundMessage));



        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddLevel([FromBody] LevelDto levelDto)
        {

            var result = await _levelService.AddLevelAsync(levelDto);

            if (result)
                return Ok(new ApiResponse(true, ConstantsClass.Success, null));

            return BadRequest(new ApiResponse(false, null, ConstantsClass.Exists));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateLevel(int id, [FromBody] UpdateLevelDto updatedLevelDto)
        {
            var result = await _levelService.UpdateLevelAsync(id, updatedLevelDto);
            if (result)
                return Ok(new ApiResponse(true, ConstantsClass.Success, null));

            return BadRequest(new ApiResponse(false, null, ConstantsClass.Exists));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteLevel(int id)
        {
            var result = await _levelService.DeleteLevelAsync(id);

            if (result)
                return Ok(new ApiResponse(true, ConstantsClass.Success, null));

            return BadRequest(new ApiResponse(false, null, ConstantsClass.NotFoundMessage));
        }


    }
}
