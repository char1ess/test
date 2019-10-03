using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class SQLStudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SQLStudentRepository> logger;

        public SQLStudentRepository(AppDbContext context,ILogger<SQLStudentRepository>logger)
        {
            _context = context;
            this.logger = logger;
        }
        public Student Add(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return student;
        }

        public Student Delete(int id)
        {
            Student student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
            return student;
        }

        public IEnumerable<Student> GetAllStudent()
        {
            logger.LogTrace("跟踪");
            logger.LogDebug("调试");
            logger.LogInformation("消息");
            logger.LogWarning("警告");
            logger.LogError("错误");
            logger.LogCritical("严重");
            return _context.Students;
        }

        public Student GetStudent(int id)
        {
            return _context.Students.Find(id);
        }

        public Student Update(Student updatestudent)
        {
            var student = _context.Students.Attach(updatestudent);
            student.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return updatestudent;
        }
    }
}
