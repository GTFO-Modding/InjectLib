using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppJsonNet;
using Il2CppJsonNet.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InjectLib.JsonNETInjection.Supports;
internal class ReferenceTypeConnector<T> : JsonConverter<T>, IBaseConnector where T : Il2CppObjectBase
{
    public NativeJsonProcessor Processor { get; set; }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var il2cppType = Il2CppType.From(typeToConvert, throwOnFailure: false);
        var node = JsonNode.Parse(ref reader, null);
        return (T)Activator.CreateInstance(typeof(T), JsonConvert.DeserializeObject(node.ToJsonString(), il2cppType, null).Pointer);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        System.Text.Json.JsonSerializer.Serialize(writer, value);
    }
}
