using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using BepInEx.Logging;
using Essentials.Options;

namespace RolesMods {

    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class Plugin : BasePlugin {
        public const string Id = "fr.hardel.toomanyrolesmodes";
        public static ManualLogSource Logger;

        public Harmony Harmony { get; } = new Harmony(Id);

        public override void Load() {
            Logger = Log;
            RegisterInIl2CppAttribute.Register();
            Harmony.PatchAll();
            CustomOption.ShamelessPlug = false;
            ResourceLoader.LoadAssets();
        }
    }
}
