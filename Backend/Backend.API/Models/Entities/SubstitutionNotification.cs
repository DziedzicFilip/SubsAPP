using System;

namespace Backend.API.Models.Entities
{
    public class SubstitutionNotification
    {
        public int Id { get; set; }
        public int SubstitutionId { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public DateTime SentAt { get; set; }

        public Substitution Substitution { get; set; }
        public User User { get; set; }
    }
}