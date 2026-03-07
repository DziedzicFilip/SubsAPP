namespace Backend.API.Models
{
    public class Overtime
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SubstitutionId { get; set; }
        public float Hours { get; set; }
        public string Status { get; set; }

        public User User { get; set; }
        public Substitution Substitution { get; set; }
    }
}