using AutoMapper;
using Boxes.Application.DTOs;
using Boxes.Application.Mapping;
using Boxes.Infrastructure.Converters;
using Boxes.Infrastructure.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boxes.Test.Infrastructure.Services;

public class WorkshopServiceIntegrationTests : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly WorkshopService _service;
    private readonly MockHttpMessageHandler _handler;

    public WorkshopServiceIntegrationTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>(), NullLoggerFactory.Instance);
        _mapper = config.CreateMapper();
        _handler = new MockHttpMessageHandler();
        _httpClient = new HttpClient(_handler)
        {
            BaseAddress = new Uri("https://dev.tecnomcrm.com/api/v1/")
        };
        _service = new WorkshopService(_httpClient, _mapper);
    }

    [Fact]
    public async Task GetActiveWorkshopsAsync_WithActiveAndInactiveWorkshops_ShouldFilterOnlyActive()
    {
        // Arrange
        var workshopsResponse = new List<WorkshopDto>
        {
            new WorkshopDto(
                1, "Taller Activo", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null, null
            ),
            new WorkshopDto(
                2, "Taller Inactivo", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false, null, null, null, null, null, null, null, null, null, null, null, null
            ),
            new WorkshopDto(
                3, "Taller Activo 2", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null, null
            )
        };

        var json = JsonSerializer.Serialize(workshopsResponse, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        _handler.SetResponse(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });

        // Act
        var result = await _service.GetActiveWorkshopsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().OnlyContain(w => w.Active);
    }

    [Fact]
    public async Task GetActiveWorkshopsAsync_WithHttpError_ShouldThrowException()
    {
        // Arrange
        _handler.SetResponse(new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent("Internal Server Error", Encoding.UTF8, "text/plain")
        });

        // Act & Assert
        var act = async () => await _service.GetActiveWorkshopsAsync();
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Error al obtener talleres desde la API externa*");
    }

    [Fact]
    public async Task GetWorkshopByIdAsync_WithExistingId_ShouldReturnWorkshop()
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
        _handler.SetResponse(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });

        // Act
        var result = await _service.GetWorkshopByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Taller 1");
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
        _handler?.Dispose();
    }
}

