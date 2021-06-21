using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using BepInEx.Logging;
using Harion.CustomOptions;
using Harion;
using Harion.Reactor;
using Harion.Cooldown;
using Harion.ModsManagers.Configuration;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Harion.ModsManagers.Mods;

namespace RolesMods {

    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(HarionPlugin.Id)]
    public class RoleModPlugin : BasePlugin, IModManager, IModManagerUpdater, IModManagerLink {
        public const string Id = "fr.hardel.toomanyrolesmodes";
        public static ManualLogSource Logger;

        public static CustomOptionHolder CrewmateHolder;
        public static CustomOptionHolder ImpostorHolder;
        public static CustomOptionHolder NeutralHolder;
        public static CustomOptionHolder DeadHolder;

        public Harmony Harmony { get; } = new Harmony(Id);

        public string DisplayName => "Too Many Roles";

        public string Version => typeof(RoleModPlugin).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        public string SmallDescription => "Add 32 roles to Among Us";

        public string Description => "This mod adds more than 30 roles, for fun, and tons of perssonalization. Using Harion.";

        public string Credit => "Hardel";

        public string GithubRepositoryName => "TooManyRolesMods";

        public string GithubAuthorName => "Hardel-DW";

        public GithubVisibility GithubRepositoryVisibility => GithubVisibility.Public;

        public string GithubAccessToken => "";

        public Dictionary<string, Sprite> ModsLinks => new Dictionary<string, Sprite>() {
            { "https://www.patreon.com/hardel", ModsSocial.PatreonSprite },
            { "https://discord.gg/HZtCDK3s",  ModsSocial.DiscordSprite }
        };

        public override void Load() {
            Logger = Log;
            RegisterInIl2CppAttribute.Register();
            Harmony.PatchAll();
            RegisterCooldownButton.Register();
            CustomOption.ShamelessPlug = false;
            ResourceLoader.LoadAssets();


            RoleModPlugin.Logger.LogInfo("Test 1");
            CrewmateHolder = CustomOption.AddHolder("Crewmate Role");
            ImpostorHolder = CustomOption.AddHolder("Impostor Role");
            NeutralHolder = CustomOption.AddHolder("Neutral Role");
            DeadHolder = CustomOption.AddHolder("Dead Role");

            RoleModPlugin.Logger.LogInfo("Test 2");
        }
    }
}