using Boxes.Application.Contracts.Interfaces;
using Boxes.Application.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Boxes.Infrastructure.Services;

public class WorkshopService : IWorkshopService
{
    private readonly HttpClient _httpClient;

    public WorkshopService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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

    public async Task<IEnumerable<Workshop>> GetActiveWorkshopsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("places/workshops", cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var workshops = JsonSerializer.Deserialize<List<WorkshopResponse>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<WorkshopResponse>();

            var activeWorkshops = workshops
                .Where(w => w.IsActive)
                .Select(w => new Workshop(w.Id, w.Name ?? string.Empty, w.Address, w.IsActive))
                .ToList();

            return activeWorkshops;
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Error al obtener talleres desde la API externa", ex);
        }
    }

    public async Task<Workshop?> GetWorkshopByIdAsync(int placeId, CancellationToken cancellationToken = default)
    {
        var workshops = await GetActiveWorkshopsAsync(cancellationToken);
        return workshops.FirstOrDefault(w => w.Id == placeId);
    }

    private record WorkshopResponse(
        int Id,
        string? Name,
        string? Address,
        bool IsActive
    );
}