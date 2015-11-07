using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Ticket
    {
        public Ticket()
        {
            this.TicketAttachments = new HashSet<TicketAttachment>();
            this.TicketComments = new HashSet<TicketComment>();
            this.TicketHistories = new HashSet<TicketHistory>();

        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public int TicketTypeId { get; set; }
        public Nullable <int> TicketPriorityId { get; set; }
        public int TicketStatusId { get; set; }
        public string OwnerUserId { get; set; }
        public string AssignedUserId { get; set; }
        public string UpdateReason { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy, hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTimeOffset Created { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy, hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTimeOffset Updated { get; set; }

        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
        public virtual ICollection<TicketComment> TicketComments  { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual Project Project { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual TicketType TicketType { get; set; }


        public virtual ApplicationUser OwnerUser { get; set; }
        public virtual ApplicationUser AssignedUser { get; set; }
    }
}