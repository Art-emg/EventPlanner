using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventsPlanner.Models
{
    public class UserEvent
    {
        public ApplicationUser User { get; set; }
        [ForeignKey("Id")]
        [Required]
        public string ApplicationUserId { get; set; }


    }

    public class UserEventContext : DbContext
    {
        public UserEventContext() : base("DefaultConnection")
        {}
        public DbSet<UserEvent> UserEvents { get; set; }
    }
}