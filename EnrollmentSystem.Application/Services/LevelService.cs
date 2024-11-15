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
    public class LevelService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Level> _levelRepository;
        private readonly IUnitOfWork _unitOfWork;


        public LevelService(IMapper mapper, IRepository<Level> levelRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _levelRepository = levelRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddLevelAsync(LevelDto levelDto)
        {
            if(levelDto.Year<1 || levelDto.Year > 5)
            {
                throw new BadRequestException();
            }

            //  Validate to avoid duplications
            var samelevel = await _unitOfWork.LevelRepository.GetByAsync(x =>
            x.Year == levelDto.Year && x.DepartmentID == levelDto.DepartmentID);
            if (samelevel != null)
            {
                return false;
            }

            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(levelDto.DepartmentID);

            if (department == null)
                throw new NotFoundException();

            var newLevel = _mapper.Map<Level>(levelDto);
            newLevel.DepartmentID = department.DepartmentID;

            await _unitOfWork.LevelRepository.AddAsync(newLevel);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        public async Task<LevelDto> GetLevelByIdAsync(int levelId)
        {
            var level = await _unitOfWork.LevelRepository.GetByIdAsync(levelId);

            if (level == null)
                throw new NotFoundException();

            var levelDto = _mapper.Map<LevelDto>(level);
            return levelDto;
        }
        public async Task<IEnumerable<LevelDto>> GetAllLevelsAsync()
        {
            var levels = await _unitOfWork.LevelRepository.GetAllAsync();
            if (levels == null)
                throw new NotFoundException();
            var levelDtos = _mapper.Map<IEnumerable<LevelDto>>(levels);
            return levelDtos;
        }

        public async Task<bool> UpdateLevelAsync(int levelId, UpdateLevelDto updatedLevelDto)
        {
            var existingLevel = await _unitOfWork.LevelRepository.GetByIdAsync(levelId);

            if (existingLevel == null)
                throw new NotFoundException();
            _mapper.Map(updatedLevelDto, existingLevel);

            await _unitOfWork.LevelRepository.UpdateAsync(existingLevel);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteLevelAsync(int levelId)
        {
            var existingLevel = await _unitOfWork.LevelRepository.GetByIdAsync(levelId);

            if (existingLevel == null)
                throw new NotFoundException();

            await _unitOfWork.LevelRepository.RemoveAsync(existingLevel);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }


    }
}
