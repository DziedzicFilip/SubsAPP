using System;
using System.Collections.Generic;

namespace Backend.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<SubstitutionNotification> SubstitutionNotifications { get; set; }
        public ICollection<SubstitutionTaken> SubstitutionTakens { get; set; }
        public ICollection<Overtime> Overtimes { get; set; }
    }
}