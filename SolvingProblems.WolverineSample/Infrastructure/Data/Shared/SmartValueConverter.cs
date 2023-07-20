using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SolvingProblems.WolverineSample.Domain.Abstract;
using System.Collections.Concurrent;

namespace SolvingProblems.WolverineSample.Infrastructure.Data.Shared;

public class SmartValueConverter : ValueConverterSelector
{
    private readonly ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo> _converters
           = new ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo>();

    public SmartValueConverter(ValueConverterSelectorDependencies dependencies)
        : base(dependencies)
    {
    }

    public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type? providerClrType = null)
    {
        var baseConverters = base.Select(modelClrType, providerClrType);
        foreach (var converter in baseConverters)
        {
            yield return converter;
        }

        var underlyingModelType = UnwrapNullableType(modelClrType)!;
        var underlyingProviderType = UnwrapNullableType(providerClrType);

        if (IsIdentifierCompatibleWithProvider<GuidEntityId, Guid>())
            yield return GetOrAddIdentifier<Guid>(typeof(GuidEntityIdValueConverter<>), modelClrType, underlyingModelType);

        if (IsIdentifierCompatibleWithProvider<AutoIncrementalEntityId, long>())
            yield return GetOrAddIdentifier<long>(typeof(AutoIncrementalEntityIdValueConverter<>), modelClrType, underlyingModelType);

        bool IsIdentifierCompatibleWithProvider<TIdentifier, TProviderType>()
        {
            return (underlyingProviderType == null || underlyingProviderType == typeof(TProviderType))
                   && typeof(TIdentifier).IsAssignableFrom(underlyingModelType);
        }
    }

    private static Type? UnwrapNullableType(Type? type) => type == null ? null : Nullable.GetUnderlyingType(type) ?? type;

    private ValueConverterInfo GetOrAddIdentifier<TProviderType>(Type valueConverterType, Type modelClrType, Type underlyingModelType)
    {
        var converterType = valueConverterType.MakeGenericType(underlyingModelType);

        return this._converters.GetOrAdd((underlyingModelType, typeof(TProviderType)), _ =>
        {
            return new ValueConverterInfo(
                modelClrType: modelClrType,
                providerClrType: typeof(TProviderType),
                factory: valueConverterInfo => (ValueConverter)Activator.CreateInstance(converterType)!);
        });
    }
}
