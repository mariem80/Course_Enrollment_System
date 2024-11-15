using EnrollmentSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnrollmentSystem.Application.DTOs
{
    public class EnrollmentDto
    {
        public int StudentID { get; set; } //FK
        public int CourseID { get; set; } //FK

        [JsonIgnore]
        public int CourseTypeID { get; set; } //FK Ignored as it cames from course table
    }
}
