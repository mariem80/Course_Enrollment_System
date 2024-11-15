using EnrollmentSystem.API.ApiResponses;
using EnrollmentSystem.Application.DTOs;
using EnrollmentSystem.Application.Services;
using EnrollmentSystem.Core.Entities;
using EnrollmentSystem.Infrastructure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace EnrollmentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[EnableCors("DisallowGet")]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _courseService;
        private readonly EnrollmentService _enrollmentService;

        public CourseController(CourseService courseService, EnrollmentService enrollmentService)
        {
            _courseService = courseService;
            _enrollmentService = enrollmentService;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddCourse([FromBody] CourseDto courseDto)
        {
            var result = await _courseService.AddCourseAsync(courseDto);
            if (result)
            {
                return Ok(new ApiResponse(true, ConstantsClass.Success, null));
            }
            else
            {
                return BadRequest(new ApiResponse(false, null, ConstantsClass.CourseAlreadyExists));
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateCourse(int id, [FromBody] UpdateCourseDto updatedCourseDto)
        {
            var result = await _courseService.UpdateCourseAsync(id, updatedCourseDto);

            if (result)
            {
                return Ok(new ApiResponse(true, new { ConstantsClass.Success }, null));
            }
            return NotFound(new ApiResponse(false, null, ConstantsClass.NotFoundMessage));


        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteCourse(int id)
        {
            var result = await _courseService.DeleteCourseAsync(id);

            if (result)
                return Ok(new ApiResponse(true, ConstantsClass.Success , null));

            return NotFound(new ApiResponse(false, null, ConstantsClass.NotFoundMessage));

        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            if (courses != null)
            {
                return Ok(new ApiResponse(true, courses, null));
            }
            else
            {
                return NotFound(new ApiResponse(false, null, ConstantsClass.NotFoundMessage));
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetCourseById(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);

            if (course != null)
            {
                return Ok(new ApiResponse(true, course, null));
            }
            else
            {
                return NotFound(new ApiResponse(false, null, ConstantsClass.NotFoundMessage));
            }
        }

        [HttpGet("courses/{Courseid}/enrollments", Name = "GetStudentsByCourseId")]
        public async Task<ActionResult<ApiResponse>> GetCourseEnrollmentsById(int Courseid)
        {
            var studentenrollments = await _enrollmentService.GetEnrollmentsForCourse(Courseid);

            if (studentenrollments != null)
            {
                return Ok(new ApiResponse(true, studentenrollments, null));
            }

            return NotFound(new ApiResponse(false, null, ConstantsClass.NotFoundMessage));


        }





    }
}
