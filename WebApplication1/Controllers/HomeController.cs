using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly HostingEnvironment hostingEnvironment;

        public HomeController(IStudentRepository studentRepository, HostingEnvironment hostingEnvironment)
        {
            _studentRepository = studentRepository;
            this.hostingEnvironment = hostingEnvironment;
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
        public IActionResult Creat(StudentCreatViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photos != null || model.Photos.Count > 0)
                {
                    //string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");//wwwroot下的images
                    //uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;//特殊唯一文件名
                    //string filePath = Path.Combine(uploadsFolder, uniqueFileName);//拼接出文件目录文件名
                    //model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));//复制图片到指定目录
                    foreach (var Photo in model.Photos)
                    {
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");//wwwroot下的images
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;//特殊唯一文件名
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);//拼接出文件目录文件名
                        Photo.CopyTo(new FileStream(filePath, FileMode.Create));//复制图片到指定目录
                    }
                }
                Student newstudent = new Student { Id = model.Id, Name = model.Name, Number = model.Number, PhotoPath = uniqueFileName };
                _studentRepository.Add(newstudent);
                return RedirectToAction("Details", new { id = newstudent.Id });
                //Student newstudent = _studentRepository.Add(student);
                //return RedirectToAction("Details", new { id = newstudent.Id });
            }
            return View();
        }
    }
}