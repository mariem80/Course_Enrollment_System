using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrollmentSystem.Application.DTOs
{
        public class CourseDto
        {
            public string CourseName { get; set; }
            public string CourseDescription { get; set; }
            public int CourseCreditHours { get; set; }

            public int DepartmentID { get; set; }
            public int InstructorID { get; set; }
            public int LevelID { get; set; }
            public int CourseTypeID { get; set; }
    }
}


