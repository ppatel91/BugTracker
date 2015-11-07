using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BugTracker.Helper
{
    public class UserProjectsHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public ICollection<Project> ListProject()
        {
            return db.Projects.ToList();
        }
        // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@   USERS/DEVELOPERS @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //checks to see if User is assigned to a project
        public bool IsUserOnProject (string userId, int projectId)
        {
            var proj = db.Projects.Find(projectId);
            return proj.Users.Any(u => u.Id.Equals(userId));
        }
        //assigns user to project
        public void AssignUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId,projectId))
            {
                var proj = db.Projects.Find(projectId);
                var user = db.Users.Find(userId);
                proj.Users.Add(user);
                db.Entry(proj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        //removes user from project
        public bool RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                var proj = db.Projects.Find(projectId);
                var user = db.Users.Find(userId);
                if (proj.Users.Remove(user))
                { 
                db.Entry(proj).State = EntityState.Modified;
                db.SaveChanges();
                return true;
                }
            }
            return false;
        }

        //return list of projects assigned to a user
        public ICollection<Project> ListUserProjects(string userId)
        {
           
            return db.Users.Find(userId).Projects.ToList();
        }

        //return list of users assigned to a single project
        public ICollection<ApplicationUser> ListProjectUsers(string projectId)
        {
            
            return db.Projects.Find(projectId).Users.ToList();
        }

        //return list of users assigned to a single project
        public ICollection<ApplicationUser> ListProjectDevelopers(int projectId)
        {

            //get user role id
            var roleId = db.Roles.SingleOrDefault(r => r.Name == "Developer").Id;
            //get all users assigned to project
            var users = db.Projects.Find(projectId).Users;
            //restrict list to developers only
            var devs = users.Where(user => user.Roles.Any(role => role.RoleId == roleId));
            //return the list of devs
            return devs.ToList();
        }

        //return list of users/developers not assigned to project
        public ICollection<ApplicationUser> ListDevelopersNotAssigned(int projectId)
        {
            //get developer role id
            var roleId = db.Roles.SingleOrDefault(r => r.Name == "Developer").Id;
            //get all users not on the project
            var users = db.Users.Where(u => !u.Projects.Any(p => p.Id == projectId));
            //restrict user list to developers only
            var devs = users.Where(user => user.Roles.Any(role => role.RoleId == roleId));
            //return list of developers
            return devs.ToList();
            
            //return db.Project.Where(p => !p.User.Any(u => u.Id == projectId)).ToList();
        }

        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@     PROJECT MANAGERS      ##########################@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

        //assigns PM to project
        public void AssignPmToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                var proj = db.Projects.Find(projectId);
                var user = db.Users.Find(userId);
                proj.Users.Add(user);
                db.Entry(proj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        //removes user from project
        public bool RemovePmFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                var proj = db.Projects.Find(projectId);
                var user = db.Users.Find(userId);
                if (proj.Users.Remove(user))
                {
                    db.Entry(proj).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }


        //return list of ProjectManagers assigned to a single project
        public ApplicationUser ListProjectManagers(int projectId)
        {

            //get user role id
            var roleId = db.Roles.SingleOrDefault(r => r.Name == "Project Manager").Id;
            //get all users assigned to project
            var users = db.Projects.Find(projectId).Users;
            //restrict list to developers only
            var pm = users.FirstOrDefault(user => user.Roles.Any(role => role.RoleId == roleId));
            //var pM = users.Where(user => user.Roles.Any(role => role.RoleId == roleId));
            //return the list of devs
            return pm;
        }

        //return list of ProjectManagers not assigned to project
        public ICollection<ApplicationUser> ListPManagerNotAssigned(int projectId)
        {
            //get PM role id
            var roleId = db.Roles.SingleOrDefault(r => r.Name == "Project Manager").Id;
            //get all users not on the project
            var users = db.Users.Where(u => !u.Projects.Any(p => p.Id == projectId));
            //restrict user list to PM only
            var pM = users.Where(user => user.Roles.Any(role => role.RoleId == roleId));
            //return list of developers
            return pM.ToList();

            //return db.Project.Where(p => !p.User.Any(u => u.Id == projectId)).ToList();
        }

    }

    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ END PROJECT MANAGER   @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


}