using EnrollmentSystem.API.ApiResponses;
using EnrollmentSystem.Application.DTOs;
using EnrollmentSystem.Application.Services;
using EnrollmentSystem.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EnrollmentSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;
        private readonly EnrollmentService _enrollmentService;
        private readonly CourseService _courseService;
        private readonly IConfiguration _configuration;

        public StudentController(StudentService studentService, EnrollmentService enrollmentService, CourseService courseService, IConfiguration configuration)
        {
            _studentService = studentService;
            _enrollmentService = enrollmentService;
            _courseService = courseService;
            _configuration = configuration;

        }
        [HttpGet("GetStudents")]
        public async Task<ActionResult<ApiResponse>> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(new ApiResponse(true, students, null));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);

            if (student != null)
                return Ok(new ApiResponse(true, student, null));

            return NotFound(new ApiResponse(false, null, ConstantsClass.NotFoundMessage));

        }


        [HttpGet("students/{Studentid}/enrollments", Name = "GetCoursesByStudentId")]
        public async Task<ActionResult<ApiResponse>> GetStudentEnrollmentsById(int Studentid)
        {

            var course_enrollments = await _enrollmentService.GetEnrollmentsForStudent(Studentid);

            if (course_enrollments == null)
            {
                return NotFound(new ApiResponse(false, null, "Not Found"));
            }

            return Ok(new ApiResponse(true, course_enrollments, null));

        }

        [HttpGet]
        [Route("GetElectives")]
        public async Task<ActionResult<ApiResponse>> GetElectiveCourses()
        {
            var electiveCourses = await _courseService.GetElectiveCourses();

            if (electiveCourses == null || !electiveCourses.Any())
            {
                return NotFound(new ApiResponse(false, null, "No Elective Courses."));
            }

            return Ok(new ApiResponse(true, electiveCourses, null));


        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddStudent([FromBody] StudentDto studentDto)
        {

            var result = await _studentService.AddStudentAsync(studentDto);
            if (result)
                return Ok(new ApiResponse(true, ConstantsClass.Success, null));

            return BadRequest(new ApiResponse(false, null, ConstantsClass.Exists));


        }
        
        [HttpPost("enroll-elective")]
        public async Task<ActionResult<ApiResponse>> EnrollElective([FromBody] EnrollmentDto request)
        {
            var result = await _enrollmentService.EnrollElectiveAsync(request.StudentID, request.CourseID);

            if (result)
                return Ok(new ApiResponse(true, ConstantsClass.Success, null));

            return BadRequest(new ApiResponse(false, null, ConstantsClass.Exists));

        }


        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateStudent(int id, [FromBody] UpdateStudentDto updatedStudentDto)
        {
            var result = await _studentService.UpdateStudentAsync(id, updatedStudentDto);

            if (result)
                return Ok(new ApiResponse(true, ConstantsClass.Success, null));

            return BadRequest(new ApiResponse(false, null, ConstantsClass.Exists));


        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteStudent(int id)
        {

            var result = await _studentService.DeleteStudentAsync(id);

            if (result)
                return Ok(new ApiResponse(true, ConstantsClass.Success, null));

            return NotFound(new ApiResponse(false, null, ConstantsClass.NotFoundMessage));

        }





    }
}

