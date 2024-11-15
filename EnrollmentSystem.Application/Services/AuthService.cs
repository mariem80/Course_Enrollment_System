using AutoMapper;
using EnrollmentSystem.Application.DTOs;
using EnrollmentSystem.Core.Entities;
using EnrollmentSystem.Core.Exceptions;
using EnrollmentSystem.Core.Repositries.Base;
using EnrollmentSystem.Core.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Serilog;
namespace EnrollmentSystem.Application.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Instructor> _instructorRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        public AuthService(IConfiguration configuration, IMapper mapper, IRepository<Student> studentRepository, IUnitOfWork unitOfWork, IRepository<Instructor> instructorRepository, ILogger logger)
        {
            _configuration = configuration;
            _mapper = mapper;
            _studentRepository = studentRepository;
            _instructorRepository = instructorRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;

        }
        public static bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            if(BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashedPassword))
            {
                return true;
            }
            return false;
        }
        public async Task<string> GenerateJwtToken(LoginDto loginDto)
        {
            var student = await _unitOfWork.StudentRepository.GetByAsync(s => s.StudentEmail == loginDto.Email);
            var instructor = await _unitOfWork.InstructorRepository.GetByAsync(s => s.Email == loginDto.Email);

            bool isStudentPasswordValid = student != null && VerifyPassword(loginDto.Password, student.StudentPassword);
            bool isInstructorPasswordValid = instructor != null && VerifyPassword(loginDto.Password, instructor.Password);

            if (!isStudentPasswordValid && !isInstructorPasswordValid)
                throw new NotFoundException(); 


            var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loginDto.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: Claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
            );
            _logger.Information("\n\n Generating token.\n\n");
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
