namespace NetSimpleAuth.Backend.Domain.Response;

public class AuthUserResponse
{
    public string Username { get; set; }
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
}