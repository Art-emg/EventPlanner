using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace EventsPlanner.Models
{
    [Table("Events")]
    public class Event
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType("DateTime")]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; }


    }

    public class EventContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
    }
}