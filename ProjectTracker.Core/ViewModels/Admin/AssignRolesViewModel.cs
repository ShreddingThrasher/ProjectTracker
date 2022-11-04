using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Admin
{
    public class AssignRolesViewModel
    {
        [Required]
        public string Employee { get; set; }

        [Required]
        public string Role { get; set; }

        public IEnumerable<string> Roles { get; set; } = new List<string>();

        public IEnumerable<string> Employees { get; set; } = new List<string>();
    }
}
