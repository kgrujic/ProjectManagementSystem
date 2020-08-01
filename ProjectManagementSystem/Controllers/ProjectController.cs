using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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

        public ProjectController(ApplicationDbContext context, IProjectRepository projectRepository)
        {
            _context = context;
            _repository = projectRepository;

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
            return View();  
        }  
   
        [HttpPost]  
        [ValidateAntiForgeryToken]  
        public ActionResult Create(Project project)  
        {  
            if (ModelState.IsValid)  
            {  
                Console.WriteLine(project.ProjectName);
                
                _repository.CreateProject(project);  
                 
                return RedirectToAction("Projects");  
            }  
            return View();  
        }  
   
        [HttpGet]  
        public ActionResult Edit(int id)  
        {  
            var project = _repository.GetProjectById(id);  
            return View(project);  
        }  
   
        [HttpPost]  
        public ActionResult Edit(Project project)  
        {  
            if (ModelState.IsValid)  
            {  
                _repository.UpdateProject(project);
                return RedirectToAction("Projects");  
   
            }  
            else  
            {  
                return View(project);  
            }            
        }  
   /*
        [HttpGet]  
        public ActionResult Delete(int id)  
        {  
            var project=_repository.GetProjectById(id);  
            return View(project);  
        }  
   
        [HttpPost]
        public ActionResult Delete(int id)  
        {  
            _repository.DeleteProject(id);  
            _repository.Save();  
            return RedirectToAction("Projects");  
        }  */
    }  
       
    
}