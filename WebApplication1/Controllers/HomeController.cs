using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        public HomeController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public IActionResult Index()
        {
            IEnumerable<Student> student = _studentRepository.GetAllStudent();
            ViewBag.Title = "所有学生信息";
            ViewBag.Title = "AAAIndex";
            return View(student);
        }
        public IActionResult Details(int? id)
        {
            Student student = _studentRepository.GetStudent(id ?? 1);
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Student = student,
                PaperTitle = "StudentDetails"

            };
            //ViewData["PageTitle"] = "StudentDetails";
            //ViewData["Student"] = student;
            //return View(student);
            //return View();
            ViewBag.Title = "AAADetials";
            return View(homeDetailsViewModel);
        }
        [HttpGet]
        public IActionResult Creat()
        {
            ViewBag.Title = "AAACreat";
            return View();
        }
        [HttpPost]
        public RedirectToActionResult Creat(Student student)
        {
            Student newstudent = _studentRepository.Add(student);
            return RedirectToAction("Details", new { id = newstudent.Id });
        }
    }
}