using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMapper _mapper;
        
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

        public TaskController(ITaskRepository repository, IUserHelper userHelper,IMapper mapper)
        {
            _repository = repository;
            _userHelper = userHelper;
            _mapper = mapper;
            
            SetStatusesAndDevelopers();
        }
        
        [Authorize(Roles = "Administrator, ProjectManager, Developer")]
        public ActionResult Tasks(int prId)
        {
          
            var tasks = new List<Task>();
            
            var loggedInUser = _userHelper.GetLoggedInUser();
            var loggedInURoleName = _userHelper.GetLoggedInUserRole();
            
            if (loggedInURoleName == Role.Administrator.ToString() || loggedInURoleName == Role.ProjectManager.ToString())
            {
                tasks = _repository.GetTasks(prId).ToList();
            }
            else if (loggedInUser.RoleName == Role.Developer.ToString())
            {
                tasks = _repository.GetTasksForUser(loggedInUser.Id, prId).ToList();
            }
            
            return View(tasks);  
        } 
        
        [Authorize(Roles = "Administrator, ProjectManager")]
        public ActionResult Create(int projId)
        {
            var taskViewModel = CreateTaskViewModel(projId);
           
            return View(taskViewModel);  
        }  
        
        [Authorize(Roles = "Administrator, ProjectManager")]
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
        
        [Authorize(Roles = "Administrator, ProjectManager, Developer")]
        [HttpGet]  
        public ActionResult Edit(int id)
        {
           
            var task = _repository.GetTaskById(id);

            var taskViewModel = _mapper.Map<TaskViewModel>(task);

            return View(taskViewModel);  
        }  
        
        [Authorize(Roles = "Administrator, ProjectManager, Developer")]
        [HttpPost]  
        public ActionResult Edit(TaskViewModel task)  
        {  
            if (ModelState.IsValid)  
            {
                var newTask = _mapper.Map<Task>(task);
                
               _repository.UpdateTask(newTask);
               
                 return RedirectToAction( "Tasks", new RouteValueDictionary( 
                    new { controller = "Task", action = "Tasks", prId = task.ProjectCode } ) );
               
            }  
            else  
            {  
                return View(task);  
            }            
        }  
        
        [Authorize(Roles = "Administrator")]
        [HttpGet]  
        public ActionResult Delete(int id)  
        {  
            var task =_repository.GetTaskById(id);  
            return View(task);  
        }  
        
        [Authorize(Roles = "Administrator")]
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

            vm.ProjectCode = projId;

            return vm;
        }
        
        private void SetStatusesAndDevelopers()
        {
          
            var developers = _userHelper.GetAllDevelopers().ToList();
         
            TaskViewModel.Developers = new List<SelectListItem>();
             
            foreach (var dev in developers)
            {
                TaskViewModel.Developers.Add(new SelectListItem(text: dev.FullName, value: dev.Id));
                
            }
            
            TaskViewModel.Statuses = new List<SelectListItem>
            {
                new SelectListItem{Text = "New", Value = Status.New.ToString()},
                new SelectListItem{Text = "In Progress", Value = Status.InProgress.ToString()},
                new SelectListItem{Text = "Finished", Value = Status.Finished.ToString()},
                
            };
            
        }

    }
}