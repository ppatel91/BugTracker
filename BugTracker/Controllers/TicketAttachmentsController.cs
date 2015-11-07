using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using System.IO;

namespace BugTracker.Controllers
{
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttachments
        public ActionResult Index()
        {
            var ticketAttachment = db.TicketAttachments.Include(t => t.Ticket);
            return View(ticketAttachment.ToList());
        }

        // GET: TicketAttachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachments = db.TicketAttachments.Find(id);
            if (ticketAttachments == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Create
        public ActionResult Create(int id)
        {
            var attachment = new TicketAttachment();
            attachment.UserId = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name).Id;
            attachment.TicketId = id;

            //ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title"); //instead of title asigneduser was there
            return View(attachment);
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,FilePath,Description,Created,UserId,FileUrl")] TicketAttachment ticketAttachments, HttpPostedFileBase attachment)
        {
            if (ModelState.IsValid)
            {
                if (attachment != null && attachment.ContentLength > 0)
                {
                    
                    //relative server path
                    var filePath = "/Attachments/Images/";
                    //path on physical drive on server
                    var absPath = Server.MapPath("~" + filePath);
                    //media Url for relative Path
                    ticketAttachments.FileUrl = filePath + attachment.FileName;
                    //save image
                    attachment.SaveAs(Path.Combine(absPath, attachment.FileName));
                }

                    ticketAttachments.Created = DateTimeOffset.Now;
                    db.TicketAttachments.Add(ticketAttachments);
                    
                db.SaveChanges();
                    return RedirectToAction("Details", "Tickets", new { id = ticketAttachments.TicketId });

            }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachments.TicketId);
            ViewBag.TicketId = ticketAttachments.TicketId;
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachments = db.TicketAttachments.Find(id);
            if (ticketAttachments == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachments.TicketId); //AssignedUser=> Title
            return View(ticketAttachments);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,FilePath,Description,Created,UserId,FileUrl")] TicketAttachment ticketAttachments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttachments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachments.TicketId); //Title => AssignedUser
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachments = db.TicketAttachments.Find(id);
            if (ticketAttachments == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachments);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachment ticketAttachments = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachments);
            db.SaveChanges();
            return RedirectToAction("Index");
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
