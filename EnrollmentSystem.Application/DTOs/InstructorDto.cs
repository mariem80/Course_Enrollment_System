using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnrollmentSystem.Application.DTOs
{
    public class InstructorDto
    {
        public required string InstructorName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int DepartmentID { get; set; }

    }
}
