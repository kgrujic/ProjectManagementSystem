using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectManagementSystem.Models.TaskViewModels
{
    public class TaskViewModel
    {
        [Key]
        public int TaskID { get; set; }
        
       
        [StringLength(50)]
        [Display(Name = "Status")]
        public string? Status { get; set; }
        
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
        
        public ApplicationUser? Assignee { get; set; }
        
        public int ProjectCode { get; set; }
        
        public Project Project{ get; set; }
        
        public List<SelectListItem> Developers { set; get; }
        public List<SelectListItem> Statuses { set; get; }
    }
}