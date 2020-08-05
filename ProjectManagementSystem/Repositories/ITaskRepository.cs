using System.Collections.Generic;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Repositories
{
    public interface ITaskRepository
    {
        IEnumerable<Task> GetTasks(int projectId);
        IEnumerable<Task> GetTasksForUser(string userId,  int prId);
        Task GetTaskById(int id);  
        void CreateTask(Task task);  
        void UpdateTask(Task task);  
        void DeleteTask(int id);  
       
    }
}