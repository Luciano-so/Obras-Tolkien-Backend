using System.Text.Json;
using System.Text.Json.Serialization;

namespace TerraMedia.Application.Converters;

public class BioConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString();
        }
        else if (reader.TokenType == JsonTokenType.StartObject)
        {
            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                if (document.RootElement.TryGetProperty("value", out JsonElement valueElement))
                {
                    return valueElement.GetString();
                }
            }
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}
