using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsCore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace StudentsCore.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        [Obsolete]
        private IHostingEnvironment _env;

        [Obsolete]
        public StudentsController(IHostingEnvironment env, ApplicationDbContext context)
        {
            _env = env;
            _context = context;
        }

        public ActionResult Index()
        {
            return View(_context.students.ToList());

        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<ActionResult> CreateAsync(Student student, List<IFormFile> File)
        {
            if (ModelState.IsValid)
            {
                foreach (var file in File)
                {
                    if (file.Length > 0)
                    {

                        //لو جيت احفظ صورة جديدة بس فيه صورة بنفس الاسم ف قاعدة البيانات
                        //تتحل عن طريق الجويد دة بيدى لكل مدخل داخل الدتابيز رقم تعريفى مختلف
                        string Image = Guid.NewGuid().ToString() + ".jpg";
                        //علشان اجيب مسار الصورة 
                        var FilePaths = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Uploads", Image);
                        //علشان احفظ مسار الصورة ف الداتا بيز
                        using (var stream = System.IO.File.Create(FilePaths))
                        {

                            //هياخد نسخة من الصورة اللى اتكريتت
                            //معموله اويت علشان ميخدش نسخة قبل ميعمل كريت
                            await file.CopyToAsync(stream);
                        }
                        //هيضيف الصورة للموديل للحقل بتاعها فى الداتا بيز
                        student.Image = Image;
                    }
                   //(
                   //     //الطريقة القديمة

                   //      //string Image = Guid.NewGuid().ToString() + ".jpg";
                   //      //string uploads = Path.Combine(_env.WebRootPath, "Uploads");
                   //      //string filename = File.FileName;
                   //      //string fullpath = Path.Combine(uploads, filename);
                   //      //File.CopyTo(new FileStream(fullpath, FileMode.Create));
                   //      //student.Image = filename;
                   //      //_context.students.Add(student);
                   //      //_context.SaveChanges();
                   // )
                    _context.students.Add(student);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View("Index");
            }
        }
        [HttpGet]
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return StatusCode(400);
            }
            var student = _context.students.Find(id);
            if (student == null)
            {
                return StatusCode(404);
            }
            var model = new Student
            {
                Name = student.Name,
                Age = student.Age,
                Sex = student.Sex,
                DateBirth = student.DateBirth,
                Address = student.Address,
                Image = student.Image
            };
            return View(model);
        }
        [HttpPost]
        [Obsolete]
        public ActionResult Edit(Student model, IFormFile File)
        {
            if (ModelState.IsValid)
            {
                var existingStudent = _context.students.Find(model.Id);
                string oldimg = existingStudent.Image;
                string oldpath = Path.Combine(_env.WebRootPath, ("Uploads"), oldimg);
                if (File != null)
                {
                    System.IO.File.Delete(oldpath);

                    string path = Path.Combine(_env.WebRootPath, ("Uploads"), File.FileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        File.CopyTo(fileStream);
                    }

                    //File.CopyTo(new FileStream(path, FileMode.Create));
                    model.Image = File.FileName;
                    _context.Entry(existingStudent).CurrentValues.SetValues(model);
                    _context.Entry(existingStudent).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                model.Image = existingStudent.Image;
                _context.Entry(existingStudent).CurrentValues.SetValues(model);
                _context.Entry(existingStudent).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);

        }

        [HttpGet]
        public ActionResult Delete(byte id)
        {
            var student = _context.students.Find(id);
            if (student == null)
            {
                return StatusCode(404);
            }

            return View(student);
        }
        [HttpPost, ActionName("Delete")]
        [Obsolete]
        public ActionResult ConfirmDelete(byte id)
        {
            var student = _context.students.Find(id);
            _context.students.Remove(student);

            string img = student.Image;
            string path = Path.Combine(_env.WebRootPath, ("Uploads"), img);

            System.IO.File.Delete(path);

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Details(byte id)
        {
            var student = _context.students.Find(id);
            return View(student);
        }


    }
}
