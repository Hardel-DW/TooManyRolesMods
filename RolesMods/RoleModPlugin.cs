using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using BepInEx.Logging;
using Harion.CustomOptions;
using Harion;
using Harion.Reactor;
using Harion.Cooldown;

namespace RolesMods {

    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(HarionPlugin.Id)]
    public class RoleModPlugin : BasePlugin {
        public const string Id = "fr.hardel.toomanyrolesmodes";
        public static ManualLogSource Logger;

        public Harmony Harmony { get; } = new Harmony(Id);

        // Game Option
        public static CustomOptionHolder CrewmateHolder = CustomOption.AddHolder("<b><color=#007ACCFF>Crewmate Role :</color></b>");
        public static CustomOptionHolder ImpostorHolder = CustomOption.AddHolder("<b><color=#FF0000FF>Impostor Role :</color></b>");
        public static CustomOptionHolder NeutralHolder = CustomOption.AddHolder("<b><color=#888888FF>Neutral Role :</color></b>");
        public static CustomOptionHolder DeadHolder = CustomOption.AddHolder("<b><color=#7137AEFF>Dead Role :</color></b>");

        public override void Load() {
            Logger = Log;
            RegisterInIl2CppAttribute.Register();
            Harmony.PatchAll();
            RegisterCooldownButton.Register();
            CustomOption.ShamelessPlug = false;
            ResourceLoader.LoadAssets();
            GameOptionConf();
        }

        private void GameOptionConf() {
            CrewmateHolder.HudStringFormat = (option, name, value) => $"\n{name}";
            ImpostorHolder.HudStringFormat = (option, name, value) => $"\n{name}";
            NeutralHolder.HudStringFormat = (option, name, value) => $"\n{name}";
            DeadHolder.HudStringFormat = (option, name, value) => $"\n{name}";
        }
    }
}