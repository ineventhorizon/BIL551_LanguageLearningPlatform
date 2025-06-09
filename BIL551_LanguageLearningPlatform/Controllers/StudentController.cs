using BIL551_LanguageLearningPlatform.Data;
using BIL551_LanguageLearningPlatform.Models;
using BIL551_LanguageLearningPlatform.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BIL551_LanguageLearningPlatform.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;
        public StudentController(AppDbContext context)
        {
            _context = context;
        }


        public IActionResult Courses()
        {
            //var userId = int.Parse(HttpContext.Session.GetString("UserID"));

            var userId = HttpContext.Session.GetInt32("UserID");

            var allCourses = _context.Courses
                .Include(c => c.Language)
                .Include(c => c.Teacher)
                .Include(c => c.Lessons)
                .ToList();

            var enrolledCourseIds = _context.Enrollments
                .Where(e => e.UserID == userId)
                .Select(e => e.CourseID)
                .ToHashSet();

            ViewBag.EnrolledCourseIds = enrolledCourseIds;

            return View(allCourses);

            
        }

        public IActionResult MyEnrollments()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return RedirectToAction("Login", "Account");

            var enrollments = _context.Enrollments
                .Where(e => e.UserID == userId)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Language)
                .Include(e => e.Course.Teacher)
                .ToList();

            return View(enrollments);
        }

        [HttpPost]
        public IActionResult Enroll(int courseId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var courseExists = _context.Courses.Any(c => c.CourseID == courseId);
            if (!courseExists)
            {
                TempData["Error"] = "The course you are trying to enroll in does not exist.";
                return RedirectToAction("Courses");
            }
            var alreadyEnrolled = _context.Enrollments
                .Any(e => e.CourseID == courseId && e.UserID == userId);

            if (!alreadyEnrolled)
            {
                var enrollment = new Enrollment
                {
                    CourseID = courseId,
                    UserID = userId.Value,
                    EnrolledAt = DateTime.Now
                };

                _context.Enrollments.Add(enrollment);
                _context.SaveChanges();
            }
            TempData["EnrollMessage"] = "You have successfully enrolled in the course.";
            TempData["MessageType"] = "success";
            return RedirectToAction("Courses");
        }

        [HttpGet]
        public IActionResult Submit(int lessonId)
        {
            var lesson = _context.Lessons
                .Include(l => l.Course)
                .FirstOrDefault(l => l.LessonID == lessonId);

            if (lesson == null) return NotFound();

            TempData["EnrollMessage"] = "You have been successfully submitted your submission.";
            TempData["MessageType"] = "success";

            return View(new Submission { LessonID = lessonId });
        }

        public IActionResult ViewCourse(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var alreadyEnrolled = _context.Enrollments
                .Any(e => e.CourseID == id && e.UserID == userId);
            var course = _context.Courses
                .Include(c => c.Language)
                .Include(c => c.Teacher)
                .Include(c => c.Lessons)
                .FirstOrDefault(c => c.CourseID == id);

            var detailView = new CourseDetailViewModel
            {
                Course = course!,
                IsEnrolled = alreadyEnrolled
            };

            if (course == null)
                return NotFound();

            return View(detailView);
        }
        [HttpPost]
        public IActionResult SubmitLesson(int lessonId, string submissionContent)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null || string.IsNullOrWhiteSpace(submissionContent))
                return RedirectToAction("Login", "Account");

            var submission = new Submission
            {
                UserID = (int)userId,
                LessonID = lessonId,
                Content = submissionContent,
                SubmittedAt = DateTime.Now
            };

            _context.Submissions.Add(submission);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Lesson submitted successfully!";
            return RedirectToAction("ViewCourse", new { id = _context.Lessons.Find(lessonId)?.CourseID });
        }
        [HttpPost]
        public IActionResult Unenroll(int courseId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var enrollment = _context.Enrollments
                .FirstOrDefault(e => e.CourseID == courseId && e.UserID == userId.Value);

            if (enrollment == null)
            {
                TempData["Error"] = "You are not enrolled in this course.";
                return RedirectToAction("Courses");
            }

            _context.Enrollments.Remove(enrollment);
            _context.SaveChanges();

            TempData["EnrollMessage"] = "You have been successfully unenrolled from the course.";
            TempData["MessageType"] = "danger";
            return RedirectToAction("MyEnrollments");
        }
    }
}
