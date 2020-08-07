using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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