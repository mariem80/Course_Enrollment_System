using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrollmentSystem.Application.DTOs
{
    public class LevelDto
    {
        public int MinCredits { get; set; }
        public int MaxCredits { get; set; }
        public int Year { get; set; }
        public int DepartmentID { get; set; }
    }
}
