using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Newtonsoft.Json;

namespace EventsPlanner.Models
{

    public class Event
    {

        [JsonProperty(PropertyName = "id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "title")]
        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "start")]
        [DataType("DateTime")]
        [Display(Name = "Начало")]
        public DateTime StartDate { get; set; }

        [JsonProperty(PropertyName = "end")]
        [Display(Name = "Конец")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [JsonIgnore]
        public string Latitude { get; set; }
        [JsonIgnore]
        public string Longitude { get; set; }

        [JsonIgnore]
        public int? WeatherDay { get; set; }

        [JsonIgnore]
        public int? WeatherNight { get; set; }

        [JsonIgnore]
        public int? WeatherTypeId { get; set; }

        [JsonIgnore]
        public List<UserEvent> UserEvents { get; set; }
        [JsonIgnore]
        public int WeatherType { get; set; }



    }

    public class EventContext : DbContext
    {
        public EventContext() : base("DefaultConnection")
        { }
        public DbSet<Event> Events { get; set; }
        public DbSet<Event> WeatherTypes { get; set; }
    }
}