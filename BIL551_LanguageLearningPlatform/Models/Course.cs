namespace BIL551_LanguageLearningPlatform.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int LanguageID { get; set; }
        public int TeacherID { get; set; }
        public Language Language { get; set; }
        public User Teacher { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
    }
}
