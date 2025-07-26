namespace Domain;

public class Route
{
    public int Id { get; private set; }
    public string Origin { get; private set; } = string.Empty;
    public string Destination { get; private set; } = string.Empty;
    public int Cost { get; private set; }

    public Route(string origin, string destination, int cost)
    {
        Origin = origin;
        Destination = destination;
        Cost = cost;
    }
}