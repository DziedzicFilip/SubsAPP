using System;

namespace Backend.API.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public int DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public User User { get; set; }
        public Group Group { get; set; }
    }
}