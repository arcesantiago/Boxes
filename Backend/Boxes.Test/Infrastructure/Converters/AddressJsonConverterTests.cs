using Boxes.Application.DTOs;
using Boxes.Infrastructure.Converters;
using FluentAssertions;
using System.Text.Json;

namespace Boxes.Test.Infrastructure.Converters;

public class AddressJsonConverterTests
{
    private readonly AddressJsonConverter _converter;
    private readonly JsonSerializerOptions _options;

    public AddressJsonConverterTests()
    {
        _converter = new AddressJsonConverter();
        _options = new JsonSerializerOptions
        {
            Converters = { _converter },
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public void Read_WithStringJson_ShouldDeserializeCorrectly()
    {
        // Arrange - Simulamos que el address viene como un string JSON serializado
        // El JSON viene como string escapado dentro de otro JSON
        var addressJsonString = "{\"address_components\":[{\"long_name\":\"Diagonal 74 & Calle 47\",\"short_name\":\"Diag. 74 & C. 47\",\"types\":[\"intersection\"]}],\"adr_address\":\"Diag. 74 & C. 47, La Plata, Provincia de Buenos Aires, Argentina\",\"formatted_address\":\"Diag. 74 & C. 47, La Plata, Provincia de Buenos Aires, Argentina\",\"geometry\":{\"location\":{\"lat\":-34.9156504,\"lng\":-57.9549363},\"viewport\":{\"south\":-34.91699938029149,\"west\":-57.95628528029151,\"north\":-34.91430141970849,\"east\":-57.9535873197085}},\"icon\":\"https://maps.gstatic.com/mapfiles/place_api/icons/v1/png_71/geocode-71.png\",\"icon_background_color\":\"#7B9EB0\",\"icon_mask_base_uri\":\"https://maps.gstatic.com/mapfiles/place_api/icons/v2/generic_pinlet\",\"name\":\"Diagonal 74 & Calle 47\",\"place_id\":\"test-place-id\",\"reference\":\"test-reference\",\"types\":[\"intersection\"],\"url\":\"https://maps.google.com/?q=test\",\"vicinity\":\"La Plata\",\"html_attributions\":[],\"utc_offset_minutes\":-180}";
        
        // Crear el JSON wrapper manualmente
        var json = $"{{\"address\":{JsonSerializer.Serialize(addressJsonString)}}}";
        var bytes = System.Text.Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(bytes);

        // Act
        reader.Read(); // StartObject
        reader.Read(); // PropertyName "address"
        reader.Read(); // String value (el JSON serializado como string)
        var result = _converter.Read(ref reader, typeof(AddressResponse), _options);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().NotBeNull();
        result.Name.Should().Contain("Diagonal 74");
    }

    [Fact]
    public void Read_WithNull_ShouldReturnNull()
    {
        // Arrange
        var json = "{\"address\": null}";
        var reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));

        // Act
        reader.Read(); // StartObject
        reader.Read(); // PropertyName "address"
        reader.Read(); // Null
        var result = _converter.Read(ref reader, typeof(AddressResponse), _options);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Read_WithEmptyString_ShouldReturnNull()
    {
        // Arrange
        var json = "{\"address\": \"\"}";
        var reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));

        // Act
        reader.Read(); // StartObject
        reader.Read(); // PropertyName "address"
        reader.Read(); // Empty string
        var result = _converter.Read(ref reader, typeof(AddressResponse), _options);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Write_WithAddressResponse_ShouldSerializeCorrectly()
    {
        // Arrange
        var address = new AddressResponse(
            AddressComponents: new List<AddressComponentResponse>
            {
                new AddressComponentResponse("Test", "T", new List<string> { "type1" })
            },
            AdrAddress: "Test Address",
            FormattedAddress: "Test Formatted Address",
            Geometry: new GeometryResponse(
                Location: new LocationResponse(-34.9156504, -57.9549363),
                Viewport: new ViewportResponse(-34.916, -57.956, -34.914, -57.953)
            ),
            Icon: "test-icon",
            IconBackgroundColor: "#FFFFFF",
            IconMaskBaseUri: "test-uri",
            Name: "Test Name",
            PlaceId: "test-place-id",
            Reference: "test-reference",
            Types: new List<string> { "type1" },
            Url: "https://test.com",
            Vicinity: "Test Vicinity",
            HtmlAttributions: new List<string>(),
            UtcOffsetMinutes: -180,
            Photos: null,
            Website: null
        );

        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);

        // Act
        _converter.Write(writer, address, _options);
        writer.Flush();

        // Assert
        stream.Position = 0;
        var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("Test Formatted Address");
    }

    [Fact]
    public void Write_WithNull_ShouldWriteNull()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);

        // Act
        _converter.Write(writer, null, _options);
        writer.Flush();

        // Assert
        stream.Position = 0;
        var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());
        json.Should().Be("null");
    }
}

