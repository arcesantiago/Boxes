using AutoMapper;
using Boxes.Application.Contracts.Interfaces;
using Boxes.Application.DTOs;
using Boxes.Infrastructure.Converters;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boxes.Infrastructure.Services;

public class WorkshopService : IWorkshopService
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new AddressJsonConverter() }
    };

    public WorkshopService(HttpClient httpClient, IMapper mapper)
    {
        _httpClient = httpClient;
        _mapper = mapper;
        ConfigureHttpClient();
    }

    private void ConfigureHttpClient()
    {
        _httpClient.BaseAddress = new Uri("https://dev.tecnomcrm.com/api/v1/");
        var credentials = Convert.ToBase64String(
            System.Text.Encoding.ASCII.GetBytes("max@tecnom.com.ar:b0x3sApp"));
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", credentials);
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<IEnumerable<WorkshopDto>> GetActiveWorkshopsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("places/workshops", cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var workshopsResponse = JsonSerializer.Deserialize<List<WorkshopDto>>(json, JsonOptions)
                ?? new List<WorkshopDto>();

            return workshopsResponse;
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Error al obtener talleres desde la API externa", ex);
        }
    }

    public async Task<WorkshopDto?> GetWorkshopByIdAsync(int placeId, CancellationToken cancellationToken = default)
    {
        var workshops = await GetActiveWorkshopsAsync(cancellationToken);
        return workshops.FirstOrDefault(w => w.Id == placeId);
    }
}