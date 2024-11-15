using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrollmentSystem.Core.Entities
{
    public class Student
    {
        public int StudentID { get; set; }
        public required string StudentName { get; set; }
        public required string StudentEmail { get; set; }
        public required string StudentPassword { get; set; }
        public int LevelID { get; set; }

        [ForeignKey("LevelID")]
        public required Level Level { get; set; }
    }
}
