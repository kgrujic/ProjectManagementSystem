using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using ProjectManagementSystem.Helpers.UserHelper;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Repositories;

namespace ProjectManagementSystem.Tests.UserHelperTests
{
    public class UserHelperTest
    {
        private Mock<IUserRepository> _userRepository;
        private List<ApplicationUser> _users;

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            
            _users = new List<ApplicationUser>();
            _users.Add(new ApplicationUser{UserName = "test1", Email = "test1@test.com",FullName = "Test 1", RoleName = "ProjectManager"});
            _users.Add(new ApplicationUser{UserName = "test2", Email = "test2@test.com",FullName = "Test 2", RoleName = "ProjectManager"});
            _users.Add(new ApplicationUser{UserName = "test3", Email = "test3@test.com",FullName = "Test 3", RoleName = "ProjectManager"});
            _users.Add(new ApplicationUser{UserName = "test4", Email = "test4@test.com",FullName = "Test 4", RoleName = "Developer"});
            _users.Add(new ApplicationUser{UserName = "test5", Email = "test5@test.com",FullName = "Test 5", RoleName = "Developer"});
        }

        [Test]
        public void TestGetAllProjectManagers()
        {
            _userRepository.Setup(a => a.GetAllQueryable()).Returns(_users.AsQueryable());
            
            var userHelper = new UserHelper(_userRepository.Object);

            var projectManagersList = userHelper.GetAllProjectManagers().ToList();
            
            Assert.IsTrue(projectManagersList.Count == 3);
            Assert.IsTrue(projectManagersList.All(pm => pm.RoleName == "ProjectManager"));
        }
        [Test]
        public void TestGetAllProjectDevelopers()
        {
            _userRepository.Setup(a => a.GetAllQueryable()).Returns(_users.AsQueryable());
            
            var userHelper = new UserHelper(_userRepository.Object);

            var projectManagersList = userHelper.GetAllDevelopers().ToList();
            
            Assert.IsTrue(projectManagersList.Count == 2);
            Assert.IsTrue(projectManagersList.All(pm => pm.RoleName == "Developer"));
        }
    }
}