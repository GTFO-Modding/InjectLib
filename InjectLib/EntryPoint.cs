using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using InjectLib.JsonNETInjection;
using System.Linq;

namespace InjectLib;
[BepInPlugin("GTFO.InjectLib", "InjectLib", VersionInfo.Version)]
[BepInDependency("dev.gtfomodding.gtfo-api", BepInDependency.DependencyFlags.HardDependency)]
internal class EntryPoint : BasePlugin
{
    public override unsafe void Load()
    {
        NativeJsonProcessorStore.Initialize();
    }

    public override bool Unload()
    {
        return base.Unload();
    }
}
