using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnrollmentSystem.Core.Entities;
using EnrollmentSystem.Application.DTOs;

namespace EnrollmentSystem.Application.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile() //  CreateMap<>() inside the ctor haa
        {
            CreateMap<Department, DepartmentDto>();
            CreateMap<Level,LevelDto>();
            CreateMap<Instructor,InstructorDto>();
            CreateMap<Course, CourseDto>();

            CreateMap<Student, StudentDto>();


            CreateMap<DepartmentDto, Department>().ReverseMap();

            CreateMap<LevelDto, Level>().ReverseMap();

            CreateMap<InstructorDto, Instructor>().ReverseMap();

            CreateMap<CourseDto, Course>().ReverseMap();
            CreateMap<StudentDto, Student>() .ReverseMap();
            CreateMap<EnrollmentDto,Enrollment>().ReverseMap();
            CreateMap<UpdateLevelDto, Level>().ReverseMap();
            CreateMap<UpdateCourseDto,Course>().ReverseMap();


            CreateMap<UpdateInstructorDto, Instructor>().ReverseMap();
            CreateMap<UpdateStudentDto, Student>().ReverseMap();

            CreateMap<GetCoursesDto, Course>().ReverseMap();
            CreateMap<GetDepartmentsDto, Department>().ReverseMap();

            CreateMap<GetEnrollmentsDto, Enrollment>().ReverseMap();

        }
        
    }
}
