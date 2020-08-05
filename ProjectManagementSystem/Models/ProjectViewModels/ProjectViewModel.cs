using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementSystem.Models.AccountViewModels;

namespace ProjectManagementSystem.Models.ProjectViewModels
{
    public class ProjectViewModel
    {
        [Key]
        [Display(Name = "Project code:")]
        public int ProjectCode { get; set; }
        
        [Required]
        [StringLength(50)]
        [Display(Name = "Project name:")]
        public string ProjectName { get; set; }
        
        [Required(ErrorMessage= "Project Manager is required")]
        public string ProjectManagerId { get; set; }
        
        public ApplicationUser ProjectManager { get; set; }
        
        public static List<SelectListItem> ProjectManagers { set; get; }
    }
}