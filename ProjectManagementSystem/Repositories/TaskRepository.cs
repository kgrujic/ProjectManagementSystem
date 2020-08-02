using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ProjectManagementSystemDatabase.Context;

namespace ProjectManagementSystem.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Task> GetTasks(int projectId)
        {
            return _context.Tasks.Include(t => t.Assignee).Include(t => t.Project).Where(t => t.ProjectCode == projectId).ToList();
        }  
        
        public IEnumerable<Task> GetTasksForUser(string userId)
        {
            return _context.Tasks.Include(t => t.Assignee).Include(t => t.Assignee).Include(t => t.Project).Where(t => t.AssigneeId == userId || t.AssigneeId == null).ToList();
        }

        public Task GetTaskById(int id)
        {
            return _context.Tasks.Include(t => t.Assignee).Include(t => t.Assignee).Include(t => t.Project).FirstOrDefault(t => t.TaskID == id);
        }

        public void CreateTask(Task task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public void UpdateTask(Task task)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges(); 
        }

        public void DeleteTask(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.TaskID == id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
        }
    }
}