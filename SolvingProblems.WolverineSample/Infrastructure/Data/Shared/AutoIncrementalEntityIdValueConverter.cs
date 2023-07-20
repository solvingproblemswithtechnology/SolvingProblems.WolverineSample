using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SolvingProblems.WolverineSample.Domain.Abstract;
using System.Reflection;
using System.Reflection.Emit;

namespace SolvingProblems.WolverineSample.Infrastructure.Data.Shared;

internal class AutoIncrementalEntityIdValueConverter<TIdentifier> : ValueConverter<TIdentifier, long> where TIdentifier : AutoIncrementalEntityId
{
    private static readonly Func<long, TIdentifier> constructor;

    /// <summary>
    /// This method compiles a Func using IL emitting to avoid calling Activator.CreateInstance and reflection every time.
    /// </summary>
    static AutoIncrementalEntityIdValueConverter()
    {
        Type identifier = typeof(TIdentifier);
        Type[] args = new Type[] { typeof(long) };
        ConstructorInfo constructorInfo = identifier.GetConstructor(args)!;

        DynamicMethod dynamicMethod = new DynamicMethod("DM$_" + identifier.Name, identifier, args, identifier);
        ILGenerator ilGen = dynamicMethod.GetILGenerator();
        ilGen.Emit(OpCodes.Ldarg, 0);
        ilGen.Emit(OpCodes.Newobj, constructorInfo);
        ilGen.Emit(OpCodes.Ret);

        constructor = (Func<long, TIdentifier>)dynamicMethod.CreateDelegate(typeof(Func<long, TIdentifier>));
    }

    public AutoIncrementalEntityIdValueConverter() : base(id => id.Id, value => constructor(value)) { }
}

internal class AutoIncrementalEntityIdValueConverter : ValueConverter<AutoIncrementalEntityId, long>
{
#pragma warning disable IDE0052 // Used by reflection
    private static readonly Func<long, AutoIncrementalEntityId> constructor;
#pragma warning restore IDE0052 // Used by reflection

    /// <summary>
    /// This method compiles a Func using IL emitting to avoid calling Activator.CreateInstance and reflection every time.
    /// </summary>
    static AutoIncrementalEntityIdValueConverter()
    {
        Type identifier = typeof(AutoIncrementalEntityId);
        Type[] args = new Type[] { typeof(long) };
        ConstructorInfo constructorInfo = identifier.GetConstructor(args)!;

        DynamicMethod dynamicMethod = new DynamicMethod("DM$_" + identifier.Name, identifier, args, identifier);
        ILGenerator ilGen = dynamicMethod.GetILGenerator();
        ilGen.Emit(OpCodes.Ldarg, 0);
        ilGen.Emit(OpCodes.Newobj, constructorInfo);
        ilGen.Emit(OpCodes.Ret);

        constructor = (Func<long, AutoIncrementalEntityId>)dynamicMethod.CreateDelegate(typeof(Func<long, AutoIncrementalEntityId>));
    }

    public AutoIncrementalEntityIdValueConverter() : base(id => id.Id, value => new AutoIncrementalEntityId(value)) { }
}
