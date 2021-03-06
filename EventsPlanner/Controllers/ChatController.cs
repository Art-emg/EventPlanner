﻿using EventsPlanner.Models;
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
            List<Event> userEvents = userEventContext.UserEvents
                                             .Where(us => us.UserId == currentUser)
                                             .Select(us => us.Event).Distinct().ToList();

            foreach(Event userEvent in userEvents.ToArray())
            {
                if (userEventContext.UserEvents.Where(ue => ue.EventId == userEvent.EventId).Select(ue => ue).Count() < 2)
                    userEvents.Remove(userEvent);
            }

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
        public string SendMessage(int EventId, string TextMessage)
        {
            if (string.IsNullOrWhiteSpace(TextMessage))
                return "";

            Message message = new Message() { MessageDateTime = DateTime.Now, MessageText = TextMessage, EventId = EventId, UserId = User.Identity.GetUserId() };
            messageContext.Messages.Add(message);
            messageContext.SaveChanges();
            return $"<tr> " +
                        $"<td style='display: none'>{message.MessageId}</td>" +
                        $"<td style='width:100px'>{UsersContext.Users.Find(message.UserId).UserName}</td>" +
                        $"<td style='width:170px'>{message.MessageDateTime}</td>" +
                        $"<td>{message.MessageText}</td>" +
                    $"<tr>";
        }

        [HttpGet]
        public string GetLastMessages(int EventId, int lastMessageId)
        {
            List<Message> eventMessages = messageContext.Messages.Where(m => m.EventId == EventId && m.MessageId> lastMessageId).Select(m => m).ToList();

            string returnString = "";
            
            foreach(Message message in eventMessages)
            {
                returnString += $"<tr> " +
                        $"<td style='display: none'>{message.MessageId}</td>" +
                        $"<td style='width:100px'>{UsersContext.Users.Find(message.UserId).UserName}</td>" +
                        $"<td style='width:170px'>{message.MessageDateTime}</td>" +
                        $"<td>{message.MessageText}</td>" +
                    $"<tr>";
            }


            return returnString;
        }
    }
}