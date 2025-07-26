using Application.Interfaces;
using Application.Requests;
using Application.Responses;
using AutoMapper;
using Domain;
using Domain.Interfaces;

namespace Application.Services;

public class RouteService : IRouteService
{
    private readonly IRouteRepository _routeRepository;
    private readonly IMapper _mapper;

    public RouteService(IRouteRepository routeRepository, IMapper mapper)
    {
        _routeRepository = routeRepository;
        _mapper = mapper;
    }

    public async Task<Result<RouteResponse>> AddRouteAsync(CreateRouteRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<Route>(request);
            var domain = await _routeRepository.AddAsync(entity, cancellationToken);
            var response = _mapper.Map<RouteResponse>(domain);
            return Result<RouteResponse>.Create(response);
        }
        catch (Exception)
        {
            return Result<RouteResponse>.Fail("Erro ao cadastrar rota");
        }
    }

    public async Task<Result<RouteResponse>> UpdateRouteAsync(UpdateRouteRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var createMapped = _mapper.Map<Route>(request);
            var domain = await _routeRepository.UpdateAsync(createMapped, cancellationToken);
            var Createresponse = _mapper.Map<RouteResponse>(domain);
            return Result<RouteResponse>.Ok(Createresponse);
        }
        catch (Exception)
        {
            return Result<RouteResponse>.Fail("Erro ao atualizar rota");
        }
    }

    public async Task<Result<RouteResponse>> DeleteRouteAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var domainResponse = await _routeRepository
                    .DeleteByIdAsync(id,cancellationToken);

            if(domainResponse is null)
                return Result<RouteResponse>.Fail("Registro não existe para deletar a rota");

            var deleteMapped = _mapper.Map<RouteResponse>(domainResponse);

            return Result<RouteResponse>.Ok(deleteMapped);
        }
        catch (Exception)
        {
            return Result<RouteResponse>.Fail("Erro ao deletar rota");
        }
    }

    public async Task<Result<List<RouteResponse>>> GetAllRoutesAsync(CancellationToken cancellationToken)
    {
        try
        {
            var domain = await _routeRepository.GetAllRoutesAsync(cancellationToken);
            var response =  _mapper.Map<List<RouteResponse>>(domain);
            return Result<List<RouteResponse>>.Ok(response);
        }
        catch (Exception)
        {
            return Result<List<RouteResponse>>.Fail("Erro ao carregar rotas");
        }
    }

    public async Task<Result<SearchFlightsResponse>> GetSearchBestRoute(SearchFlightsRequest request, CancellationToken cancellationToken)
    {
        var routes = await _routeRepository.GetAllRoutesAsync(cancellationToken);
        var routesMapped = _mapper.Map<List<RouteResponse>>(routes);

        var graph = BuildGraph(routesMapped);

        var costs = new Dictionary<string, int>();
        var previous = new Dictionary<string, string>();
        var visited = new HashSet<string>();
        var queue = new PriorityQueue<string, int>();

        foreach (var node in graph.Keys)
            costs[node] = int.MaxValue;

        costs[request.Origin] = 0;
        queue.Enqueue(request.Origin, 0);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (visited.Contains(current)) continue;
            visited.Add(current);

            foreach (var (neighbor, cost) in graph.GetValueOrDefault(current, new()))
            {
                int newCost = costs[current] + cost;
                if (newCost < costs.GetValueOrDefault(neighbor, int.MaxValue))
                {
                    costs[neighbor] = newCost;
                    previous[neighbor] = current;
                    queue.Enqueue(neighbor, newCost);
                }
            }
        }

        if (!costs.ContainsKey(request.Destination) || costs[request.Destination] == int.MaxValue)
            return Result<SearchFlightsResponse>.Fail("Rota não localizada");

        var path = new List<string>();
        for (var at = request.Destination; at != null; at = previous.GetValueOrDefault(at))
            path.Insert(0, at);

        return Result<SearchFlightsResponse>.Ok(new SearchFlightsResponse {
            Resultado = $"{string.Join(" => ",path)} ao custo de R$ {costs[request.Destination]}"
        });
    }   

    private Dictionary<string, List<(string, int)>> BuildGraph(List<RouteResponse> routes)
    {
        var graph = new Dictionary<string, List<(string, int)>>();

        foreach (var route in routes)
        {
            if (!graph.ContainsKey(route.Origin))
                graph[route.Origin] = new List<(string, int)>();

            graph[route.Origin].Add((route.Destination, route.Cost));
        }

        return graph;
    }
}
