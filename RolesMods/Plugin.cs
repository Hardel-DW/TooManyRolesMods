using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using BepInEx.Logging;
using HardelAPI.CustomOptions;
using HardelAPI;
using HardelAPI.Reactor;
using HardelAPI.Cooldown;
using HardelAPI.ModsManagers.Configuration;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using HardelAPI.ModsManagers.Mods;

namespace RolesMods {

    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(HardelApiPlugin.Id)]
    public class Plugin : BasePlugin, IModManager, IModManagerUpdater, IModManagerLink {
        public const string Id = "fr.hardel.toomanyrolesmodes";
        public static ManualLogSource Logger;

        public Harmony Harmony { get; } = new Harmony(Id);

        public string DisplayName => "Too Many Roles";

        public string Version => typeof(Plugin).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

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

            Logger.LogInfo("Hello world");
            RegisterInIl2CppAttribute.Register();
            Harmony.PatchAll();
            RegisterCooldownButton.Register();
            CustomOption.ShamelessPlug = false;
            ResourceLoader.LoadAssets();
        }
    }
}