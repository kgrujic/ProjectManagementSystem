using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementSystem.Helpers.UserHelper;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Models.ProjectViewModels;
using ProjectManagementSystem.Repositories;

namespace ProjectManagementSystem.Controllers
{
    public class ProjectController : Controller
    {
      
        private readonly IProjectRepository _repository;
        private readonly IUserHelper _userHelper;
        
        private readonly IMapper _mapper;
        
        enum Role
        {
            Administrator,
            ProjectManager,
            Developer
        }

        public ProjectController(IProjectRepository projectRepository,IUserHelper userHelper, IMapper mapper )
        {
            
            _repository = projectRepository;
            _userHelper = userHelper;
            _mapper = mapper;
            
            AddProjManagersToProjectViewModel();

        }
        
        [Authorize(Roles = "Administrator, ProjectManager, Developer")]
        public ActionResult Projects()
        {
            var projects = new List<Project>();
            
            var loggedInUser = _userHelper.GetLoggedInUser();

            var loggedInURoleName = _userHelper.GetLoggedInUserRole();
            
            if (loggedInURoleName == Role.Administrator.ToString())
            {
               projects = _repository.GetAllProjects().ToList();
            }
            else if (loggedInURoleName == Role.ProjectManager.ToString())
            {
                projects = _repository.GetProjectsForProjectManager(loggedInUser.Id).ToList();
            }   
            else if (loggedInURoleName == Role.Developer.ToString())
            {
                projects = _repository.GetProjectsForDeveloper(loggedInUser.Id).ToList();
            }
            
            return View(projects);  
        } 
        
        public ActionResult Details(int id)  
        {  
            var project= _repository.GetProjectById(id);  
            return View(project);  
        }

        
        [Authorize(Roles = "Administrator, ProjectManager")]
        public ActionResult Create()
        {
            var projectViewModel = new ProjectViewModel();
          
            return View(projectViewModel);  
        }  
        
        [Authorize(Roles = "Administrator, ProjectManager")]
        [HttpPost]  
        [ValidateAntiForgeryToken]  
        public ActionResult Create(ProjectViewModel project)  
        {  
            if (ModelState.IsValid)  
            {  
                Console.WriteLine(project.ProjectName);
                
                var newProj = new Project();
                var loggedInUserRole = _userHelper.GetLoggedInUserRole();

                if (loggedInUserRole == Role.Administrator.ToString())
                {
                    newProj.ProjectName = project.ProjectName;
                    newProj.ProjectManagerId = project.ProjectManagerId;

                } else if (loggedInUserRole == Role.ProjectManager.ToString())
                {
                    var loggedInUser = _userHelper.GetLoggedInUser();

                    newProj.ProjectName = project.ProjectName;
                    newProj.ProjectManagerId = loggedInUser.Id;

                }
             
                
                _repository.CreateProject(newProj);  
                 
                return RedirectToAction("Projects");  
            }  
            return View();  
        }  
   
        [Authorize(Roles = "Administrator")]
        [HttpGet]  
        public ActionResult Edit(int id)
        {
            
            var project = _repository.GetProjectById(id);

            var projectViewModel = _mapper.Map<ProjectViewModel>(project);
            
           return View(projectViewModel);  
        }  
            
        [Authorize(Roles = "Administrator")]
        [HttpPost]  
        public ActionResult Edit(ProjectViewModel project)  
        {  
            if (ModelState.IsValid)  
            {
                var newProject = _mapper.Map<Project>(project);
                
                _repository.UpdateProject(newProject);
                
                return RedirectToAction("Projects");  
   
            }  
            else  
            {  
                return View(project);  
            }            
        }  
        
        [Authorize(Roles = "Administrator")]
        [HttpGet]  
        public ActionResult Delete(int id)  
        {  
            var project=_repository.GetProjectById(id);  
            return View(project);  
        }  
   
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)  
        {  
            _repository.DeleteProject(id);  
           
            return RedirectToAction("Projects");  
        }  
   
       private void AddProjManagersToProjectViewModel()
       {

           var projectManagers = _userHelper.GetAllProjectManagers().ToList();

           ProjectViewModel.ProjectManagers = new List<SelectListItem>();
                 
           foreach (var pm in projectManagers)
           {
               ProjectViewModel.ProjectManagers.Add(new SelectListItem(text: pm.FullName, value: pm.Id));
                    
           }

           
       } 
    }  
       
    
}