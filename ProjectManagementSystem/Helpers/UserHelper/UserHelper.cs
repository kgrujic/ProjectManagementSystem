using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ProjectManagementSystemDatabase.Context;
using ProjectManagementSystem.Repositories;


namespace ProjectManagementSystem.Helpers.UserHelper
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IUserRepository _userRepository;
        public UserHelper(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserHelper(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor,  IUserRepository userRepository)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        enum Role
        {
            Administrator,
            ProjectManager,
            Developer
        }
        
        public ApplicationUser GetLoggedInUser()
        {
            var userId = _userManager.GetUserId( _httpContextAccessor.HttpContext.User);
            var usr = _userManager.FindByIdAsync(userId).Result;
            return usr;
        }

        public string GetLoggedInUserRole()
        {
            var userId = _userManager.GetUserId( _httpContextAccessor.HttpContext.User);
            var usr = _userManager.FindByIdAsync(userId).Result;
            return usr.RoleName;
        }

        public IEnumerable<ApplicationUser> GetAllProjectManagers()
        {
          
            
                return  _userRepository.GetAllQueryable().Where(u => u.RoleName.Equals(Role.ProjectManager.ToString()));;

        }

        public IEnumerable<ApplicationUser> GetAllDevelopers()
        {
            
                return _userRepository.GetAllQueryable().Where(u => u.RoleName.Equals(Role.Developer.ToString()));
            
        }

       
    }
}