namespace NetPOC.Backend.Domain.Dto
{
    public class AuthUserResponse
    {
        public string Username { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}