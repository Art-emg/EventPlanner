using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventsPlanner.Models
{
    public class UserEvent
    {
        [ForeignKey("AspNetUsers"), Required]
        public string  UserId { get; set; }

        [ForeignKey("AspNetUsers"), Required]
        public string CreatorId { get; set; }

        [ForeignKey("Events"), Required]
        public int EventId { get; set; }

    }

    public class UserEventContext : DbContext
    {
        public DbSet<UserEvent> UserEvents { get; set; }
    }
}