using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrollmentSystem.Application.DTOs
{
    public class GetEnrollmentsDto
    {
        public int EnrollmentId { get; set; }
        public int StudentID { get; set; } 
        public int CourseID { get; set; } 
        
        public int CourseTypeID { get; set; }
    }
}
