namespace BIL551_LanguageLearningPlatform.Models
{
    public class Language
    {
        public int LanguageID { get; set; }
        public string Name { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
