using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using Praedico.Exceptions;

namespace Praedico.Bookings.Api.Converters;

public class DateTimeConverter : JsonConverter<DateTimeOffset?>
{
    private const string UtcDateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateTimeStr = reader.GetString();
        if (string.IsNullOrEmpty(dateTimeStr))        
            return null;

        if (DateTimeOffset.TryParseExact(dateTimeStr, UtcDateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, 
                out var result))
            return result;

        throw new BusinessException($"DateTime must be in UTC format '{UtcDateTimeFormat}'.");
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToUniversalTime().ToString(UtcDateTimeFormat));
    }
}