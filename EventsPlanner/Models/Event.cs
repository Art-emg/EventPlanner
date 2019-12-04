using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Newtonsoft.Json;

namespace EventsPlanner.Models
{
    [Table("Events")]
    public class Event
    {
        [JsonProperty(PropertyName = "id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "title")]
        [Required]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "start")]
        [DataType("DateTime")]
        public DateTime StartDate { get; set; }

        [JsonProperty(PropertyName = "end")]
        public DateTime EndDate { get; set; }

        public string Description { get; set; }


    }

    public class EventContext : DbContext
    {
        public EventContext() : base("DefaultConnection")
        { }
        public DbSet<Event> Events { get; set; }
    }
}