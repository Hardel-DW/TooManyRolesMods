using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using Essentials.CustomOptions;
using BepInEx.Logging;

namespace RolesMods {
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class RolesMods : BasePlugin {
        public const string Id = "fr.hardel.rolesmodes";
        public static ManualLogSource Logger;
        public Harmony Harmony { get; } = new Harmony(Id);

        // Investigator CustomOptions
        public static CustomToggleOption EnableInvestigator = CustomOption.AddToggle("Enable Investigator", false);
        public static CustomNumberOption NumberInvestigator = CustomOption.AddNumber("Number Investigator", 1f, 1f, 10f, 1f);
        public static CustomNumberOption footPrintSize = CustomOption.AddNumber("FootPrint Size", 300, 50, 1200, 50f);
        public static CustomNumberOption fontPrintInterval = CustomOption.AddNumber("FootPrint Interval", 3f, 0.25f, 10f, 0.25f);
        public static CustomNumberOption fontPrintDuration = CustomOption.AddNumber("FootPrint Duration", 10f, 3f, 30f, 3f);
        public static CustomToggleOption AnonymousFootPrint = CustomOption.AddToggle("Anonymous FootPrint", false);

        public override void Load() {
            Logger = Log;
            Logger.LogInfo("Mods RolesMods is ready !");
            RegisterInIl2CppAttribute.Register();

            Harmony.PatchAll();
        }
    }
}
