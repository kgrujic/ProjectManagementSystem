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
         
        private IContextFactory _contextFactory;
        public ProjectRepository(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;

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
                return context.Projects.Find(id);
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
            using (var context = _contextFactory.CreateContext())
            {
                context.Entry(project).State = EntityState.Modified;
                context.SaveChanges();
            }
          
        }

        public void DeleteProject(int id)
        {
            using (var context = _contextFactory.CreateContext())
            {
                var project = context.Projects.Find(id);
                if (project != null)
                {
                    context.Projects.Remove(project);
                    context.SaveChanges();
                }
                
            }
            
        }

       
    }
}