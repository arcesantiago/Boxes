using AutoMapper;
using Boxes.Application.DTOs;
using Boxes.Infrastructure.Services;
using FluentAssertions;
using Moq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Boxes.Test.Infrastructure.Services;

public class WorkshopServiceTests : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly Mock<IMapper> _mapperMock;
    private readonly WorkshopService _service;
    private readonly HttpMessageHandler _handler;

    public WorkshopServiceTests()
    {
        _mapperMock = new Mock<IMapper>();
        _handler = new MockHttpMessageHandler();
        _httpClient = new HttpClient(_handler)
        {
            BaseAddress = new Uri("https://dev.tecnomcrm.com/api/v1/")
        };
        _service = new WorkshopService(_httpClient, _mapperMock.Object);
    }

    [Fact]
    public async Task GetActiveWorkshopsAsync_WithValidResponse_ShouldReturnWorkshops()
    {
        // Arrange
        var workshopsResponse = new List<WorkshopDto>
        {
            new WorkshopDto(
                1, "Taller 1", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null, null
            ),
            new WorkshopDto(
                2, "Taller 2", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null, null
            )
        };

        var json = JsonSerializer.Serialize(workshopsResponse);
        ((MockHttpMessageHandler)_handler).SetResponse(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });

        // Act
        var result = await _service.GetActiveWorkshopsAsync();

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetWorkshopByIdAsync_WithExistingId_ShouldReturnWorkshop()
    {
        // Arrange
        var workshopsResponse = new List<WorkshopDto>
        {
            new WorkshopDto(
                1, "Taller 1", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null, null
            )
        };

        var json = JsonSerializer.Serialize(workshopsResponse);
        ((MockHttpMessageHandler)_handler).SetResponse(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });

        // Act
        var result = await _service.GetWorkshopByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetWorkshopByIdAsync_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        var workshopsResponse = new List<WorkshopDto>();
        var json = JsonSerializer.Serialize(workshopsResponse);
        ((MockHttpMessageHandler)_handler).SetResponse(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });

        // Act
        var result = await _service.GetWorkshopByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
        _handler?.Dispose();
    }
}

// Helper class para mockear HttpClient
public class MockHttpMessageHandler : HttpMessageHandler
{
    private HttpResponseMessage? _response;

    public void SetResponse(HttpResponseMessage response)
    {
        _response = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_response ?? new HttpResponseMessage(HttpStatusCode.NotFound));
    }
}

