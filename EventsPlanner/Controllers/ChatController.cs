using EventsPlanner.Models;
using Microsoft.AspNet.Identity;
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
            Message message = new Message() { MessageDateTime = DateTime.Now, MessageText = "dfsdf", EventId = 1 , UserId="dfdf"};
            messageContext.Messages.Add(message);
            messageContext.SaveChanges();

            return View();
        }

        [HttpPost]
        public ActionResult SendMessage(int EventId)
        {

            return View(eventContext.Events);
        }
    }
}