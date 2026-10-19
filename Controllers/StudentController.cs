using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using lab_1.Models;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace lab_1.Controllers
{
    public class StudentController : Controller
    {
        private static List<Student> listStudents;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;

            if (listStudents == null)
            {
                listStudents = new List<Student>()
                {
                    new Student() { Id = 101, Name = "Hải Nam", Branch = Branch.IT,
                        Gender = Gender.Male, IsRegular = true,
                        Address = "A1-2018", Email = "nam@g.com" },
                    new Student() { Id = 102, Name = "Minh Tú", Branch = Branch.BE,
                        Gender = Gender.Female, IsRegular = true,
                        Address = "A1-2019", Email = "tu@g.com" },
                    new Student() { Id = 103, Name = "Hoàng Phong", Branch = Branch.CE,
                        Gender = Gender.Male, IsRegular = false,
                        Address = "A1-2020", Email = "phong@g.com" },
                    new Student() { Id = 104, Name = "Xuân Mai", Branch = Branch.EE,
                        Gender = Gender.Female, IsRegular = false,
                        Address = "A1-2021", Email = "mai@g.com" }
                };
            }
        }

        [Route("Admin/Student/List", Name = "StudentList")]
        public IActionResult Index()
        {
            return View(listStudents);
        }

        [Route("Admin/Student/Add", Name = "StudentAdd")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            ViewBag.AllBranches = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "IT", Value = "1" },
                new SelectListItem() { Text = "BE", Value = "2" },
                new SelectListItem() { Text = "CE", Value = "3" },
                new SelectListItem() { Text = "EE", Value = "4" }
            };
            return View();
        }

        [Route("Admin/Student/Add", Name = "StudentAdd")]
        [HttpPost]
        public IActionResult Create(Student s)
        {
            s.Id = listStudents.Count > 0 ? listStudents.Last<Student>().Id + 1 : 1;

            if (s.AvatarFile != null && s.AvatarFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + s.AvatarFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    s.AvatarFile.CopyTo(fileStream);
                }

                s.AvatarUrl = "/uploads/" + uniqueFileName;
            }
            else
            {
                s.AvatarUrl = "/images/default-avatar.png";
            }

            listStudents.Add(s);
            return RedirectToAction("Index");
        }
    }
}