namespace NetSimpleAuth.Backend.Domain.Dto;

public class LogFilterDto
{
    public string Ip { get; set; }
    public int? Hour { get; set; }
    public string UserAgent { get; set; }
    public string Order { get; set; }
    public string Direction { get; set; }
}