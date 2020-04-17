using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EventsPlanner.Models
{
    public class WeatherType
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }
        public List<Event> Events { get; set; }
    }
    public class WeatherTypeContext : DbContext
    {
        public WeatherTypeContext() : base("DefaultConnection")
        { }
        public DbSet<WeatherType> WeatherTypes { get; set; }
    }
}