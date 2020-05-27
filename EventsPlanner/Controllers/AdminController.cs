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
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        EventContext eventContext = new EventContext();
        ApplicationDbContext UsersContext = new ApplicationDbContext();
        UserEventContext userEventContext = new UserEventContext();

        #region Events

        public ActionResult Events()
        {
            return View(eventContext.Events.ToList());
        }

        public ActionResult InfoEventForm(int id)
        {
            string currentUser = User.Identity.GetUserId();
            ViewBag.Event = eventContext.Events.Find(id);

            ViewBag.CurrentUser = true;


            string creatorID = userEventContext.UserEvents
                .Where(ue => ue.EventId == id && ue.UserId == ue.CreatorId)
                .Select(ue => ue.UserId).FirstOrDefault();
            string creatorUsername = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(creatorID).UserName;


            IEnumerable<string> invitedID = userEventContext.UserEvents
               .Where(ue => ue.EventId == id && ue.UserId != ue.CreatorId)
               .Select(ue => ue.UserId).ToList();

            List<string> invitdUsername = new List<string>();

            foreach (string invitedId in invitedID)
                invitdUsername.Add(System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(invitedId).UserName);

            ViewBag.InvitedUserNames = invitdUsername;
            ViewBag.CreatorUserName = creatorUsername;

            return PartialView();
        }

        #endregion


        #region Users
        public ActionResult Users()
        {
            return View(UsersContext.Users.ToList());
           
        }
        public ActionResult UserEdit(string id)
        {
            return View(UsersContext.Users.Where(p => p.Id == id).Select(us => us).FirstOrDefault());
        }
        public ActionResult UserDetails(string id)
        {
            return View(UsersContext.Users.Where(p => p.Id == id).Select(us => us).FirstOrDefault());
        }

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