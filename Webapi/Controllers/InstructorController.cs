using EnrollmentSystem.API.ApiResponses;
using EnrollmentSystem.Application.DTOs;
using EnrollmentSystem.Application.Services;
using EnrollmentSystem.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : ControllerBase
    {
        private readonly InstructorService _instructorService;

        public InstructorController(InstructorService instructorService)
        {
            _instructorService = instructorService;

        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllInstructors()
        {
            var instructors = await _instructorService.GetAllInstructorsAsync();
            return Ok(new ApiResponse(true, instructors, null));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetInstructorById(int id)
        {
            var instructorDto = await _instructorService.GetInstructorByIdAsync(id);

            if (instructorDto == null)
            {
                return NotFound(new ApiResponse(false, null, ConstantsClass.NotFoundMessage));
            }

            return Ok(new ApiResponse(true, instructorDto, null));

        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddInstructor([FromBody] InstructorDto instructorDto)
        {

            var result = await _instructorService.AddInstructorAsync(instructorDto);
            if (result)
                return Ok(new ApiResponse(true, ConstantsClass.Success, null));

            return BadRequest(new ApiResponse(false, null, ConstantsClass.Exists));

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateInstructor(int id, [FromBody] UpdateInstructorDto updatedInstructorDto)
        {

            var result = await _instructorService.UpdateInstructorAsync(id, updatedInstructorDto);

            if (result)
                return Ok(new ApiResponse(true, ConstantsClass.Success, null));

            return BadRequest(new ApiResponse(false, null, ConstantsClass.Exists));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteInstructor(int id)
        {

            var result = await _instructorService.DeleteInstructorAsync(id);

            if (result)
                return Ok(new ApiResponse(true, ConstantsClass.Success, null));

            return NotFound(new ApiResponse(false, null, ConstantsClass.NotFoundMessage));
        }

    }
}
