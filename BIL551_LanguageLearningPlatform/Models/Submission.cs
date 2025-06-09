namespace BIL551_LanguageLearningPlatform.Models
{
    public class Submission
    {
        public int SubmissionID { get; set; }
        public int LessonID { get; set; }
        public int UserID { get; set; }
        public string Content { get; set; }
        public DateTime SubmittedAt { get; set; }

        public Lesson Lesson { get; set; }
        public User User { get; set; }
    }
}
