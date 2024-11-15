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
    public class DepartmentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public DepartmentService(IMapper mapper, IRepository<Department> departmentRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddDepartmentAsync(DepartmentDto departmentDto)
        {
            if(departmentDto== null || departmentDto.DepartmentName=="")
                throw new NotFoundException();

            var getDepartment = await _unitOfWork.DepartmentRepository.GetByNameAsync("DepartmentName", departmentDto.DepartmentName);

            if (getDepartment != null)
                return false;
       
            else
            {
                var newDepartment = _mapper.Map<Department>(departmentDto);

                await _unitOfWork.DepartmentRepository.AddAsync(newDepartment);

                await _unitOfWork.SaveChangesAsync();

                return true;
            }
        }
        public async Task<bool> UpdateDepartmentAsync(int departmentId, DepartmentDto updatedDepartmentDto)
        {
            var existingDepartment = await _unitOfWork.DepartmentRepository.GetByIdAsync(departmentId);

            if (existingDepartment == null)
                throw new NotFoundException();

            var otherDepartmentWithSameName = await _unitOfWork.DepartmentRepository.GetByAsync(d => d.DepartmentName == updatedDepartmentDto.DepartmentName && d.DepartmentID != departmentId);

            if (otherDepartmentWithSameName != null)
            {
                return false;
            }

            _mapper.Map(updatedDepartmentDto, existingDepartment);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteDepartmentAsync(int departmentId)
        {
            var existingDepartment = await _unitOfWork.DepartmentRepository.GetByIdAsync(departmentId);

            if (existingDepartment == null)
                throw new NotFoundException();

            await _unitOfWork.DepartmentRepository.RemoveAsync(existingDepartment);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<List<GetDepartmentsDto>> GetAllDepartmentsAsync()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            if (departments == null)
                throw new NotFoundException();
            return _mapper.Map<List<GetDepartmentsDto>>(departments);
        }

        public async Task<DepartmentDto> GetDepartmentByIdAsync(int departmentId)
        {
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(departmentId);
            if (department == null)
                throw new NotFoundException();
            return _mapper.Map<DepartmentDto>(department);
        }

    }

 
 }
