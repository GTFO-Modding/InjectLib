using Il2CppJsonNet;
using Il2CppJsonNet.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace InjectLib.JsonNETInjection.Converter;
public abstract class Il2CppJsonUnmanagedTypeConverter<T> : INativeJsonConverter where T : unmanaged
{
    public virtual string Name => GetType().Name;

    public Il2CppSystem.Object ReadJson(JToken jToken, IntPtr existingValue, JsonSerializer serializer)
    {
        if (existingValue == IntPtr.Zero)
        {
            return ToIl2CppObject(Read(jToken, default, serializer));
        }
        var wrappedValue = new Il2CppSystem.Object(existingValue);
        return ToIl2CppObject(Read(jToken, wrappedValue.Unbox<T>(), serializer));
    }

    public void WriteJson(JsonWriter writer, IntPtr valueToWrite, JsonSerializer serializer)
    {
        if (valueToWrite == IntPtr.Zero)
        {
            Write(writer, default, serializer);
            return;
        }
        var wrappedValue = new Il2CppSystem.Object(valueToWrite);
        Write(writer, wrappedValue.Unbox<T>(), serializer);
    }

    protected abstract T Read(JToken jToken, T existingValue, JsonSerializer serializer);
    protected abstract void Write(JsonWriter writer, T value, JsonSerializer serializer);
    protected abstract Il2CppSystem.Object ToIl2CppObject(T value);
}