using AutoMapper;
using EnrollmentSystem.API.ApiResponses;
using EnrollmentSystem.Application.DTOs;
using EnrollmentSystem.Application.Services;
using EnrollmentSystem.Core.Entities;
using EnrollmentSystem.Core.Repositries.Base;
using EnrollmentSystem.Core.UnitOfWork;
using EnrollmentSystem.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentSystem.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentService _departmentService;

        public DepartmentController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddDepartment([FromBody] DepartmentDto departmentDto)
        {
            var result = await _departmentService.AddDepartmentAsync(departmentDto);
            if (result)
                return Ok(new ApiResponse(true, new { ConstantsClass.Success }, null));
            return BadRequest(new ApiResponse(false, null, ConstantsClass.Exists));

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateDepartment(int id, [FromBody] DepartmentDto updatedDepartmentDto)
        {

            var result = await _departmentService.UpdateDepartmentAsync(id, updatedDepartmentDto);

            if (result)

                return Ok(new ApiResponse(true, ConstantsClass.Success, null));

            return BadRequest(new ApiResponse(false, null, ConstantsClass.Exists));


        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteDepartment(int id)
        {
            var result = await _departmentService.DeleteDepartmentAsync(id);

            if (result)
                return Ok(new ApiResponse(true, ConstantsClass.Success, null));

            return NotFound(new ApiResponse(false, null, ConstantsClass.NotFoundMessage));


        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            if (departments != null)
                return Ok(new ApiResponse(true, departments, null));

            return NotFound(new ApiResponse(false, null, ConstantsClass.NotFoundMessage));


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetDepartmentById(int id)
        {

            var department = await _departmentService.GetDepartmentByIdAsync(id);

            if (department != null)
                return Ok(new ApiResponse(true, department, null));

            return NotFound(new ApiResponse(false, null, ConstantsClass.NotFoundMessage));


        }



    }
}
