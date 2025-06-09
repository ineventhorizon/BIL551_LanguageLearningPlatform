namespace BIL551_LanguageLearningPlatform.Data
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        public Task<int> SaveChangesAsync() => context.SaveChangesAsync();

    }
}
