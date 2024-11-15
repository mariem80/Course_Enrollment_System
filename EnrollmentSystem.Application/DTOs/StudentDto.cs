using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnrollmentSystem.Application.DTOs
{
    public class StudentDto
    {
        
        public required string StudentName { get; set; }
        public required string StudentEmail { get; set; }
        public required string StudentPassword { get; set; }
        public int LevelID { get; set; }
    }
}
