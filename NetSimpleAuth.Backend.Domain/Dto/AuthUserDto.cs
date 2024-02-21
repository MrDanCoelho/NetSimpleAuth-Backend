namespace NetSimpleAuth.Backend.Domain.Dto;

public class AuthUserDto
{
    public string Identity { get; set; }
    public string Password { get; set; }
    public string IpAddress { get; set; }
}