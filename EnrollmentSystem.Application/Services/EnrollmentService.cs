using AutoMapper;
using EnrollmentSystem.Application.DTOs;
using EnrollmentSystem.Core.Entities;
using EnrollmentSystem.Core.Exceptions;
using EnrollmentSystem.Core.Repositries.Base;
using EnrollmentSystem.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrollmentSystem.Application.Services
{
    public class EnrollmentService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Enrollment> _enrollmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EnrollmentService(IMapper mapper, IRepository<Enrollment> enrollmentRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;

        }


        public async  Task<bool> DeleteEnrollmentAsync(int enrollmentID)
        {

            var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentID);
            if (enrollment == null)
                throw new NotFoundException();
            await _enrollmentRepository.RemoveAsync(enrollment);
            await _unitOfWork.SaveChangesAsync();
            return true;

        }
        public async Task<bool> AddStudentToRequiredCourses(int studentId)
        {
            var enrolledCourseIds = new List<int>();

            var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);
            var availableCourses = await _unitOfWork.CourseRepository.GetListAsync(c => c.CourseTypeID == 1);

            if (student == null || availableCourses == null || !availableCourses.Any())
                throw new NotFoundException();

            foreach (var course in availableCourses)
            {
                var isEnrolled = await _unitOfWork.EnrollmentRepository.ExistsAsync(
                    e => e.StudentID == studentId && e.CourseID == course.CourseID);

                if (!isEnrolled)
                {

                    var newEnrollmentDto = new EnrollmentDto
                    {
                        StudentID = studentId,
                        CourseID = course.CourseID,
                        CourseTypeID = course.CourseTypeID
                    };

                    var newEnrollment = _mapper.Map<Enrollment>(newEnrollmentDto);

                    await _enrollmentRepository.AddAsync(newEnrollment);
                    await _unitOfWork.SaveChangesAsync();

                    enrolledCourseIds.Add(course.CourseID);
                }
            }

            return true;

        }

        // Enroll students as an admin
        public async Task<bool> AddEnrollmentAsync(EnrollmentDto enrollmentDto)
        {
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(enrollmentDto.CourseID);
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(enrollmentDto.StudentID);

            if (course == null || student == null)
                throw new NotFoundException();

            var existingEnrollment = await _unitOfWork.EnrollmentRepository.GetByAsync(
                e => e.StudentID == enrollmentDto.StudentID
                    && e.CourseID == enrollmentDto.CourseID
                    );

            if (existingEnrollment != null)
            {
                return false;
            }

            var courseType = course.CourseTypeID;  // Retrieve CourseTypeID from Course Table


            var newEnrollment = _mapper.Map<Enrollment>(enrollmentDto);

            newEnrollment.Course = course;
            newEnrollment.CourseTypeID = courseType; // Set CourseTypeID on the new enrollment
            newEnrollment.Student = student;


            await _enrollmentRepository.AddAsync(newEnrollment);
            await _unitOfWork.SaveChangesAsync();

            return true;

        }

        public async Task<List<EnrollmentDto>> GetEnrollmentsForStudent(int studentId)
        {
            var enrollments = await _unitOfWork.EnrollmentRepository.GetListAsync(e => e.StudentID == studentId);

            if (enrollments == null || !enrollments.Any())
                throw new NotFoundException();

            var enrollmentsDto = _mapper.Map<List<EnrollmentDto>>(enrollments);
            return enrollmentsDto;
        }


        public async Task<List<EnrollmentDto>> GetEnrollmentsForCourse(int courseId)
        {
            var enrollments = await _unitOfWork.EnrollmentRepository.GetListAsync(e => e.CourseID == courseId);

            if (enrollments == null || !enrollments.Any())
                throw new NotFoundException();

            var enrollmentsDto = _mapper.Map<List<EnrollmentDto>>(enrollments);
            return enrollmentsDto;
        }

        public async Task<List<GetEnrollmentsDto>> GetAllEnrollmentsAsync()
        {
            var enrollments = await _unitOfWork.EnrollmentRepository.GetAllAsync();

            if (enrollments == null)
                throw new NotFoundException();
            else
            {
                return _mapper.Map<List<GetEnrollmentsDto>>(enrollments);
            }
        }

        public async Task<bool> EnrollElectiveAsync(int studentID, int CourseID)
        {
            var course = await _unitOfWork.CourseRepository.GetByAsync(c => CourseID == c.CourseID && c.CourseTypeID == 2);
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentID);
            if (course == null || student == null)
            {
                throw new NotFoundException();
            }
            else
            {

                var GetAllStudent = await _unitOfWork.StudentRepository.GetByAsync(s => s.StudentID == studentID);
                var getLevelID = GetAllStudent.LevelID;

                var Level = await _unitOfWork.LevelRepository.GetByIdAsync(getLevelID);
                var getMaxHours = Level.MaxCredits;

                var StudentEnrollments = await _unitOfWork.EnrollmentRepository.GetListAsync(x => x.StudentID == studentID);  // getting all the enrollments for the student

                if (StudentEnrollments == null)
                {
                    throw new BadRequestException();
                }
                else
                {
                    int totalCreditHours = 0;

                    foreach (var enrollment in StudentEnrollments)
                    {
                        var studentCourseId = enrollment.CourseID;

                        var getCourseDetails = await _unitOfWork.CourseRepository.GetByAsync(c => c.CourseID == studentCourseId);
                        var creditHour = getCourseDetails.CourseCreditHours;

                        totalCreditHours += creditHour;
                    }

                    if (totalCreditHours > getMaxHours)
                    {
                        throw new BadRequestException();

                    }
                    else
                    {
                        var addEnrollment = new EnrollmentDto
                        {
                            CourseID = CourseID,
                            StudentID = studentID
                        };

                        var enrollElectiveCourse = await AddEnrollmentAsync(addEnrollment);


                        if (enrollElectiveCourse)
                            return true;

                        return false;
                    }

                }
            }
        }
    }


}

