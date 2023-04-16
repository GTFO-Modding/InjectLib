using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InjectLib.JsonNETInjection.Supports;
public sealed class InjectLibConnector : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        var il2cppType = Il2CppType.From(typeToConvert, throwOnFailure: false);
        if (il2cppType != null)
        {
            if (NativeJsonProcessorStore.TryGetProcessor(il2cppType.Pointer, out _))
            {
                return true;
            }
        }

        return false;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var il2cppType = Il2CppType.From(typeToConvert, throwOnFailure: false);
        if (!NativeJsonProcessorStore.TryGetProcessor(il2cppType.Pointer, out NativeJsonProcessor processor))
        {
            return null;
        }

        JsonConverter converter;
        IBaseConnector connector;
        if (typeToConvert.IsAssignableTo(typeof(Il2CppObjectBase)))
        {
            converter = (JsonConverter)Activator.CreateInstance(
                typeof(ReferenceTypeConnector<>).MakeGenericType(
                    new Type[] { typeToConvert }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null);

            connector = (IBaseConnector)converter;
            connector.Processor = processor;
            return converter;
        }
        else if (typeToConvert.IsValueType)
        {
            converter = (JsonConverter)Activator.CreateInstance(
                typeof(ValueTypeConnector<>).MakeGenericType(
                    new Type[] { typeToConvert }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null);

            connector = (IBaseConnector)converter;
            connector.Processor = processor;
            return converter;
        }

        return null;
    }
}
