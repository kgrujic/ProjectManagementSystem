using System.ComponentModel.DataAnnotations;

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
    }
}