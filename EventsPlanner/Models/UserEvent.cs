using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace EventsPlanner.Models
{
    public class UserEvent
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EventId { get; set; }
   
        public string UserId { get; set; }
        public string CreatorId { get; set; }



        public Event Event { get; set; }

    }

    public class UserEventContext : DbContext
    {
        public UserEventContext() : base("DefaultConnection")
        {}
        public DbSet<UserEvent> UserEvents { get; set; }
    }
}