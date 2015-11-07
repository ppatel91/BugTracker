using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }

        public ApplicationUser()
        {
            //this.TicketNotification = new HashSet<TicketNotifications>();
           // this.TicketHistory = new HashSet<TicketHistories>();
          //  this.TicketComment = new HashSet<TicketComments>();
          //  this.TicketAttachment = new HashSet<TicketAttachments>();
            this.Projects = new HashSet<Project>();
            this.TicketsOwned = new HashSet<Ticket>();
            this.TicketsAssigned = new HashSet<Ticket>();

        }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        // public virtual ICollection<TicketNotifications> TicketNotification { get; set; }
        //public virtual ICollection<TicketHistories> TicketHistory { get; set; }
        //public virtual ICollection<TicketComments> TicketComment { get; set; }
       // public virtual ICollection<TicketAttachments> TicketAttachment{ get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        [InverseProperty ("OwnerUser")]
        public virtual ICollection<Ticket> TicketsOwned { get; set; }
        [InverseProperty ("AssignedUser")]
        public virtual ICollection<Ticket> TicketsAssigned { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("AzureConnection", throwIfV1Schema: false)
          //  : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<TicketAttachment> TicketAttachments { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }

    }
}