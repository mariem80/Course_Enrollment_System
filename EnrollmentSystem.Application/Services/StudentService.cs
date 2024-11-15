using AutoMapper;
using EnrollmentSystem.Application.DTOs;
using EnrollmentSystem.Core.Entities;
using EnrollmentSystem.Core.Repositries.Base;
using EnrollmentSystem.Core.UnitOfWork;
using EnrollmentSystem.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnrollmentSystem.Core.Exceptions;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace EnrollmentSystem.Application.Services
{
    public class StudentService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IRepository<Student> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly EnrollmentService _enrollmentService;

        public StudentService(IMapper mapper, IRepository<Student> studentRepository, IUnitOfWork unitOfWork, EnrollmentService enrollmentService, IConfiguration configuration)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
            _enrollmentService = enrollmentService;
            _configuration = configuration;
        }
        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            if (students == null)
                throw new NotFoundException();
            var studentDtos = _mapper.Map<IEnumerable<StudentDto>>(students);
            return studentDtos;
        }

        public async Task<StudentDto> GetStudentByIdAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);

            if (student == null)
                throw new NotFoundException();

            var studentDto = _mapper.Map<StudentDto>(student);
            return studentDto;
        }

        public async Task<bool> AddStudentAsync(StudentDto studentDto)
        {
            var existingLevel = await _unitOfWork.LevelRepository.GetByIdAsync(studentDto.LevelID);

            if (existingLevel == null)
                throw new NotFoundException();

            //checking that the student exists at the same level or not
            var existingStudentLevel = await _studentRepository.GetByAsync(s => studentDto.StudentName == s.StudentName && studentDto.LevelID == s.LevelID);
            if (existingStudentLevel != null)
            {
                return false;
            }

            var newStudent = _mapper.Map<Student>(studentDto);
            newStudent.Level = existingLevel;
            var hashedpassword = BCrypt.Net.BCrypt.HashPassword(studentDto.StudentPassword);
            newStudent.StudentPassword = hashedpassword;

            await _studentRepository.AddAsync(newStudent);
            await _unitOfWork.SaveChangesAsync();


            // enroll students to required courses to this student
            var added = await _enrollmentService.AddStudentToRequiredCourses(newStudent.StudentID);

            return true;
        }

        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            var existingStudent = await _studentRepository.GetByIdAsync(studentId);

            if (existingStudent == null)
                throw new NotFoundException();

            await _studentRepository.RemoveAsync(existingStudent);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateStudentAsync(int studentId, UpdateStudentDto updatedStudentDto)
        {
            var existingStudent = await _studentRepository.GetByIdAsync(studentId);

            if (existingStudent == null)
                throw new NotFoundException();
            _mapper.Map(updatedStudentDto, existingStudent);

            await _studentRepository.UpdateAsync(existingStudent);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }

}
