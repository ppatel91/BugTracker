using BugTracker.Helper;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class RoleManagerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper helper = new UserRolesHelper();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            // get a list of all roles in the DB
            var roles = db.Roles.ToList();
            //Instantiate the view model
            var model = new List<RolesViewModel>();
            //loop through all the roles in the DB and add a new RolesViewModel object for each one
            foreach (var role in roles)
            {
                model.Add(new RolesViewModel { RoleId = role.Id, RoleName = role.Name });
            }
            //semd the model
            return View(model);
        }

        // GET: RoleManager/AssignUser
        [Authorize(Roles = "Admin")]
        public ActionResult AssignToRole(string id)
        {
            var role = db.Roles.Find(id);
            var model = new RolesViewModel();
            model.RoleId = role.Id;
            model.RoleName = role.Name;
            var userRoleList = helper.UsersNotInRole(role.Name);

            model.Users = new MultiSelectList(userRoleList.OrderBy(m => m.DisplayName), "Id", "DisplayName", null);

            return View(model);
        }

        //Post: RoleManager/AssignUser
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult AssignToRole(RolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string id in model.SelectedUsers)
                    {
                        helper.AddUserToRole(id, model.RoleName);
                    }
                }
                return RedirectToAction("Index", "RoleManager");
            }
            return View(model);
        }


        // GET: RoleManager/RemoveUserRole
        [Authorize(Roles = "Admin")]
        public ActionResult RemoveFromRole(string Id)
        {
            var role = db.Roles.Find(Id);
            var model = new RolesViewModel();
            model.RoleId = role.Id;
            model.RoleName = role.Name;
            var userRoleList = helper.UsersInRole(role.Name);

            model.Users = new MultiSelectList(userRoleList.OrderBy(m => m.DisplayName), "Id", "DisplayName", null);

            return View(model);
        }

        //Post: RoleManager/RemoveUserRole
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromRole(RolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string id in model.SelectedUsers)
                    {
                        helper.RemoveUserFromRole(id, model.RoleName);
                    }
                }
                return RedirectToAction("Index", "RoleManager");
            }
            return View(model);
        }
    }
}