using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ProjectManagementSystemDatabase.Context;
using ProjectManagementSystem.Repositories;

namespace ProjectManagementSystem.Tests.ProjectRepositoryTests
{
    public class ProjectRepositoryTest
    {
        private Mock<ApplicationDbContext> _context;
        private Mock<DbSet<Project>> _projects;

        [SetUp]
        public void Setup()
        {
            _context = new Mock<ApplicationDbContext>();
            
        }

        [Test]
        public void TestGetProjectsForProjectManager()
        {
            var list = GetQueryableMockDocumentDbSet().Object;
            _context.Setup(c => c.Set<Project>()).Returns(list);
            var projectRepository = new ProjectRepository(_context.Object);

            var projects = projectRepository.GetProjectsForProjectManager("1").ToList();

            Assert.IsTrue(projects.Count == 4);
            Assert.IsTrue(projects.All(p => p.ProjectManagerId == "1"));
        }

        private static Mock<DbSet<Project>> GetQueryableMockDocumentDbSet()
        {
            var data = new List<Project>
            {
                new Project
                {
                    ProjectCode = 1, ProjectName = "Project1", ProjectManagerId = "1", ProjectManager = new ApplicationUser(), 
                    Tasks = new List<Task>()
                },
                new Project {ProjectCode = 2, ProjectName = "Project2", ProjectManagerId = "1",ProjectManager = new ApplicationUser(), 
                    Tasks = new List<Task>()},
                new Project {ProjectCode = 3, ProjectName = "Project3", ProjectManagerId = "1",ProjectManager = new ApplicationUser(), 
                    Tasks = new List<Task>()},
                new Project {ProjectCode = 4, ProjectName = "Project4", ProjectManagerId = "1",ProjectManager = new ApplicationUser(), 
                    Tasks = new List<Task>()},
                new Project {ProjectCode = 5, ProjectName = "Project5", ProjectManagerId = "2",ProjectManager = new ApplicationUser(), 
                    Tasks = new List<Task>()},
            };

            var mockDocumentDbSet = new Mock<DbSet<Project>>();
            mockDocumentDbSet.As<IQueryable<Project>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
            mockDocumentDbSet.As<IQueryable<Project>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
            mockDocumentDbSet.As<IQueryable<Project>>().Setup(m => m.ElementType)
                .Returns(data.AsQueryable().ElementType);
            mockDocumentDbSet.As<IQueryable<Project>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockDocumentDbSet.Setup(m => m.Add(It.IsAny<Project>())).Callback<Project>(data.Add);
            return mockDocumentDbSet;
        }
    }
}