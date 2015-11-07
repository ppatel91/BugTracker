using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy, hh:mm tt}", ApplyFormatInEditMode = true)]
        public System.DateTimeOffset Changed { get; set; }
        public string UserId { get; set; }
        public string Property { get; set; }
        public bool NotificationSeen { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }

        public TicketHistory()
        {
            NotificationSeen = false;
        }
    }


}