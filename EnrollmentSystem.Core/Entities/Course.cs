using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrollmentSystem.Core.Entities
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public int CourseCreditHours { get; set; }

        public int DepartmentID { get; set; }
        public int InstructorID { get; set; }
        public int LevelID { get; set; }
        public int CourseTypeID { get; set; }


        [ForeignKey("DepartmentID")]
        public required Department Department { get; set; }

        [ForeignKey("InstructorID")]
        public required Instructor Instructor { get; set; }

        [ForeignKey("LevelID")]
        public required Level Level { get; set; }

        [ForeignKey("CourseTypeID")]
        public required CourseType CourseType { get; set; }
    }

}
