using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementSystem.Helpers.UserHelper;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Models.ProjectViewModels;
using ProjectManagementSystem.ProjectManagementSystemDatabase.Context;
using ProjectManagementSystem.Repositories;

namespace ProjectManagementSystem.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProjectRepository _repository;
        private readonly IUserHelper _userHelper;

        public ProjectController(ApplicationDbContext context, IProjectRepository projectRepository,IUserHelper userHelper )
        {
            _context = context;
            _repository = projectRepository;
            _userHelper = userHelper;

        }
        
        public ActionResult Projects()
        {
            
            var projects = _repository.GetProjects().ToList();
            
            return View(projects);  
        } 
        
        public ActionResult Details(int id)  
        {  
            var project= _repository.GetProjectById(id);  
            return View(project);  
        }


        public ActionResult Create()
        {
            var vm = CreateProjectViewModel();
            return View(vm);  
        }  
   
        [HttpPost]  
        [ValidateAntiForgeryToken]  
        public ActionResult Create(ProjectViewModel project)  
        {  
            if (ModelState.IsValid)  
            {  
                Console.WriteLine(project.ProjectName);
                
                var newProj = new Project
                {
                    ProjectName = project.ProjectName,
                    ProjectManagerId = project.ProjectManagerId
                        
                };
                
                _repository.CreateProject(newProj);  
                 
                return RedirectToAction("Projects");  
            }  
            return View();  
        }  
   
        [HttpGet]  
        public ActionResult Edit(int id)
        {
            var vm = CreateProjectViewModel();
            var project = _repository.GetProjectById(id); 
            
                vm.ProjectCode = project.ProjectCode;
               vm.ProjectName = project.ProjectName;
               vm.ProjectManager = new ApplicationUser();
               vm.ProjectManagerId = project.ProjectManagerId;
               vm.ProjectManager.FullName = project.ProjectManager.FullName;

           return View(vm);  
        }  
   
        [HttpPost]  
        public ActionResult Edit(ProjectViewModel project)  
        {  
            if (ModelState.IsValid)  
            {  
                var newPr = new Project{ProjectCode = project.ProjectCode,ProjectName = project.ProjectName, ProjectManagerId = project.ProjectManagerId};
                _repository.UpdateProject(newPr);
                return RedirectToAction("Projects");  
   
            }  
            else  
            {  
                return View(project);  
            }            
        }  
   
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
   
   private ProjectViewModel CreateProjectViewModel()
   {
       var vm = new ProjectViewModel();
       var projManagers = _userHelper.GetAllProjectManagers().ToList();
       vm.ProjectManagers = new List<SelectListItem>();
             
       foreach (var pm in projManagers)
       {
           vm.ProjectManagers.Add(new SelectListItem(text: pm.FullName, value: pm.Id));
                
       }

       return vm;
   }
    }  
       
    
}