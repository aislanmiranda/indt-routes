namespace Application.Responses;

public class RouteResponse
{
    public int Id { get; set; }
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public int Cost { get; set; }
}