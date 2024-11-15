using EnrollmentSystem.Core.Entities;
using EnrollmentSystem.Core.Repositries.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrollmentSystem.Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Department> DepartmentRepository { get; }
        IRepository<Level> LevelRepository { get; }
        IRepository<Instructor> InstructorRepository { get; }
        IRepository<Course> CourseRepository { get; }
        IRepository<CourseType> CourseTypeRepository { get; }
        IRepository<Student> StudentRepository { get; }
        IRepository<Enrollment> EnrollmentRepository { get; }
        Task<int> SaveChangesAsync();

    }
}
