using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Serilog;

namespace Praedico.Bookings.Infrastructure.Data.Converters;

public static class ValueConverters
{
    public static ValueConverter<DateTime, DateTime> DateTimeUtcConverter => 
        new(x => x, x => DateTime.SpecifyKind(x, DateTimeKind.Utc));

    public static ValueConverter<DateTime?, DateTime?> NullableDateTimeUtcConverter => 
        new(x => x, x => x.HasValue ? DateTime.SpecifyKind(x.Value, DateTimeKind.Utc) : x);

}

public static class ValueConverterExtensions
{
    public static void ApplyValueConverters(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.IsKeyless)
                continue;

            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(ValueConverters.DateTimeUtcConverter);
                    Log.Debug($"{nameof(ValueConverterExtensions)}:{nameof(ApplyValueConverters)} SetValueConverter for DateTime property: {entityType.Name}.{property.Name} has been applied...");
                }
                else if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(ValueConverters.NullableDateTimeUtcConverter);
                    Log.Debug($"{nameof(ValueConverterExtensions)}:{nameof(ApplyValueConverters)} SetValueConverter for DateTime? property: {entityType.Name}.{property.Name} has been applied...");
                }
            }
        }
    }
}
