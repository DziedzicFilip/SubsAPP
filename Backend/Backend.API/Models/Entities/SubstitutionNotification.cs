using System;

namespace Backend.API.Models
{
    public class SubstitutionNotification
    {
        public int Id { get; set; }
        public int SubstitutionId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public DateTime SentAt { get; set; }

        public Substitution Substitution { get; set; }
        public User User { get; set; }
    }
}