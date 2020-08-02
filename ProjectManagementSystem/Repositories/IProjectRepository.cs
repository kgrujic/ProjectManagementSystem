using System.Collections.Generic;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Repositories
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetProjects();
        IEnumerable<Project> GetProjectsForProjectManager(string pmId);

        public IEnumerable<Project> GetProjectsForDeveloper(string devId);
        Project GetProjectById(int id);  
        void CreateProject(Project project);  
        void UpdateProject(Project project);  
        void DeleteProject(int id);  
       
    }
}