using System.Text.Json;
using System.Text.Json.Serialization;

namespace Praedico.Bookings.Api.Converters;

public class StringConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var s = reader.GetString();
        return string.IsNullOrEmpty(s) ? null : s.Trim();
    }

    public override void Write(Utf8JsonWriter writer, string? s, JsonSerializerOptions options)
    {
        writer.WriteStringValue(s ?? null);
    }
}