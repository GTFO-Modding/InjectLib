using BepInEx.Unity.IL2CPP.Hook;
using GTFO.API;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using Il2CppJsonNet;
using Il2CppJsonNet.Serialization;
using System;

namespace InjectLib.JsonNETInjection.Detours;
internal class Detour_JsonConverterTrampoline
{
    public unsafe delegate IntPtr GetMatchingConverterDel(IntPtr converters, IntPtr objectType, Il2CppMethodInfo* methodInfo);
    public unsafe delegate IntPtr GetConverterDel(IntPtr attributeProvider, Il2CppMethodInfo* methodInfo);

    private static INativeDetour _Detour1, _Detour2;
    private static GetMatchingConverterDel _Original1;
    private static GetConverterDel _Original2;

    public unsafe static void Patch()
    {
        var method1 = Il2CppAPI.GetIl2CppMethod<JsonSerializer>(
            nameof(JsonSerializer.GetMatchingConverter),
            "Newtonsoft.Json.JsonConverter",
            isGeneric: false,
            [
                "System.Collections.Generic.IList<Newtonsoft.Json.JsonConverter>",
                typeof(Type).FullName
            ]);

        _Detour1 = INativeDetour.CreateAndApply((nint)method1, Detour1, out _Original1);

        var klass = Il2CppClassPointerStore.GetNativeClassPointer(typeof(JsonTypeReflector));
        var methodName = nameof(JsonTypeReflector.GetJsonConverter);
        void** ptr = (void**)IL2CPP.GetIl2CppMethod(klass, false, methodName, "Newtonsoft.Json.JsonConverter", typeof(object).FullName).ToPointer();
        void* methodPtr = (ptr == null) ? ptr : *ptr;
        _Detour2 = INativeDetour.CreateAndApply((nint)methodPtr, Detour2, out _Original2);

        //JsonConverter GetConverter(JsonContract contract, JsonConverter memberConverter, JsonContainerContract containerContract, JsonProperty containerProperty)
    }

    private unsafe static IntPtr Detour1(IntPtr converters, IntPtr objectType, Il2CppMethodInfo* methodInfo)
    {
        if (NativeJsonProcessorStore.TryGetConverterPointer(objectType, out var converter))
        {
            return converter;
        }

        return _Original1(converters, objectType, methodInfo);
    }

    private unsafe static IntPtr Detour2(IntPtr attributeProvider, Il2CppMethodInfo* methodInfo)
    {
        var obj = new Il2CppSystem.Object(attributeProvider);
        var type = obj.TryCast<Il2CppSystem.Type>();
        if (type != null && NativeJsonProcessorStore.TryGetConverterPointer(type.Pointer, out var converterPtr))
        {
            return converterPtr;
        }

        return _Original2(attributeProvider, methodInfo);
    }
}
