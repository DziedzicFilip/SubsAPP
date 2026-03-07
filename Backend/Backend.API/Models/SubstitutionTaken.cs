using System;

namespace Backend.API.Models
{
    public class SubstitutionTaken
    {
        public int Id { get; set; }
        public int SubstitutionId { get; set; }
        public int TakenByUserId { get; set; }
        public DateTime TakenAt { get; set; }

        public Substitution Substitution { get; set; }
        public User TakenByUser { get; set; }
    }
}