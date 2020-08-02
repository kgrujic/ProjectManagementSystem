using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        
        public string ProjectManagerId { get; set; }
        
        public ApplicationUser ProjectManager { get; set; }
        
        public string UserRole { get; set; }
        public List<SelectListItem> ProjectManagers { set; get; }
    }
}