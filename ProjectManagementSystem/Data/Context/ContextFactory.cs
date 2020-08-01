using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ProjectManagementSystem.ProjectManagementSystemDatabase.Context
{
    public class ContextFactory : IContextFactory
    {
        private ApplicationDbContext _context;

            public ContextFactory(ApplicationDbContext context)
            {
                _context = context;
            }
            /// <summary>
            /// CreateContext method creates new instance of UploadServiceContext class
            /// </summary>
            /// <returns>UploadServiceContext object</returns>
            public ApplicationDbContext CreateContext()
            {
                return new ApplicationDbContext(_context.GetService<DbContextOptions<ApplicationDbContext>>());
            }
        }
    
}