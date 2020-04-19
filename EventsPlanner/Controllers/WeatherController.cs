using EventsPlanner.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventsPlanner.Controllers
{
    public class WeatherController : Controller
    {
        [HttpGet]
        public ActionResult GetThreeMonthTemperature(string city, int firstMonth, int year)
        {
            DateTime date = DateTime.Parse(string.Format("01.{0}.{1}", firstMonth.ToString("d2"), year)).AddMonths(1);

            Dictionary<string, DayTemperature> dict = new Dictionary<string, DayTemperature>();


            foreach (DayTemperature dt in DayTemperature.GetThreeMonthTemperature(city, date.Month, date.Year))
            {
                dict[dt.date.ToString("yyyy-MM-dd")] = dt;
            }
            string threeMonthWeather = JsonConvert.SerializeObject(dict);
            string threeMonthWeatherForCity = "{\"" + city + "\":" + threeMonthWeather + "}";
            return Content(threeMonthWeatherForCity);
        }
        public ActionResult GetDayTemperature(string city, int day, int month, int year)
        {

            string dayWeather = JsonConvert.SerializeObject(
                DayTemperature.GetDayTemperature(city, day, month, year)
                );
            return Content(dayWeather);
        }
    }
}