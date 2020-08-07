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
        
        private readonly ApplicationDbContext _context;
        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;

        }
        
        public IEnumerable<Project> GetAllProjects()
        {
            return _context.Projects.Include(p => p.ProjectManager).Include(p => p.Tasks).ToList();
            
        } 
        public IEnumerable<Project> GetProjectsForProjectManager(string pmId)
        {
            return _context.Projects.Include(p => p.ProjectManager).Include(p => p.Tasks).Where(p => p.ProjectManagerId == pmId).ToList();
                
        }
        public IEnumerable<Project> GetProjectsForDeveloper(string devId)
        {
            var tasks = _context.Tasks.Where(t => t.AssigneeId == devId).ToList();
            var projects = new List<Project>();

            var contxProj = _context.Projects.Include(p => p.Tasks).Include(p => p.ProjectManager).ToList();

            foreach (var project in contxProj)
            {
                foreach (var task in tasks)
                {
                    if (project.Tasks.Contains(task))
                    {
                        projects.Add(project);
                        break;
                    }
                }
            }

            return projects;

        }

        public Project GetProjectById(int id)
        {
            return _context.Projects.Include(p => p.ProjectManager).FirstOrDefault(p => p.ProjectCode == id);
        }

        public void CreateProject(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();

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