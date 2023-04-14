using Il2CppInterop.Runtime.InteropTypes;
using Il2CppJsonNet;
using Il2CppJsonNet.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.JsonNETInjection.Converter;
public abstract class Il2CppJsonReferenceTypeConverter<T> : INativeJsonConverter where T : Il2CppObjectBase
{
    public virtual string Name => GetType().Name;

    public Il2CppSystem.Object ReadJson(JToken jToken, IntPtr existingValue, JsonSerializer serializer)
    {
        if (existingValue == IntPtr.Zero)
        {
            return Read(jToken, null, serializer).Cast<Il2CppSystem.Object>();
        }
        var wrappedValue = (T)Activator.CreateInstance(typeof(T), existingValue);
        return Read(jToken, wrappedValue, serializer).Cast<Il2CppSystem.Object>();
    }

    public void WriteJson(JsonWriter writer, IntPtr valueToWrite, JsonSerializer serializer)
    {
        if (valueToWrite == IntPtr.Zero)
        {
            Write(writer, null, serializer);
            return;
        }
        var wrappedValue = (T)Activator.CreateInstance(typeof(T), valueToWrite);
        Write(writer, wrappedValue, serializer);
    }

    protected abstract T Read(JToken jToken, T existingValue, JsonSerializer serializer);
    protected abstract void Write(JsonWriter writer, T value, JsonSerializer serializer);
}