using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{

    public class ProjectViewModel
    {
        public int id { get; set; }
        public string projectName { get; set; }
        public string PName { get; set; }
    }

    public class ProjectUsersView
    {
        public string ProjectId { get; set; }

        [Display(Name = "Project Name")]
        public string Project { get; set; }
    }


    public class ProjectUserViewModels
    {
        public int ProjectId { get; set; }
        public string ProjectManager { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Developers")]
        public System.Web.Mvc.MultiSelectList Users { get; set; }

        public string[] SelectedUsers { get; set; }
    }
}