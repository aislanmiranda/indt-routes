using System.Reflection;
using Application.Interfaces;
using Application.Requests;
using Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class InjectionApplication
{
    public static void AddApplication(
        this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreateRouteRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateRouteRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SearchFlightsRequestValidator>();

        services.AddTransient<IRouteService, RouteService>();
    }
}