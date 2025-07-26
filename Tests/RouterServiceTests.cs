using AutoMapper;
using Application.Services;
using Application.Requests;
using Application.Responses;
using Domain;
using Domain.Interfaces;
using Moq;

namespace ApplicationTests;

public class RouteServiceTests
{
    private readonly Mock<IRouteRepository> _routeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly RouteService _service;

    // Dados comuns
    private readonly CreateRouteRequest _validRequest;
    private readonly UpdateRouteRequest _validUpdateRequest;
    private readonly Route _mappedEntity;
    private readonly Route _savedEntity;
    private readonly RouteResponse _expectedResponse;
    private readonly List<Route> _existingRoutes;
    private readonly List<RouteResponse> _expectedResponseList;
    private readonly SearchFlightsRequest _validSearchFlightsRequest;

    public RouteServiceTests()
    {
        _routeRepositoryMock = new Mock<IRouteRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new RouteService(_routeRepositoryMock.Object, _mapperMock.Object);

        _validRequest = new CreateRouteRequest { Origin = "ABC", Destination = "XYZ", Cost = 100 };
        _validUpdateRequest = new UpdateRouteRequest { Id = 1, Origin = "ABC", Destination = "XYZ", Cost = 100 };

        _mappedEntity = new Route("ABC", "XYZ", 100);
        _savedEntity = _mappedEntity;
        _expectedResponse = new RouteResponse { Origin = "ABC", Destination = "XYZ", Cost = 100 };

        _validSearchFlightsRequest = new SearchFlightsRequest { Origin = "AAA", Destination = "CCC" };
        
        _existingRoutes = new List<Route>
        {
            new Route("AAA", "BBB", 1),
            new Route("BBB", "CCC", 1)
        };

        _expectedResponseList = new List<RouteResponse>
        {
            new() { Origin = "AAA", Destination = "BBB", Cost = 1 },
            new() { Origin = "BBB", Destination = "CCC", Cost = 1 }
        };
    }

    //private readonly Mock<IRouteRepository> _routeRepositoryMock;
    //private readonly Mock<IMapper> _mapperMock;
    //private readonly RouteService _service;

    //public RouteServiceTests()
    //{
    //    _routeRepositoryMock = new Mock<IRouteRepository>();
    //    _mapperMock = new Mock<IMapper>();
    //    _service = new RouteService(_routeRepositoryMock.Object, _mapperMock.Object);
    //}

