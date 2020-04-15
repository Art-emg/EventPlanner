using EventsPlanner.Models;
using System;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity.Owin;
using System.Net;

namespace EventsPlanner.Controllers
{
    public class HomeController : Controller
    {
        EventContext eventContext = new EventContext();
        UserEventContext userEventContext = new UserEventContext();
        ApplicationDbContext UsersContext = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }



    // PartialViews

        public ActionResult InfoEventForm(int id)
        {
            string currentUser = User.Identity.GetUserId();
            ViewBag.Event = eventContext.Events.Find(id);
            var buff = userEventContext.UserEvents.Where(ue => ue.EventId == id).Where( ue=> ue.CreatorId == currentUser).ToList();
            if (buff.Count != 0)
            {
                ViewBag.CurrentUser = true;
            }

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
        public ActionResult ListEventsForm(string date)
        {
            DateTime dateTime = DateTime.Parse(date);
            
            string currentUser = User.Identity.GetUserId();
            IEnumerable<Event> createdEvents = userEventContext.UserEvents
                            .Where(ue=>ue.UserId == currentUser && ue.CreatorId == currentUser)
                            .Select(ue=>ue.Event)
                            .Where(p => (p.StartDate <= dateTime && p.EndDate >= dateTime) );
            IEnumerable<Event> invitedEvents = userEventContext.UserEvents
                .Where(ue => ue.UserId == currentUser && ue.CreatorId != currentUser)
                .Select(ue => ue.Event)
                .Where(p => (p.StartDate <= dateTime && p.EndDate >= dateTime));



            ViewBag.Date = dateTime;
            ViewBag.CreatedEvents = createdEvents;
            ViewBag.InvitedEvents = invitedEvents;

            return PartialView();
        }

        public ActionResult AddEventForm()
        {
            string currentUser = User.Identity.GetUserId();
            ViewBag.UserNames = UsersContext.Users
                .Where(p => p.Id != currentUser).ToList().Select(p => p.UserName);

            return PartialView("EditEventForm");
        }

        [HttpGet]
        [ActionName("AddEventFormStartDate")]
        public ActionResult AddEventForm(string startDate)
        {
            string currentUser = User.Identity.GetUserId();
            ViewBag.UserNames = UsersContext.Users
                .Where(p => p.Id != currentUser).ToList().Select(p => p.UserName);
            ViewBag.EventStartDate = startDate;
            return PartialView("EditEventForm");
        }

        [HttpGet]
        [ActionName("AddEventFormStartEndDate")]
        public ActionResult AddEventForm(string startDate, string endDate)
        {
            string currentUser = User.Identity.GetUserId();
            ViewBag.UserNames = UsersContext.Users
                .Where(p=>p.Id!= currentUser).ToList().Select(p => p.UserName);
            ViewBag.EventStartDate = DateTime.Parse(startDate).ToString("d");
            ViewBag.EventEndDate = DateTime.Parse(endDate).ToString("d");
            return PartialView("EditEventForm");
        }


        [HttpGet]
        public ActionResult EditEventForm(int id)
        {
            Event ev = eventContext.Events.Find(id);
            ViewBag.IsEditForm = true;
            ViewBag.EventId = ev.EventId;
            ViewBag.EventName = ev.Name;
            ViewBag.EventStartDate = ev.StartDate.ToString("d");
            ViewBag.EventEndDate = ev.EndDate.ToString("d");
            ViewBag.EventDescr = ev.Description;

            return PartialView();
        }


    //Value results

        [HttpGet]
        public string GetCreatedEvents()
        {
            try
            {
                string currentUser = User.Identity.GetUserId();
                IEnumerable<Event> userEvents = userEventContext.UserEvents
                                                        .Where(us => us.UserId == currentUser 
                                                                && us.CreatorId == currentUser)
                                                        .Select(us => us.Event);
                foreach(Event ev in userEvents)
                {
                    if (ev.EndDate != ev.StartDate)
                        ev.EndDate = ev.EndDate.AddDays(1);
                }
                string json = JsonConvert.SerializeObject(userEvents);
                return json;
            }
            catch { return null; }
        }

        [HttpGet]
        public string GetInvitedEvents()
        {
            try
            {
                string currentUser = User.Identity.GetUserId();
                IEnumerable<Event> userEvents = userEventContext.UserEvents
                                                        .Where(us => us.UserId == currentUser 
                                                                && us.CreatorId != currentUser)
                                                        .Select(us => us.Event);
                foreach (Event ev in userEvents)
                {
                    if (ev.EndDate != ev.StartDate)
                        ev.EndDate = ev.EndDate.AddDays(1);
                }
                string json = JsonConvert.SerializeObject(userEvents);
                return json;
            }
            catch { return null; }
        }


        [HttpPost]
        public ActionResult AddEvent(Event ev, string InvitedUsers)
        {
            string currentUser = User.Identity.GetUserId();
            if (currentUser != null)
            {
                string[] invitedUsers = InvitedUsers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                

                eventContext.Events.Add(ev);
                eventContext.SaveChanges();

                //полуучает объект пользователя по id
                //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                
                userEventContext.UserEvents.Add(new UserEvent()
                {
                    EventId = ev.EventId,
                    UserId = currentUser,
                    CreatorId = currentUser
                });

                foreach (string user in invitedUsers)
                {
                    string invitedId = UsersContext.Users.Where(u => u.UserName.Equals(user)).Select(id => id.Id).FirstOrDefault();
                    if (invitedId.Equals(currentUser))
                        continue;

                    userEventContext.UserEvents.Add(new UserEvent()
                    {
                        EventId = ev.EventId,
                        UserId = invitedId,
                        CreatorId = currentUser
                    });
                }

                userEventContext.SaveChanges();
                return Content(ev.EventId.ToString());
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public ActionResult EditEvent(Event ev)
        {
            Event editEvent = eventContext.Events.Find(ev.EventId);
            editEvent.Name = ev.Name;
            editEvent.StartDate = ev.StartDate;
            editEvent.EndDate = ev.EndDate;
            editEvent.Description = ev.Description;

            eventContext.SaveChanges();

            return Content(ev.EventId.ToString());
        }
        public ActionResult DelEvent(int id)
        {
            string currentUser = User.Identity.GetUserId();

            var createdEvents = userEventContext.UserEvents.Where(ue => ue.EventId == id).Where(ue => ue.CreatorId == currentUser).ToList();
            if (createdEvents.Count == 0)
            {
                userEventContext.UserEvents.Remove(
                    userEventContext.UserEvents.Where(ue => ue.EventId==id 
                                                         && ue.UserId == currentUser).First()
                );
                userEventContext.SaveChanges();
            }
            else {
                eventContext.Events.Remove(eventContext.Events.Find(id));
                eventContext.SaveChanges();
            }
           
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        //WEATHER

        [HttpGet]
        public ActionResult GetThreeMonthTemperature(int firstMonth, int year)
        {
            DateTime date = DateTime.Parse(string.Format("01.{0}.{1}", firstMonth.ToString("d2"), year)).AddMonths(1);

            Dictionary<string, DayTemperature> dict = new Dictionary<string, DayTemperature>();
            

            foreach (DayTemperature dt in DayTemperature.GetThreeMonthTemperature("minsk", date.Month, date.Year))
            {
                dict[dt.date.ToString("yyyy-MM-dd")] = dt;
            }
            string threeMonthWeather = JsonConvert.SerializeObject(dict);

            return Content(threeMonthWeather);
        }


    }
}