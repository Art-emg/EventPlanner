using EventsPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace EventsPlanner.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        EventContext eventContext = new EventContext();
        ApplicationDbContext UsersContext = new ApplicationDbContext();


        #region Events

        public ActionResult Events()
        {
            return View(eventContext.Events.ToList());
        }

        #endregion


        #region Users
        public ActionResult Users()
        {
            return View(UsersContext.Users.ToList());
            
        }

        [HttpGet]
        public ActionResult UserBlock (Guid userId, bool block)
        {
            ApplicationUser currentUser = UsersContext.Users.Find(userId.ToString());
            currentUser.LockoutEnabled = block;
            UsersContext.SaveChanges();
            return RedirectToAction("Users");
            //return new HttpStatusCodeResult(UsersContext.Users.Find(userId).LockoutEnabled !=block?HttpStatusCode.OK: HttpStatusCode.BadRequest);
        }
        public ActionResult UserDelete(Guid userId)
        {
            ApplicationUser currentUser = UsersContext.Users.Find(userId.ToString());
            UsersContext.Users.Remove(currentUser);
            UsersContext.SaveChanges();
            return RedirectToAction("Users");
            //return new HttpStatusCodeResult(UsersContext.Users.Find(userId).LockoutEnabled !=block?HttpStatusCode.OK: HttpStatusCode.BadRequest);
        }
        #endregion

    }
}