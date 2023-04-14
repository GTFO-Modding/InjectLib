using BepInEx.Unity.IL2CPP.Hook;
using GTFO.API;
using Il2CppInterop.Runtime.Runtime;
using Il2CppJsonNet;
using Il2CppJsonNet.Converters;
using InjectLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.JsonNETInjection.Detours;
internal class Detour_JsonConverterTrampoline
{
    public unsafe delegate IntPtr GetMatchingConverterDel(IntPtr converters, IntPtr objectType, Il2CppMethodInfo* methodInfo);
    private static INativeDetour _Detour;
    private static GetMatchingConverterDel _Original;

    public unsafe static void Patch()
    {
        var method = Il2CppAPI.GetIl2CppMethod<JsonSerializer>(
            nameof(JsonSerializer.GetMatchingConverter),
            "Newtonsoft.Json.JsonConverter",
            isGeneric: false,
            new string[]
            {
                "System.Collections.Generic.IList<Newtonsoft.Json.JsonConverter>",
                typeof(Type).FullName
            });

        _Detour = INativeDetour.CreateAndApply((nint)method, Detour, out _Original);
    }

    private unsafe static IntPtr Detour(IntPtr converters, IntPtr objectType, Il2CppMethodInfo* methodInfo)
    {
        if (NativeJsonProcessorStore.TryGetConverter(objectType, out var converter))
        {
            return converter;
        }

        return _Original(converters, objectType, methodInfo);
    }
}
