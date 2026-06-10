using System;

namespace Backend.API.Models.Entities
{
    public class SubstitutionTaken
    {
        public int Id { get; set; }
        public int SubstitutionId { get; set; }
        public string TakenByUserId { get; set; }
        public DateTime TakenAt { get; set; }

        public Substitution Substitution { get; set; }
        public User TakenByUser { get; set; }
    }
}