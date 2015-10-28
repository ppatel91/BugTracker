using BugTracker.Helper;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class ProjectManagerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserProjectsHelper helper = new UserProjectsHelper();


        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Index()
        {
            // get a list of all Projects in the DB
            var projects = db.Projects.ToList();
            //Instantiate the view model
            var model = new List<ProjectUserViewModels>();
            //loop through all the Projects in the DB and add a new ProjectUserViewModel object for each one
            foreach (var project in projects)
            {
                model.Add(new ProjectUserViewModels { ProjectId = project.Id, ProjectName = project.Name });
            }
            //semd the model
            return View(model);
        }



        // GET: ProjectManager/AssignUsers/Developers
        [Authorize (Roles = "Admin, Project Manager")]
        public ActionResult AssignUserProjects(int id)
        {
            var project = db.Projects.Find(id);
            var model = new ProjectUserViewModels { ProjectId = id, ProjectName = project.Name };
            var userProjectList = helper.ListDevelopersNotAssigned(id);

            model.Users = new MultiSelectList(userProjectList.OrderBy(m => m.DisplayName), "id", "DisplayName", null);

            return View(model);
        }

        //Post: ProjectManager/AssignUsers/Developers
        [HttpPost]
        [Authorize(Roles = "Admin, Project Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUserProjects(ProjectUserViewModels model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string userId in model.SelectedUsers)
                    {
                        helper.AssignUserToProject(userId, model.ProjectId);
                    }
                }
                return RedirectToAction("Index", "ProjectManager");
            }
            return View(model);
        }


        // GET: ProjectManager/RemoveUsers/Developers
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult RemoveUserProjects(int id)
        {
            var project = db.Projects.Find(id);
            var model = new ProjectUserViewModels { ProjectId = id, ProjectName = project.Name };
            var userProjectList = helper.ListProjectDevelopers(id);

            model.Users = new MultiSelectList(userProjectList.OrderBy(m => m.DisplayName), "Id", "DisplayName", null);

            return View(model);
        }

        //Post: ProjectManager/RemoveUsers/Developers
        [HttpPost]
        [Authorize(Roles = "Admin, Project Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveUserProjects(ProjectUserViewModels model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string id in model.SelectedUsers)
                    {
                        helper.RemoveUserFromProject(id, model.ProjectId);
                    }
                }
                return RedirectToAction("Index", "ProjectManager");
            }
            return View(model);
        }

        //@@@@@@@@@@@@@@@@@@@@@@@ PROJECT MANAGERS @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

        // GET: ProjectManager/AssignPROJECTMANAGERS
     //   [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult AssignPmProjects(int id)
        {
            var project = db.Projects.Find(id);
            var model = new ProjectUserViewModels { ProjectId = id, ProjectName = project.Name };
            var userProjectList = helper.ListPManagerNotAssigned(id);

            model.Users = new SelectList(userProjectList.OrderBy(m => m.DisplayName), "Id", "DisplayName", null);

            return View(model);
        }

        //Post: ProjectManager/AssignPROJECTMANAGERS
        [HttpPost]
      //  [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult AssignPmProjects(ProjectUserViewModels model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string userId in model.SelectedUsers)
                    {
                        var pmId = helper.ListProjectManagers(model.ProjectId);
                        helper.RemovePmFromProject(userId, model.ProjectId);
                        helper.AssignUserToProject(model.SelectedUsers[0], model.ProjectId);
                    }
                }
                return RedirectToAction("Index", "ProjectManager");
            }
            return View(model);
        }


        // GET: ProjectManager/RemovePROJECTMANAGERS
        [Authorize(Roles = "Admin")]
        public ActionResult RemovePmProjects(int id)
        {
            var project = db.Projects.Find(id);
            var model = new ProjectUserViewModels { ProjectId = id, ProjectName = project.Name };
            var userProjectList = helper.ListProjectManagers(id);

            model.Users = new MultiSelectList(userProjectList.OrderBy(m => m.DisplayName), "Id", "DisplayName", null);

            return View(model);
        }

        //Post: ProjectManager/RemovePROJECTMANAGERS
        [HttpPost]
      //  [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult RemovePmProjects(ProjectUserViewModels model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string userId in model.SelectedUsers)
                    {
                        helper.RemovePmFromProject(userId, model.ProjectId);
                    }
                }
                return RedirectToAction("Index", "ProjectManager");
            }
            return View(model);
        }
      
    
    
    
    
    
    
    }
}