using AutoMapper;
using EnrollmentSystem.Core.Entities;
using EnrollmentSystem.Core.Repositries.Base;
using EnrollmentSystem.Core.UnitOfWork;
using EnrollmentSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnrollmentSystem.Core.Exceptions;
using BCrypt.Net;

namespace EnrollmentSystem.Application.Services
{
    public class InstructorService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Instructor> _instructorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InstructorService(IMapper mapper, IRepository<Instructor> instructorRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _instructorRepository = instructorRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task <bool> AddInstructorAsync(InstructorDto instructorDto)
        {
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(instructorDto.DepartmentID);
            if (department == null)
                throw new NotFoundException();
         
            var existingInstructor = await _unitOfWork.InstructorRepository.GetByAsync(i => i.InstructorName == instructorDto.InstructorName);

            if (existingInstructor != null)
            {
                return false;
            }

            var newInstructor = _mapper.Map<Instructor>(instructorDto);

            var hashedpassword = BCrypt.Net.BCrypt.HashPassword(instructorDto.Password);
            newInstructor.Password = hashedpassword;

            await _unitOfWork.InstructorRepository.AddAsync(newInstructor);
            await _unitOfWork.SaveChangesAsync();
            return true;

        }
        public async Task<InstructorDto> GetInstructorByIdAsync(int instructorId)
        {
            var instructor = await _unitOfWork.InstructorRepository.GetByIdAsync(instructorId);

            if (instructor == null)
                throw new NotFoundException();

            var instructorDto = _mapper.Map<InstructorDto>(instructor);
            return instructorDto;
        }
        public async Task<IEnumerable<InstructorDto>> GetAllInstructorsAsync()
        {
            var instructors = await _unitOfWork.InstructorRepository.GetAllAsync();
            if (instructors == null)
                throw new NotFoundException();
            var instructorDto = _mapper.Map<IEnumerable<InstructorDto>>(instructors);
            return instructorDto;
        }

        public async Task<bool> UpdateInstructorAsync(int instructorId, UpdateInstructorDto updatedInstructorDto)
        {
            var existingInstructor = await _unitOfWork.InstructorRepository.GetByIdAsync(instructorId);

            if (existingInstructor == null)
                throw new NotFoundException();

            _mapper.Map(updatedInstructorDto, existingInstructor);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteInstructorAsync(int instructorId)
        {
            var existingInstructor = await _unitOfWork.InstructorRepository.GetByIdAsync(instructorId);

            if (existingInstructor == null)
                throw new NotFoundException();

            await _unitOfWork.InstructorRepository.RemoveAsync(existingInstructor);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
