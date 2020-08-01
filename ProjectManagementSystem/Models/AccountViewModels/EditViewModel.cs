using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectManagementSystem.Models.AccountViewModels
{
    public class EditViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Username")]
        public string Id { get; set; }
        
        [Required]
        [StringLength(100)]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
        
        public List<SelectListItem> Roles { set; get; }
        
    }
}
