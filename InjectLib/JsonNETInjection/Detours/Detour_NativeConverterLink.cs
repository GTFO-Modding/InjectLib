using BepInEx.Unity.IL2CPP.Hook;
using GTFO.API;
using Il2CppJsonNet;
using Il2CppJsonNet.Converters;
using InjectLib.FieldInjection;
using InjectLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.JsonNETInjection.Detours;
internal class Detour_NativeConverterLink
{
    public unsafe delegate IntPtr ReadJsonDel(IntPtr _this, IntPtr reader, IntPtr objectType, IntPtr existingValue, IntPtr serializer);
    public unsafe delegate void WriteJsonDel(IntPtr _this, IntPtr writer, IntPtr value, IntPtr serializer);

    private static INativeDetour _ReadDetour, _WriteDetour;
    private static ReadJsonDel _ReadOriginal;
    private static WriteJsonDel _WriteOriginal;


    public unsafe static void Patch()
    {
        var readmethod = Il2CppAPI.GetIl2CppMethod<DataSetConverter>(
            nameof(DataSetConverter.ReadJson),
            "System.Object",
            isGeneric: false,
            new string[]
            {
                "Newtonsoft.Json.JsonReader",
                "System.Type",
                "System.Object",
                "Newtonsoft.Json.JsonSerializer"
            });

        var writemethod = Il2CppAPI.GetIl2CppMethod<DataSetConverter>(
            nameof(DataSetConverter.WriteJson),
            typeof(void).FullName,
            isGeneric: false,
            new string[]
            {
                "Newtonsoft.Json.JsonWriter",
                "System.Object",
                "Newtonsoft.Json.JsonSerializer"
            });

        _ReadDetour = INativeDetour.CreateAndApply((nint)readmethod, Detour_Read, out _ReadOriginal);
        _WriteDetour = INativeDetour.CreateAndApply((nint)writemethod, Detour_Write, out _WriteOriginal);
    }

    private unsafe static IntPtr Detour_Read(IntPtr _this, IntPtr reader, IntPtr objectType, IntPtr existingValue, IntPtr serializer)
    {
        var processor = new DataSetConverter(_this).GetNativeJsonProcessor();
        if (processor != null)
        {
            var jsonReader = new JsonReader(reader);
            var jsonSerializer = new JsonSerializer(serializer);
            var result = processor.ProcessRead(jsonReader, existingValue, jsonSerializer);
            return result;
        }
        return _ReadOriginal(_this, reader, objectType, existingValue, serializer);
    }

    private unsafe static void Detour_Write(IntPtr _this, IntPtr writer, IntPtr value, IntPtr serializer)
    {
        var processor = new DataSetConverter(_this).GetNativeJsonProcessor();
        if (processor != null)
        {
            var jsonWriter = new JsonWriter(writer);
            var jsonSerializer = new JsonSerializer(serializer);
            processor.ProcessWrite(jsonWriter, value, jsonSerializer);
            return;
        }

        _WriteOriginal(_this, writer, value, serializer);
    }
}
