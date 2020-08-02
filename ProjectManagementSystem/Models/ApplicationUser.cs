using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ProjectManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Username:")]
        public override string UserName { get; set; }
        
        [Required]
        [StringLength(50)]
        [Display(Name = "Full name:")]
        public string FullName { get; set; }
        
        [Required]
        [StringLength(50)]
        [Display(Name = "Role:")]
        public string RoleName { get; set; }
        
        public List<Project> Projects { get; set; }
        
        public List<Task> Tasks { get; set; }

    }
}
