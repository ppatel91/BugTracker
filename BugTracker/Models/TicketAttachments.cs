﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    using System;
    using System.Collections.Generic;

    public class TicketAttachment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }
        public string FileUrl { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTimeOffset Created { get; set; }


        public virtual ApplicationUser User { get; set;}
        public virtual Ticket Ticket { get; set; }

    }
}