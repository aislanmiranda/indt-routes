using Domain;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RouteRepository : IRouteRepository
{

    protected readonly AppDbContext _context;

    public RouteRepository(AppDbContext context)
        => _context = context;

    public async Task<Route> AddAsync(Route route, CancellationToken cancellationToken)
    {
        _context.Set<Route>().Add(route);
        await _context.SaveChangesAsync(cancellationToken);
        return route;
    }

    public async Task<Route> UpdateAsync(Route route, CancellationToken cancellationToken)
    {
        var model = _context.Update(route).Entity;
        await _context.SaveChangesAsync(cancellationToken);
        return model;
    }

    public async Task<Route> DeleteByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _context
            .Set<Route>()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (entity == null)
            throw new Exception("Registro não encontrato.");

        _context.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<List<Route>?> GetAllRoutesAsync(CancellationToken cancellationToken)
    {
        var list = await _context
            .Set<Route>()
            .ToListAsync(cancellationToken);

        return list;
    }
}

