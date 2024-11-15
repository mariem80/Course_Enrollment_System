using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrollmentSystem.Core.Entities
{
    public class Level
    {
        public int LevelID { get; set; }
        public int Year { get; set; }
        public int MinCredits { get; set; }
        public int MaxCredits { get; set; }

        public int DepartmentID { get; set; } // To assign level with Department, I already used data annotations so no need for it
        [ForeignKey("DepartmentID")]
        public required Department Department { get; set; }
       
    }
}
