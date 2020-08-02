using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class Task
    {
        [Key]
        public int TaskID { get; set; }
        
        [Required]
        [StringLength(50)]
        [Display(Name = "Status")]
        public string Status { get; set; }
        
        [Required]
        [Display(Name = "Progress")]
        public int Progress { get; set; }
        
        [Required]
        [Display(Name = "Deadline")]
        public DateTime Deadline { get; set; }
        
        [Required]
        [StringLength(250)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        
        public string AssigneeId { get; set; }
        
        public ApplicationUser Assignee { get; set; }
        
        public int ProjectCode { get; set; }
        
        public Project Project{ get; set; }
        
        
        
    }
}