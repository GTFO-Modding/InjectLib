using Il2CppJsonNet;
using Il2CppJsonNet.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.JsonNETInjection.Converter;
internal interface INativeJsonConverter
{
    public string Name { get; }
    public Il2CppSystem.Object ReadJson(JToken jToken, IntPtr existingValue, JsonSerializer serializer);
    public void WriteJson(JsonWriter writer, IntPtr valueToWrite, JsonSerializer serializer);
}
