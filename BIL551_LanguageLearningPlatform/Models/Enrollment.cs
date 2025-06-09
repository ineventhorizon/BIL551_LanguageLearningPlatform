namespace BIL551_LanguageLearningPlatform.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int UserID { get; set; }
        public int CourseID { get; set; }
        public DateTime EnrolledAt { get; set; }
        public User User { get; set; }
        public Course Course { get; set; }
    }
}
