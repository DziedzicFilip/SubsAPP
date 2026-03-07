namespace Backend.API.Models
{
    public class SubstitutionGroup
    {
        public int Id { get; set; }
        public int SubstitutionId { get; set; }
        public int GroupId { get; set; }

        public Substitution Substitution { get; set; }
        public Group Group { get; set; }
    }
}