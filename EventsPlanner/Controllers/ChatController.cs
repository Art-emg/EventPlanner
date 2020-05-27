using EventsPlanner.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventsPlanner.Controllers
{
    public class ChatController : Controller
    {
        MessageContext messageContext = new MessageContext();
        ApplicationDbContext UsersContext = new ApplicationDbContext();
        EventContext eventContext = new EventContext();
        UserEventContext userEventContext = new UserEventContext();

        public ActionResult Index()
        {
            string currentUser = User.Identity.GetUserId();
            IEnumerable<Event> userEvents = userEventContext.UserEvents
                                             .Where(us => us.UserId == currentUser)
                                             .Select(us => us.Event).Distinct();

            return View(userEvents);
        }

        public ActionResult Event(int Id)
        {
           
            Dictionary<string, string> users = new Dictionary<string, string>();

            var usersId = userEventContext.UserEvents
                                             .Where(us => us.EventId == Id)
                                             .Select(us => us.UserId);

            foreach(string id in usersId)
            {
                users[id] = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(id).UserName;
            }
            IEnumerable<Message> eventMessages = messageContext.Messages.Where(m => m.EventId == Id).Select(m => m);

            ViewBag.Users = users;
            ViewBag.EventId = Id;

            ViewBag.Title = eventContext.Events.Where(ev=> ev.EventId == Id).FirstOrDefault().Name;

            return View(eventMessages);
        }

        [HttpPost]
        public ActionResult SendMessage(int EventId, string TextMessage)
        {
            Message message = new Message() { MessageDateTime = DateTime.Now, MessageText = TextMessage, EventId = EventId, UserId = User.Identity.GetUserId() };
            messageContext.Messages.Add(message);
            messageContext.SaveChanges();
            return RedirectToAction("Event/" + EventId);
        }
    }
}