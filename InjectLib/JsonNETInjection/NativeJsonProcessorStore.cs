using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppJsonNet.Converters;
using InjectLib.JsonNETInjection.Converter;
using InjectLib.JsonNETInjection.Detours;
using InjectLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.JsonNETInjection;
internal static class NativeJsonProcessorStore
{
    private static readonly Dictionary<IntPtr, NativeJsonProcessor> _RegisteredProcessors = new();

    internal static void Initialize()
    {
        Detour_JsonConverterTrampoline.Patch();
        Detour_NativeConverterLink.Patch();
    }

    public static bool Prepare<T>(out NativeJsonProcessor processor)
    {
        var type = Il2CppType.From(typeof(T));
        if (type != null && type.Pointer != IntPtr.Zero)
        {
            if (_RegisteredProcessors.TryGetValue(type.Pointer, out processor))
            {
                return true;
            }

            var converter = new DataSetConverter();

            processor = _RegisteredProcessors[type.Pointer] = new NativeJsonProcessor();
            processor.Type = type;
            processor.DummyConverter = converter;
            
            converter.SetNativeJsonProcessor(processor);
            return true;
        }
        else
        {
            Logger.Error($"Type: '{typeof(T).Name}' is not in il2cpp domain!");
            processor = null;
            return false;
        }
    }

    public static bool TryGetConverterPointer(IntPtr typePtr, out IntPtr converterPtr)
    {
        if (TryGetProcessor(typePtr, out var processor))
        {
            if (processor.DummyConverter != null)
            {
                converterPtr = processor.DummyConverter.Pointer;
                return converterPtr != IntPtr.Zero;
            }
        }

        converterPtr = IntPtr.Zero;
        return false;
    }

    public static bool TryGetProcessor(IntPtr typePtr, out NativeJsonProcessor processor)
    {
        if (_RegisteredProcessors.TryGetValue(typePtr, out processor))
        {
            if (processor == null)
            {
                goto RETURN_NULL;
            }

            if (!processor.Enabled)
            {
                goto RETURN_NULL;
            }

            return processor != null;
        }

    RETURN_NULL:
        processor = null;
        return false;
    }
}
