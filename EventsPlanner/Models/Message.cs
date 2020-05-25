using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EventsPlanner.Models
{
    public class Message
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }

        public int EventId { get; set; }

        public string UserId { get; set; }

        public DateTime MessageDateTime { get; set; }

        public string MessageText { get; set; }
    }


    public class MessageContext : DbContext
    {
        public MessageContext() : base("DefaultConnection")
        { }
        public DbSet<Message> Messages { get; set; }

    }
}