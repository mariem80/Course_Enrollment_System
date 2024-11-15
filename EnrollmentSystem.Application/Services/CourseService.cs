using AutoMapper;
using EnrollmentSystem.Core.Entities;
using EnrollmentSystem.Core.Exceptions;
using EnrollmentSystem.Core.Repositries.Base;
using EnrollmentSystem.Core.UnitOfWork;
using EnrollmentSystem.Application.DTOs;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.VisualBasic;

namespace EnrollmentSystem.Application.Services
{
    public class CourseService
    {
        private readonly IMapper _mapper;
        //private readonly IRepository<Course> _courseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IMapper mapper, IRepository<Course> courseRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
           // _courseRepository = courseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddCourseAsync(CourseDto courseDto)
        {
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(courseDto.DepartmentID);
            var instructor = await _unitOfWork.InstructorRepository.GetByIdAsync(courseDto.InstructorID);
            var level = await _unitOfWork.LevelRepository.GetByIdAsync(courseDto.LevelID);

            if (department == null || instructor == null || level == null)
            {
                throw new NotFoundException();
            }

            var existingCourse = await _unitOfWork.CourseRepository.GetByAsync(c =>
                c.CourseName == courseDto.CourseName
                && c.DepartmentID == courseDto.DepartmentID);

            if (existingCourse != null)
            {
                return false;
            }

            var newCourse = _mapper.Map<Course>(courseDto);

            await _unitOfWork.CourseRepository.AddAsync(newCourse);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCourseAsync(int courseId, UpdateCourseDto updatedCourseDto)
        {
            var existingCourse = await _unitOfWork.CourseRepository.GetByIdAsync(courseId);

            if (existingCourse == null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(updatedCourseDto, existingCourse);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var existingCourse = await _unitOfWork.CourseRepository.GetByIdAsync(courseId);

            if (existingCourse == null)
                throw new NotFoundException();
            

            await _unitOfWork.CourseRepository.RemoveAsync(existingCourse);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }


        public async Task<IEnumerable<GetCoursesDto>> GetAllCoursesAsync()
        {
            var courses = await _unitOfWork.CourseRepository.GetAllAsync();
            if (courses == null)
                throw new NotFoundException();

            return _mapper.Map<IEnumerable<GetCoursesDto>>(courses);
        }

        public async Task<CourseDto> GetCourseByIdAsync(int courseId)
        {
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(courseId);
            if (course == null)
                throw new NotFoundException();

            return _mapper.Map<CourseDto>(course);
        }

        public async Task<List<GetCoursesDto>> GetElectiveCourses()
        {
            var Electivecourses = await _unitOfWork.CourseRepository.GetListAsync(c => c.CourseTypeID == 2);

            if (Electivecourses == null)
            {
                throw new NotFoundException();
            }
            else
                return _mapper.Map<List<GetCoursesDto>>(Electivecourses);

        }
    }


}



