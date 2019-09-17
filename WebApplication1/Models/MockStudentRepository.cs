using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class MockStudentRepository : IStudentRepository
    {
        private List<Student> _student;
        public MockStudentRepository()
        {
            _student = new List<Student>()
            {
                new Student{ Id=1,Name="a",Number=NumberEnum.None},
                new Student{ Id=2,Name="b",Number=NumberEnum.One},
                new Student{ Id=3,Name="c",Number=NumberEnum.Two},
                new Student{ Id=4,Name="d",Number=NumberEnum.Three},
            };
        }

        public Student Add(Student student)
        {
            student.Id = _student.Max(x => x.Id) + 1;
            _student.Add(student);
            return student;
        }

        public IEnumerable<Student> GetAllStudent()
        {
            return _student;
        }

        public Student GetStudent(int id)
        {
            return _student.FirstOrDefault(a => a.Id == id);
        }
    }
}
