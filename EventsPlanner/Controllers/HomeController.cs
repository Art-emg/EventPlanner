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


        public ActionResult Index()
        {
            return View();
        }




        public ActionResult InfoEventForm(int id)
        {
            ViewBag.Event = eventContext.Events.Find(id);
            return PartialView();
        }
        public ActionResult ListEventsForm(string date)
        {
            DateTime dateTime = DateTime.Parse(date);
            ViewBag.Date = dateTime;

            ViewBag.Events = eventContext.Events.Where(
                p => (p.StartDate <= dateTime && p.EndDate > dateTime) || 
                (p.StartDate== p.EndDate && p.StartDate == dateTime)
                );

            return PartialView();
        }

        public ActionResult AddEventForm()
        {

            return PartialView("EditEventForm");
        }

        [HttpGet]
        [ActionName("AddEventFormStartDate")]
        public ActionResult AddEventForm(string startDate)
        {
            ViewBag.EventStartDate = startDate;
            return PartialView("EditEventForm");
        }

        [HttpGet]
        [ActionName("AddEventFormStartEndDate")]
        public ActionResult AddEventForm(string startDate, string endDate)
        {
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


        [HttpGet]
        public string GetEvents()
        {
            string json = JsonConvert.SerializeObject(eventContext.Events);
            return json;
        }

        [HttpPost]
        public ActionResult AddEvent(Event ev)
        {
            eventContext.Events.Add(ev);
            eventContext.SaveChanges();
            ApplicationDbContext applicationDb = new ApplicationDbContext();

            // ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            return Content(ev.EventId.ToString());
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
            eventContext.Events.Remove(eventContext.Events.Find(id));
            eventContext.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}