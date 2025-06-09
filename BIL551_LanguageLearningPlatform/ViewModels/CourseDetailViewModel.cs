using BIL551_LanguageLearningPlatform.Models;

namespace BIL551_LanguageLearningPlatform.ViewModels
{
    public class CourseDetailViewModel
    {
        public Course Course { get; set; }
        public bool IsEnrolled { get; set; } = false;
    }
}
