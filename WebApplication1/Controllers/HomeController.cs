using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly HostingEnvironment hostingEnvironment;
        private readonly ILogger<HomeController> logger;

        public HomeController(IStudentRepository studentRepository, HostingEnvironment hostingEnvironment,ILogger<HomeController> logger)
        {
            _studentRepository = studentRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }
        public IActionResult Index()
        {
            IEnumerable<Student> student = _studentRepository.GetAllStudent();
            ViewBag.Title = "所有学生信息";
            ViewBag.Title = "AAAIndex";
            return View(student);
        }
        public IActionResult Details(int id)
        {
            logger.LogTrace("跟踪");
            logger.LogDebug("调试");
            logger.LogInformation("消息");
            logger.LogWarning("警告");
            logger.LogError("错误");
            logger.LogCritical("严重");
            Student student = _studentRepository.GetStudent(id);
            if (student == null)
            {
                Response.StatusCode = 404;
                return View("StudentNotFound", id);
            }
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
                //if (model.Photos != null || model.Photos.Count > 0)
                //{
                //    //string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");//wwwroot下的images
                //    //uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;//特殊唯一文件名
                //    //string filePath = Path.Combine(uploadsFolder, uniqueFileName);//拼接出文件目录文件名
                //    //model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));//复制图片到指定目录
                //    foreach (var Photo in model.Photos)
                //    {
                //        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");//wwwroot下的images
                //        uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;//特殊唯一文件名
                //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);//拼接出文件目录文件名
                //        Photo.CopyTo(new FileStream(filePath, FileMode.Create));//复制图片到指定目录
                //    }
                //}
                uniqueFileName = ProcessUploadedFile(model);
                Student newstudent = new Student { Id = model.Id, Name = model.Name, Number = model.Number, PhotoPath = uniqueFileName };
                _studentRepository.Add(newstudent);
                return RedirectToAction("Details", new { id = newstudent.Id });
                //Student newstudent = _studentRepository.Add(student);
                //return RedirectToAction("Details", new { id = newstudent.Id });
            }
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Student student = _studentRepository.GetStudent(id);
            if (student == null)
            {
                Response.StatusCode = 404;
                return View("StudentNotFound", id);
            }
            StudentEditViewModel studentEditViewModel = new StudentEditViewModel
            {
                Id = student.Id,
                Name = student.Name,
                Number = student.Number,
                ExistingPhotoPath = student.PhotoPath
            };
            return View(studentEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(StudentEditViewModel model)
        {
            //满足模型验证
            if (ModelState.IsValid)
            {
                Student student = _studentRepository.GetStudent(model.Id);
                student.Id = model.Id;
                student.Name = model.Name;
                student.Number = model.Number;
                if (model.Photos.Count > 0)
                {
                    if (model.ExistingPhotoPath != null)//存在图片路径则删除就图片
                    {
                        string filename = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filename);
                    }
                }
                student.PhotoPath = ProcessUploadedFile(model);
                Student updateStudent = _studentRepository.Update(student);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        /// <summary>
        /// 拼接图片存放路径，复制图片到指定路径
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string ProcessUploadedFile(StudentCreatViewModel model)
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
                    using (var copyFilePath = new FileStream(filePath, FileMode.Create))
                    {
                        Photo.CopyTo(copyFilePath);//复制图片到指定目录
                    }
                }
            }
            return uniqueFileName;
        }
    }
}