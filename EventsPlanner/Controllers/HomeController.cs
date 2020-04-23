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
using System.Data.Entity;

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

        public ActionResult Events()
        {

            string currentUser = User.Identity.GetUserId();
            IEnumerable<Event> userEvents = userEventContext.UserEvents
                                                    .Where(us => us.UserId == currentUser)                        
                                                    .Select(us => us.Event);

            return View(userEvents);
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
            DateTime dateTime = DateTime.Parse(date).Date;
            
            string currentUser = User.Identity.GetUserId();
            IEnumerable<Event> createdEvents = userEventContext.UserEvents
                            .Where(ue=>ue.UserId == currentUser && ue.CreatorId == currentUser)
                            .Select(ue=>ue.Event)
                            .Where(p => DbFunctions.TruncateTime(p.StartDate) <= dateTime && DbFunctions.TruncateTime(p.EndDate)>= dateTime ).ToList();
            IEnumerable<Event> invitedEvents = userEventContext.UserEvents
                .Where(ue => ue.UserId == currentUser && ue.CreatorId != currentUser)
                .Select(ue => ue.Event)
                .Where(p => DbFunctions.TruncateTime(p.StartDate) <= dateTime && DbFunctions.TruncateTime(p.EndDate) >= dateTime).ToList();



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
            ViewBag.EventStartDate = DateTime.Parse(startDate).ToString("dd.MM.yyyy H:mm");
            ViewBag.EventEndDate = DateTime.Parse(startDate).ToString("dd.MM.yyyy H:mm");
            ///
            return PartialView("EditEventForm");
        }

        [HttpGet]
        [ActionName("AddEventFormStartEndDate")]
        public ActionResult AddEventForm(string startDate, string endDate)
        {
            string currentUser = User.Identity.GetUserId();
            ViewBag.UserNames = UsersContext.Users
                .Where(p=>p.Id!= currentUser).ToList().Select(p => p.UserName);
            ViewBag.EventStartDate = DateTime.Parse(startDate).ToString("dd.MM.yyyy H:mm");
            ViewBag.EventEndDate = DateTime.Parse(endDate).ToString("dd.MM.yyyy H:mm");
            return PartialView("EditEventForm");
        }


        [HttpGet]
        public ActionResult EditEventForm(int id)
        {
            Event ev = eventContext.Events.Find(id);
            ViewBag.IsEditForm = true;
            ViewBag.EventId = ev.EventId;
            ViewBag.EventName = ev.Name;
            ViewBag.EventStartDate = ev.StartDate.ToString("dd.MM.yyyy H:mm");
            ViewBag.EventEndDate = ev.EndDate.ToString("dd.MM.yyyy H:mm");
            ViewBag.EventDescr = ev.Description;
            ViewBag.EventLatitude = ev.Latitude;
            ViewBag.EventLongitude = ev.Longitude;
            ViewBag.Event = ev;

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
                //foreach(Event ev in userEvents)
                //{
                //    if (ev.EndDate != ev.StartDate)
                //        ev.EndDate = ev.EndDate.AddDays(1);
                //}
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
                //foreach (Event ev in userEvents)
                //{
                //    if (ev.EndDate != ev.StartDate)
                //        ev.EndDate = ev.EndDate.AddDays(1);
                //}
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
                if (ev.WeatherRegion != null)
                {
                    DayTemperature dayTemperature = DayTemperature.GetDayTemperature
                           (ev.WeatherRegion, ev.StartDate.Day, ev.StartDate.Month, ev.StartDate.Year);
                    ev.WeatherDay = dayTemperature.dayTemp;
                    ev.WeatherNight = dayTemperature.nightTemp;
                    ev.WeatherType = eventContext.WeatherTypes.Where(i => i.Description == dayTemperature.description).First();
                }

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

                string[] invitedUsers = InvitedUsers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

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
        public ActionResult EditEvent(Event ev, bool ResizeOrDrug = false)
        {


            Event editEvent = eventContext.Events.Find(ev.EventId);
            editEvent.Name = ev.Name;
            editEvent.StartDate = ev.StartDate;
            editEvent.EndDate = ev.EndDate;
            editEvent.Description = ev.Description;
            editEvent.Latitude = ev.Latitude ?? editEvent.Latitude;
            editEvent.Longitude = ev.Longitude ?? editEvent.Longitude;
            editEvent.WeatherRegion = ev.WeatherRegion ?? editEvent.WeatherRegion;

            if (editEvent.WeatherRegion != null)
            {
                DayTemperature dayTemperature = DayTemperature.GetDayTemperature
                       (editEvent.WeatherRegion, ev.StartDate.Day, ev.StartDate.Month, ev.StartDate.Year);
                editEvent.WeatherDay = dayTemperature.dayTemp;
                editEvent.WeatherNight = dayTemperature.nightTemp;
                editEvent.WeatherType = eventContext.WeatherTypes.Where(i => i.Description == dayTemperature.description).First();
            }

            
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



    }
}