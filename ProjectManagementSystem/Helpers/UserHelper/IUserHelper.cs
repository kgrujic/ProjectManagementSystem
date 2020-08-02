using System.Collections.Generic;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Helpers.UserHelper
{
    public interface IUserHelper
    {
        public ApplicationUser GetLoggedInUser();
        public string GetLoggedInUserRole();
        public IEnumerable<ApplicationUser> GetAllProjectManagers();
        public IEnumerable<ApplicationUser> GetAllDevelopers();
       
        
    }
}