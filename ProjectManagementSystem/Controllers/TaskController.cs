using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using ProjectManagementSystem.Helpers.UserHelper;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Models.TaskViewModels;
using ProjectManagementSystem.Repositories;

namespace ProjectManagementSystem.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskRepository _repository;
        private readonly IUserHelper _userHelper;
        
        enum Role
        {
            Administrator,
            ProjectManager,
            Developer
        }    
        enum Status
        {
            New,
            InProgress,
            Finished
        }

        public TaskController(ITaskRepository repository, IUserHelper userHelper)
        {
            _repository = repository;
            _userHelper = userHelper;
        }
        
        public ActionResult Tasks(int prId)
        {
            var tasks = new List<Task>();
            var loggedInUser = _userHelper.GetLoggedInUser();
            
            if (loggedInUser.RoleName == Role.Administrator.ToString())
            {
                tasks = _repository.GetTasks(prId).ToList();
            }
            else if (loggedInUser.RoleName == Role.ProjectManager.ToString())
            {
                tasks = _repository.GetTasks(prId).ToList();
                
            }   
            else if (loggedInUser.RoleName == Role.Developer.ToString())
            {
                tasks = _repository.GetTasksForUser(loggedInUser.Id).ToList();
            }
            
            return View(tasks);  
        } 
        
        
        public ActionResult Create(int projId)
        {
            var vm = CreateTaskViewModel(projId);
           
            return View(vm);  
        }  
        
        [HttpPost]  
        [ValidateAntiForgeryToken]  
        public ActionResult Create(TaskViewModel task)  
        {  
            if (ModelState.IsValid)  
            {
               
                var newTask = new Task();

                newTask.Status = task.Status ?? Status.New.ToString();
               
                newTask.Progress = 0;
                newTask.Deadline = task.Deadline;
                newTask.Description = task.Description;
                if (task.AssigneeId != null)
                {
                    newTask.AssigneeId = task.AssigneeId;
                }

                newTask.ProjectCode = task.ProjectCode;
               
                _repository.CreateTask(newTask);  
                 
                 return RedirectToAction( "Tasks", new RouteValueDictionary( 
                    new { controller = "Task", action = "Tasks", prId = task.ProjectCode } ) );
            }  
            return View(task);  
        }  
        
        [HttpGet]  
        public ActionResult Edit(int id)
        {
            var vm = CreateTaskViewModel();
            var task = _repository.GetTaskById(id); 
            
            vm.TaskID = task.TaskID;
            vm.Status = task.Status;
            vm.Progress = task.Progress;
            vm.AssigneeId = task.AssigneeId;
            vm.ProjectCode = task.ProjectCode;
            vm.Assignee = new ApplicationUser();
            vm.Project = new Project();
            vm.Deadline = task.Deadline;
            vm.Description = task.Description;

            if (task.Assignee != null)
            {
                vm.Assignee.FullName = task.Assignee.FullName;
            }
            vm.Project.ProjectName = task.Project.ProjectName;

            return View(vm);  
        }  
   
        [HttpPost]  
        public ActionResult Edit(TaskViewModel task)  
        {  
            if (ModelState.IsValid)  
            {  
                
                var newTask = new Task
                {
                    TaskID = task.TaskID,Status = task.Status, Progress = task.Progress, Deadline = task.Deadline, Description = task.Description,
                    AssigneeId = task.AssigneeId,ProjectCode =  task.ProjectCode
                        
                };
               _repository.UpdateTask(newTask);
                 return RedirectToAction( "Tasks", new RouteValueDictionary( 
                    new { controller = "Task", action = "Tasks", prId = task.ProjectCode } ) );
   
            }  
            else  
            {  
                return View(task);  
            }            
        }  
        
        [HttpGet]  
        public ActionResult Delete(int id)  
        {  
            var task =_repository.GetTaskById(id);  
            return View(task);  
        }  
   
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var pc = _repository.GetTaskById(id).ProjectCode;
            _repository.DeleteTask(id);  
            
           
            return RedirectToAction( "Tasks", new RouteValueDictionary( 
                new { controller = "Task", action = "Tasks", prId = pc } ) );
        }  
        
        private TaskViewModel CreateTaskViewModel(int projId)
        {
            var vm = new TaskViewModel();
            var developers = _userHelper.GetAllDevelopers().ToList();
         
            vm.Developers = new List<SelectListItem>();
             
            foreach (var dev in developers)
            {
                vm.Developers.Add(new SelectListItem(text: dev.FullName, value: dev.Id));
                
            }
            
            vm.Statuses = new List<SelectListItem>
            {
                new SelectListItem{Text = "New", Value = Status.New.ToString()},
                new SelectListItem{Text = "In Progress", Value = Status.InProgress.ToString()},
                new SelectListItem{Text = "Finished", Value = Status.Finished.ToString()},
                
            };
            
            Console.WriteLine(projId + "met");

            vm.ProjectCode = projId;

            return vm;
        }private TaskViewModel CreateTaskViewModel()
        {
            var vm = new TaskViewModel();
            var developers = _userHelper.GetAllDevelopers().ToList();
         
            vm.Developers = new List<SelectListItem>();
             
            foreach (var dev in developers)
            {
                vm.Developers.Add(new SelectListItem(text: dev.FullName, value: dev.Id));
                
            }
            
            vm.Statuses = new List<SelectListItem>
            {
                new SelectListItem{Text = "New", Value = Status.New.ToString()},
                new SelectListItem{Text = "In Progress", Value = Status.InProgress.ToString()},
                new SelectListItem{Text = "Finished", Value = Status.Finished.ToString()},
                
            };
            

            return vm;
        }

    }
}