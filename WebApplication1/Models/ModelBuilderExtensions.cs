using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Student>().HasData(
                new Student { Id = 1, Name = "a", Number = NumberEnum.One },
                new Student { Id = 2, Name = "b", Number = NumberEnum.Two },
                new Student { Id = 3, Name = "c", Number = NumberEnum.Three }
                );
        }
    }
}
