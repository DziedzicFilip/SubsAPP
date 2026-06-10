using System.Collections.Generic;

namespace Backend.API.Models.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<SubstitutionGroup> SubstitutionGroups { get; set; }
    }
}