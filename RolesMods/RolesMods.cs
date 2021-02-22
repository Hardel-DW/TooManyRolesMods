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
        public static CustomNumberOption footPrintSize = CustomOption.AddNumber("Footprint Size", 4f, 1f, 10f, 0.5f);
        public static CustomNumberOption fontPrintInterval = CustomOption.AddNumber("Footprint Interval", 1f, 0.25f, 5f, 0.25f);
        public static CustomNumberOption fontPrintDuration = CustomOption.AddNumber("Footprint Duration", 10f, 3f, 30f, 1f);
        public static CustomToggleOption AnonymousFootPrint = CustomOption.AddToggle("Anonymous Footprint", false);
        public static CustomToggleOption VentFootprintVisible = CustomOption.AddToggle("Footprint are visible arround vent", false);

        // TimeMaster
        public static CustomToggleOption EnableTimeMaster = CustomOption.AddToggle("Enable Time Master", false);
        public static CustomToggleOption EnableReiveTimeMaster = CustomOption.AddToggle("Enable Rivive during rewind", false);
        public static CustomNumberOption TimeMasterDuration = CustomOption.AddNumber("Rewind Duration", 5f, 3f, 30f, 1f);
        public static CustomNumberOption TimeMasterCooldown = CustomOption.AddNumber("Rewind Cooldown", 30f, 10f, 120f, 5f);

        //Lighter
        public static CustomToggleOption EnableLighter = CustomOption.AddToggle("Enable Lighter", false);
        public static CustomNumberOption NumberLighter = CustomOption.AddNumber("Number Lighter", 1f, 1f, 10f, 1f);
        public static CustomNumberOption LighterMultiplier = CustomOption.AddNumber("Lighter Multiplier", 1.5f, 0f, 5f, 0.05f);
        public static CustomToggleOption LighterSabotageVision = CustomOption.AddToggle("Lighter sees during electrical sabotage", true);

        public override void Load() {
            Logger = Log;
            Logger.LogInfo("Mods RolesMods is ready !");
            RegisterInIl2CppAttribute.Register();

            Harmony.PatchAll();
        }
    }
}
