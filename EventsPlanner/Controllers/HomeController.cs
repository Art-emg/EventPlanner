using EventsPlanner.Models;
using System;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventsPlanner.Controllers
{
    public class HomeController : Controller
    {
        EventContext eventContext = new EventContext();
        UserEventContext UserEventContext = new UserEventContext();
        

        public ActionResult Index()
        {


             
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}