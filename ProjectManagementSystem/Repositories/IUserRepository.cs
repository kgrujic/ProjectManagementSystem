using System.Linq;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Repositories
{
    public interface IUserRepository
    {
        public IQueryable<ApplicationUser> GetAllQueryable();
    }
}