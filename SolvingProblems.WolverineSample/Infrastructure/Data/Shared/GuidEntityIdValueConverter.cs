using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SolvingProblems.WolverineSample.Domain.Abstract;
using System.Reflection;
using System.Reflection.Emit;

namespace SolvingProblems.WolverineSample.Infrastructure.Data.Shared;

internal class GuidEntityIdValueConverter<TIdentifier> : ValueConverter<TIdentifier, Guid>
    where TIdentifier : GuidEntityId
{
    private static readonly Func<Guid, TIdentifier> constructor;

    /// <summary>
    /// This method compiles a Func using IL emitting to avoid calling Activator.CreateInstance and reflection every time.
    /// </summary>
    static GuidEntityIdValueConverter()
    {
        Type identifier = typeof(TIdentifier);
        Type[] args = new Type[] { typeof(Guid) };
        ConstructorInfo constructorInfo = identifier.GetConstructor(args)!;

        DynamicMethod dynamicMethod = new DynamicMethod("DM$_" + identifier.Name, identifier, args, identifier);
        ILGenerator ilGen = dynamicMethod.GetILGenerator();
        ilGen.Emit(OpCodes.Ldarg, 0);
        ilGen.Emit(OpCodes.Newobj, constructorInfo);
        ilGen.Emit(OpCodes.Ret);

        constructor = (Func<Guid, TIdentifier>)dynamicMethod.CreateDelegate(typeof(Func<Guid, TIdentifier>));
    }

    public GuidEntityIdValueConverter() : base(id => id.AsGuid(), value => constructor(value)) { }
}
