namespace Domain.Interfaces;

public interface IRouteRepository
{
    Task<Route> AddAsync(Route route, CancellationToken cancellationToken);
    Task<Route> UpdateAsync(Route route, CancellationToken cancellationToken);
    Task<Route> DeleteByIdAsync(int id, CancellationToken cancellationToken);
    Task<List<Route>?> GetAllRoutesAsync(CancellationToken cancellationToken);
}