    [Fact]
    public async Task AddRouteAsync_ShouldReturnCreatedResult_WhenSuccess()
    {
        // Arrange
        _mapperMock.Setup(m => m.Map<Route>(_validRequest)).Returns(_mappedEntity);
        _routeRepositoryMock.Setup(r => r.AddAsync(_mappedEntity, It.IsAny<CancellationToken>())).ReturnsAsync(_savedEntity);
        _mapperMock.Setup(m => m.Map<RouteResponse>(_savedEntity)).Returns(_expectedResponse);

        // Act
        var result = await _service.AddRouteAsync(_validRequest, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(201, result.StatusCode);
        Assert.Equal(_expectedResponse.Origin, result.Data!.Origin);
        Assert.Equal(_expectedResponse.Destination, result.Data.Destination);
        _routeRepositoryMock.Verify(r => r.AddAsync(_mappedEntity, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddRouteAsync_ShouldReturnFail_WhenExceptionThrown()
    {
        // Arrange
        _mapperMock.Setup(m => m.Map<Route>(_validRequest)).Throws(new Exception("Mapping failed"));

        // Act
        var result = await _service.AddRouteAsync(_validRequest, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Erro ao cadastrar rota", result.Error);
        _routeRepositoryMock.Verify(r => r.AddAsync(_mappedEntity, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetSearchBestRoute_ShouldReturnBestPath_WhenRouteExists()
    {
        // Arrange
        _routeRepositoryMock.Setup(r => r.GetAllRoutesAsync(It.IsAny<CancellationToken>()))
                            .ReturnsAsync(_existingRoutes);
        _mapperMock.Setup(m => m.Map<List<RouteResponse>>(_existingRoutes))
                   .Returns(_expectedResponseList);

        // Act
        var result = await _service.GetSearchBestRoute(_validSearchFlightsRequest, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("AAA => BBB => CCC ao custo de R$ 2", result.Data!.Resultado);
        _routeRepositoryMock.Verify(r => r.GetAllRoutesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRouteAsync_ShouldReturnSuccess_WhenUpdateSucceeds()
    {
        // Arrange
        _mapperMock.Setup(m => m.Map<Route>(_validUpdateRequest)).Returns(_mappedEntity);
        _routeRepositoryMock.Setup(r => r.UpdateAsync(_mappedEntity, It.IsAny<CancellationToken>())).ReturnsAsync(_savedEntity);
        _mapperMock.Setup(m => m.Map<RouteResponse>(_savedEntity)).Returns(_expectedResponse);

        // Act
        var result = await _service.UpdateRouteAsync(_validUpdateRequest, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(_expectedResponse.Origin, result.Data!.Origin);
        Assert.Equal(_expectedResponse.Destination, result.Data.Destination);
        Assert.Equal(_expectedResponse.Cost, result.Data.Cost);
        _routeRepositoryMock.Verify(r => r.UpdateAsync(_mappedEntity, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRouteAsync_ShouldReturnFail_WhenRepositoryThrows()
    {
        // Arrange
        _mapperMock.Setup(m => m.Map<Route>(_validUpdateRequest)).Returns(_mappedEntity);
        _routeRepositoryMock.Setup(r => r.UpdateAsync(_mappedEntity, It.IsAny<CancellationToken>()))
                            .ThrowsAsync(new Exception("DB Error"));

        // Act
        var result = await _service.UpdateRouteAsync(_validUpdateRequest, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Erro ao atualizar rota", result.Error);
        Assert.Null(result.Data);
        _routeRepositoryMock.Verify(r => r.UpdateAsync(_mappedEntity, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRouteAsync_ShouldReturnFail_WhenMappingFails()
    {
        // Arrange
        _mapperMock.Setup(m => m.Map<Route>(_validUpdateRequest))
                   .Throws(new Exception("Mapping error"));

        // Act
        var result = await _service.UpdateRouteAsync(_validUpdateRequest, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Erro ao atualizar rota", result.Error);
        _routeRepositoryMock.Verify(r => r.UpdateAsync(_mappedEntity, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteRouteAsync_ShouldReturnOk_WhenDeleted()
    {
        // Arrange
        var routeId = It.IsAny<int>();
        
        _routeRepositoryMock.Setup(r => r.DeleteByIdAsync(routeId, It.IsAny<CancellationToken>())).ReturnsAsync(_mappedEntity);
        _mapperMock.Setup(m => m.Map<RouteResponse>(_mappedEntity)).Returns(_expectedResponse);

        // Act
        var result = await _service.DeleteRouteAsync(routeId, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(_expectedResponse, result.Data);
        _routeRepositoryMock.Verify(r => r.DeleteByIdAsync(routeId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRouteAsync_ShouldReturnFail_WhenRepositoryThrows()
    {
        // Arrange
        var routeId = It.IsAny<int>();

        _routeRepositoryMock.Setup(r => r.DeleteByIdAsync(routeId, It.IsAny<CancellationToken>()))
                            .ThrowsAsync(new Exception("Database failure"));

        // Act
        var result = await _service.DeleteRouteAsync(routeId, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Erro ao deletar rota", result.Error);
        Assert.Null(result.Data);
        _routeRepositoryMock.Verify(r => r.DeleteByIdAsync(routeId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRouteAsync_ShouldReturnFail_WhenMappingFails()
    {
        // Arrange
        var routeId = It.IsAny<int>();

        _routeRepositoryMock.Setup(r => r.DeleteByIdAsync(routeId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(_mappedEntity);

        _mapperMock.Setup(m => m.Map<RouteResponse>(_mappedEntity))
                   .Throws(new Exception("Mapping error"));

        // Act
        var result = await _service.DeleteRouteAsync(routeId, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Erro ao deletar rota", result.Error);
        _routeRepositoryMock.Verify(r => r.DeleteByIdAsync(routeId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRouteAsync_ShouldReturnFail_WhenRouteNotFound()
    {
        // Arrange
        var routeId = It.IsAny<int>();

        _routeRepositoryMock.Setup(r => r.DeleteByIdAsync(routeId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Route?)null);

        // Act
        var result = await _service.DeleteRouteAsync(routeId, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Registro não existe para deletar a rota", result.Error);
        Assert.Null(result.Data);
        _routeRepositoryMock.Verify(r => r.DeleteByIdAsync(routeId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllRoutesAsync_ShouldReturnSuccess_WithMappedRoutes()
    {
        // Arrange
        _routeRepositoryMock.Setup(r => r.GetAllRoutesAsync(It.IsAny<CancellationToken>()))
                            .ReturnsAsync(_existingRoutes);

        _mapperMock.Setup(m => m.Map<List<RouteResponse>>(_existingRoutes))
                   .Returns(_expectedResponseList);

        // Act
        var result = await _service.GetAllRoutesAsync(CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count);
        Assert.Equal(_expectedResponseList, result.Data);
        _routeRepositoryMock.Verify(r => r.GetAllRoutesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllRoutesAsync_ShouldReturnFail_WhenRepositoryThrows()
    {
        // Arrange
        _routeRepositoryMock.Setup(r => r.GetAllRoutesAsync(It.IsAny<CancellationToken>()))
                            .ThrowsAsync(new Exception("Erro no banco"));

        // Act
        var result = await _service.GetAllRoutesAsync(CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Erro ao carregar rotas", result.Error);
        Assert.Null(result.Data);
        _routeRepositoryMock.Verify(r => r.GetAllRoutesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllRoutesAsync_ShouldReturnFail_WhenMappingThrows()
    {
        // Arrange
       

        _routeRepositoryMock.Setup(r => r.GetAllRoutesAsync(It.IsAny<CancellationToken>()))
                            .ReturnsAsync(_existingRoutes);

        _mapperMock.Setup(m => m.Map<List<RouteResponse>>(_existingRoutes))
                   .Throws(new Exception("Erro ao mapear"));

        // Act
        var result = await _service.GetAllRoutesAsync(CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Erro ao carregar rotas", result.Error);
        Assert.Null(result.Data);
        _routeRepositoryMock.Verify(r => r.GetAllRoutesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllRoutesAsync_ShouldReturnSuccess_WhenNoRoutesExist()
    {
        // Arrange
        var emptyDomainRoutes = new List<Route>();
        var emptyResponseRoutes = new List<RouteResponse>();

        _routeRepositoryMock.Setup(r => r.GetAllRoutesAsync(It.IsAny<CancellationToken>()))
                            .ReturnsAsync(emptyDomainRoutes);

        _mapperMock.Setup(m => m.Map<List<RouteResponse>>(emptyDomainRoutes))
                   .Returns(emptyResponseRoutes);

        // Act
        var result = await _service.GetAllRoutesAsync(CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data);
        _routeRepositoryMock.Verify(r => r.GetAllRoutesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
