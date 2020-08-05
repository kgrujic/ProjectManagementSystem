using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Models.AccountViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.ProjectManagementSystemDatabase.Context;


namespace ProjectManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        
  

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,   
            ILogger<AccountController> logger,ApplicationDbContext context, IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _mapper = mapper;


        }
        
        private void AddErrors(IdentityResult result)
        {
            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
 
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }
        
        public ActionResult Users()
        {
            var users = _userManager.Users.ToList();
          
            return View(users);  
        }  

        
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            
            AddRolesToRegisterViewModel();
            var vm = new RegisterViewModel();
            
          
            return View(vm);
        }
 
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {

                ApplicationUser user = _mapper.Map<ApplicationUser>(model);
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user,  model.Role);
                    
                   // await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");
                    return RedirectToAction("Users");
                }
                AddErrors(result);
            }
 
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
 
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
 
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
               
                
               // Console.WriteLine(user.FullName);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    //return RedirectToLocal(returnUrl);
                    return RedirectToAction("Projects", "Project");
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
 
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
       
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Login");
        }
        
        public ActionResult Details(string id)  
        {  
            var user = _userManager.FindByIdAsync(id).Result;
            return View(user);  
        }  

        
        [HttpGet] 
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
          

            EditViewModel editViewModel = _mapper.Map<EditViewModel>(user);
            
            EditViewModel.Roles = new List<SelectListItem>
            {
                new SelectListItem{Text = "Administrator", Value = "Administrator"},
                new SelectListItem{Text = "ProjectManager", Value = "ProjectManager"},
                new SelectListItem{Text = "Developer", Value = "Developer"}
                
            };
            
            return View(editViewModel);  
        }  
   
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(EditViewModel user)
        {
            if (ModelState.IsValid)
            {
                var oldUser = _userManager.FindByIdAsync(user.Id).Result;
                oldUser.Id = user.Id;
                oldUser.FullName = user.FullName;
                oldUser.UserName = user.UserName;
                oldUser.Email = user.Email;
                oldUser.RoleName = user.RoleName;
                
                IdentityResult result = await _userManager.UpdateAsync(oldUser);
                
                var oldRoleId = _context.UserRoles.FirstOrDefault(r => r.UserId == user.Id)?.RoleId;
                var oldRole = _context.Roles.FirstOrDefault(r => r.Id == oldRoleId)?.Name;
                
                await _userManager.RemoveFromRoleAsync(oldUser, oldRole);
                await _userManager.AddToRoleAsync(oldUser, user.RoleName);
                
                return RedirectToAction("Users");
   
            }  
            else  
            {  
                return View(user);  
            }            
        }  
   
        [HttpGet]  
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(string id)  
        {  
            var user=_userManager.FindByIdAsync(id).Result;  
            return View(user);  
        }  
   
        [HttpPost] 
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(ApplicationUser user)
        {
          
            var us = _userManager.FindByIdAsync(user.Id).Result;
            var oldRoleId = _context.UserRoles.FirstOrDefault(r => r.UserId == us.Id)?.RoleId;
            var oldRole = _context.Roles.FirstOrDefault(r => r.Id == oldRoleId)?.Name;
            
           _userManager.DeleteAsync(us);
           _userManager.RemoveFromRoleAsync(user, oldRole);
           return RedirectToAction("Index","Home");
        }

        public void AddRolesToRegisterViewModel()
        {
            RegisterViewModel.Roles = new List<SelectListItem>
            {
                new SelectListItem{Text = "Administrator", Value = "Administrator"},
                new SelectListItem{Text = "ProjectManager", Value = "ProjectManager"},
                new SelectListItem{Text = "Developer", Value = "Developer"}
                
            };
        }
      

    }
}
