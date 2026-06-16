namespace Backend.API.Models.DTOs
{
    public class CreateUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public string  Password { get; set; }
        public List<string> Groups { get; set; }
        public List<string> Schedules { get; set; }
        
    }
}