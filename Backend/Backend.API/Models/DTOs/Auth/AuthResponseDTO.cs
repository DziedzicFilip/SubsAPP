namespace Backend.API.Models.DTOs
{
    public class AuthResponseDTO
    {


        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UserResponseDTO User { get; set; }
       
    }
}