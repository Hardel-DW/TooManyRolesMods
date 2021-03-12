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
    public class RolesMods : BasePlugin {
        public const string Id = "fr.hardel.toomanyrolesmodes";
        public static ManualLogSource Logger;
        public Harmony Harmony { get; } = new Harmony(Id);

        // Investigator CustomOptions
        public static CustomToggleOption EnableInvestigator = CustomOption.AddToggle("Enable Investigator", false);
        public static CustomNumberOption NumberInvestigator = CustomOption.AddNumber("Number Investigator", 1f, 1f, 10f, 1f);
        public static CustomNumberOption footPrintSize = CustomOption.AddNumber("Footprint Size", 0.75f, 0.3f, 1f, 0.1f);
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
        
        //Lighter
        public static CustomToggleOption EnablePsychic = CustomOption.AddToggle("Enable Psychic", false);
        public static CustomNumberOption NumberPsychic = CustomOption.AddNumber("Number Psychic", 1f, 1f, 10f, 1f);
        public static CustomNumberOption PsychicDuration = CustomOption.AddNumber("Vision Duration", 5f, 3f, 30f, 1f);
        public static CustomNumberOption PsychicCooldown = CustomOption.AddNumber("Vision Cooldown", 30f, 10f, 120f, 5f);
        public static CustomToggleOption AnonymousPlayerMinimap = CustomOption.AddToggle("Anonymous player on minimap", false);
        public static CustomToggleOption DeadBodyVisible = CustomOption.AddToggle("Dead body visible", false);


        public override void Load() {
            Logger = Log;
            Logger.LogInfo("Mods RolesMods is ready !");
            RegisterInIl2CppAttribute.Register();
            Harmony.PatchAll();

            ResourceLoader.LoadAssets();
        }
    }
}
