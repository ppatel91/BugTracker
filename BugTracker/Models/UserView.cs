using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{

    public class ChangeUserNameViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
    }

    public class UserView
    {
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "User Roles")]
        public List<string> UserRoles { get; set; }

        [Display(Name = "Assigned Projects")]
        public List<Project> UserProjects { get; set; }

        [Display(Name = "Display Name")]
        public List<Ticket> TicketsOwned { get; set; }

        [Display(Name = "Display Name")]
        public List<Ticket> TicketsAssigned { get; set; }

        public List<TicketHistory> TicketNotifications { get; set; }

    }
}