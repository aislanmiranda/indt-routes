using Application.Requests;
using Application.Responses;
using AutoMapper;
using Domain;

namespace Application.Mappers;
	
public class RouteMap : Profile
{
    public RouteMap()
    {
        CreateMap<CreateRouteRequest, Route>();
        CreateMap<UpdateRouteRequest, Route>();
        CreateMap<Route, RouteResponse>();
    }
}