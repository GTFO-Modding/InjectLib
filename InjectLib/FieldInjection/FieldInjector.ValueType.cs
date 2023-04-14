using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.FieldInjection;
public static partial class FieldInjector<Il2CppType> where Il2CppType : Il2CppObjectBase
{
    private static int SizeOfUnmanagedType<Value>() where Value : unmanaged
    {
        if (typeof(Value) == typeof(bool))
        {
            return sizeof(bool);
        }
        else
        {
            return Marshal.SizeOf<Value>();
        }
    }

    private static unsafe void InjectValueField<Value>(string fieldName) where Value : unmanaged
    {
        var size = SizeOfUnmanagedType<Value>();
        var objectClass = UnityVersionHandler.Wrap((Il2CppClass*)Il2CppClassPointerStore<Il2CppType>.NativeClassPtr);
        objectClass.InstanceSize += (uint)size;
        objectClass.ActualSize = objectClass.InstanceSize;

        _OffsetLookup[fieldName] = new()
        {
            FieldOffset = (nint)(objectClass.InstanceSize - size),
            FieldSize = size
        };
    }

    public static unsafe bool TryGetValueField<Value>(Il2CppType obj, string fieldName, out Value value) where Value : unmanaged
    {
        if (obj == null)
        {
            value = default;
            return false;
        }

        if (obj.Pointer == IntPtr.Zero)
        {
            value = default;
            return false;
        }

        if (!_OffsetLookup.TryGetValue(fieldName, out var fieldInfo))
        {
            value = default;
            return false;
        }

        var size = SizeOfUnmanagedType<Value>();
        if (size != fieldInfo.FieldSize)
        {
            value = default;
            return false;
        }

        value = *(Value*)(obj.Pointer + fieldInfo.FieldOffset);
        return true;
    }

    public static unsafe bool TrySetValueField<Value>(Il2CppType obj, string fieldName, Value value) where Value : unmanaged
    {
        if (obj == null)
        {
            return false;
        }

        if (obj.Pointer == IntPtr.Zero)
        {
            return false;
        }

        if (!_OffsetLookup.TryGetValue(fieldName, out var fieldInfo))
        {
            return false;
        }

        var size = SizeOfUnmanagedType<Value>();
        if (size != fieldInfo.FieldSize)
        {
            return false;
        }

        *(Value*)(obj.Pointer + fieldInfo.FieldOffset) = value;
        return true;
    }
}
