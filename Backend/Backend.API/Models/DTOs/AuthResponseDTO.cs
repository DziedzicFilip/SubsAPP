namespace Backend.API.Models.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UserResponseDTO User { get; set; }
       
    }
}