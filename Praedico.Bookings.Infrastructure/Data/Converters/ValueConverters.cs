using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Praedico.Bookings.Domain;
using System.Reflection;

namespace Praedico.Bookings.Infrastructure.Data.Converters;

public static class ValueConverters
{
    public static ValueConverter<DateTime, DateTime> DateTimeUtcConverter => 
        new(x => x, x => DateTime.SpecifyKind(x, DateTimeKind.Utc));

    public static ValueConverter<DateTime?, DateTime?> NullableDateTimeUtcConverter => 
        new(x => x, x => x.HasValue ? DateTime.SpecifyKind(x.Value, DateTimeKind.Utc) : x);

    public static ValueConverter<T, string> EnumerationConverter<T>() where T : Enumeration =>
        new(x => x.Name, x => Enumeration.FromName<T>(x));
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
                }
                else if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(ValueConverters.NullableDateTimeUtcConverter);
                }
                else if (IsEnumerationType(property.ClrType))
                {
                    var converter = CreateEnumerationConverter(property.ClrType);
                    if (converter != null)
                    {
                        property.SetValueConverter(converter);
                    }
                }
            }
        }
    }

    private static bool IsEnumerationType(Type type) =>
        type.IsSubclassOf(typeof(Enumeration));

    private static ValueConverter? CreateEnumerationConverter(Type type)
    {
        var method = typeof(ValueConverters)
            .GetMethod(nameof(ValueConverters.EnumerationConverter), BindingFlags.Public | BindingFlags.Static)?
            .MakeGenericMethod(type);

        return method?.Invoke(null, null) as ValueConverter;
    }
}
