using Harion.ModsManagers;
using Harion.ModsManagers.Configuration;
using Harion.ModsManagers.Mods;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RolesMods {
    public class ModManager : ModRegistry, IModManager, IModManagerUpdater, IModManagerLink {

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
    }
}
