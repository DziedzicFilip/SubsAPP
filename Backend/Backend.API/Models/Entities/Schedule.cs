using System;

namespace Backend.API.Models.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int GroupId { get; set; }
        public int DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public User User { get; set; }
        public Group Group { get; set; }
    }
}