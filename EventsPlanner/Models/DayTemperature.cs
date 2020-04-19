using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;


namespace EventsPlanner.Models
{
    class DayTemperature
    {
        public DateTime date { get; set; }
        public int dayTemp { get; set; }
        public int nightTemp { get; set; }
        public string description { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}/{2} ({3})", date, dayTemp, nightTemp, description);
        }

        public static List<DayTemperature> GetMonthTemperature(string city, int month, int year)
        {
            try
            {
                List<DayTemperature> monthTemperature = new List<DayTemperature>();
                string htmlString;

                CultureInfo ci = new CultureInfo("en-US");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                    string.Format("https://world-weather.ru/pogoda/belarus/{0}/{1}-{2}/",
                        city,
                        ci.DateTimeFormat.GetMonthName(month).ToLower(),
                        year));

                WebResponse response = request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        htmlString = reader.ReadToEnd();
                    }
                }
                response.Close();


                HtmlDocument html = new HtmlDocument();
                html.LoadHtml(htmlString);
                HtmlNode document = html.DocumentNode;

                IEnumerable<HtmlNode> htmlNodes = document.QuerySelectorAll("ul.ww-month > li");


                foreach (HtmlNode htmlNode in htmlNodes)
                {
                    try
                    {
                        if (htmlNode.InnerText.Equals(string.Empty))
                            continue;

                        DayTemperature dayTemperature = new DayTemperature();
                        dayTemperature.date = DateTime.Parse(
                            string.Format("{0}.{1}.{2}",
                                Convert.ToInt32(htmlNode.QuerySelector("div").InnerText).ToString("d2"),
                                month.ToString("d2"),
                                year.ToString("d4")
                            ));
                        dayTemperature.dayTemp = Convert.ToInt32(htmlNode.QuerySelector("span").InnerText.TrimEnd(new char[] { '°' }));
                        dayTemperature.nightTemp = Convert.ToInt32(htmlNode.QuerySelector("p").InnerText.TrimEnd(new char[] { '°' }));
                        dayTemperature.description = htmlNode.QuerySelector("i").Attributes["title"].Value;
                        monthTemperature.Add(dayTemperature);
                    }
                    catch
                    {

                    }
                }
                return monthTemperature;
            }
            catch
            {
                return null;
            }

        }


        public static List<DayTemperature> GetThreeMonthTemperature(string city, int middleMonth, int year)
        {
            DateTime date = DateTime.Parse(string.Format("01.{0}.{1}", middleMonth, year));
            date = date.AddMonths(-1);
            List<DayTemperature> threeMonthTemerature = new List<DayTemperature>();

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    threeMonthTemerature.AddRange(GetMonthTemperature(city, date.Month, date.Year));
                }
                catch (ArgumentNullException) { }

                date = date.AddMonths(1);
            }

            return threeMonthTemerature;
        }
        public static DayTemperature GetDayTemperature(string city, int day, int month, int year)
        {
            DateTime date = DateTime.Parse(string.Format("01.{0}.{1}", month, year));
            List<DayTemperature> monthTemerature = new List<DayTemperature>();


            try
            {
                foreach(DayTemperature dayTemp in GetMonthTemperature(city, date.Month, date.Year))
                {
                    if (dayTemp.date.Day == day)
                        return dayTemp;
                }
            }
            catch (ArgumentNullException) { }


            return null;
        }

    }
}
