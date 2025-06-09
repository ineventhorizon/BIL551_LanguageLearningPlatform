namespace BIL551_LanguageLearningPlatform.Data
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

    }
}
