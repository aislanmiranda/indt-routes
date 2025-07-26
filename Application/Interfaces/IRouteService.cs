using Application.Requests;
using Application.Responses;

namespace Application.Interfaces;

public interface IRouteService
{
    Task<Result<RouteResponse>> AddRouteAsync(CreateRouteRequest request, CancellationToken cancellationToken);
    Task<Result<RouteResponse>> UpdateRouteAsync(UpdateRouteRequest request, CancellationToken cancellationToken);
    Task<Result<RouteResponse>> DeleteRouteAsync(int id, CancellationToken cancellationToken);
    Task<Result<List<RouteResponse>>> GetAllRoutesAsync(CancellationToken cancellationToken);

    Task<Result<SearchFlightsResponse>> GetSearchBestRoute(SearchFlightsRequest request, CancellationToken cancellationToken);
}
