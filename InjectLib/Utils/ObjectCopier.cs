using Il2CppInterop.Runtime.InteropTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.Utils;
internal static class ObjectCopier
{
    public static void CopyProperties<T>(T source, T target) where T : Il2CppObjectBase
    {
        foreach (var prop in typeof(T).GetProperties())
        {
            var propertyType = prop.PropertyType;

            if (prop.Name.Contains("_k__BackingField"))
            {
                continue;
            }

            if (propertyType == typeof(IntPtr))
            {
                continue;
            }

            if (!prop.CanRead || !prop.CanWrite)
            {
                continue;
            }

            prop.SetValue(target, prop.GetValue(source));
        }
    }
}
