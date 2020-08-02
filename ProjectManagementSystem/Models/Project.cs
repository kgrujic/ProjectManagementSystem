using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementSystem.Models
{
    public class Project
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
        
        public List<Task> Tasks { get; set; }
        
    }
}