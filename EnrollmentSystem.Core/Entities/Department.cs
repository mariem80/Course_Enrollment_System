using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrollmentSystem.Core.Entities
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public required string DepartmentName { get; set; }


    }
}
