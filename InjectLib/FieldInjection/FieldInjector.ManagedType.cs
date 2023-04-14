using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.FieldInjection;
public static partial class FieldInjector<Il2CppType> where Il2CppType : Il2CppObjectBase
{
    private static unsafe void InjectManagedField<Value>(string fieldName) where Value : class
    {
        var size = sizeof(ulong);
        var objectClass = UnityVersionHandler.Wrap((Il2CppClass*)Il2CppClassPointerStore<Il2CppType>.NativeClassPtr);
        objectClass.InstanceSize += (uint)size;
        objectClass.ActualSize = objectClass.InstanceSize;

        _OffsetLookup[fieldName] = new()
        {
            FieldOffset = (nint)(objectClass.InstanceSize - size),
            FieldSize = size
        };
    }

    public static unsafe bool TryGetManagedField<Value>(Il2CppType obj, string fieldName, out Value value) where Value : class
    {
        if (obj == null)
        {
            value = null;
            return false;
        }

        if (obj.Pointer == IntPtr.Zero)
        {
            value = null;
            return false;
        }

        if (!_OffsetLookup.TryGetValue(fieldName, out var fieldInfo))
        {
            value = null;
            return false;
        }

        var size = sizeof(ulong);
        if (size != fieldInfo.FieldSize)
        {
            value = null;
            return false;
        }

        var handle = *(ulong*)(obj.Pointer + fieldInfo.FieldOffset);
        if (handle == 0ul)
        {
            value = null;
            return false;
        }

        value = ManagedReferenceStore<Il2CppType, Value>.Get(handle);
        return value != null;
    }

    public static unsafe bool TrySetManagedField<Value>(Il2CppType obj, string fieldName, Value value) where Value : class
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

        var size = sizeof(ulong);
        if (size != fieldInfo.FieldSize)
        {
            return false;
        }

        var handle = *(ulong*)(obj.Pointer + fieldInfo.FieldOffset);
        if (handle == 0ul)
        {
            var newHandle = ManagedReferenceStore<Il2CppType, Value>.UniqueKey;
            ManagedReferenceStore<Il2CppType, Value>.Set(newHandle, value);
            *(ulong*)(obj.Pointer + fieldInfo.FieldOffset) = newHandle;
            return true;
        }

        ManagedReferenceStore<Il2CppType, Value>.Set(handle, value);
        return true;
    }
}
