namespace BIL551_LanguageLearningPlatform.Models
{
    public class Lesson
    {
        public int LessonID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CourseID { get; set; }

        public Course Course { get; set; }
        public ICollection<Submission> Submissions { get; set; }
    }
}
