using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrollmentSystem.Core.Entities
{
    public class Instructor
    {
        public int InstructorID { get; set; }
        public required string InstructorName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int DepartmentID { get; set; } // To assign instructor with Department
        public virtual Department Department { get; set; }
    }
}
