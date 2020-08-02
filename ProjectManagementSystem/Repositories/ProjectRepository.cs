using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ProjectManagementSystemDatabase.Context;

namespace ProjectManagementSystem.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
         
        private readonly IContextFactory _contextFactory;
        private readonly ApplicationDbContext _context;
        public ProjectRepository(IContextFactory contextFactory, ApplicationDbContext context)
        {
            _contextFactory = contextFactory;
            _context = context;

        }
        public IEnumerable<Project> GetProjects()
        {
            using (var context = _contextFactory.CreateContext())
            {
                return context.Projects.Include(p => p.ProjectManager).ToList();
            }
            
        }

        public Project GetProjectById(int id)
        {
            using (var context = _contextFactory.CreateContext())
            {
                return context.Projects.Include(p => p.ProjectManager).FirstOrDefault(p => p.ProjectCode == id);
            }
           
        }

        public void CreateProject(Project project)
        {
            using (var context = _contextFactory.CreateContext())
            {
               
                context.Projects.Add(project);
                context.SaveChanges();
              
            }
           
            
        }

        public void UpdateProject(Project project)
        {

            _context.Projects.Update(project);
                _context.SaveChanges();
            
          
        }

        public void DeleteProject(int id)
        {
           
            var project = _context.Projects.FirstOrDefault(p => p.ProjectCode == id);
                if (project != null)
                {
                  
                    _context.Projects.Remove(project);
                    _context.SaveChanges();
                }
                
            
            
        }

       
    }
}