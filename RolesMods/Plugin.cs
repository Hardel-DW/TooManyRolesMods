using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using BepInEx.Logging;
using HardelAPI.CustomOptions;
using HardelAPI;
using HardelAPI.Reactor;
using HardelAPI.Cooldown;

namespace RolesMods {

    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(HardelApiPlugin.Id)]
    public class Plugin : BasePlugin {
        public const string Id = "fr.hardel.toomanyrolesmodes";
        public static ManualLogSource Logger;

        public Harmony Harmony { get; } = new Harmony(Id);

        public override void Load() {
            Logger = Log;

            Logger.LogInfo("Hello world");
            RegisterInIl2CppAttribute.Register();
            Harmony.PatchAll();
            RegisterCooldownButton.Register();
            CustomOption.ShamelessPlug = false;
            ResourceLoader.LoadAssets();
        }
    }
}