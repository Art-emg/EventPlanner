using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventsPlanner.Models
{
    public class WeatherType
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }
        public List<WeatherType> WeatherTypes { get; set; }
    }
}