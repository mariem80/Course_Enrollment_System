using EnrollmentSystem.API.ApiResponses;
using EnrollmentSystem.Application.DTOs;
using EnrollmentSystem.Application.Services;
using EnrollmentSystem.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace EnrollmentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly EnrollmentService _enrollmentService;


        public EnrollmentController(EnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost]
        [Route("addEnrollment")]
        public async Task<ActionResult<ApiResponse>> AddEnrollment([FromBody] EnrollmentDto enrollmentDto)
        {

            var result = await _enrollmentService.AddEnrollmentAsync(enrollmentDto);

            if (result)
                return Ok(new ApiResponse(true, new { ConstantsClass.Success }, null));

            return BadRequest(new ApiResponse(false, null, ConstantsClass.Exists));

        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<ApiResponse>> GetAllEnrollments()
        {
            var result = await _enrollmentService.GetAllEnrollmentsAsync();
            if (result != null)
                return Ok(new ApiResponse(true, result, null));
            return NotFound(new ApiResponse(false, null, ConstantsClass.NotFoundMessage));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteEnrollment(int id)
        {
            var result = await _enrollmentService.DeleteEnrollmentAsync(id);
            if (result)
                return Ok(new ApiResponse(true, ConstantsClass.Success, null));
            return NotFound(new ApiResponse(false, null, ConstantsClass.NotFoundMessage));


        }
    }
}
