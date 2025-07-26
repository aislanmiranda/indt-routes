using Api.Controllers;
using Application.Interfaces;
using Application.Requests;
using Application.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ControllerTests;

public class RouterControllerTests
{
    private readonly Mock<IRouteService> _routeServiceMock;
    private readonly routeController _controller;

    public RouterControllerTests()
    {
        _routeServiceMock = new Mock<IRouteService>();
        _controller = new routeController(_routeServiceMock.Object);
    }

    #region GetSearchBestRoute
    [Fact]
    public async Task GetSearchBestRoute_ShouldReturn200_WhenSuccess()
    {
        //Arrange
        var request = new SearchFlightsRequest { Origin = "AAA", Destination = "BBB" };
        var response = new SearchFlightsResponse();
        _routeServiceMock.Setup(x => x.GetSearchBestRoute(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<SearchFlightsResponse>.Ok(response));

        //Act
        var result = await _controller.GetSearchBestRoute(request, CancellationToken.None) as ObjectResult;

        //Assert
        Assert.Equal(200, result?.StatusCode);
        Assert.NotNull(result?.Value);
        _routeServiceMock.Verify(x => x.GetSearchBestRoute(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetSearchBestRoute_ShouldReturnError_WhenFail()
    {
        //Arrange
        var request = new SearchFlightsRequest();
        _routeServiceMock.Setup(x => x.GetSearchBestRoute(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<SearchFlightsResponse>.Fail("Erro"));

        //Act
        var result = await _controller.GetSearchBestRoute(request, CancellationToken.None) as ObjectResult;

        //Assert
        Assert.Equal(400, result?.StatusCode);
        Assert.Contains("error", result?.Value?.ToString());
        _routeServiceMock.Verify(x => x.GetSearchBestRoute(request, It.IsAny<CancellationToken>()), Times.Once);
    }
    #endregion

    #region Create Route
    [Fact]
    public async Task CreateRoute_ShouldReturn201_WhenSuccess()
    {
        //Arrange
        var request = new CreateRouteRequest();
        var routeResponse = new RouteResponse();
        _routeServiceMock.Setup(x => x.AddRouteAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<RouteResponse>.Ok(routeResponse, 201));

        //Act
        var result = await _controller.CreateRoute(request, CancellationToken.None) as ObjectResult;

        //Assert
        Assert.Equal(201, result?.StatusCode);
        _routeServiceMock.Verify(x => x.AddRouteAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateRoute_ShouldReturn400_WhenFail()
    {
        //Arrange
        var request = new CreateRouteRequest();
        _routeServiceMock.Setup(x => x.AddRouteAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<RouteResponse>.Fail("Erro"));

        //Act
        var result = await _controller.CreateRoute(request, CancellationToken.None) as ObjectResult;

        //Assert
        Assert.Equal(400, result?.StatusCode);
        _routeServiceMock.Verify(x => x.AddRouteAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }
    #endregion

    #region Update Route
    [Fact]
    public async Task UpdateRoute_ShouldReturn200_WhenSuccess()
    {
        //Arrange
        var request = new UpdateRouteRequest();
        var response = new RouteResponse();
        _routeServiceMock.Setup(x => x.UpdateRouteAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<RouteResponse>.Ok(response));

        //Act
        var result = await _controller.UpdateRoute(request, CancellationToken.None) as ObjectResult;

        //Assert
        Assert.Equal(200, result?.StatusCode);
        _routeServiceMock.Verify(x => x.UpdateRouteAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRoute_ShouldReturnError_WhenFail()
    {
        //Arrange
        var request = new UpdateRouteRequest();
        _routeServiceMock.Setup(x => x.UpdateRouteAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<RouteResponse>.Fail("Erro"));

        //Act
        var result = await _controller.UpdateRoute(request, CancellationToken.None) as ObjectResult;

        //Assert
        Assert.Equal(400, result?.StatusCode);
        _routeServiceMock.Verify(x => x.UpdateRouteAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }
    #endregion

    #region Delete Route
    [Fact]
    public async Task DeleteRoute_ShouldReturn200_WhenSuccess()
    {
        //Arrange
        _routeServiceMock.Setup(x => x.DeleteRouteAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<RouteResponse>.Ok(new RouteResponse()));

        //Act
        var result = await _controller.DeleteRoute(1, CancellationToken.None) as ObjectResult;

        //Assert
        Assert.Equal(200, result?.StatusCode);
        _routeServiceMock.Verify(x => x.DeleteRouteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRoute_ShouldReturnError_WhenFail()
    {
        //Arrange
        _routeServiceMock.Setup(x => x.DeleteRouteAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<RouteResponse>.Fail("Erro"));

        //Act
        var result = await _controller.DeleteRoute(1, CancellationToken.None) as ObjectResult;

        //Assert
        Assert.Equal(400, result?.StatusCode);
        _routeServiceMock.Verify(x => x.DeleteRouteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }
    #endregion

    #region Get All Routes
    [Fact]
    public async Task GetAllRoutes_ShouldReturn200_WhenSuccess()
    {
        //Arrange
        var routes = new List<RouteResponse> { new() };
        _routeServiceMock.Setup(x => x.GetAllRoutesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<List<RouteResponse>>.Ok(routes));

        //Act
        var result = await _controller.GetAllRoutes(CancellationToken.None) as ObjectResult;

        //Assert
        Assert.Equal(200, result?.StatusCode);
        _routeServiceMock.Verify(x => x.GetAllRoutesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllRoutes_ShouldReturnError_WhenFail()
    {
        //Arrange
        _routeServiceMock.Setup(x => x.GetAllRoutesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<List<RouteResponse>>.Fail("Erro"));

        //Act
        var result = await _controller.GetAllRoutes(CancellationToken.None) as ObjectResult;

        //Assert
        Assert.Equal(400, result?.StatusCode);
        _routeServiceMock.Verify(x => x.GetAllRoutesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    #endregion

}

