using System;
using System.Collections.Generic;

namespace Backend.API.Models.Entities
{
    public class Substitution
    {
        public int Id { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; }

        public User CreatedByUser { get; set; }
        public ICollection<SubstitutionGroup> SubstitutionGroups { get; set; }
        public ICollection<SubstitutionNotification> SubstitutionNotifications { get; set; }
        public ICollection<SubstitutionTaken> SubstitutionTakens { get; set; }
        public ICollection<Overtime> Overtimes { get; set; }
    }
}