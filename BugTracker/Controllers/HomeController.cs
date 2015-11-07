using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Notifications()
        {
            var user = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
            var model = new UserView
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DisplayName = user.DisplayName,
                Email = user.Email,
                UserProjects = user.Projects.ToList(),
                TicketsAssigned = user.TicketsAssigned.ToList(),
                TicketsOwned = user.TicketsOwned.ToList(),
                TicketNotifications = db.TicketHistories.Where(h => h.NotificationSeen == false && h.UserId == user.Id).ToList()
            };
            return View(model);
        }

        public ActionResult Dashboard()
        {
            ViewBag.Message = "Your Dashboard page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public int? GetNotifications(string name)
        {
            if (name != "" && name != null)
            {
                var num = db.TicketHistories.Where(h => h.NotificationSeen == false && h.User.UserName == name).Count();
                return num;
            }
            return null;
        }
    }
}