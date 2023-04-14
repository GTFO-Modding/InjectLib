using Il2CppInterop.Runtime.InteropTypes;
using Il2CppJsonNet.Converters;
using InjectLib.FieldInjection;
using InjectLib.JsonNETInjection.Converter;
using InjectLib.JsonNETInjection.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.JsonNETInjection;
public static class JsonInjector
{
    static JsonInjector()
    {
        FieldInjector<DataSetConverter>.DefineManagedField<NativeJsonProcessor>("m_JsonProcessor");
    }

    public static void SetConverter<T>(Il2CppJsonReferenceTypeConverter<T> converter) where T : Il2CppObjectBase
    {
        if (converter == null)
        {
            throw new ArgumentNullException(nameof(converter));
        }

        if (NativeJsonProcessorStore.Prepare<T>(out var processor))
        {
            if (processor.Converter != null)
            {
                Logger.Warn($"Converter (type: {typeof(T).FullName}) will be replaced! This would leads to unwanted behaviour!");
                Logger.Warn($" - Suggetion: If your converter does not require to deep-level altering of JSON, consider use {nameof(Il2CppJsonReferenceTypeHandler<T>)} instead.");
                Logger.Warn($"Previous Owner: {processor.Converter.GetType().Assembly.FullName} / {processor.Converter.GetType().FullName}");
                Logger.Warn($"New Owner: {converter.GetType().Assembly.FullName} / {converter.GetType().FullName}");
            }
            processor.Converter = converter;
        }
    }

    public static void SetConverter<T>(Il2CppJsonUnmanagedTypeConverter<T> converter) where T : unmanaged
    {
        if (converter == null)
        {
            throw new ArgumentNullException(nameof(converter));
        }

        if (NativeJsonProcessorStore.Prepare<T>(out var processor))
        {
            if (processor.Converter != null)
            {
                Logger.Warn($"Converter (type: {typeof(T).FullName}) will be replaced! This would leads to unwanted behaviour!");
                Logger.Warn($" - Suggetion: If your converter does not require to deep-level altering of JSON, consider use {nameof(Il2CppJsonUnmanagedTypeHandler<T>)} instead.");
                Logger.Warn($" - Previous Owner: {processor.Converter.GetType().Assembly.FullName} / {processor.Converter.GetType().FullName}");
                Logger.Warn($" - New Owner: {converter.GetType().Assembly.FullName} / {converter.GetType().FullName}");
            }
            processor.Converter = converter;
        }
    }

    public static void AddHandler<T>(Il2CppJsonReferenceTypeHandler<T> handler) where T : Il2CppObjectBase
    {
        if (handler == null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        if (NativeJsonProcessorStore.Prepare<T>(out var processor))
        {
            processor.Handlers.Add(handler);
        }
    }

    public static void AddHandler<T>(Il2CppJsonUnmanagedTypeHandler<T> handler) where T : unmanaged
    {
        if (handler == null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        if (NativeJsonProcessorStore.Prepare<T>(out var processor))
        {
            processor.Handlers.Add(handler);
        }
    }
}
