using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public interface IStudentRepository
    {
        Student GetStudent(int id);
        IEnumerable< Student> GetAllStudent();
        Student Add(Student student);
        Student Update(Student updatestudent);
        Student Delete(int id);
    }
}
