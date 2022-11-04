﻿using ProjectTracker.Core.ViewModels.Department;
using ProjectTracker.Infrastructure.DataConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Project
{
    public class CreateProjectViewModel
    {
        [Required]
        [StringLength(ProjectConstants.NameMaxLength,
            MinimumLength = ProjectConstants.NameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(ProjectConstants.DescriptionMaxLength,
            MinimumLength = ProjectConstants.DescriptionMinLength)]
        public string Description { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }

        public IEnumerable<CreateProjectDepartmentModel> Departments { get; set; } 
            = new List<CreateProjectDepartmentModel>();
    }
}
