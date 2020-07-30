using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ProjectManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public override string UserName { get; set; }
        public string FullName { get; set; }

    }
}
