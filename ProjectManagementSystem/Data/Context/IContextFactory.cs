namespace ProjectManagementSystem.ProjectManagementSystemDatabase.Context
{
    public interface IContextFactory
    {
        public  ApplicationDbContext CreateContext();
    }
}