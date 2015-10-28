﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }

        public string UserId { get; set; }
        public string Comment { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTimeOffset Created { get; set; }


        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User {get; set;}
    }
}