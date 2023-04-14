using Il2CppInterop.Runtime.InteropTypes;
using Il2CppJsonNet;
using Il2CppJsonNet.Converters;
using Il2CppJsonNet.Linq;
using InjectLib.JsonNETInjection.Converter;
using InjectLib.JsonNETInjection.Handler;
using InjectLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.JsonNETInjection;
internal class NativeJsonProcessor
{
    public Il2CppSystem.Type Type { get; set; }
    public DataSetConverter DummyConverter { get; set; }
    public INativeJsonConverter Converter { get; set; }
    public IList<INativeJsonHandler> Handlers { get; } = new List<INativeJsonHandler>();
    public bool Enabled { get; set; } = true;

    public IntPtr ProcessRead(JsonReader reader, IntPtr existingValue, JsonSerializer serializer)
    {
        var jtoken = JToken.ReadFrom(reader);

        IntPtr result;
        if (Converter != null)
        {
            
            result = Converter.ReadJson(jtoken, existingValue, serializer).Pointer;
        }
        else
        {
            SetUsingContractConverter(serializer, use: false);
            result = serializer.Deserialize(jtoken.CreateReader(), Type).Pointer;
            SetUsingContractConverter(serializer, use: true);
        }

        Il2CppSystem.Object value = PtrToObject(result);
        foreach (var h in Handlers)
        {
            h.OnRead(value, jtoken);
        }
        return result;
    }

    public void ProcessWrite(JsonWriter writer, IntPtr valueToWrite, JsonSerializer serializer)
    {
        if (Converter != null)
        {
            Converter.WriteJson(writer, valueToWrite, serializer);
        }
        else
        {
            SetUsingContractConverter(serializer, use: false);
            serializer.Serialize(writer, PtrToObject(valueToWrite));
            SetUsingContractConverter(serializer, use: true);
        }
    }

    private static Il2CppSystem.Object PtrToObject(IntPtr ptr)
    {
        if (ptr == IntPtr.Zero) return null;

        return new Il2CppSystem.Object(ptr);
    }

    private void SetUsingContractConverter(JsonSerializer serializer, bool use)
    {
        var contract = serializer.ContractResolver.ResolveContract(Type);
        if (use)
        {
            contract.Converter = DummyConverter;
            contract.InternalConverter = DummyConverter;
        }
        else
        {
            contract.Converter = null;
            contract.InternalConverter = null;
        }
    }
}
