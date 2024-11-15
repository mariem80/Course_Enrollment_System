using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrollmentSystem.Core.Entities
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }

        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public int CourseTypeID { get; set; }
        public required Student Student { get; set; } //FK
        public required Course Course { get; set; } //FK
        public required CourseType CourseType { get; set; } //FK

    }
}
