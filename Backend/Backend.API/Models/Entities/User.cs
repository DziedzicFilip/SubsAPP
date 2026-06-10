using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
namespace Backend.API.Models.Entities
{
    public class User : IdentityUser
    {
       // public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Email { get; set; }
        //public string PasswordHash { get; set; }
       // public string Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<SubstitutionNotification> SubstitutionNotifications { get; set; }
        public ICollection<SubstitutionTaken> SubstitutionTakens { get; set; }
        public ICollection<Overtime> Overtimes { get; set; }
    }
}