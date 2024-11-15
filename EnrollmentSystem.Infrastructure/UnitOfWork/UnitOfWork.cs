using EnrollmentSystem.Core.Entities;
using EnrollmentSystem.Core.Repositries.Base;
using EnrollmentSystem.Core.UnitOfWork;
using EnrollmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrollmentSystem.Infrastructure.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IRepository<Department> DepartmentRepository { get; }
        public IRepository<Level> LevelRepository { get; }

        public IRepository<Instructor> InstructorRepository { get; }

        public IRepository<Course> CourseRepository { get; }
        public IRepository<CourseType> CourseTypeRepository { get; }

        public IRepository<Student> StudentRepository { get; }

        public IRepository<Enrollment> EnrollmentRepository { get; }

        public UnitOfWork ( AppDbContext context, 
            IRepository<Department> departmentRepository, IRepository<Level> levelRepository, 
            IRepository<Instructor> instructorRepository, IRepository<Course> courseRepository, 
            IRepository<Student> studentRepository, IRepository<Enrollment> enrollmentRepository,
            IRepository<CourseType> courseTypeRepository)
        {
            _context = context;
            DepartmentRepository = departmentRepository;
            LevelRepository = levelRepository;
            InstructorRepository = instructorRepository;
            CourseRepository = courseRepository;
            StudentRepository = studentRepository;
            EnrollmentRepository = enrollmentRepository;

            CourseTypeRepository = courseTypeRepository;


        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
