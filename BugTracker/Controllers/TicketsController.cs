using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.Helper;
using System.Threading.Tasks;

namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RolesViewModel helper = new RolesViewModel();

        // GET: Tickets
        [Authorize]
        public ActionResult Index(int? projectId)
        {
            var tickets = db.Tickets.Include(t => t.OwnerUser).Include(t => t.AssignedUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);

            if (projectId != null) // view by project
            {
                return View(tickets.Where(t => t.ProjectId == projectId).ToList());
            }
            else // view by role
            {
                if (User.IsInRole("Admin"))
                {
                    return View(tickets);
                }
                else if (User.IsInRole("Project Manager"))
                {
                    return View(tickets.Where(t => t.Project.Users.Any(u => u.UserName == User.Identity.Name)));
                }
                else if (User.IsInRole("Developer"))
                {
                    return View(tickets.Where(t => t.AssignedUser.UserName == User.Identity.Name));
                }
                else
                {
                    return View(tickets.Where(t => t.OwnerUser.UserName == User.Identity.Name));
                }

            }
        }
        // GET: Tickets/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // GET: Tickets/Create
        [Authorize]
        public ActionResult Create()
        {
            Ticket model = new Ticket();
            model.Created = new DateTimeOffset(DateTime.Now);
            model.Updated = new DateTimeOffset(DateTime.Now);
            model.OwnerUser = db.Users.Single(u => u.UserName == User.Identity.Name);
            model.OwnerUserId = model.OwnerUser.Id;
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "UserName");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "UserName");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View(model);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectId,AssignedUserId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,Title,Description,UpdateReason,Created,Updated")] Ticket tickets)
        {
            if (ModelState.IsValid)
            {
                tickets.Created = DateTimeOffset.Now;
                tickets.Updated = DateTimeOffset.Now;
                tickets.OwnerUser = db.Users.Find(tickets.OwnerUserId);
                db.Tickets.Add(tickets);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "UserName", tickets.AssignedUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "UserName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name",tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name",tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name",tickets.TicketTypeId);
            return View(tickets);
        }

        // GET: Tickets/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }

            UserRolesHelper helper = new UserRolesHelper();

            ViewBag.AssignedUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);

            TempData["OldTicket"] = tickets;
            
            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProjectId,AssignedUserId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,Title,Description,UpdateReason,Created,Updated")] Ticket tickets)
        {
            if (ModelState.IsValid)
            {
                var user = await db.Users.SingleOrDefaultAsync(u => u.UserName == User.Identity.Name);
                var changedTime = new DateTimeOffset(DateTime.Now);
                Ticket oldTicket = (Ticket)TempData["OldTicket"];

                if (tickets.AssignedUserId != oldTicket.AssignedUserId)
                {
                    string temp = "";

                    if (oldTicket.AssignedUserId == null)
                    {
                        temp = "Unassigned";
                    }
                    else
                    {
                        temp = oldTicket.AssignedUser.DisplayName;
                    }

                    db.TicketHistories.Add(new TicketHistory
                    {
                        Changed = changedTime,
                        Property = "Assigned User",
                        TicketId = tickets.Id,
                        OldValue = temp,
                        NewValue = db.Users.Find(tickets.AssignedUserId).DisplayName,
                        UserId = user.Id
                    });
                }

                if (tickets.TicketTypeId != oldTicket.TicketTypeId)
                {
                    db.TicketHistories.Add(new TicketHistory
                    {
                        Changed = changedTime,
                        Property = "Ticket Type",
                        TicketId = tickets.Id,
                        OldValue = oldTicket.TicketType.Name,
                        NewValue = db.TicketTypes.Find(tickets.TicketTypeId).Name,
                        UserId = user.Id
                    });
                }

                if (tickets.TicketPriorityId != oldTicket.TicketPriorityId)
                {
                    db.TicketHistories.Add(new TicketHistory
                    {
                        Changed = changedTime,
                        Property = "Ticket Priority",
                        TicketId = tickets.Id,
                        OldValue = oldTicket.TicketPriority.Name,
                        NewValue = db.TicketPriorities.Find(tickets.TicketPriorityId).Name,
                        UserId = user.Id
                    });
                }

                if (tickets.TicketStatusId != oldTicket.TicketStatusId)
                {
                    db.TicketHistories.Add(new TicketHistory
                    {
                        Changed = changedTime,
                        Property = "Ticket Status",
                        TicketId = tickets.Id,
                        OldValue = oldTicket.TicketStatus.Name,
                        NewValue = db.TicketStatuses.Find(tickets.TicketStatusId).Name,
                        UserId = user.Id
                    });
                }
                tickets.Updated = new DateTimeOffset(DateTime.Now);
                tickets.OwnerUser = db.Users.Find(tickets.OwnerUserId);
                tickets.AssignedUser = db.Users.Find(tickets.AssignedUserId);
                db.Entry(tickets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = tickets.Id });
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);
            ViewBag.Description = new SelectList(db.Tickets, "Id", "Description", tickets.Description);
            return View(tickets);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket tickets = db.Tickets.Find(id);
            db.Tickets.Remove(tickets);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult MarkNotificationSeen(int id)
        {
            var history = db.TicketHistories.Find(id);
            history.NotificationSeen = true;
            db.Entry(history).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
