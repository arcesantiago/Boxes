using Boxes.Application.DTOs;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boxes.Infrastructure.Converters;

/// <summary>
/// Convertidor JSON personalizado que maneja cuando Address viene como string JSON serializado
/// </summary>
public class AddressJsonConverter : JsonConverter<AddressResponse?>
{
    public override AddressResponse? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var jsonString = reader.GetString();
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return null;
            }

            var basicOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            return JsonSerializer.Deserialize<AddressResponse>(jsonString, basicOptions);
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var basicOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            return JsonSerializer.Deserialize<AddressResponse>(doc.RootElement.GetRawText(), basicOptions);
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, AddressResponse? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        var basicOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        JsonSerializer.Serialize(writer, value, basicOptions);
    }
}