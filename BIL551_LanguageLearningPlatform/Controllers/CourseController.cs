using BIL551_LanguageLearningPlatform.Data;
using BIL551_LanguageLearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BIL551_LanguageLearningPlatform.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        public CourseController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Create()
        {
            var languages = _context.Languages.ToList();
            ViewBag.Languages = languages;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Course course)
        {
            var teacherId = HttpContext.Session.GetInt32("UserID");
            if (teacherId == null) return RedirectToAction("Login", "Account");


            course.TeacherID = teacherId.Value;

    

            _context.Courses.Add(course);
            _context.SaveChanges();

            return RedirectToAction("MyCourses");
        }

        public IActionResult MyCourses()
        {
            var teacherId = HttpContext.Session.GetInt32("UserID");
            if (teacherId == null) return RedirectToAction("Login", "Account");
            
            var myCourses = _context.Courses
                .Where(c => c.TeacherID == teacherId)
                .Include(c => c.Language)
                .Include(c => c.Enrollments)
                .Include(c => c.Lessons)
                .ToList();

            return View(myCourses);
        }

        public IActionResult Manage(int id)
        {
            var course = _context.Courses
                .Include(c => c.Lessons)
                    .ThenInclude(l => l.Submissions)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.User)
                .FirstOrDefault(c => c.CourseID == id);

            if (course == null)
                return NotFound();

            return View(course);
        }

        [HttpGet]
        public IActionResult AddLesson(int id)
        {
            return View(new Lesson { CourseID = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddLesson(Lesson lesson)
        {

            _context.Lessons.Add(lesson);
            _context.SaveChanges();
            return RedirectToAction("Manage", new { id = lesson.CourseID });

          
        }

        public IActionResult DeleteLesson(int id)
        {
            var lesson = _context.Lessons.FirstOrDefault(l => l.LessonID == id);
            if (lesson == null) return NotFound();

            int courseId = lesson.CourseID;

            _context.Lessons.Remove(lesson);
            _context.SaveChanges();
            TempData["EnrollMessage"] = "You have successfully deleted your lesson.";
            TempData["MessageType"] = "danger";

            return RedirectToAction("Manage", new { id = courseId });
        }

        public IActionResult LessonSubmissions(int id)
        {
            var lesson = _context.Lessons
                .Include(l => l.Submissions)
                    .ThenInclude(s => s.User)
                .FirstOrDefault(l => l.LessonID == id);

            if (lesson == null) return NotFound();

            return View(lesson);
        }

        [HttpGet]
        public IActionResult EditLesson(int id)
        {
            var lesson = _context.Lessons.FirstOrDefault(l => l.LessonID == id);
            if (lesson == null) return NotFound();
            return View(lesson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditLesson(Lesson updatedLesson)
        {
            var lesson = _context.Lessons.FirstOrDefault(l => l.LessonID == updatedLesson.LessonID);
            if (lesson == null) return NotFound();

            lesson.Title = updatedLesson.Title;
            lesson.Content = updatedLesson.Content;

            _context.SaveChanges();
            TempData["EnrollMessage"] = "You have successfully edited your lesson.";
            TempData["MessageType"] = "success";
            return RedirectToAction("Manage", new { id = lesson.CourseID });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseID == id);
            if (course == null) return NotFound();

            ViewBag.Languages = _context.Languages.ToList();
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Course updatedCourse)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseID == updatedCourse.CourseID);
            if (course == null) return NotFound();

            course.Title = updatedCourse.Title;
            course.Description = updatedCourse.Description;
            course.LanguageID = updatedCourse.LanguageID;

            _context.SaveChanges();
            TempData["EnrollMessage"] = "You have successfully edited your course.";
            TempData["MessageType"] = "success";
            return RedirectToAction("Manage", new { id = course.CourseID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var course = _context.Courses
                .Include(c => c.Lessons)
                .Include(c => c.Enrollments)
                .FirstOrDefault(c => c.CourseID == id);

            if (course == null) return NotFound();

            _context.Lessons.RemoveRange(course.Lessons);
            _context.Enrollments.RemoveRange(course.Enrollments);
            _context.Courses.Remove(course);
            _context.SaveChanges();

            return RedirectToAction("MyCourses");
        }



    }
}
